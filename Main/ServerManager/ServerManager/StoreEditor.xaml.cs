using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para agregar y modificar tiendas
    /// </summary>
    public partial class StoreEditor
    {
        #region Constants, Variables and Properties

        bool categoriesLoaded;
        private string session;

        /// <summary>
        /// El identificador de sesión
        /// </summary>
        public string Session
        {
            get { return session; }
            set { session = value; }
        }

        /// <summary>
        /// La ventana para seleccionar categorías
        /// </summary>
        private CategorySelectorWindow categoryWindow;

        private EditionMode mode;
        /// <summary>
        /// El modo del componente.
        /// </summary>
        public EditionMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;

                if (mode == EditionMode.Add)
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddStore;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditStore;
                }
            }
        }

        private StoreEntity store;
        /// <summary>
        /// La entidad que está siendo agregada o modificada
        /// </summary>
        public StoreEntity Store
        {
            get { return store; }
            set
            {
                Initialize();

                store = value;
                TxtName.Text = store.Name;
                TxtPhone.Text = store.TelephoneNumber;
                TxtContact.Text = store.ContactName;
                TxtWebsite.Text = store.WebAddress;
                TxtEmail.Text = store.Email;
                TxtNumber.Text = store.LocalNumber;
                TxtInternalPhone.Text = store.InternalPhoneNumber;
                TxtOwner.Text = store.OwnerName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public StoreEditor()
        {
            this.InitializeComponent();
            store = new StoreEntity();

            Initialize();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Establece el foco en la primer caja de texto del formulario
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(TxtName);
        }

        /// <summary>
        /// Carga todas las categorías desde un servicio web
        /// </summary>
        /// <param name="session">El identificador de sesión</param>
        /// <returns>true si la carga se realiza con éxito</returns>
        public bool LoadCategories(string sessionId)
        {
            this.session = sessionId;
            bool result = categoryWindow.selector.Load(sessionId);

            if (categoriesLoaded) return true;

            if (mode == EditionMode.Edit)
            {
                foreach (StoreCategoryEntity storeCategory in store.StoreCategory)
                {
                    categoryWindow.AddSelectedCategory(storeCategory.Category);
                }
            }
            categoriesLoaded = true;

            return result;
        }

        #endregion

        #region Protected and Private Instance Methods

        private void Initialize()
        {
            categoryWindow = new CategorySelectorWindow();
            categoryWindow.Closing += new System.ComponentModel.CancelEventHandler(categoryWindow_Closing);
            this.categoriesLoaded = false;
        }

        /// <summary>
        /// Carga el contenido del formulario en un objeto de entidad
        /// </summary>
        private void Load()
        {
            if (mode == EditionMode.Add)
            {
                store = new StoreEntity();
            }

            store.Name = TxtName.Text.Trim();
            store.TelephoneNumber = TxtPhone.Text.Trim();
            store.ContactName = TxtContact.Text.Trim();
            store.WebAddress = TxtWebsite.Text.Trim();
            store.Email = TxtEmail.Text.Trim();
            store.LocalNumber = TxtNumber.Text.Trim();
            store.InternalPhoneNumber = TxtInternalPhone.Text.Trim();
            store.OwnerName = TxtOwner.Text.Trim();

            System.Collections.Generic.List<StoreCategoryEntity> toRemove = new System.Collections.Generic.List<StoreCategoryEntity>();

            // si las categorías fueron modificadas
            if (this.categoriesLoaded)
            {
                // recorremos todas las categorías de la tienda
                foreach (StoreCategoryEntity storeCategory in store.StoreCategory)
                {
                    bool exists = false;

                    foreach (CategoryEntity category in categoryWindow.Selected)
                    {
                        // no se puede eliminar una categoría que ya está seleccionada
                        if (storeCategory.Category.Name == category.Name)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        toRemove.Add(storeCategory);
                    }
                }

                foreach (StoreCategoryEntity storeCategory in toRemove)
                {
                    store.StoreCategory.Remove(storeCategory);
                }

                // recorremos todas las categorías seleccionadas
                foreach (CategoryEntity category in categoryWindow.Selected)
                {
                    bool exists = false;

                    foreach (StoreCategoryEntity storeCategory in store.StoreCategory)
                    {
                        if (storeCategory.Category.Name == category.Name)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        StoreCategoryEntity newStoreCategory = new StoreCategoryEntity();
                        newStoreCategory.Category = category;
                        store.StoreCategory.Add(newStoreCategory);
                    }
                }
            }

        }

        /// <summary>
        /// Limpiar el contenido del formulario
        /// </summary>
        private void Clear()
        {
            TxtName.Text = "";
            TxtPhone.Text = "";
            TxtContact.Text = "";
            TxtWebsite.Text = "";
            TxtEmail.Text = "";
            TxtNumber.Text = "";
            TxtInternalPhone.Text = "";
            TxtOwner.Text = "";

            categoriesLoaded = false;

            categoryWindow.selector.Clear();
        }

        /// <summary>
        /// Validar la entrada de datos del formulario
        /// </summary>
        /// <param name="message">Mensaje de error si la validación falla</param>
        /// <returns>true si el contenido es válido</returns>
        private bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(store.Name))
            {
                message = UtnEmall.ServerManager.Properties.Resources.NameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(store.TelephoneNumber))
            {
                message = UtnEmall.ServerManager.Properties.Resources.PhoneIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(store.ContactName))
            {
                message = UtnEmall.ServerManager.Properties.Resources.ContactNameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(store.WebAddress))
            {
                message = UtnEmall.ServerManager.Properties.Resources.WebsiteIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(store.Email))
            {
                message = UtnEmall.ServerManager.Properties.Resources.EmailIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(store.LocalNumber))
            {
                message = UtnEmall.ServerManager.Properties.Resources.StoreNumberIsEmpty;
                return false;
            }

            if (!Validator.IsNumber(store.LocalNumber))
            {
                message = UtnEmall.ServerManager.Properties.Resources.StoreNumberNotValid;
                return false;
            }

            if (string.IsNullOrEmpty(store.InternalPhoneNumber))
            {
                message = UtnEmall.ServerManager.Properties.Resources.InternalPhoneIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(store.OwnerName))
            {
                message = UtnEmall.ServerManager.Properties.Resources.OwnerNameIsEmpty;
                return false;
            }

            message = UtnEmall.ServerManager.Properties.Resources.OK;
            return true;
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón OK
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            string message;
            Load();

            if (!Validate(out message))
            {
                Util.ShowErrorDialog(message);
                return;
            }
            else if (categoryWindow.IsVisible)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.CategoryWindowStillOpen);
                return;
            }

            Clear();

            if (OkSelected != null)
            {
                OkSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón OK
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Clear();

            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona una tecla en el formulario
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnOkClicked(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Categorías
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnCategoriesClicked(object sender, RoutedEventArgs e)
        {
            ItemCollection items = null;

            if (categoryWindow != null && categoryWindow.Visibility == Visibility.Visible)
            {
                categoryWindow.Focus();
                return;
            }

            if (categoryWindow != null)
            {
                items = categoryWindow.selector.selected.Items;
            }
            if (!categoriesLoaded)
            {
                categoryWindow.ClearSelected();

                LoadCategories(session);

                if (items != null)
                {
                    foreach (string item in items)
                    {
                        categoryWindow.AddSelectedCategory(item);
                    }
                }
            }
            categoryWindow.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Método invocado cuando se cierra la ventana de categorías.
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void categoryWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!categoryWindow.IsOkClicked && categoryWindow.Selected.Count==0)
            {
                categoriesLoaded = false;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Evento creado cuando se selecciona el botón OK
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botón Cancelar
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}