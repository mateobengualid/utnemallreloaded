using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtnEmall.Server.Base;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Threading;
using UtnEmall.ServerManager;

namespace UtnEmall.ServerManager
{
    class LoginThread
    {
        private bool onLogin;

        /// <summary>
        /// El componente que emite el evento desde la interfaz de usuario
        /// </summary>
        public UserControl1 Control
        {
            get;
            set;
        }

        /// <summary>
        /// El id de usuario
        /// </summary>
        public string User
        {
            get;
            set;
        }

        /// <summary>
        /// La contraseña del usuario
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// El id de sesión
        /// </summary>
        public string Session
        {
            get;
            set;
        }

        /// <summary>
        /// Estado del proceso
        /// </summary>
        public bool Succeed
        {
            get;
            set;
        }

        /// <summary>
        /// Mensaje del estado del proceso
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="user">The Id de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <param name="control">El componente que emite el evento desde la interfaz de usuario</param>
        public LoginThread(string user, string password, UserControl1 control)
        {
            User = user;
            Password = password;
            Control = control;
        }

        private void DoLogin()
        {
            try
            {
                Session = Services.Session.ValidateUser(User, Utilities.CalculateHashString(Password));
                Succeed = true;
            }
            catch (TargetInvocationException)
            {
                Succeed = false;
                Message = UtnEmall.ServerManager.Properties.Resources.ConnectionErrorAuthenticationFailed;
            }
            catch (CommunicationException)
            {
                Succeed = false;
                Message = UtnEmall.ServerManager.Properties.Resources.ConnectionErrorAuthenticationFailed;
            }

            onLogin = false;

            if (LoginFinished != null)
            {
                Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
                {
                    LoginFinished(this, new EventArgs());
                }));
            }
        }
        
        /// <summary>
        /// Inicia el hilo para realizar el proceso de login
        /// </summary>
        public void Login()
        {
            if (!onLogin)
            {
                onLogin = true;
                Thread login = new Thread(new ThreadStart(DoLogin));
                login.Start();
            }
        }

        /// <summary>
        /// Evento creado cuando el proceso de login ha finalizado
        /// </summary>
        public event EventHandler LoginFinished;

    }
}
