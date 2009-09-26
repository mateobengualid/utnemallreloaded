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
using UtnEmall.Server.Base;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para agregar y editar clientes.
    /// </summary>
    public partial class CustomerEditor
    {
        #region Constants, Variables and Properties

        private CustomerEntity customer;
        /// <summary>
        /// El cliente que está siendo creado o editado.
        /// </summary>
        public CustomerEntity Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                TxtFirstName.Text = customer.Name;
                TxtLastName.Text = customer.Surname;
                TxtUserName.Text = customer.UserName;
                TxtPhone.Text = customer.PhoneNumber;
                TxtAddress.Text = customer.Address;
            }
        }

        private EditionMode mode;
        /// <summary>
        /// El modo del componente, puede ser "Add" para crear una nueva entidad o "Edit" para editar una ya existente.
        /// </summary>
        public EditionMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                if (mode == EditionMode.Add)
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddCustomer;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditCustomer;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CustomerEditor()
        {
            this.InitializeComponent();
            customer = new CustomerEntity();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Enfoca en el primer cuadro de texto del formulario.
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(TxtFirstName);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Carga el contenido del formulario en un objeto entidad.
        /// </summary>
        private void Load()
        {
            if (mode == EditionMode.Add)
            {
                customer = new CustomerEntity();
            }

            customer.Name = TxtFirstName.Text.Trim();
            customer.Surname = TxtLastName.Text.Trim();
            customer.UserName = TxtUserName.Text.Trim();
            customer.Password = Utilities.CalculateHashString(TxtPassword.Password);
            customer.PhoneNumber = TxtPhone.Text.Trim();
            customer.Address = TxtAddress.Text.Trim();
        }

        /// <summary>
        /// Limpia el contenido del formulario.
        /// </summary>
        private void Clear()
        {
            TxtFirstName.Text = "";
            TxtLastName.Text = "";
            TxtUserName.Text = "";
            TxtPassword.Password = "";
            TxtConfirm.Password = "";
            TxtPhone.Text = "";
            TxtAddress.Text = "";
        }

        /// <summary>
        /// Valida la entrada del usuario en el formulario.
        /// </summary>
        /// <param name="message">
        /// El mensaje que se mostrará si la validación falla.
        /// </param>
        /// <returns>
        /// Verdadero si es válido, sino, falso.
        /// </returns>
        private bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(customer.Name.Trim()))
            {
                message = UtnEmall.ServerManager.Properties.Resources.FirstNameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(customer.Surname.Trim()))
            {
                message = UtnEmall.ServerManager.Properties.Resources.LastNameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(customer.UserName.Trim()))
            {
                message = UtnEmall.ServerManager.Properties.Resources.UserNameIsEmpty;
                return false;
            }

            if (TxtPassword.Password != TxtConfirm.Password)
            {
                message = UtnEmall.ServerManager.Properties.Resources.PasswordAndConfirmNotEqual;
                return false;
            }

            if (mode == EditionMode.Add)
            {
                if (string.IsNullOrEmpty(TxtPassword.Password))
                {
                    message = UtnEmall.ServerManager.Properties.Resources.PasswordCantBeEmpty;
                    return false;
                }
            }

            if (string.IsNullOrEmpty(customer.PhoneNumber.Trim()))
            {
                message = UtnEmall.ServerManager.Properties.Resources.PhoneIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(customer.Address.Trim()))
            {
                message = UtnEmall.ServerManager.Properties.Resources.AddressIsEmpty;
                return false;
            }

            message = UtnEmall.ServerManager.Properties.Resources.OK;
            return true;
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Aceptar.
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
        /// Método invocado cuando se hace click en el botón Cancelar.
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

            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona una tecla sobre un ítem del formulario, si es enter se simula un click en el botón Aceptar.
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
