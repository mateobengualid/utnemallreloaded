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
using System.Diagnostics;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para agregar y editar categorías.
    /// </summary>
    public partial class CategoryEditor
    {
        #region Constants, Variables and Properties

        private bool isRootCategory;
        /// <summary>
        /// Indica si la categoría que se está creando es raiz o tiene padre.
        /// </summary>
        public bool IsRootCategory
        {
            get { return isRootCategory; }
            set { isRootCategory = value; }
        }

        private EditionMode mode;
        /// <summary>
        /// El modo del componente, puede ser "Add" para crear una nueva entidad, o "Edit" para modificar una ya existente.
        /// </summary>
        public EditionMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                if (mode == EditionMode.Add)
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddCategory;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditCategory;
                }
            }
        }

        private CategoryEntity category;
        /// <summary>
        /// La entidad que se está modificando o creando en el editor.
        /// </summary>
        public CategoryEntity Category
        {
            get { return category; }
            set
            {
                category = value;
                oldCategory.Name = value.Name;
                oldCategory.Description = value.Description;
                oldCategory.Childs = value.Childs;
                TxtName.Text = category.Name;
                TxtDescription.Text = category.Description;
            }
        }

        private CategoryEntity oldCategory;
        /// <summary>
        /// La categoría que está modificándose.
        /// </summary>
        public CategoryEntity OldCategory
        {
            get { return oldCategory; }
        }


        private CategoryEntity parentCategory;
        /// <summary>
        /// La categoría padre de la categoría que se está modificando o creando.
        /// </summary>
        public CategoryEntity ParentCategory
        {
            get { return parentCategory; }
            set
            {
                parentCategory = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CategoryEditor()
        {
            this.InitializeComponent();
            category = new CategoryEntity();
            oldCategory = new CategoryEntity();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Enfoca el primer campo de texto del formulario.
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(TxtName);
        }

        /// <summary>
        /// Limpia el contenido del formulario.
        /// </summary>
        public void Clear()
        {
            TxtName.Text = "";
            TxtDescription.Text = "";
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Carga el contenido del formulario en un objeto de entidad.
        /// </summary>
        private void Load()
        {
            if (Mode == EditionMode.Add)
            {
                category = new CategoryEntity();
            }

            category.Name = TxtName.Text.Trim();
            category.Description = TxtDescription.Text.Trim();
        }

        /// <summary>
        /// Valida la entrada de usuario en el formulario.
        /// </summary>
        /// <param name="message">
        /// El mensaje a mostrar si falla la validación.
        /// </param>
        /// <returns>
        /// Verdadero si el contenido es válido, falso de otro modo.
        /// </returns>
        private bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                message = UtnEmall.ServerManager.Properties.Resources.NameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(category.Description))
            {
                message = UtnEmall.ServerManager.Properties.Resources.DescriptionIsEmpty;
                return false;
            }

            message = "OK";
            return true;
        }

        /// <summary>
        /// Método invocado cuando se clickea el botón Aceptar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            string message;
            Load();

            if (!Validate(out message))
            {
                Util.ShowErrorDialog(message);
                return;
            }

            Clear();

            if (OkSelected != null)
            {
                OkSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se clickea el botón Cancelar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Clear();
            category = null;
            oldCategory = new CategoryEntity();

            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando una tecla se presiona sobre un ítem. En caso de que se presione un enter, simula un click en el botón Aceptar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnOkClicked(sender, e);
            }
        }

        #endregion

        #endregion

        #region Events
        /// <summary>
        /// Evento lanzado cuando se selecciona el botón Aceptar.
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// Evento lanzado cuando se selecciona el botón Cancelar.
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}
