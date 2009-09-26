using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UtnEmall.ServerManager;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual que muestra un mensaje de bienvenida y permite al 
    /// usuario  iniciar sesión en el sistema.
    /// </summary>
    public partial class Welcome : IDisposable
    {
        #region Instance Variables and Properties

        private bool isPreferenceVisible;
        private System.Threading.Timer timer;

        #endregion

        #region Constructors

        /// <summary>
        /// constructor de clase
        /// </summary>
        public Welcome()
        {
            this.InitializeComponent();
            host.Text = Services.Server;
            port.Text = Services.Port;
        }

        #endregion
        
        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia los elementos de la lista
        /// </summary>
        public void Clear()
        {
            user.Text = "";
            password.Password = "";
        }

        /// <summary>
        /// Establece el foco en la primer caja de texto
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(user);
        }

        /// <summary>
        /// Muestra un mensaje para indicar un fallo en el inicio de sesión
        /// </summary>
        public void ShowLogOnError()
        {
            errorMessage.Visibility = Visibility.Visible;
            TimerCallback tc = new TimerCallback(HideErrorMessage);
            timer = new Timer(tc, null, 5000, Timeout.Infinite);
        }

        /// <summary>
        /// Muestra un mensaje para informar el éxito en el inicio de sesión
        /// </summary>
        public void ShowLogOnSuccessful()
        {
            errorMessage.Visibility = Visibility.Hidden;
            Grid.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Hidden;
            successMessage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Habilita componentes del formulario
        /// </summary>
        public void Enable()
        {
            user.IsEnabled = true;
            password.IsEnabled = true;
            LoginButton.IsEnabled = true;
        }

        /// <summary>
        /// Deshabilita los componentes del formulario
        /// </summary>
        public void Disable()
        {
            user.IsEnabled = false;
            password.IsEnabled = false;
            LoginButton.IsEnabled = false;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            Services.Server = host.Text;
            Services.Port = port.Text;
            Services.SaveServerData();

            if (LogOnSelected != null)
            {
                LogOnSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona la tecla Enter
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void PasswordKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona la tecla Enter
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void UserKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.Focus(password);
            }
        }

        /// <summary>
        /// Esconde el mensaje de error
        /// </summary>
        private void HideErrorMessage(object o)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
            {
                errorMessage.Visibility = Visibility.Hidden;
            }));
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler LogOnSelected;

        #endregion

        #region IDisposable Members

        protected virtual void Dispose(bool doDispose)
        {
            if (doDispose)
            {
                timer.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void OnPreferencesClicked(object sender, RoutedEventArgs e)
        {
            if (isPreferenceVisible)
            {
                host.Visibility = Visibility.Hidden;
                port.Visibility = Visibility.Hidden;
                lHost.Visibility = Visibility.Hidden;
                lPort.Visibility = Visibility.Hidden;
                Preferences.Content = UtnEmall.ServerManager.Properties.Resources.ShowPreferences;
                isPreferenceVisible = false;
            }
            else
            {
                host.Visibility = Visibility.Visible;
                port.Visibility = Visibility.Visible;
                lHost.Visibility = Visibility.Visible;
                lPort.Visibility = Visibility.Visible;
                Preferences.Content = UtnEmall.ServerManager.Properties.Resources.HidePreferences;
                isPreferenceVisible = true;
            }
        }
    }
}