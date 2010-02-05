using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.ServerManager.SAL.ServiceBuilder;
using UtnEmall.ServerManager.SAL.CustomerServiceData;
using UtnEmall.ServerManager.SAL.DataModel;
using UtnEmall.ServerManager.SAL.Store;
using UtnEmall.ServerManager.SAL.Category;
using UtnEmall.ServerManager.SAL.Service;
using UtnEmall.ServerManager.SAL.UserAction;
using UtnEmall.ServerManager.SAL.RegisterAssociation;
using UtnEmall.ServerManager.SAL.StatisticsAnalyzer;
using UtnEmall.ServerManager.SAL.Group;
using UtnEmall.ServerManager.SAL.User;
using UtnEmall.ServerManager.SAL.Customer;
using UtnEmall.ServerManager.SAL.SessionManager;
using System.Xml.XPath;
using System.Xml;
using System.Diagnostics;
using UtnEmall.ServerManager.SAL.Campaign;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define propiedades estáticas para acceder servicios web de una manera sencilla
    /// </summary>
    class Services
    {
        #region Instance Variables and Properties

        private static string server;
        private static BasicHttpBinding _defaultBinding;
        private static BasicHttpBinding DefaultBinding
        {
            get
            {
                if (_defaultBinding == null)
                {
                    _defaultBinding = new BasicHttpBinding();
                    _defaultBinding.MaxReceivedMessageSize = 1048576;
                    _defaultBinding.ReaderQuotas.MaxDepth = 1024;
                    // Establece un tiempo de espera de 10 minutos debido a la construcción de servicios en el servidor puede tomar varios minutos
                    _defaultBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                    _defaultBinding.SendTimeout = new TimeSpan(0, 10, 0);
                }
                return _defaultBinding;
            }
        }

        public static string Server
        {
            get
            {
                if (server == null)
                {
                    LoadServerData();
                }

                return Services.server;
            }

            set
            { 
                Services.server = value;
                InvalidateProxies();
            }
        }

        private static string port;

        public static string Port
        {
            get
            {
                if (port == null)
                {
                    LoadServerData();
                }

                return Services.port;
            }
            set
            { 
                Services.port = value;
                InvalidateProxies();
            }
        }

        private static void InvalidateProxies()
        {
            category = null;
            customer = null;
            customerServiceData = null;
            dataModel = null;
            group = null;
            registerAssociation = null;
            service = null;
            serviceBuilder = null;
            session = null;
            statisticsAnalyzer = null;
            store = null;
            user = null;
            userAction = null;
        }

        private static ServiceBuilderClient serviceBuilder;

        /// <summary>
        /// Guarda los datos de servidor y puerto en un archivo XML
        /// </summary>
        public static void SaveServerData()
        {
            try
            {
                // Crea el archivo XML si no existe
                XmlTextWriter writer = new XmlTextWriter("server.xml", null);
                // Comienza un nuevo documento
                writer.WriteStartDocument();
                // Se agregan los elementos al archivo
                writer.WriteStartElement("server");
                writer.WriteStartElement("host");
                writer.WriteString(Server);
                writer.WriteEndElement();
                writer.WriteStartElement("port");
                writer.WriteString(Port);
                writer.WriteEndElement();
                writer.WriteEndElement();
                // Finaliza el documento
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception error)
            {
                Debug.WriteLine("Exception: {0}", error.ToString());
            } 
        }

        /// <summary>
        /// Carga los datos necesarios para conectarse a un servidor
        /// </summary>
        public static void LoadServerData()
        {
            try
            {
                XPathDocument doc = new XPathDocument("server.xml");
                XPathNavigator nav = doc.CreateNavigator();

                server = nav.SelectSingleNode("/server/host").Value;
                port = nav.SelectSingleNode("/server/port").Value;
            }
            catch (Exception)
            {
                server = "localhost";
                port = "8081";
            }
        }

        /// <summary>
        /// Proxy de servicio web para construir servicios
        /// </summary>
        public static ServiceBuilderClient ServiceBuilder
        {
            get
            {
                if (serviceBuilder == null)
                {
                    serviceBuilder = new ServiceBuilderClient(DefaultBinding, new EndpointAddress(GetServiceURL("ServiceBuilder")));
                }

                return serviceBuilder;
            }
        }

        private static CustomerServiceDataClient customerServiceData;
        /// <summary>
        /// Proxy de servicio web para datos de servicio
        /// </summary>
        public static CustomerServiceDataClient CustemerServiceData
        {
            get
            {
                if (customerServiceData == null)
                {
                    customerServiceData = new CustomerServiceDataClient(DefaultBinding, new EndpointAddress(GetServiceURL("CustomerServiceData")));
                }
                return customerServiceData;
            }
        }

        private static DataModelClient dataModel;
        /// <summary>
        /// Proxy de servicio web para modelo de datos
        /// </summary>
        public static DataModelClient DataModel
        {
            get
            {
                if (dataModel == null)
                {
                    dataModel = new DataModelClient(DefaultBinding, new EndpointAddress(GetServiceURL("DataModel")));
                }

                return dataModel;
            }
        }

        private static StoreClient store;
        /// <summary>
        /// Proxy de servicio web para tiendas
        /// </summary>
        public static StoreClient Store
        {
            get
            {
                if (store == null)
                {
                    store = new StoreClient(DefaultBinding, new EndpointAddress(GetServiceURL("Store")));
                }

                return store;
            }
        }

        private static CategoryClient category;
        /// <summary>
        /// Proxy de servicio web para categorías
        /// </summary>
        public static CategoryClient Category
        {
            get
            {
                if (category == null)
                {
                    category = new CategoryClient(DefaultBinding, new EndpointAddress(GetServiceURL("Category")));
                }

                return category;
            }
        }

        private static ServiceClient service;
        /// <summary>
        /// Proxy de servicio web para servicios
        /// </summary>
        public static ServiceClient Service
        {
            get
            {
                if (service == null)
                {
                    service = new ServiceClient(DefaultBinding, new EndpointAddress(GetServiceURL("Service")));
                }

                return service;
            }
        }

        private static GroupClient group;
        /// <summary>
        /// Proxy de servicio web para grupos
        /// </summary>
        public static GroupClient Group
        {
            get
            {
                if (group == null)
                {
                    group = new GroupClient(DefaultBinding, new EndpointAddress(GetServiceURL("Group")));
                }

                return group;
            }
        }

        private static UserClient user;
        /// <summary>
        /// Proxy de servicio web para usuarios
        /// </summary>
        public static UserClient User
        {
            get
            {
                if (user == null)
                {
                    user = new UserClient(DefaultBinding, new EndpointAddress(GetServiceURL("User")));
                }

                return user;
            }
        }

        private static UserActionClient userAction;
        /// <summary>
        /// Proxy de servicio web para acciones de usuario
        /// </summary>
        public static UserActionClient UserAction
        {
            get
            {
                if (userAction == null)
                {
                    userAction = new UserActionClient(DefaultBinding, new EndpointAddress(GetServiceURL("UserAction")));
                }

                return userAction;
            }
        }

        private static CustomerClient customer;
        /// <summary>
        /// Proxy de servicio web para clientes
        /// </summary>
        public static CustomerClient Customer
        {
            get
            {
                if (customer == null)
                {
                    customer = new CustomerClient(DefaultBinding, new EndpointAddress(GetServiceURL("Customer")));
                }

                return customer;
            }
        }

        private static SessionManagerClient session;
        /// <summary>
        /// Proxy de servicio web para sesión
        /// </summary>
        public static SessionManagerClient Session
        {
            get
            {
                if (session == null)
                {
                    session = new SessionManagerClient(DefaultBinding, new EndpointAddress(GetServiceURL("SessionManager")));
                }

                return session;
            }
        }

        private static RegisterAssociationClient registerAssociation;
        /// <summary>
        /// Proxy de servicio web para registrar asociaciones
        /// </summary>
        public static RegisterAssociationClient RegisterAssociation
        {
            get
            {
                if (registerAssociation == null)
                {
                    registerAssociation = new RegisterAssociationClient(DefaultBinding, new EndpointAddress(GetServiceURL("RegisterAssociation")));
                }

                return registerAssociation;
            }
        }

        private static StatisticsAnalyzerClient statisticsAnalyzer;
        /// <summary>
        /// Proxy de servicio web para análisis de estadísticas
        /// </summary>
        public static StatisticsAnalyzerClient StatisticsAnalyzer
        {
            get
            {
                if (statisticsAnalyzer == null)
                {
                    statisticsAnalyzer = new StatisticsAnalyzerClient(DefaultBinding, new EndpointAddress(GetServiceURL("StatisticsAnalyzer")));
                }

                return statisticsAnalyzer;
            }
        }

        private static CampaignClient campaign;
        /// <summary>
        /// Proxy de servicio web para campañas
        /// </summary>
        public static CampaignClient Campaign
        {
            get
            {
                if (campaign == null)
                {
                    campaign = new CampaignClient(DefaultBinding, new EndpointAddress(GetServiceURL("Campaign")));
                }

                return campaign;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        private Services()
        {
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Genera una URL usando el nombre de un servicio
        /// </summary>
        /// <param name="serviceName">El nombre del servicio</param>
        /// <returns>Una URL</returns>
        public static string GetServiceURL(string serviceName)
        {
            return "http://" + server + ":" + port + "/" + serviceName;
        }

        #endregion
    }
}
