using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.ServerManager;
using System.Reflection;
using System.ServiceModel;
using System.Diagnostics;

namespace UtnEmall.ServerManager
{
    class SessionController
    {
        private UserControl1 control;
        private Welcome welcome;

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">Una referencia al control que contiene a este componente</param>
        public SessionController(UserControl1 control, Welcome welcome)
        {
            this.control = control;
            this.welcome = welcome;

            welcome.LogOnSelected += OnLoginSelected;
            welcome.FocusFirst();
        }

        /// <summary>
        /// Cierre de sesión del servidor
        /// </summary>
        public void LogOff()
        {
            if (control.IsLoggedIn)
            {
                try
                {
                    Services.Session.UserLogOff(control.Session);
                }
                catch (TargetInvocationException targetInvocationError)
                {
                    Debug.WriteLine(targetInvocationError.Message);
                }
                catch (EndpointNotFoundException endpointNotFoundError)
                {
                    Debug.WriteLine(endpointNotFoundError.Message);
                }
                catch (CommunicationException communicationError)
                {
                    Debug.WriteLine(communicationError.Message);
                }

                control.Session = null;
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona sobre el botón Iniciar Sesión
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnLoginSelected(object sender, EventArgs e)
        {
            string userName = welcome.user.Text;
            string password = welcome.password.Password;
            if (string.IsNullOrEmpty(userName))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.EmptyUserName);
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.EmptyPassword);
                return;
            }

            welcome.Disable();

            LoginThread loginThread = new LoginThread(userName, password, control);
            loginThread.LoginFinished += new EventHandler(LoginFinished);

            loginThread.Login();

        }

        /// <summary>
        /// Método invocado cuando se finaliza el proceso de inicio de sesión
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void LoginFinished(object sender, EventArgs e)
        {
            LoginThread thread = (LoginThread)sender;

            control.Session = thread.Session;

            if (!thread.Succeed)
            {
                welcome.Enable();
                Util.ShowErrorDialog(thread.Message);
            }

            if (!control.IsLoggedIn)
            {
                welcome.Enable();
                welcome.ShowLogOnError();
                welcome.password.Password = "";
                return;
            }

            welcome.ShowLogOnSuccessful();
        }

    }
}
