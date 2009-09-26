using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Core;
using System.ServiceModel;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Channels;
using System.Reflection;
using System.Security;
using UtnEmall.Server.BusinessLogic;
using System.Globalization;
using System.Security.Permissions;
using System.Collections.ObjectModel;
using WpfCore.Properties;
using UtnEmall.Server.WpfCore;
using WpfCore;
using UtnEmall.Server.DataModel;

namespace UtnEmall.Server.ServiceCompiler
{
    /// <summary>
    /// Interfase para constructor de servicios
    /// </summary>
    [ServiceContract]
    public interface IServiceBuilder
    {
        /// <summary>
        /// Construir y publicar servicios de infraestructura.
        /// </summary>
        /// <param name="serviceDataModel">Modelo de datos para generar servicios de infratestructura.</param>
        /// <param name="insertTestData">Establece si dtos de ejemplo serán insertados en el servicio.</param>
        /// <param name="sessionIdentifier">Sesión de usuario</param>
        /// <returns>True si la construcción y publicación se realizaron exitosamente.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        [PermissionSet(SecurityAction.LinkDemand)]
        bool BuildAndImplementInfrastructureService(DataModelEntity serviceDataModel, bool insertTestData, string sessionIdentifier);

        /// <summary>
        /// Construir y publicar servicios
        /// </summary>
        /// <param name="customerService">Servicio a partir del cual se generarán los ensamblados .NET.</param>
        /// <param name="sessionIdentifier">Sesión de usuario</param>
        /// <returns>True si se ha creado y publicado con éxito.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        [PermissionSet(SecurityAction.LinkDemand)]
        bool BuildAndImplementCustomService(CustomerServiceDataEntity customerService, string sessionIdentifier);
    }

    /// <summary>
    /// Implementación de servicio de Constructor de servicios 
    /// Esta clase construye ensamblados .NET para servidor y dispositivos Windows Mobile.
    /// </summary>
	public class ServiceBuilder : IServiceBuilder 
    {
        public const string AssembliesFolder = "assemblies";
        static string _compactFrameworkMscorlibPartialPath = "\\Microsoft.NET\\SDK\\CompactFramework\\v3.5\\WindowsCE\\mscorlib.dll";
        static string _compactFrameworkPartialPath = "\\Microsoft.NET\\SDK\\CompactFramework\\v3.5\\WindowsCE";
        static string _netFramework3PartialPath = "\\Reference Assemblies\\Microsoft\\Framework\\v3.0";

        // Propiedades para ruta de ensamblados referenciadas

        internal static string CompactFrameworkMscorlibPartialPath
        {
            get
            {
                return _compactFrameworkMscorlibPartialPath;
            }
        }
        internal static string CompactFrameworkPartialPath
        {
            get
            {
                return _compactFrameworkPartialPath;
            }
        }
        internal static string NetFramework3PartialPath
        {
            get
            {
                return _netFramework3PartialPath;
            }
        }

        // Ruta opcional de servidor. Si no es provista, se usará la ruta de AppDomain
        private string serverPath;

