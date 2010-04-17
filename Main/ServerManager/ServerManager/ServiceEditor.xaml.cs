using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using UtnEmall.Server.EntityModel;
using PresentationLayer.DataModelDesigner;
using UtnEmall.Server.BusinessLogic;
using System.Collections.Generic;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para editar servicios
    /// </summary>
    public partial class ServiceEditor
    {
        #region Constants, Variables and Properties

        bool categoriesLoaded;

        /// <summary>
        /// La ventana para seleccionar categorías
        /// </summary>
        private Dictionary<String, StoreEntity> stores;

        public Dictionary<String, StoreEntity> Stores
        {
            get
            { 
                return stores;
            }

            set
            { 
                stores = value;
                storesById.Clear();
                storeCombo.Items.Clear();

                storeCombo.Items.Add(UtnEmall.ServerManager.Properties.Resources.NoStore);

                foreach (StoreEntity store in value.Values)
                {
                    storeCombo.Items.Add(store.Name);
                    storesById.Add(store.Id, store);
                }

                storeCombo.SelectedIndex = 0;
            }
        }

        private Dictionary<int, StoreEntity> storesById;

        private List<StoreEntity> storeList;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<StoreEntity> StoreList
        {
            get
            {
                return storeList;
            }

            set
            {
                storeList = value;
                Dictionary<string, StoreEntity> dict = new Dictionary<string, StoreEntity>();

                foreach(StoreEntity store in storeList)
                {
                    dict.Add(store.Name, store);
                }

                Stores = dict;
            }
        }

        private CategorySelectorWindow categoryWindow;
        private string session;

        /// <summary>
        /// La clave de sesión
        /// </summary>
        public string Session
        {
            get { return session; }
            set { session = value; }
        }

        private EditionMode mode;
        /// <summary>
        /// El modo del componente (agregar o modificar entidad)
        /// </summary>
        public EditionMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;

                if (mode == EditionMode.Add)
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddService;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditService;
                }
            }
        }

        private ServiceEntity service;
        /// <summary>
        /// La entidad que está siendo creada o modificada
        /// </summary>
        public ServiceEntity Service
        {
            get { return service; }
            set
            {
                Initialize();

                service = value;
                TxtName.Text = service.Name;
                TxtDescription.Text = service.Description;
                StartDate.Date = service.StartDate;
                StopDate.Date = service.StopDate;

                storeCombo.SelectedIndex = 0;

                for (int i = 1; i < storeCombo.Items.Count; i++)
                {
                    if (storesById.ContainsKey(service.IdStore) && (string)storeCombo.Items[i] == storesById[service.IdStore].Name)
                    {
                        storeCombo.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public ServiceEditor()
        {
            this.InitializeComponent();

            storesById = new Dictionary<int, StoreEntity>();
            service = new ServiceEntity();

            Initialize();
            
            StartDate.Date = DateTime.Now;
            StopDate.Date = DateTime.Now.AddDays(1);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Establece el foco en la primera caja de texto del formulario
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(TxtName);
        }


        /// <summary>
        /// Carga todas las categorías desde un servicio web
        /// </summary>
        /// <param name="session">
        /// El identificador de sesión para enviar al servicio web
        /// </param>
        /// <returns>
        /// true si se cargó correctamente
        /// </returns>
        public bool LoadCategories(string sessionId)
        {            
            this.session = sessionId;
            bool result = categoryWindow.selector.Load(sessionId);

            if (categoriesLoaded) return true;

            if (mode == EditionMode.Edit)
            {
                foreach (ServiceCategoryEntity serviceCategory in service.ServiceCategory)
                {
                    categoryWindow.AddSelectedCategory(serviceCategory.Category);
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
            categoryWindow.Closing += new System.ComponentModel.CancelEventHandler(OnCategoryWindowClosing);
            this.categoriesLoaded = false;
        }

        /// <summary>
        /// Carga el contenido del formulario en un objeto de entidad
        /// </summary>
        private void Load()
        {
            if (mode == EditionMode.Add)
            {
                service = new ServiceEntity();
            }

            service.Name = TxtName.Text.Trim();
            service.Description = TxtDescription.Text.Trim();
            service.StartDate = StartDate.Date;
            service.StopDate = StopDate.Date;

            if (storeCombo.SelectedIndex == 0)
            {
                service.Store = null;
            }
            else
            {
                service.Store = stores[(string)storeCombo.SelectedItem];
            }

            System.Collections.Generic.List<ServiceCategoryEntity> toRemove = new System.Collections.Generic.List<ServiceCategoryEntity>();

            // Si las categorías fueron modificadas
            if (this.categoriesLoaded)
            {
                // se recorren todas las categorías de servicio
                foreach (ServiceCategoryEntity serviceCategory in service.ServiceCategory)
                {
                    bool exists = false;

                    foreach (CategoryEntity category in categoryWindow.Selected)
                    {
                        // No se elimina una categoría ya asignada
                        if (serviceCategory.Category.Name == category.Name)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        toRemove.Add(serviceCategory);
                    }
                }

                foreach (ServiceCategoryEntity serviceCategory in toRemove)
                {
                    service.ServiceCategory.Remove(serviceCategory);
                }

                // recorremos todas las categorías
                foreach (CategoryEntity category in categoryWindow.Selected)
                {
                    bool exists = false;

                    foreach (ServiceCategoryEntity serviceCategory in service.ServiceCategory)
                    {
                        if (serviceCategory.Category.Name == category.Name)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        ServiceCategoryEntity newServiceCategory = new ServiceCategoryEntity();
                        newServiceCategory.Category = category;
                        service.ServiceCategory.Add(newServiceCategory);
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
            TxtDescription.Text = "";
            categoriesLoaded = false;

            categoryWindow.selector.Clear();
        }

        /// <summary>
        /// Valida los datos de entrada
        /// </summary>
        /// <param name="message">
        /// Mensaje a mostrar si la validación falla
        /// </param>
        /// <returns>
        /// true si el contenido es válido
        /// </returns>
        private bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(service.Name))
            {
                message = UtnEmall.ServerManager.Properties.Resources.NameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(service.Description))
            {
                message = UtnEmall.ServerManager.Properties.Resources.DescriptionIsEmpty;
                return false;
            }

            if (!StartDate.IsValidDate)
            {
                message = UtnEmall.ServerManager.Properties.Resources.InvalidStartDate;
                return false;
            }

            if (!StopDate.IsValidDate)
            {
                message = UtnEmall.ServerManager.Properties.Resources.InvalidStopDate;
                return false;
            }

            if (mode == EditionMode.Add)
            {
                if (service.StopDate.Date.Date < DateTime.Now.Date)
                {
                    message = UtnEmall.ServerManager.Properties.Resources.StopDateInThePast;
                    return false;
                }
            }

            if (service.StopDate <= service.StartDate)
            {
                message = UtnEmall.ServerManager.Properties.Resources.StopDateSmallerStart;
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
        /// Método invocado cuando se presiona el botón Cancelar
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
        /// Método invocado cuando se presiona una tecla
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
        /// Método invocado cuando se presiona el botón de Categorías
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
            if(!categoriesLoaded)
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
        /// Método invocado cuando se cierra la ventana de categorías
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void OnCategoryWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!categoryWindow.IsOkClicked && categoryWindow.Selected.Count == 0)
            {
                categoriesLoaded = false;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Evento creado cuando se presiona el botón OK
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// Evento creado cuando se presiona el botón cancelar
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}