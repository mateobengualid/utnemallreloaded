using System;
using UtnEmall.Client.EntityModel;
using System.IO;
using UtnEmall.Client.ServiceAccessLayer;
using System.ServiceModel;
using System.Reflection;
using System.Collections.Generic;
using UtnEmall.Client.BusinessLogic;
using System.Windows.Forms;
using UtnEmall.Client.BackgroundBroadcastService;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;
using UtnEmall.Client.SmartClientLayer;
using UtnEmall.Client.DataModel;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UtnEmall.Client.PresentationLayer
{
    public class UtnEmallClientApplication
    {
        private CustomerEntity customer;
        private PrincipalForm mainMenu;
        private int portServer;
        private int pingTime;
        private string ipServer;
        private static UtnEmallClientApplication singleton;

        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_ACCESS_DENIED = 5;
        
        public const string AssembliesFolder = ".";
        const string RemovedServiceListFilename = "removedservices.xml";

        int[] removedServiceList;
        static Dictionary<int, BaseForm> servicesForms = new Dictionary<int, BaseForm>();

        /// <summary>
        /// Devuelve la lista de servicios eliminados
        /// </summary>
        /// <returns>Una matriz de enteros con los Id de los servicios eliminados</returns>
        public int[] GetRemovedServiceList()
        {
            if (removedServiceList == null)
            {
                this.LoadRemovedServicesList();
            }
            return this.removedServiceList;
        }

        /// <summary>
        /// Obtiene o establece el cliente logueado
        /// </summary>
        public CustomerEntity Customer
        {
            get 
            { 
                return customer; 
            }
            set 
            { 
                customer = value; 
            }
        }

        /// <summary>
        /// Retorna si la aplicación esta en línea con el server
        /// </summary>
        public static bool IsOnline
        {
            get 
            { 
                return UtnEmall.Client.SmartClientLayer.Connection.IsConnected; 
            }
        }

        /// <summary>
        /// Constructor: es una clase singleton
        /// </summary>
        private UtnEmallClientApplication()
        {
            CheckAssembliesFolder();
        }

        /// <summary>
        /// Controla si la carpeta para los assemblies existe, sino la crea
        /// </summary>
        private static void CheckAssembliesFolder()
        {
            string path = Path.Combine(Utilities.AppPath, AssembliesFolder);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Transfiere un archivo desde el servidor al dispositivo.
        /// </summary>
        /// <param name="fileName">
        /// El nombre del archivo a transferrir
        /// </param>
        public static void TransferFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.FileNameNullArgument);
            }

            LastSyncDataAccess syncDataAccess = new LastSyncDataAccess();

            // Crear el servicio de transferencia
            FileTransferServiceClient fileTransfer = new
                FileTransferServiceClient(BaseForm.ServerBinding,
                 new EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "FileTransferService"));

            // Cargar la información de sincronización
            Collection<LastSyncEntity> results = syncDataAccess.LoadWhere("entityName", fileName.Substring(fileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1), false, OperatorType.Equal);

            DownloadRequest request = new DownloadRequest();
            request.FileName = Path.GetFileName(fileName);

            if (results.Count > 0)
            {
                request.FileDateTime = results[0].LastTimestamp.Ticks.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                request.FileDateTime = DateTime.MinValue.Ticks.ToString(CultureInfo.InvariantCulture);
            }

            if (!File.Exists(Path.Combine(Utilities.AppPath, fileName)))
            {
                request.FileDateTime = DateTime.MinValue.Ticks.ToString(CultureInfo.InvariantCulture);
            }

            // Realizar la transferencia
            RemoteFileInfo returnInfo = null;
            try
            {
                returnInfo = fileTransfer.DownloadFile(request, UtnEmall.Client.SmartClientLayer.Connection.Session);
            }
            catch (TargetInvocationException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.TargetInvocationExceptionMessage,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;

            }
            catch (CommunicationException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.CommunicationException,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;

            }
            FileStream writer = null;

            if (returnInfo != null)
            {
                fileName = Path.Combine(Utilities.AppPath, fileName);
                // Guardar el archivo a disco
                try
                {
                    writer = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    writer.Write(returnInfo.FileByteStream, 0, returnInfo.FileByteStream.Length);

                    // Guardar la información para futuras sincronizaciones
                    LastSyncEntity fileSync = new LastSyncEntity();
                    fileSync.EntityName = returnInfo.FileName;
                    fileSync.LastTimestamp = new DateTime(Convert.ToInt64(returnInfo.FileDateTime, CultureInfo.InvariantCulture));
                    if (File.Exists(fileName) && results.Count > 0)
                    {
                        fileSync.Id = results[0].Id;
                        fileSync.IsNew = false;
                        fileSync.Changed = true;
                    }
                    syncDataAccess.Save(fileSync);
                }
                finally
                {
                    if (writer != null) writer.Close();
                }
            }
        }

        /// <summary>
        /// Éste metodo valida si el cliente existe y retorna la entidad representando el cliente
        /// </summary>
        /// <param name="userName">
        /// Nombre de usuario del cliente
        /// </param>
        /// <param name="password">
        /// Clave del cliente
        /// </param>
        /// <returns>La entidad del Cliente</returns>
        public static CustomerEntity ValidateAndGetCustomer(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.UserNameEmpty);
            }

            Collection<CustomerEntity> customers = new Collection<CustomerEntity>();

            SessionManagerClient sessionManagerClient = new SessionManagerClient(SessionManagerClient.CreateDefaultBinding(), new EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "SessionManager"));
            string session = sessionManagerClient.ValidateCustomer(userName, Utilities.CalculateHashString(password));

            CustomerClient customerClient = new CustomerClient(CustomerClient.CreateDefaultBinding(), new EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "Customer"));
            customers = customerClient.GetCustomerWhereEqual(CustomerEntity.DBUserName, userName, true, session);

            if (customers.Count == 0)
            {
                throw new UtnEmallBusinessLogicException(
                    global::PresentationLayer.GeneralResources.LoginFailedUserIncorrect);
            }

            string passwordHash = Utilities.CalculateHashString(password);
            CustomerEntity customer = customers[0];
            if (String.Compare(customer.Password, passwordHash, StringComparison.Ordinal) != 0)
            {
                throw new UtnEmallBusinessLogicException(
                    global::PresentationLayer.GeneralResources.LoginFailedPasswordIncorrect);
            }

            return customer;
        }

        /// <summary>
        /// Guarda un cliente a la base local.
        /// </summary>
        /// <param name="customer">
        /// La entidad a guardar en la base local
        /// </param>
        public static void SaveCustomerToLocalDatabase(CustomerEntity customer)
        {
            if (customer == null)
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.CustomerNullArgument);
            }
            Customer customerBusiness = new Customer();

            // Obtener todos los clientes, aún cuando nunca sea retorne más de uno
            Collection<CustomerEntity> localCustomers = customerBusiness.GetAllCustomer();
            foreach (CustomerEntity localCustomer in localCustomers)
            {
                customerBusiness.Delete(localCustomer);
            }

            customerBusiness.Save(customer);

            UtnEmallClientApplication.Instance.Customer = customer;

            UtnEmallClientApplication.Instance.ReconfigureBackgroundBroadcastService();
        }

        /// <summary>
        /// Controla si existe un cliente registrado en la base local
        /// </summary>
        private bool CheckCustomerConfiguration()
        {
            Customer customerBusiness = new Customer();
            Collection<CustomerEntity> customers = customerBusiness.GetAllCustomer(true);
            if (customers.Count > 0)
            {
                Customer = customers[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Aborta la ejecución del hilo que corre el broadcast service
        /// </summary>
        public static void KillBackgroundService()
        {
            if(BackgroundBroadcastService.BackgroundBroadcast.Instance!=null)
                BackgroundBroadcastService.BackgroundBroadcast.Instance.Terminate();
        }

        /// <summary>
        /// Obtiene la instancia para UtnEmallClientApplication
        /// </summary>
        /// <returns>La instancia de UtnEmallClientApplication.</returns>
        public static UtnEmallClientApplication Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new UtnEmallClientApplication();
                }

                return singleton;
            }
        }

        /// <summary>
        /// Ejecuta un servicio en particular.
        /// </summary>
        /// <param name="serviceName">El nombre del servicio.</param>
        /// <param name="serviceArgument">Argumento que puede ser requerido para la ejecución del servicio.</param>
        /// <param name="className">Nombre de la clase del servicio</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We need to capture all errors while trying to run a dynamic service.")]
        public void RunService(ServiceEntity service, string serviceArgument, string className)
        {
            if (servicesForms.ContainsKey(service.Id))
            {
                servicesForms[service.Id].ShowDialog();
                return;
            }

            string path = Path.Combine(Utilities.AppPath,
            Path.Combine(UtnEmallClientApplication.AssembliesFolder, Path.GetFileNameWithoutExtension(service.RelativePathAssembly) + ".dll"));

            if (File.Exists(path))
            {
                AssemblyName assemblyName = new AssemblyName();
                assemblyName.CodeBase = Path.GetDirectoryName(path);
                assemblyName.Name = Path.GetFileNameWithoutExtension(path);
                assemblyName.Version = new Version(0, 0, 0, 0);
                // Cargar el assembly especificado
                Assembly dll = Assembly.Load(assemblyName);

                BaseForm serviceForm = null;
                BaseForm.ServerRemoteAddress = new EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "/" + Path.GetFileNameWithoutExtension(service.PathAssemblyServer));

                // Buscar el tipo del primer formulario
                foreach (Type type in dll.GetTypes())
                {
                    if (!type.Name.Contains("CustomService") && !type.Name.EndsWith("UserControl", StringComparison.Ordinal))
                    {
                        if (((bool)type.GetProperty("IsStartForm", BindingFlags.Public | BindingFlags.Static).GetValue(null, null)) == true)
                        {
                            serviceForm = (BaseForm)(type.GetConstructor(new Type[] { })).Invoke(null);
                            break;
                        }
                        else
                        {
                            serviceForm = null;
                        }
                    }
                }
                // Abrir el primer formulario del servicio
                if (serviceForm != null)
                {
                    servicesForms.Add(service.Id, serviceForm);
                    serviceForm.Owner = mainMenu;
                    serviceForm.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Itenta obtener la configuración del usuario (cliente) y pregunta por uno nuevo si no encuentra configuración previa
        /// </summary>
        /// <returns>True si el usuario se configuro exitosamente</returns>
        public bool ConfigureCustomer()
        {
            bool existCustomerConfiguration = CheckCustomerConfiguration();

            if (!existCustomerConfiguration)
            {
                try
                {
                    SessionManagerClient sessionManagerClient = new SessionManagerClient(SessionManagerClient.CreateDefaultBinding(), new EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "SessionManager"));
                    sessionManagerClient.ValidateCustomer("", "");

                    CustomerConfigurationForm customerConfigurationForm = new CustomerConfigurationForm();
                    customerConfigurationForm.Owner = mainMenu;
                    customerConfigurationForm.ShowDialog();
                }
                catch (TargetInvocationException invocationException)
                {
                    Debug.WriteLine(invocationException.Message);
                    HelpForm helpForm = new HelpForm(global::PresentationLayer.GeneralResources.ServerNotFoundHelp);
                    helpForm.ShowDialog();
                    return false;
                }
                catch (CommunicationException communicationError)
                {
                    Debug.WriteLine(communicationError.Message);
                    HelpForm helpForm = new HelpForm(global::PresentationLayer.GeneralResources.ServerNotFoundHelp);
                    helpForm.ShowDialog();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Elimina el hilo del servicio de broadcast y lo ejecuta nuevamente.
        /// </summary>
        public void ReconfigureBackgroundBroadcastService()
        {
            if (BackgroundBroadcastService.BackgroundBroadcast.Instance != null)
            {
                KillBackgroundService();
                ReadConfigFileMallServer();
                UtnEmall.Client.SmartClientLayer.Connection.ServerUri = new Uri("http://" + ipServer + ":8081");
                BackgroundBroadcast.Run(ipServer, portServer, pingTime, Customer);
                BackgroundBroadcast.Instance.ConnectionStateChanged += new EventHandler(Instance_ConnectionStateChanged);
            }
        }

        /// <summary>
        /// Crea el archivo de configuración XML con la información del servidor
        /// </summary>
        public static void CreateMallServerFile(string ipServer, string portServer, string pingTime)
        {
            if (String.IsNullOrEmpty(ipServer))
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.IpNullArgument);
            }

            if (String.IsNullOrEmpty(portServer))
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.PortNullArgument);
            }

            if (String.IsNullOrEmpty(pingTime))
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.PingNullArgument);
            }

            string path = Path.Combine(Utilities.AppPath, "MallServer.xml");
            XmlTextWriter mallXmlWriter = null;

            try
            {
                mallXmlWriter = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                mallXmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                mallXmlWriter.WriteComment("This file specifies the source & catalog for the connection string");
                mallXmlWriter.WriteStartElement("mallserver");
                mallXmlWriter.WriteElementString("ipServer", ipServer);
                mallXmlWriter.WriteElementString("portServer", portServer);
                mallXmlWriter.WriteElementString("pingTime", pingTime);
                mallXmlWriter.WriteEndElement();
                // Escribe el archivo XML
                mallXmlWriter.Flush();
            }
            finally
            {
                if(mallXmlWriter!=null){
                    mallXmlWriter.Close();
                }
            }
        }

        /// <summary>
        /// Lee la configuración del servidor desde el archivo XML de configuración.
        /// Si no existe muestra un formulario para solicitar la información del servidor.
        /// </summary>
        public void ConfigureMall()
        {
            bool existMallServerConfiguration = ReadConfigFileMallServer();
            if (!existMallServerConfiguration)
            {
                MallConfigurationForm mallConfigurationForm = new MallConfigurationForm();
                mallConfigurationForm.Owner = mainMenu;
                mallConfigurationForm.ShowDialog();
                ReadConfigFileMallServer();
            }

            UtnEmall.Client.SmartClientLayer.Connection.ServerUri = new Uri("http://" + ipServer + ":8081");
        }

        /// <summary>
        /// Inicia UtnEmallClientApplication.
        /// </summary>
        internal void Run()
        {
            try
            {
                mainMenu = new PrincipalForm();

                ConfigureMall();
                ConfigureCustomer();

                if (Customer != null)
                {
                    // Inicializa el servicio de broadcast
                    BackgroundBroadcast.Run(ipServer, portServer, pingTime, Customer);

                    // configura eventos para la conexión/desconexión y la comunicación de estadísticas
                    BackgroundBroadcast.Instance.ConnectionStateChanged += new EventHandler(Instance_ConnectionStateChanged);
                    BackgroundBroadcast.Instance.ConnectionStateChanged += new EventHandler(new StatisticsDataRecollection().SendCollectedStatistics);

                    Application.Run(mainMenu);
                    PersistRemovedServicesList();
                }
            }
            finally
            {
                if (BackgroundBroadcast.Instance != null)
                    BackgroundBroadcast.Instance.Dispose();
            }
        }


        void Instance_ConnectionStateChanged(object sender, EventArgs e)
        {
            mainMenu.Invoke(new EventHandler(this.UpdateTitle));
            Thread updateProfieThread = new Thread(new ThreadStart(this.SynchronizeProfile));
            updateProfieThread.Start();
        }

        /// <summary>
        /// Si el perfil local es más nuevo que el remoto la funcion lo actualiza en el servidor
        /// update it on the server.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Catch all exceptions to prevent application crash because this is called on a background thread.")]
        private void SynchronizeProfile()
        {
            try
            {
                // Si esta conectado
                if (Connection.IsConnected)
                {
                    // Obtener el perfil remoto
                    CustomerClient customerClient = new CustomerClient(Connection.ServerBinding, new EndpointAddress(Connection.ServerUri + "Customer"));
                    CustomerEntity remoteCustomer = customerClient.GetCustomer(this.Customer.Id, true, Connection.Session);
                    // Y el perfil local
                    CustomerDataAccess customerDataAccess = new CustomerDataAccess();
                    CustomerEntity localCustomer = customerDataAccess.Load(this.Customer.Id, true);
                    // Si el perfil local es más nuevo que el remoto
                    if (localCustomer.Timestamp > remoteCustomer.Timestamp)
                    {
                        // Establecer los campos comunes
                        remoteCustomer.Name = localCustomer.Name;
                        remoteCustomer.PhoneNumber = localCustomer.PhoneNumber;
                        remoteCustomer.Surname = localCustomer.Surname;
                        remoteCustomer.UserName = localCustomer.UserName;
                        remoteCustomer.Address = localCustomer.Address;
                        // Establecer las preferencias
                        foreach (PreferenceEntity remotePreference in remoteCustomer.Preferences)
                        {
                            foreach (PreferenceEntity localPreference in localCustomer.Preferences)
                            {
                                if (remotePreference.IdCategory == localPreference.IdCategory)
                                {
                                    remotePreference.Active = localPreference.Active;
                                }
                            }
                        }

                        CustomerEntity returnCustomer = customerClient.Save(remoteCustomer, Connection.Session);
                        if (returnCustomer != null)
                        {
                            Debug.WriteLine("Internal application error saving remote customer.");
                        }
                        else
                        {
                            Debug.WriteLine("Remote customer updated.");
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error synchronizing profile. " + error.Message);
            }
        }

        private void UpdateTitle(object sender, EventArgs args)
        {
            mainMenu.Text = global::PresentationLayer.GeneralResources.OnlineMessage;

            if (!UtnEmall.Client.SmartClientLayer.Connection.IsConnected)
            {
                mainMenu.Text = global::PresentationLayer.GeneralResources.OfflineMessage;
            }
        }

        /// <summary>
        /// Lee la configuración del servidor desde el archivo XML
        /// </summary>
        public bool ReadConfigFileMallServer()
        {
            string path = Path.Combine(Utilities.AppPath, "MallServer.xml");

            bool error = false;
            if (File.Exists(path))
            {
                XmlReader reader = null;
                try
                {
                    reader = new XmlTextReader(new StreamReader(path));
                    reader.ReadStartElement("mallserver");
                    reader.ReadStartElement("ipServer");
                    ipServer = reader.ReadString();

                    if (String.IsNullOrEmpty(ipServer))
                    {
                        error = true;
                    }
                    reader.ReadEndElement();

                    reader.ReadStartElement("portServer");
                    portServer = reader.ReadContentAsInt();
                    reader.ReadEndElement();

                    reader.ReadStartElement("pingTime");
                    pingTime = reader.ReadContentAsInt();
                    reader.ReadEndElement();

                    reader.ReadEndElement();
                }
                catch (IOException)
                {
                    error = true;
                }
                catch (FormatException)
                {
                    error = true;
                }
                catch (XmlException)
                {
                    error = true;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            else
            {
                error = true;
            }
            // Si hace falta configurar el servidor
            if (error)
            {
                MallConfigurationForm mallConfigurationForm = new MallConfigurationForm();
                mallConfigurationForm.Owner = mainMenu;
                mallConfigurationForm.ShowDialog();
                return ReadConfigFileMallServer();
            }
            return true;
        }

        /// <summary>
        /// Actualiza el perfil local y remoto del cliente.
        /// </summary>
        /// <param name="customer">Datos del cliente/usuario</param>
        /// <returns>Perfil del cliente como es validado por los metodos para guardarlo</returns>
        internal static CustomerEntity UpdateCustomerProfile(CustomerEntity customer)
        {
            CustomerEntity returnCustomer;
            if (UtnEmallClientApplication.IsOnline)
            {
                // Guardar el cliente remotamente si se esta en línea
                CustomerClient customerClient = new CustomerClient(ServiceClient.CreateDefaultBinding(), new System.ServiceModel.EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "Customer"));
                returnCustomer = customerClient.Save(customer, UtnEmall.Client.SmartClientLayer.Connection.Session);
                if (returnCustomer != null)
                {
                    Debug.WriteLine("Internal application error saving remote customer.");
                }
            }
            // Siempre actualizar en la base local
            returnCustomer = new BusinessLogic.Customer().Save(customer);
            if (returnCustomer != null)
            {
                Debug.WriteLine("Internal application error saving local customer.");
            }
            return returnCustomer;
        }

        /// <summary>
        /// Vuelve a cargar la información del desde el servidor.
        /// No controla si se esta conectado ni captura excepciones.
        /// </summary>
        internal void ReloadCustomer()
        {
            CustomerClient customerClient = new CustomerClient(Connection.ServerBinding, new System.ServiceModel.EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "Customer"));
            this.Customer = customerClient.GetCustomer(this.Customer.Id, true, Connection.Session);
        }

        /// <summary>
        /// Borrar la lista de servicios eliminados
        /// </summary>
        public void ClearRemovedServices()
        {
            removedServiceList = new int[1];
        }
        /// <summary>
        /// Agrega un servicio a la lista de eliminados
        /// </summary>
        /// <param name="serviceId">Identificador del Servicio</param>
        public void AddRemovedService(int serviceId)
        {
            if (removedServiceList == null)
            {
                removedServiceList = new int[1];
                removedServiceList[0] = serviceId;
            }
            else
            {
                int[] newList = new int[removedServiceList.Length + 1];
                removedServiceList.CopyTo(newList, 0);
                newList[removedServiceList.Length] = serviceId;
                removedServiceList = newList;
            }
        }

        /// <summary>
        /// Persistir la lista de servicios eliminados en el disco
        /// </summary>
        void PersistRemovedServicesList()
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(RemovedServiceListFilename);
                XmlSerializer serializer = new XmlSerializer(typeof(int[]));
                serializer.Serialize(writer, removedServiceList);
            }
            catch (InvalidCastException)
            {
                Debug.WriteLine("InvalidCastException writting removed service list.");
            }
            catch (InvalidOperationException)
            {
                Debug.WriteLine("InvalidOperationException writting removed service list.");
            }
            finally
            {
                if (writer!= null)
                {
                    writer.Close();
                }
            }
        }
        /// <summary>
        /// Cargar la lista de servicios eliminados desde el disco
        /// </summary>
        void LoadRemovedServicesList()
        {
            removedServiceList = new int[0];
            if (!File.Exists(RemovedServiceListFilename))
            {
                return;
            }
            TextReader reader = null;
            try
            {
                reader = new StreamReader(RemovedServiceListFilename);
                XmlSerializer serializer = new XmlSerializer(typeof(int[]));
                int[] tempList = (int[])serializer.Deserialize(reader);
                removedServiceList = tempList;
            }
            catch (InvalidCastException)
            {
                Debug.WriteLine("InvalidCastException loading removed service list.");
            }
            catch (InvalidOperationException)
            {
                Debug.WriteLine("InvalidOperationException loading removed service list.");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}

