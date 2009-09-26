using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using UtnEmall.Server.Base;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.ServiceCompiler;
using System.Windows.Controls;
using WpfCore;
using UtnEmall.Server.Core;
using System.Data.Common;
using System.Security.Permissions;
using System.Configuration;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Representa el servidor de UTN Emall 
    ///
    /// Inicializa los servicios web
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", 
        "CA1506:AvoidExcessiveClassCoupling", Justification = "This class centralize the initialization and termination of WCF services.")]
    public class ServerHost
    {
        // Nombre de servidor y puerto para pubicar servicios
        private string serverName;
        private int serverPort = 8081;
        // Bandera que indica si el método Run se ha invocado
        private bool running;
        // Referencia a instancia de ServerHost
        private static ServerHost instance;
        private static string serverSession;

        private Dictionary<string, ServiceHost> hostedServices;

        public const string CleanAssemblyFolderKey = "clean_assembly_folder";

        /// <summary>
        /// Implementa el patrón Singleton
        /// </summary>
        private ServerHost()
        {
            serverName = Dns.GetHostName();
            hostedServices = new Dictionary<string, ServiceHost>();
        }

        /// <summary>
        /// Obtiene una instancia de la clase ServerHost
        /// </summary>
        /// <returns>Una instancia de ServerHost.</returns>
        public static ServerHost Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerHost();
                }

                return instance;
            }
        }

        /// <summary>
        /// Inicia el ServerHost
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand)]
        public void Run()
        {
            SessionManager sessionManager = SessionManager.Instance;
            sessionManager.Run();

            ProfileUpdater profileUpdater = ProfileUpdater.Instance;
            profileUpdater.Run();

            lock (this)
            {
                // Verifica si el método Run ha sido invocado
                if (running)
                {
                    return;
                }
                else
                {
                    running = true;
                }
                // Crea una colección de servicios
                hostedServices = new Dictionary<string, ServiceHost>();
            }

            ConsoleWriter.SetText("UtnEmall Server Running...");
            ConsoleWriter.SetText("Session Manager Starting...");

            // Limpia la carpeta de ensamblados si es necesario
            CheckForClean();

            // Inicializa el administrador de sesión
            InitSessionManager();

            // Inicialización de servicios
            InitServices();
            ConsoleWriter.SetText("Discovery Service Starting...");            
        }

        /// <summary>
        /// Limpiar la carpeta de ensamblados cuando el servidor de configuración cambie
        /// a otra base de datos
        /// </summary>
        private static void CheckForClean()
        {
            try
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = "wpfcore.config";
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[ServerHost.CleanAssemblyFolderKey] != null)
                {
                    // Borrar todos los archivos en la carpeta de ensamblados dinámicos
                    string[] filesToDelete = Directory.GetFiles(Path.GetFullPath(ServiceBuilder.AssembliesFolder), "*.*", SearchOption.TopDirectoryOnly);
                    foreach (string fileToDelete in filesToDelete)
                    {
                        if (Path.GetFileName(fileToDelete).StartsWith("CustomService", StringComparison.Ordinal) || Path.GetFileName(fileToDelete).StartsWith("Store", StringComparison.Ordinal))
                        {
                            try
                            {
                                Debug.WriteLine("Deleting file: {0}", fileToDelete);
                                File.Delete(fileToDelete);
                            }
                            catch (UnauthorizedAccessException unauthorizedError)
                            {
                                Debug.WriteLine("Error cleaning assemblies: {0}", unauthorizedError.Message);
                            }
                            catch (IOException ioError)
                            {
                                Debug.WriteLine("Error cleaning assemblies: {0}", ioError.Message);
                            }
                        }
                    }
                    config.AppSettings.Settings.Remove(CleanAssemblyFolderKey);
                    config.Save();
                    // Verificar si la base de datos actual tiene servicios que compilar
                    DataModelDataAccess dataModelDataAccess = new DataModelDataAccess();
                    Collection<DataModelEntity> dataModels = dataModelDataAccess.LoadWhere(DataModelEntity.DBDeployed, true, false, OperatorType.Equal);
                    if (dataModels.Count > 0)
                    {
                        System.Windows.MessageBox.Show(Resources.RecompilationOfServices, UtnEmall.Server.WpfCore.Resources.InformationTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);                        
                    }

                }
            }
            catch (UtnEmallDataAccessException dbError)
            {
                Debug.WriteLine("Error cleaning assemblies: {0}", dbError.Message);
            }
            catch (UnauthorizedAccessException unauthorizedError)
            {
                Debug.WriteLine("Error cleaning assemblies: {0}", unauthorizedError.Message);
            }
            catch (DirectoryNotFoundException directoryNotFoundError)
            {
                Debug.WriteLine("Error cleaning assemblies: {0}", directoryNotFoundError.Message);
            }
            catch (IOException ioError)
            {
                Debug.WriteLine("Error cleaning assemblies: {0}", ioError.Message);
            }
        }

        /// <summary>
        /// Verificar la configuración de acceso a datos
        /// </summary>
        public static void CheckDataAccess()
        {
            try
            {
                // Intenta abrir una conexión con una base de datos
                IDbConnection dbConnection = DataAccessConnection.Instance.GetNewConnection();
                ConsoleWriter.ConfigurationServer(DataAccessConnection.Instance.Source, DataAccessConnection.Instance.Catalog);
                dbConnection.Open();
            }
            catch (DbException dbException)
            {
                // Si hay un error, avisar al usuario que cambie la configuración
                ConsoleWriter.SetText("The data base doesn't exists or the server name is incorrect");
                throw new UtnEmallDataAccessException("The data base doesn't exists or the server name is incorrect", dbException.InnerException);
            }
            catch (Exception exception)
            {
                ConsoleWriter.SetText("The data base doesn't exists or the server name is incorrect");
                throw new UtnEmallDataAccessException("The data base doesn't exists or the server name is incorrect", exception.InnerException);
            }
        }

        /// <summary>
        /// Cerrar todos los servicios WCF
        /// </summary>
        private void CloseServices()
        {
            lock (hostedServices)
            {
                foreach (ServiceHost service in hostedServices.Values)
                {
                    service.Close();
                    ConsoleWriter.SetText("Service " + service.BaseAddresses[0].AbsoluteUri + " closed.");
                }
            }
        }

        /// <summary>
        /// Cerrar todos los servicios y detener los procesos deamons
        /// </summary>
        public void Close()
        {            
            CloseServices();

            // Actualizar la bandera
            lock (this)
            {
                running = false;
            }     
        }

        /// <summary>
        /// Invocar a la inicilización de servicios web WCF
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", 
            "CA1506:AvoidExcessiveClassCoupling", Justification = "This method call for the initialization of all WCF main application services")]
        [PermissionSet(SecurityAction.LinkDemand)]
        private void InitServices()
        {
            lock (hostedServices)
            {
                // Datos de servicio de cliente
                InitService(typeof(ICustomerServiceData), typeof(CustomerServiceData));
                // categoría
                InitService(typeof(ICategory), typeof(Category));
                // Asociación de categoría
                InitService(typeof(ICategoryAssociation), typeof(CategoryAssociation));
                // cliente
                InitService(typeof(ICustomer), typeof(Customer));
                // modelo de datos
                InitService(typeof(UtnEmall.Server.BusinessLogic.IDataModel), typeof(UtnEmall.Server.BusinessLogic.DataModel));
                // transferencia de archivo
                InitService(typeof(IFileTransferService), typeof(FileTransferService));
                // grupo
                InitService(typeof(IGroup), typeof(Group));
                // registrar asociación
                InitService(typeof(IRegisterAssociation), typeof(RegisterAssociation));
                // servicio
                InitService(typeof(IService), typeof(Service));
                // constructor de servicios
                InitService(typeof(IServiceBuilder), typeof(ServiceBuilder));
                // analizador de estadísticas
                InitService(typeof(IStatisticsAnalyzer), typeof(StatisticsAnalyzer));
                // recolección de estadísticas
                InitService(typeof(IStatisticsDataRecollection), typeof(StatisticsDataRecollection));                
                // tienda
                InitService(typeof(IStore), typeof(Store));
                // usuario
                InitService(typeof(IUser), typeof(User));
                // acción de usuario
                InitService(typeof(IUserAction), typeof(UserAction));
                // inicializar servicios de infraestructura
                InitInfrastructureServices();
                // inicializar servicios
                InitCustomServices();
            }
        }

        /// <summary>
        /// Inicializa los servicios de cliente existentes.
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand)]
        private void InitCustomServices()
        {
            try
            {
                // Cargar los modelos de datos y verificar los ensamblados
                ServiceDataAccess serviceDataAccess = new ServiceDataAccess();
                Collection<ServiceEntity> services = serviceDataAccess.LoadAll(true);

                foreach (ServiceEntity service in services)
                {
                    string assemblyFileName = service.PathAssemblyServer;
                    if (assemblyFileName != null)
                    {
                        if (File.Exists(Path.Combine(ServiceBuilder.AssembliesFolder, assemblyFileName)))
                        {
                            Type[] servicesTypes = ServiceBuilder.GetCustomServiceTypes(assemblyFileName);
                            Binding serviceBinding = new BasicHttpBinding();
                            if (PublishCustomService(servicesTypes[0], servicesTypes[1], serviceBinding))
                            {
                                Debug.WriteLine("SUCCESS : custom service published.");
                            }
                            else
                            {
                                Debug.WriteLine("FAILURE : trying to publish custom service.");
                            }
                        }
                        else 
                        {
                            CustomerServiceDataDataAccess customerServiceData = new CustomerServiceDataDataAccess();
                            CustomerServiceDataEntity customerService = customerServiceData.Load(service.IdCustomerServiceData, true);
                            ServiceBuilder builder = new ServiceBuilder();
                            service.Deployed = false;
                            customerService.Service = service;
                            builder.BuildAndImplementCustomService(customerService, serverSession);
                        }
                    }
                }
            }
            catch (DataException dataError)
            {                
                Debug.WriteLine("ERROR : Data exception running infrastructure services. MESSAGE : " + dataError.Message);
            }
            catch (IOException ioError)
            {
                Debug.WriteLine("ERROR : IO error running infrastructure services. MESSAGE : " + ioError.Message);
            }
        }

        /// <summary>
        /// Inicializar los servicios WCF desde el administrador de sesiones.
        /// </summary>
        /// <returns>true si se inicializa con éxito.</returns>
        private bool InitSessionManager()
        {
            Uri httpBaseAddress;
            string uriString;

            uriString = "http://" + serverName + ":" + serverPort.ToString(CultureInfo.InvariantCulture) + "/SessionManager";
            httpBaseAddress = new Uri(uriString);

            UserDataAccess userDataAccess = new UserDataAccess();

            if (userDataAccess.LoadAll(false).Count == 0)
            {
                GroupDataAccess groupDataAccess = new GroupDataAccess();
                UserEntity user = new UserEntity();
                GroupEntity group = new GroupEntity();
                string[] bussinesClasses = { "Category", "CustomerServiceData", "Customer", "DataModel", "Group", "Permission", "RegisterAssociation", "Service", "Store", "User", "UserAction" };

                user.Name = "admin";
                user.Password = Utilities.CalculateHashString("admin");
                user.Charge = "admin";
                user.IsUserActive = true;
                user.PhoneNumber = "0";
                user.Surname = "admin";
                user.UserName = "admin";

                // Crear un grupo de administradores
                group.Name = "admin";
                group.Description = "admin group";
                group.IsGroupActive = true;
                group.Permissions = new Collection<PermissionEntity>();

                foreach (String name in bussinesClasses)
                {
                    PermissionEntity permission = new PermissionEntity();
                    permission.BusinessClassName = name;
                    permission.AllowDelete = true;
                    permission.AllowNew = true;
                    permission.AllowRead = true;
                    permission.AllowUpdate = true;

                    group.Permissions.Add(permission);
                }

                UserGroupEntity userGroupEntity = new UserGroupEntity();
                Collection<UserGroupEntity> userGroupEntities = new Collection<UserGroupEntity>();
                userGroupEntity.Group = group;

                groupDataAccess.Save(group);

                userGroupEntities.Add(userGroupEntity);
                user.UserGroup = userGroupEntities;

                userDataAccess.Save(user);
            }

            try
            {
                ServiceHost host = new ServiceHost(SessionManager.Instance, httpBaseAddress);
                BasicHttpBinding binding = new BasicHttpBinding();
                
                host.AddServiceEndpoint(typeof(ISessionManager), binding, httpBaseAddress);

                ValidationService.Instance.SessionManager = SessionManager.Instance;
                // Habilitar conexión
                if (!host.Description.Behaviors.Contains(typeof(ServiceMetadataBehavior)))
                {
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    host.Description.Behaviors.Add(smb);
                }

                // Abrir servicio
                host.Open();
                ConsoleWriter.SetText(httpBaseAddress.AbsoluteUri + " WS Initialized...");
                Debug.WriteLine(httpBaseAddress.AbsoluteUri + " WS Initialized...");
                
                lock (hostedServices)
                {
                    hostedServices.Add("SessionManager", host);
                }

                serverSession = SessionManager.Instance.ValidateUser("admin", Utilities.CalculateHashString("admin"));

                return true;
            }
            catch (InvalidOperationException initError)
            {
                ConsoleWriter.SetText("Error initializing " + httpBaseAddress.AbsoluteUri + " service. ERROR : " + initError.Message);
            }
            catch (CommunicationException initError)
            {
                ConsoleWriter.SetText("Error initializing " + httpBaseAddress.AbsoluteUri + " service. ERROR : " + initError.Message);
            }
            return false;
        }

        /// <summary>
        /// Inicializar servicios de infraestructura
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand)]
        private void InitInfrastructureServices()
        {
            try
            {
                // Cargar todos los modelos de datos y verificar ensamblados
                DataModelDataAccess dataModelDataAccess = new DataModelDataAccess();
                Collection<DataModelEntity> result = dataModelDataAccess.LoadWhere(DataModelEntity.DBIdStore, "0", false, OperatorType.Equal);
                dataModelDataAccess = new DataModelDataAccess();
                Collection<DataModelEntity> dataModels = dataModelDataAccess.LoadAll(true);

                if (result.Count == 0)
                {
                    ConsoleWriter.SetText("Creating mall data model");
                    DataModelEntity dataModel = new DataModelEntity();

                    MallEntity mall = new MallEntity();
                    mall.MallName = "Mall";

                    (new MallDataAccess()).Save(mall);
                    dataModel.Mall = mall;

                    dataModelDataAccess.Save(dataModel);
                }

                foreach (DataModelEntity dataModel in dataModels)
                {
                    string assemblyFileName = dataModel.ServiceAssemblyFileName;
                    if (assemblyFileName != null)
                    {
                        if (File.Exists(Path.Combine(ServiceBuilder.AssembliesFolder, assemblyFileName)))
                        {
                            Type[] servicesTypes = ServiceBuilder.GetInfrastructureServiceTypes(assemblyFileName);
                            Binding serviceBinding = new BasicHttpBinding();
                            if (PublishInfrastructureService(servicesTypes[0], servicesTypes[1], serviceBinding))
                            {
                                Debug.WriteLine("SUCCESS : infrastructure service published.");
                            }
                            else
                            {
                                Debug.WriteLine("FAILURE : trying to publish infrastructure service.");
                            }
                        }
                        else 
                        {
                            ServiceBuilder builder = new ServiceBuilder();
                            dataModel.Deployed = false;
                            builder.BuildAndImplementInfrastructureService(dataModel, false, serverSession);
                        }
                    }
                }
            }
            catch (DataException dataError)
            {
                Debug.WriteLine("ERROR : Data exception running infrastructure services. MESSAGE : " + dataError.Message);
            }
            catch (IOException ioError)
            {
                Debug.WriteLine("ERROR : IO error running infrastructure services. MESSAGE : " + ioError.Message);
            }
        }

        /// <summary>
        /// Inicializar los servicios WCF desde el tipo de contrato y tipo de servicio.
        /// </summary>
        /// <param name="serviceContractType">El tipo de contrato del servicio.</param>
        /// <param name="serviceType">El tipo que implementa el contrato de servicio.</param>
        private bool InitService(Type serviceContractType, Type serviceType)
        {
            Uri httpBaseAddress;
            string uriString;

            uriString = "http://" + serverName + ":" + serverPort.ToString(CultureInfo.InvariantCulture) + "/" + serviceType.Name;
            httpBaseAddress = new Uri(uriString);

            try
            {
                ServiceHost host = new ServiceHost(serviceType, httpBaseAddress);
                BasicHttpBinding binding = new BasicHttpBinding();
                
                binding.MaxReceivedMessageSize = 1048576;
                binding.ReaderQuotas.MaxDepth = 1024;
                host.AddServiceEndpoint(serviceContractType, binding, httpBaseAddress);

                // Habilitar conexión
                if (!host.Description.Behaviors.Contains(typeof(ServiceMetadataBehavior)))
                {
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    host.Description.Behaviors.Add(smb);
                }

                // Abrir servicio
                host.Open();

                ConsoleWriter.SetText(httpBaseAddress.AbsoluteUri + " WS Initialized...");
                Debug.WriteLine(httpBaseAddress.AbsoluteUri + " WS Initialized...");

                lock (hostedServices)
                {
                    hostedServices.Add(serviceType.Name, host);
                }
                return true;
            }
            catch (InvalidOperationException initError)
            {
                ConsoleWriter.SetText("Error initializing " + httpBaseAddress.AbsoluteUri + " service. ERROR : " + initError.Message);
            }
            catch (CommunicationException initError)
            {
                ConsoleWriter.SetText("Error initializing " + httpBaseAddress.AbsoluteUri + " service. ERROR : " + initError.Message);
            }
            return false;
        }

        public bool StopInfrastructureService(string serviceTypeName)
        {
            try
            {
                if (!hostedServices.ContainsKey(serviceTypeName)) return true;

                ServiceHost serviceHost = hostedServices[serviceTypeName];

                if (serviceHost != null)
                {
                    serviceHost.Close();
                    hostedServices.Remove(serviceTypeName);
                    ConsoleWriter.SetText("Service " + serviceHost.BaseAddresses[0].AbsoluteUri + " closed.");
                    return true;
                }
            }
            catch (System.ServiceModel.CommunicationObjectFaultedException error)
            {
                Debug.WriteLine("FAILURE : stop infrastructure service. ERROR : " + error.Message);
                return true;
            }
            catch (System.TimeoutException error)
            {
                Debug.WriteLine("FAILURE : stop infrastructure service. ERROR : " + error.Message);
                return true;
            }

            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "It is required a Type to call ServiceHost constructor")]
        public bool PublishInfrastructureService(Type serviceContractType, Type serviceType, Binding serviceBinding)
        {
            Uri httpBaseAddress;
            string uriString;

            // Construir una URL
            uriString = "http://" + serverName + ":" + serverPort.ToString(CultureInfo.InvariantCulture) + "/" + serviceType.Name;
            httpBaseAddress = new Uri(uriString);

            try
            {
                ServiceHost host = new ServiceHost(serviceType, httpBaseAddress);
                host.AddServiceEndpoint(serviceContractType, serviceBinding, httpBaseAddress);

                // Habilitar conexión
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                host.Description.Behaviors.Add(smb);

                // Abrir servicio
                host.Open();
                ConsoleWriter.SetText(httpBaseAddress.AbsoluteUri + " WS Initialized...");
                Debug.WriteLine(httpBaseAddress.AbsoluteUri + " WS Initialized...");
                lock (hostedServices)
                {
                    hostedServices.Add(serviceType.Name, host);
                }
                return true;
            }
            catch (InvalidOperationException initError)
            {
                ConsoleWriter.SetText("Error initializing " + httpBaseAddress.AbsoluteUri + " service. ERROR : " + initError.Message);
            }
            catch (CommunicationException initError)
            {
                ConsoleWriter.SetText("Error initializing " + httpBaseAddress.AbsoluteUri + " service. ERROR : " + initError.Message);
            }

            return false;
        }

        internal bool PublishCustomService(Type serviceContractType, Type serviceType, Binding serviceBinding)
        {
            return PublishInfrastructureService(serviceContractType, serviceType, serviceBinding);
        }

        internal bool StopCustomService(string serviceTypeName)
        {
            return StopInfrastructureService(serviceTypeName);
        }
    }
}