        /// <summary>
        /// Retorna una instancia de ServiceBuilder
        /// </summary>
        public ServiceBuilder()
        {
            // Por defecto, establece la ruta del servidor al directorio de dominio de aplicación base
            serverPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Ruta de servidor opcional. Si no es provista, se usará AppDomain
        /// </summary>
        public string ServerPath
        {
            get
            {
                return serverPath;
            }
            set
            {
                serverPath = value;
            }
        }
        /// <summary>
        /// Verifica si la carpeta de ensamblados existe y la crea en caso contrario
        /// </summary>
        private void CheckAssembliesFolder()
        {
            string path = Path.Combine(serverPath,AssembliesFolder);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        
        /// <summary>
        /// Construye e implementa un servicio de infraestructura para una tienda o el centro comercial.
        /// </summary>
        /// <param name="serviceDataModel">El modelo de datos desde el cuál generar el servicio.</param>
        /// <param name="sessionIdentifier">Identificador de sesión.</param>
        /// <returns>true si tuvo éxito.</returns>
        [PermissionSet(SecurityAction.LinkDemand)]
        public bool BuildAndImplementInfrastructureService(DataModelEntity serviceDataModel, bool insertTestData, string sessionIdentifier)
        {
            // Verificar argumentos
            if (serviceDataModel == null)
            {
                throw new ArgumentException("Invalid null argument. Must provide an instance of DataModelEntity.", "serviceDataModel");
            }
            if (sessionIdentifier == null)
            {
                throw new ArgumentException("Invalid null argument.", "sessionIdentifier");
            }

            // Verificar el identificado de sesión con el administrador de sesión
            // Verifica si un servicio es válido
            BusinessLogic.DataModel dataModel = new BusinessLogic.DataModel();
            if (!dataModel.Validate(serviceDataModel))
            {
                throw new ArgumentException("Provided data model is not valid.", "serviceDataModel");
            }
            // Si el servicio se desplegó lanzar un error
            if (serviceDataModel.Deployed)
            {
                throw new FaultException(Resources.DataModelAlreadyDeployed);
            }
            // Debe contener almenos una tabla
            if (serviceDataModel.Tables.Count == 0)
            {
                throw new FaultException(Resources.DataModelMustHaveOneTable);
            }

            Console.WriteLine("Building infrastructure service.");

            // Construir el programa Meta D++
            string fileName = BuildMetaDppProgramForInfrastructureService(serviceDataModel, false);
            // Intentar bajar el servicio si está activo
            if (ServerHost.Instance.StopInfrastructureService( Path.GetFileNameWithoutExtension(fileName) + "Service" ))
            {
                // Compilar el programa Meta D++ en una librería
                if (CompileMetaDppProgram(new Uri(fileName), false))
                {
                    // Construir la versión móvil del servicio de infraestructura
                    string clientVersionFileName = BuildMetaDppProgramForInfrastructureService(serviceDataModel, true);
                    if (CompileMetaDppProgram(new Uri(clientVersionFileName), true))
                    {
                        Debug.WriteLine("Infrastructure Service Dll for Mobile succeful builded.");
                        fileName = Path.ChangeExtension(fileName, ".dll");
                        // Iniciar el servicio web
                        if (PublishInfrastructureService(fileName, insertTestData))
                        {
                            // Actualizar la información del modelo de datos
                            serviceDataModel.ServiceAssemblyFileName = fileName;
                            serviceDataModel.Deployed = true;
                            if (SaveDataModel(serviceDataModel, sessionIdentifier))
                            {
                                Debug.WriteLine("SUCCESS : saving data model.");
                                Debug.WriteLine("Building infrastructure service successful.");
                                return true;
                            }
                            else
                            {
                                Debug.WriteLine("FAILURE : error trying to save data model.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error building Infrastructure Service Dll for Mobile.");
                    }
                }
            }
            return false;
        }

        
        #region Utils functions for Infrastructure Service
        /// <summary>
        /// Actualiza la entidad DataModelEntity en la base de datos
        /// </summary>
        /// <param name="serviceDataModel">Modelo de datos a guardar.</param>
        /// <param name="sessionIdentifier">Identificador de sesión.</param>
        /// <returns>true si tiene éxito.</returns>
        static private bool SaveDataModel(DataModelEntity serviceDataModel, string sessionIdentifier)
        {
            DataModelDataAccess dataModelLogic = new DataModelDataAccess();
            serviceDataModel.ServiceAssemblyFileName = Path.GetFileName(serviceDataModel.ServiceAssemblyFileName);
            serviceDataModel.Deployed = true;
            try
            {
                dataModelLogic.Save(serviceDataModel);
                return true;
            }
            catch (UtnEmallDataAccessException error)
            {
                Debug.WriteLine("FAILURE : While updating data model entity. ERROR : " + error.Message);
                return false;
            }
        }
        /// <summary>
        /// Retorna el servicio y tipo de contrato para el servicio de infraestructura.
        /// </summary>
        /// <param name="fileName">Nombre del archivo del ensamblado que contiene el tipo para el servicio de infraestructura.</param>
        /// <returns>Un tipo de contrato como primer elemento, un tipo de servicio como segundo elemento en un vector. Retorna null si no puede cargar o encontrar los tipos.</returns>
        internal static Type[] GetInfrastructureServiceTypes(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentException("Argument can not be null.", "fileName");
            }
            try
            {
                byte[] fileBytes = File.ReadAllBytes(Path.Combine(AssembliesFolder, fileName));
                Assembly serviceAssembly = Assembly.Load(fileBytes);
                Type serviceType = null, contractType = null;
                string serviceTypeName = Path.GetFileNameWithoutExtension(fileName) + "Service";

                foreach (Type someType in serviceAssembly.GetTypes())
                {
                    if (someType.Name.ToUpperInvariant() == serviceTypeName.ToUpperInvariant())
                    {
                        serviceType = someType;
                    }

                    if (someType.Name.ToUpperInvariant() == "I" + serviceTypeName.ToUpperInvariant())
                    {
                        contractType = someType;
                    }
                }
                return new Type[] { contractType, serviceType };
            }
            catch (FileNotFoundException fileError)
            {
                Debug.WriteLine("FAILURE : infrastructure service assembly \"" + fileName + "\" not found. ERROR : " + fileError.Message);
                return null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
            "CA1031:DoNotCatchGeneralExceptionTypes")]
        static private bool PublishInfrastructureService(string fileName, bool insertTestData)
        {
            if (fileName == null)
            {
                throw new ArgumentException("Argument can not be null.", "fileName");
            }
            fileName = Path.Combine(AssembliesFolder, fileName);
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("File \"" + fileName + "\" does not exists.", "fileName");
            }

            try
            {
                Type[] servicesTypes = GetInfrastructureServiceTypes(fileName);
                Type serviceType = servicesTypes[1];
                // Si es requerido, insertar datos de ejemplo en el método Test
                if (insertTestData)
                {
                    try
                    {
                        serviceType.GetMethod("RunTest").Invoke(null, null);
                    }
                    catch
                    {
                        Debug.WriteLine(Resources.FailureRunningTestOnInfrastructureService);
                    }
                }
                Binding serviceBinding = new BasicHttpBinding();
                // Publicar servicios
                if (ServerHost.Instance.PublishInfrastructureService(servicesTypes[0], serviceType, serviceBinding))
                {
                    Debug.WriteLine(Resources.SuccessInfrastructureServicePublished);
                    return true;
                }
                Debug.WriteLine(Resources.FailureTryingToPublishInfrastructureService);
                return false;
            }
            catch (SecurityException securityError)
            {
                Debug.WriteLine(Resources.FailureSecurityErrorLoadingInfrastructureService + securityError.Message);
                return false;
            }
            catch (Exception otherError)
            {
                Debug.WriteLine(Resources.FailureLoadingServiceAssemblyOrOtherError + otherError.Message);
                return false;
            }
        }

        /// <summary>
        /// Compilar programa Meta D++ en librerías .NET dinámicas
        /// </summary>
        /// <param name="serverFileName">El nombre de archivo del programa Meta D++.</param>
        /// <param name="mobile">Establecer si es versión móvil.</param>
        /// <returns>True si la compilación es exitosa.</returns>
        [PermissionSet(SecurityAction.LinkDemand)]
        private bool CompileMetaDppProgram(Uri serverFileName, bool mobile)
        {
            // Bloquear la instancia del servicio de construcción
            try
            {
                Debug.WriteLine("Begin compilation of LayerD source file. " + Path.GetFileName(serverFileName.OriginalString) );

                // Construir argumentos
                string arguments = "\"" +
                    Path.GetDirectoryName(serverPath) + "\" " +
                    "\"" + Path.GetDirectoryName(serverFileName.OriginalString) +
                    "\" " + Path.GetFileNameWithoutExtension(serverFileName.OriginalString);

                if (mobile)
                {
                    // Agregar argumentos para la generación de argumentos de móvil
                    arguments += " /nostdlib+ /define:DEBUG;TRACE;PocketPC /reference:\\\"" + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + CompactFrameworkMscorlibPartialPath + "\\\"";
                }
                // Invocar al archivo por lotes
                ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(serverPath, "build.bat"), arguments);

                // Ocultar salida
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;

                Process serverCompilation = Process.Start(startInfo);
                // Esperar a que el proceso finalice
                serverCompilation.WaitForExit();
                
                Debug.WriteLine("End compilation of LayerD source file. ");

                // Verificar salidas
                if (File.Exists(
                    Path.ChangeExtension(serverFileName.OriginalString, ".dll")))
                {
                    // Retornar true si se ha realizado exitosamente
                    Debug.WriteLine("SUCCESS : compilation result was found.");
                    return true;
                }
                else
                {
                    // En caso de error, retornar false
                    Debug.WriteLine("FAILURE : compilation result was not found.");
                    return false;
                }
            }
            catch (FileNotFoundException error)
            {
                Debug.WriteLine("FAILURE : Error while trying to compile assembly. ERROR : " + error.Message);
                return false;
            }
            catch (InvalidOperationException error)
            {
                Debug.WriteLine("FAILURE : Error while trying to compile assembly. ERROR : " + error.Message);
                return false;
            }
        }
        /// <summary>
        /// Construir el programa Meta D++
        /// </summary>
        /// <param name="serviceDataModel">Datos de servicio de infraestructura.</param>
        /// <returns>El nombre del archivo, null si falló.</returns>
        private string BuildMetaDppProgramForInfrastructureService(DataModelEntity serviceDataModel, bool buildClient)
        {
            // Verifica argumento
            if (serviceDataModel == null)
            {
                throw new ArgumentException("Must provide not null argument.", "serviceDataModel");
            }
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace("\\", "\\\\");
            string compactFrameworkPath = CompactFrameworkPartialPath.Replace("\\", "\\\\");
            string netFramework3PartialPath = NetFramework3PartialPath.Replace("\\", "\\\\");
            // Construye programa Meta D++
            string fileStr = null;
            fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assembly=mscorlib\";\n";
            if (!buildClient)
            {
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assembly=System\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assembly=System.Drawing\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assembly=System.Data\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assembly=System.Web.Services\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + netFramework3PartialPath + "\\\\System.ServiceModel.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + netFramework3PartialPath + "\\\\System.Runtime.Serialization.dll\";\n";
                fileStr += "import \"UtnEmall\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=..\\\\BaseDesktop.dll\";\n";
            }
            else
            {
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + compactFrameworkPath + "\\\\System.Drawing.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + compactFrameworkPath + "\\\\System.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + compactFrameworkPath + "\\\\System.Data.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + compactFrameworkPath + "\\\\System.Xml.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + compactFrameworkPath + "\\\\System.ServiceModel.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=" + programFilesPath + compactFrameworkPath + "\\\\System.Runtime.Serialization.dll\";\n";
                fileStr += "import \"System\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=.\\\\libsclient\\\\System.Data.SqlServerCe.dll\";\n";
                fileStr += "import \"UtnEmall\", \"platform=DotNET\", \"ns=DotNET\", \"assemblyfilename=.\\\\libsclient\\\\BaseMobile.dll\";\n";
            }
            fileStr += "\n";
            fileStr += "using Zoe;\n";
            fileStr += "using zoe::lang;\n";
            
            fileStr += "using DotNET::System;\n";
            fileStr += "using DotNET::System::Collections;\n";
            fileStr += "using DotNET::System::Data;\n";
            fileStr += "using DotNET::System::Data::Common;\n";
            fileStr += "using DotNET::System::Data::SqlClient;\n";
            fileStr += "using UtnEmall::Store" + serviceDataModel.IdStore + ";\n";
            fileStr += "using UtnEmall::Store" + serviceDataModel.IdStore + "::EntityModel;\n";
            fileStr += "using UtnEmall::Store" + serviceDataModel.IdStore + "::BusinessLogic;\n";
            
            if (!buildClient)
            {
                fileStr += "using DotNET::UtnEmall::Server::EntityModel;\n";
                fileStr += "using DotNET::UtnEmall::Server::DataModel;\n";
                fileStr += "using DotNET::UtnEmall::Server::BusinessLogic;\n";
                fileStr += "using DotNET::UtnEmall::Server::Base;\n";
                
                fileStr += "using DotNET::System::ServiceModel;\n";
                fileStr += "using DotNET::System::Runtime::Serialization;\n";
                fileStr += "using UtnEmall::Server::EntityModel;\n";
            }
            else
            {
                fileStr += "using DotNET::System::Xml;\n";
                fileStr += "using DotNET::System::ServiceModel;\n";
                fileStr += "using DotNET::System::ServiceModel::Channels;\n";
                fileStr += "using DotNET::System::Data::SqlServerCe;\n";

                fileStr += "using DotNET::UtnEmall::Client::EntityModel;\n";
                fileStr += "using DotNET::UtnEmall::Client::DataModel;\n";
                fileStr += "using DotNET::UtnEmall::Client::BusinessLogic;\n";
                fileStr += "using DotNET::UtnEmall::Client::PresentationLayer;\n";
                fileStr += "using UtnEmall::Client::PresentationLayer;\n";
                fileStr += "using UtnEmall::Client::EntityModel;\n";

            }
            fileStr += "using UtnEmall::Utils;\n";
            
            fileStr += "using DotNET::System::Collections::Generic;\n";
            fileStr += "using DotNET::System::Collections::ObjectModel;\n";
            
            fileStr += "using System::Collections::Generic;\n";
            fileStr += "\n";

            fileStr += "Model::DefineNamespace(UtnEmall::Store" + serviceDataModel.IdStore + "::EntityModel);\n";
            fileStr += "Model::DefineBusinessNamespace(UtnEmall::Store" + serviceDataModel.IdStore + "::BusinessLogic);\n";
            fileStr += "Model::DefineIdentity(false);\n";

            if (buildClient)
            {
                fileStr += "Model::DefineMobil(true);\n";
                fileStr += "ModelBusiness::IsWindowsMobile(true, false);\n";
                fileStr += "InfService::IsMobil(true);\n";
            }

            fileStr += "\n";
            fileStr += "namespace UtnEmall{\n";
            fileStr += "\n";
            if (!buildClient)
            {
                fileStr += "Model::DefineMobil(false);\n";
            }
            else
            {
                fileStr += "Model::DefineMobil(true);\n";
            }

            string[] relationTypes = new string[] { "OneToOne", "OneToMany", "ManyToMany" };
            string[] fieldTypes = new string[] { "String", "Integer", "DateTime", "Boolean", "UtnEmall::Utils::Image" };
            // El servicio
            fileStr += "InfService::New(InfrastructureService" + serviceDataModel.Id.ToString( System.Globalization.CultureInfo.CurrentCulture.NumberFormat) + ", " + serviceDataModel.IdStore + ", \"Service description\"){\n";
            {
                // Las tablas
                foreach (TableEntity table in serviceDataModel.Tables)
                {
                    if (table.IsStorage)
                    {
                        fileStr += "InfService::Table(" + UtnEmall.Server.Base.Utilities.GetValidIdentifier(table.Name, false) + ", true){ };\n";
                    }
                    else
                    {
                        fileStr += "InfService::Table(" + UtnEmall.Server.Base.Utilities.GetValidIdentifier(table.Name, false) + "){\n";
                        // Los campos
                        foreach (FieldEntity field in table.Fields)
                        {
                            fileStr += "InfService::Field(" + UtnEmall.Server.Base.Utilities.GetValidIdentifier(field.Name, false, false) + ", " + fieldTypes[field.DataType - 1] + ", \"\" );\n";
                        }
                        fileStr += "};\n"; 

                    }
                }
                // Las relaciones
                foreach (RelationEntity relation in serviceDataModel.Relations)
                {
                    fileStr += "InfService::Relation(" + UtnEmall.Server.Base.Utilities.GetValidIdentifier(relation.Source.Name, false) + ", " + UtnEmall.Server.Base.Utilities.GetValidIdentifier(relation.Target.Name, false) + ", " + relationTypes[relation.RelationType - 1] + ");\n";
                }
            }
            fileStr += "};\n";

            fileStr += "\n";
            fileStr += "}\n";

            // Guardar el programa Meta D++ en el disco
            string outputFileName;
            StreamWriter file = null;

            if (!buildClient)
            {
                outputFileName = Path.GetDirectoryName(serverPath) + Path.DirectorySeparatorChar + AssembliesFolder + Path.DirectorySeparatorChar + "Store" + serviceDataModel.IdStore.ToString(CultureInfo.InvariantCulture) + "Infrastructure.dpp";
            }
            else
            {
                // Si el ensamblado del dispositivo móvil existe, crear uno diferente
                outputFileName = Path.GetDirectoryName(serverPath) + Path.DirectorySeparatorChar + AssembliesFolder + Path.DirectorySeparatorChar + "Store" + serviceDataModel.IdStore.ToString(CultureInfo.InvariantCulture) + "Infrastructure_Mobile.dpp";
            }

            try
            {
                CheckAssembliesFolder();
                file = new StreamWriter(outputFileName);
                file.Write(fileStr);
            }
            catch (IOException ioError)
            {
                Debug.WriteLine("FAILURE : While saving Meta D++ program to disk. ERROR : " + ioError.Message);
                return null;
            }
            finally
            {
                if (file != null) file.Close();
            }

            // En caso de éxito, retornar el nombre del programa
            return outputFileName;
        }

        #endregion

        /// <summary>
        /// Construir e implementar el servicio de cliente. Incluye la construcción y publicación de los  ensamblados de servidor y cliente. 
        /// </summary>
        /// <param name="customerService">El servicio a construir y publicar.</param>
        /// <param name="sessionIdentifier">Identificador de sesión.</param>
        /// <returns>true si tuvo éxito.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
            "CA1031:DoNotCatchGeneralExceptionTypes", Justification="We must catch any exception while trying to delete files")]
        [PermissionSet(SecurityAction.LinkDemand)]
        public bool BuildAndImplementCustomService(CustomerServiceDataEntity customerService, string sessionIdentifier)
        {
            // Verificar argumentos
            if (customerService == null)
                throw new ArgumentException("Invalid null argument. Must provide an instance of DataModelEntity.", "customerService");
            if (sessionIdentifier == null)
                throw new ArgumentException("Invalid null argument.", "sessionIdentifier");

            // Verificar si el servicio es válido
            CustomerServiceData customerServiceLogic = new CustomerServiceData();
            if (!customerServiceLogic.Validate(customerService))
            {
                throw new ArgumentException("Provided customer service is not valid.", "customerService");
            }
            // Si el servicio ya está desplegado, lanzar un error
            if (customerService.Service == null)
            {
                // Si el servicio no fue cargado, cargarlo
                Service serviceLogic = new Service();
                customerService.Service = serviceLogic.GetService(customerService.IdService, false, sessionIdentifier);
            }
            if (customerService.Service.Deployed)
            {
                throw new FaultException(Resources.CustomerServiceAlreadyDeployed);
            }
            // El modelo de datos asociado debe ser desplegado
            if (!customerService.DataModel.Deployed)
            {
                throw new FaultException(Resources.DataModelMustBeDeployed);
            }


            ConsoleWriter.SetText("Building custom service.");

            // Carga el contenedor del servicio
            LoadService(customerService, sessionIdentifier);

            // Construir el programa Meta D++
            string fileName = BuildMetaDppProgramForCustomerService(customerService, false);
            string clientVersionFileName = BuildMetaDppProgramForCustomerService(customerService, true);

            // Intentar transferir el servicio si está activo
            if (ServerHost.Instance.StopCustomService(Path.GetFileNameWithoutExtension(fileName)))
            {
                try
                {
                    File.Delete(Path.ChangeExtension(fileName, ".dll"));
                    File.Delete(Path.ChangeExtension(clientVersionFileName, ".dll"));
                }
                catch
                {
                    Debug.WriteLine("Old services files couldn´t be deleted.");
                }

                // Compilar el programa Meta D++
                if (CompileMetaDppProgram(new Uri(fileName), false))
                {
                    // Construir la versión móvil del servicio de infraestructura
                    
                    if (CompileMetaDppProgram(new Uri(clientVersionFileName), true))
                    {
                        ConsoleWriter.SetText("Building custom service successful.");

                        Debug.WriteLine("Custom Service Dll for Mobile succeful builded.");
                        fileName = Path.ChangeExtension(fileName, ".dll");
                        clientVersionFileName = Path.ChangeExtension(clientVersionFileName, ".dll");

                        SaveCustomServiceModel(customerService, fileName, clientVersionFileName, sessionIdentifier);

                        // Inciar el servicio web
                        return PublishCustomService(fileName);
                    }
                    else
                    {
                        Debug.WriteLine("Error building Custom Service Dll for Mobile.");
                    }
                }
            }
            return false;
		}
        /// <summary>
        /// Cargar la entidad del servicio
        /// </summary>
        /// <param name="serviceModel">El servicio de cliente a partir del cual cargar la entidad.</param>
        /// <param name="sessionIdentifier">Identificador de sesión.</param>
        private static void LoadService(CustomerServiceDataEntity serviceModel, string sessionIdentifier)
        {
            if (serviceModel == null)
            {
                throw new ArgumentException("The argument serviceModel can not be null.", "serviceModel");
            }
            // Si el servicio ya está desplegado, retornar.
            if (serviceModel.Service != null)
            {
                return;
            }
            // en caso contrario, cargarlo desde la base de datos
            try
            {
                Service serviceLogic = new Service();
                serviceModel.Service = serviceLogic.GetService(serviceModel.IdService, false, sessionIdentifier);
            }
            catch (Exception)
            {
                throw new FaultException("Error loading service data.");
            }
        }

        
        /// <summary>
        /// Publicar el servicio.
        /// <seealso cref="ServerHost"/>
        /// </summary>
        /// <param name="fileName">Ensamblado del servicio a publicar</param>
        /// <returns>true si se publica exitosamente.</returns>
        static private bool PublishCustomService(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentException("Argument can not be null.", "fileName");
            }
            fileName = Path.Combine(AssembliesFolder, fileName);

