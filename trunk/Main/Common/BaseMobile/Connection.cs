using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

namespace UtnEmall.Client.SmartClientLayer
{
    /// <summary>
    /// Centraliza los datos estáticos necesarios para la conexión con el servidor en las capas SmartClient y otras clases relacionadas con el estado de la conexión
    /// </summary>
    static public class Connection
    {
        // Indica si el dispositivo esta conectado con el servidor
        private static bool isConnected;
        // Indica cuando se desconecto el dispositivo por última vez
        private static System.DateTime lastTimeDisconnected = DateTime.Now;
        private static string session;
        private static System.Uri serverUri;
        private static System.ServiceModel.Channels.Binding serverBinding;

        /// <summary>
        /// Indica si la aplicación esta conectada con el servidor.
        /// Cuidado, puede retornar verdadero aún cuando no se pueda contactar al servidor. La actualización del estado no es instantanea
        /// </summary>
        public static bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                if (isConnected != value && value == false)
                {
                    lastTimeDisconnected = System.DateTime.Now;
                }
                isConnected = value;
            }
        }

        /// <summary>
        /// Un valir de fecha muy bajo que puede usarse en .NET y SQL
        /// </summary>
        public static System.DateTime MinDate
        {
            get
            {
                return new DateTime(1900, 1, 1);
            }
        }

        /// <summary>
        /// La última vez que se desconecto del servidor la aplicación
        /// </summary>
        public static System.DateTime LastTimeDisconnected
        {
            get
            {
                return lastTimeDisconnected;
            }
        }
        /// <summary>
        /// La Uri base del servidor
        /// </summary>
        public static System.Uri ServerUri
        {
            get
            {
                if (serverUri == null)
                {
                    serverUri = new Uri("http://UtnEmallserver:8080/");
                }
                return serverUri;
            }
            set
            {
                serverUri = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el objeto Binding a ser usado en las llamadas a servicios WCF
        /// </summary>
        public static System.ServiceModel.Channels.Binding ServerBinding
        {
            get
            {
                if (serverBinding == null)
                {
                    System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
                    HttpTransportBindingElement transport = new HttpTransportBindingElement();
                    transport.MaxReceivedMessageSize = Int32.MaxValue;
                    transport.KeepAliveEnabled = false;
                    binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), transport });
                    serverBinding = binding;
                }
                return serverBinding;
            }
            set
            {
                serverBinding = value;
            }
        }

        /// <summary>
        /// El identificador de sesión para conectarse con el servidor
        /// </summary>
        public static string Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
            }
        }

    }
}
