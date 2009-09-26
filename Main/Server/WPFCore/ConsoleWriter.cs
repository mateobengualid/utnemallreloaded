using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Clase de utilidad para escribir mensajes en la pantalla del servidor
    /// </summary>
    public static class ConsoleWriter
    {
        private static System.Windows.Controls.TextBox serverDebug;
        private static System.Windows.Controls.TextBox serverHostName;
        private static System.Windows.Controls.TextBox serverDataBaseName;
        private static DependencyObject parent;

        /// <summary>
        /// Enlazer las cajas de texto
        /// </summary>
        /// <param name="debug">Caja de texto de Debug</param>
        /// <param name="hostname">Caja de texto de nombre de host</param>
        /// <param name="database">Caja de texto de nombre de base de datos</param>
        public static void BindComponents(TextBox debug, TextBox hostName, TextBox database)
        {
            serverDebug = debug;
            serverHostName = hostName;
            serverDataBaseName = database;
            parent = serverDebug.Parent;
        }

        /// <summary>
        /// Agrega texto a la salida por consola del servidor
        /// </summary>
        /// <param name="text">texto a mostrar</param>
        public static void SetText(string text)
        {
            parent.Dispatcher.Invoke(DispatcherPriority.Normal, new Action( delegate()
            {
                serverDebug.AppendText(text + Environment.NewLine);
            }));
        }

        /// <summary>
        /// Muestra un mensaje de confirmación de regeneración de servicios.
        /// </summary>
        /// <param name="assemblyFileName">Nombre del ensamblado a regenerar.</param>
        /// <returns>True si el usuario confirma la regeneración</returns>
        public static bool BuildCustomService(string assemblyFileName)
        { 
            
            string message = Resources.CustomServiceFile + assemblyFileName + Resources.CustomServiceBuild;

            return System.Windows.MessageBox.Show(message, Resources.CustomServiceTitle, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Muestra un mensaje de confirmación de regeneración de servicio de infraestructura
        /// </summary>
        /// <param name="assemblyFileName">Nombre del ensamblado a regenerar.</param>
        /// <returns>True si el usuario confirma la regeneración</returns>
        public static bool BuildInfrastructureService(string assemblyFileName)
        {
            string message = Resources.InfrastructureServiceFile + assemblyFileName + Resources.InfrastructureServiceBuild;

            return System.Windows.MessageBox.Show(message, Resources.InfrastructureServiceTitle, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Establece el nombre de host y de base de datos
        /// </summary>
        /// <param name="host">Nombre de host</param>
        /// <param name="database">Nombre de base de datos</param>
        public static void ConfigurationServer(string host, string database)
        {
            // Establece el texto sincronizando con la interfaz de usuario
            parent.Dispatcher.Invoke(DispatcherPriority.Normal, new Action( delegate()
            {
                serverHostName.Text = host;
                serverDataBaseName.Text = database;
            }));
        }
    }
}