            if (!File.Exists(fileName))
            {
                throw new ArgumentException("File \"" + fileName + "\" does not exists.", "fileName");
            }

            try
            {
                Type[] servicesTypes = GetCustomServiceTypes(fileName);
                Binding serviceBinding = new BasicHttpBinding();
                // Publicar servicio
                if (ServerHost.Instance.PublishCustomService(servicesTypes[0], servicesTypes[1], serviceBinding))
                {
                    Debug.WriteLine("SUCCESS : custom service published.");
                    return true;
                }
                Debug.WriteLine("FAILURE : trying to publish custom service.");
                return false;
            }
            catch (SecurityException securityError)
            {
                Debug.WriteLine("FAILURE : security error while loading custom service. ERROR : " + securityError.Message);
                return false;
            }
        }

        /// <summary>
        /// Retorna el tipo de contrato y servicio para el servicio.
        /// </summary>
        /// <param name="fileName">Nombre del archivo del ensamblado.</param>
        /// <returns>El tipo de contrato y el tipo de servicio en un vector.</returns>
        internal static Type[] GetCustomServiceTypes(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentException("Argument can not be null.", "fileName");
            }
            try
            {
                byte[] assemblyBytes = File.ReadAllBytes(Path.Combine(AssembliesFolder, fileName));
                Assembly serviceAssembly = Assembly.Load(assemblyBytes);
                Type serviceType = null;
                string serviceTypeName = Path.GetFileNameWithoutExtension(fileName);

                foreach (Type someType in serviceAssembly.GetTypes())
                {
                    if (someType.Name.ToUpperInvariant() == serviceTypeName.ToUpperInvariant())
                    {
                        serviceType = someType;
                    }
                }
                return new Type[] { serviceType, serviceType };
            }
            catch (FileNotFoundException fileError)
            {
                Debug.WriteLine("FAILURE : custom service assembly \"" + fileName + "\" not found. ERROR : " + fileError.Message);
                return null;
            }
            catch (BadImageFormatException badImage)
            {
                Debug.WriteLine("FAILURE : custom service assembly \"" + fileName + "\" not found. ERROR : " + badImage.Message);
                return null;
            }
        }

        /// <summary>
        /// Actualiza los archivos de ensamblado en la entidad de servicio.
        /// </summary>
        /// <param name="serviceModel">El modelo de servicio desde el cuál se actualizan los datos.</param>
        /// <param name="fileName">El nombre de archivo.</param>
        /// <param name="clientVersionFileName">El nombre del archivo del cliente</param>
        /// <param name="sessionIdentifier">Sesión de usuario</param>
        /// <returns>True si se guardó con éxito.</returns>
        static private bool SaveCustomServiceModel(CustomerServiceDataEntity serviceModel, string fileName, string clientVersionFileName, string sessionIdentifier)
        {
            try
            {
                ServiceDataAccess serviceDataAccess = new ServiceDataAccess();
                ServiceEntity serviceEntity = serviceModel.Service;
                if (serviceEntity == null)
                {
                    Service serviceLogic = new Service();
                    serviceEntity = serviceLogic.GetService(serviceModel.IdService, false, sessionIdentifier);
                }
                serviceEntity.Deployed = true;
                serviceEntity.PathAssemblyServer = Path.GetFileNameWithoutExtension(fileName) + ".dll";
                serviceEntity.RelativePathAssembly = Path.GetFileNameWithoutExtension(clientVersionFileName) + ".dll";
                serviceDataAccess.Save(serviceEntity);
                return true;
            }
            catch (UtnEmallDataAccessException error)
            {
                Debug.WriteLine("FAILURE : While updating custom service entity. ERROR : " + error.Message);
                return false;
            }
            catch (UtnEmallBusinessLogicException error)
            {
                Debug.WriteLine("FAILURE : While updating custom service entity. ERROR : " + error.Message);
                return false;
            }
        }

        /// <summary>
        /// Construir programa Meta D++
        /// </summary>
        /// <param name="serviceModel">El modelo de servicio</param>
        /// <param name="buildMobil">True para construir la versión móvil</param>
        /// <returns>El nombre del programa</returns>
        private string BuildMetaDppProgramForCustomerService(CustomerServiceDataEntity serviceModel, bool buildMobil)
        {
            string filename = null;
            string fileContent = null;
            if (buildMobil)
            {
                filename = "CustomService" + serviceModel.Id + "_Mobile.dpp";
                fileContent = DppExporter.Service(serviceModel, true);
            }
            else
            {
                filename = "CustomService" + serviceModel.Id + ".dpp";
                fileContent = DppExporter.Service(serviceModel, false);
            }
            filename = Path.GetDirectoryName(serverPath) + Path.DirectorySeparatorChar + AssembliesFolder + Path.DirectorySeparatorChar + filename;
            try
            {
                CheckAssembliesFolder();
                StreamWriter file = new StreamWriter(filename);
                file.Write(fileContent);
                file.Close();
            }
            catch (SecurityException secuirtyError)
            {
                Debug.WriteLine("ERROR : security error while saving meta dpp program for custom service. ORIGINAL ERROR : " + secuirtyError.Message);
            }
            catch (IOException ioError)
            {
                Debug.WriteLine("ERROR : io error while saving meta dpp program for custom service. ORIGINAL ERROR : " + ioError.Message);
            }
            return filename;
        }

	}
}