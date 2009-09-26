using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using UtnEmall.Server.Base;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para agregar o editar usuarios
    /// </summary>
    public partial class UserEditor
    {
        #region Constants, Variables and Properties

        private Dictionary<string, StoreEntity> storeDict;

        private UserEntity user;
        /// <summary>
        /// La entidad que está siendo creada o modificada
        /// </summary>
        public UserEntity User
        {
            get { return user; }
            set
            {
                user = value;
                TxtFirstName.Text = user.Name;
                TxtLastName.Text = user.Surname;
                TxtUserName.Text = user.UserName;
                TxtPhone.Text = user.PhoneNumber;
                TxtPosition.Text = user.Charge;

                if (user.Store != null)
                {
                    stores.SelectedItem = user.Store.Name;
                }
                else
                {
                    stores.SelectedIndex = 0;
                }
            }
        }

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
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddUser;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditUser;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public UserEditor()
        {
            this.InitializeComponent();
            user = new UserEntity();
            storeDict = new Dictionary<string, StoreEntity>();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Establece el foco en la primer caja de texto del formulario.
        /// </summary>
        public void FocusFirst()
        {
            Keyboard.Focus(TxtFirstName);
        }

        /// <summary>
        /// Carga todas las tiendas desde un servicio web.
        /// </summary>
        /// <param name="session">
        /// El identificador de sesión
        /// </param>
        /// <returns>
        /// true si se carga exitosamente
        /// </returns>
        public bool LoadStores(string session)
        {
            int index = 0;
            int count = 0;

            try
            {
                stores.Items.Clear();
                storeDict.Clear();

                stores.Items.Add(UtnEmall.ServerManager.Properties.Resources.NoStore);
                storeDict.Add(UtnEmall.ServerManager.Properties.Resources.NoStore, null);

                foreach (StoreEntity store in Services.Store.GetAllStore(false, session))
                {
                    if (storeDict.ContainsKey(store.Name))
                    {
                        count++;
                        continue;
                    }

                    stores.Items.Add(store.Name);
                    storeDict.Add(store.Name, store);

                    if (mode == EditionMode.Edit && user.IdStore == store.Id)
                    {
                        index = count + 1;
                    }

                    count++;
                }

                stores.SelectedIndex = index;

                return true;
            }
            catch (TargetInvocationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorItemNotSaved);
                return false;
            }
            catch (CommunicationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorItemNotSaved);
                return false;
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Carga el contenido del formulario en un objeto de entidad
        /// </summary>
        private void Load()
        {
            if (mode == EditionMode.Add)
            {
                user = new UserEntity();
                user.IsUserActive = true;
            }

            user.Name = TxtFirstName.Text.Trim();
            user.Surname = TxtLastName.Text.Trim();
            user.UserName = TxtUserName.Text.Trim();

            if (!string.IsNullOrEmpty(TxtPassword.Password) || !string.IsNullOrEmpty(TxtConfirm.Password))
            {
                user.Password = Utilities.CalculateHashString(TxtPassword.Password);
            }

            user.PhoneNumber = TxtPhone.Text.Trim();
            user.Charge = TxtPosition.Text.Trim();

            user.Store = storeDict[(string)stores.SelectedItem];
        }

        /// <summary>
        /// Limpia el contenido del formulario
        /// </summary>
        private void Clear()
        {
            TxtFirstName.Text = "";
            TxtLastName.Text = "";
            TxtUserName.Text = "";
            TxtPassword.Password = "";
            TxtConfirm.Password = "";
            TxtPhone.Text = "";
            TxtPosition.Text = "";
            stores.SelectedIndex = 0;
        }

        /// <summary>
        /// Valida la entrada de datos del formulario
        /// </summary>
        /// <param name="message">
        /// Mensaje a mostrar en caso de fallo
        /// </param>
        /// <returns>
        /// true si el contenido se validó correctamente
        /// </returns>
        private bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                message = UtnEmall.ServerManager.Properties.Resources.FirstNameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(user.Surname))
            {
                message = UtnEmall.ServerManager.Properties.Resources.LastNameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                message = UtnEmall.ServerManager.Properties.Resources.UserNameIsEmpty;
                return false;
            }

            if (string.CompareOrdinal(TxtPassword.Password, TxtConfirm.Password) != 0)
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

            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                message = UtnEmall.ServerManager.Properties.Resources.PhoneIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(user.Charge))
            {
                message = UtnEmall.ServerManager.Properties.Resources.PositionIsEmpty;
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
        /// Método invocado cuando se presiona una tecla en el formulario.
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnKeyPressed(object sender, KeyEventArgs e)
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