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

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para agregar y editar permisos.
    /// </summary>
    public partial class AuthorizationEditor  
    {
        #region Constants, Variables and Properties

        private string[] bussinesClasses = { "Category", "Customer", "CustomerServiceData", "DataModel", "Group", "Permission", "Service", "Store", "User" };

        private PermissionEntity permission;
        /// <summary>
        /// La entidad que está siendo modificada o creada en el editor.
        /// </summary>
        public PermissionEntity Permission
        {
            get { return permission; }
            set
            {
                permission = value;

                klass.SelectedValue = permission.BusinessClassName;
                read.IsChecked = permission.AllowRead;
                delete.IsChecked = permission.AllowDelete;
                create.IsChecked = permission.AllowNew;
                update.IsChecked = permission.AllowUpdate;
            }
        }

        private EditionMode mode;
        /// <summary>
        /// El modo del componente, puede ser "Add" para agregar una entidad nueva o   "Edit" para editar una entidad ya existente.
        /// </summary>
        public EditionMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;

                if (mode == EditionMode.Add)
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddPermission;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditPermission;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public AuthorizationEditor()
        {
            this.InitializeComponent();

            permission = new PermissionEntity();

            foreach (string bussinesClass in bussinesClasses)
            {
                klass.Items.Add(bussinesClass);
            }
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia el contenido del formulario.
        /// </summary>
        public void Clear()
        {
            klass.SelectedIndex = 0;
            read.IsChecked = false;
            delete.IsChecked = false;
            create.IsChecked = false;
            update.IsChecked = false;
        }

        /// <summary>
        /// Enfoca el primer campo de texto del formulario.
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(klass);
        }

        #endregion

        #region Protected and Private Instance Methods

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
            if (mode == EditionMode.Add)
            {
                permission = new PermissionEntity();
            }

            permission.BusinessClassName = (string)klass.SelectedValue;
            permission.AllowDelete = (bool)delete.IsChecked;
            permission.AllowNew = (bool)create.IsChecked;
            permission.AllowRead = (bool)read.IsChecked;
            permission.AllowUpdate = (bool)update.IsChecked;

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
            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        #endregion

        #endregion

        #region Events
        /// <summary>
        /// Evento lanzado cuando el botón Aceptar es seleccionado.
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// Evento lanzado cuando el botón Cancelar es seleccionado.
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}