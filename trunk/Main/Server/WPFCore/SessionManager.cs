using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading;
using UtnEmall.Server.Base;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using WpfCore;
using System.Diagnostics;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Administra las sesiones de usuarios y clientes
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SessionManager : ISessionManager, IValidator
    {
        #region Nested Classes and Structures

        /// <summary>
        /// Utilidad para almacenar Usuario/Timestamp
        /// </summary>
        private class MobileTimestampPair
        {
            private string userID;
            private DateTime timestamp;

            public string User
            {
                get { return userID; }
            }

            public DateTime Timestamp
            {
                get { return timestamp; }
                set { timestamp = value; }
            }

            public MobileTimestampPair(string key, DateTime value)
            {
                userID = key;
                timestamp = value;
            }
        }

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        private const int cleanupThreadWait = 15000;
        private const int maxMinutesAllowed = 2;
        private static Object getInstanceLock = new Object();

        private static SessionManager instance;
        /// <summary>
        /// Obtiene la instancia del SessionManager
        /// </summary>
        /// <returns>La instancia del SessionManager.</returns>
        public static SessionManager Instance
        {
            get
            {
                lock (getInstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SessionManager();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Instance Variables and Properties

        private Dictionary<string, MobileTimestampPair> connectionTimeFrames;
        private Dictionary<string, List<PermissionEntity>> permissionDictionary;
        private Dictionary<string, IEntity> activeSessions;
        private List<PermissionEntity> customerPermissions;
        private Dictionary<string, string> loggedMobiles;

        /// <summary>
        /// Diccionario con moviles logueados
        /// </summary>
        public Dictionary<string, string> LoggedMobiles
        {
            get
            {
                return loggedMobiles;
            }
        }

        #endregion

        #endregion

        #region Constructors

        private SessionManager()
        {
            // Crear la tabla para almacenar sesiones
            connectionTimeFrames = new Dictionary<string, MobileTimestampPair>();

            // Crear la tabla de permisos
            permissionDictionary = new Dictionary<string, List<PermissionEntity>>();

            // Crear la tabla de secciones activas
            activeSessions = new Dictionary<string, IEntity>();

            // Crear tabla de moviles logueados
            loggedMobiles = new Dictionary<string, string>();

            // Crear una lista de permisos por defecto de un cliente
            customerPermissions = new List<PermissionEntity>();

            // Llenar los permisos por defecto
            PermissionEntity permission = new PermissionEntity();
            permission.BusinessClassName = "Service";
            permission.AllowRead = true;

            customerPermissions.Add(permission);

            permission = new PermissionEntity();
            permission.BusinessClassName = "Customer";
            permission.AllowRead = true;
            permission.AllowNew = true;
            permission.AllowUpdate = true;

            customerPermissions.Add(permission);

            permission = new PermissionEntity();
            permission.BusinessClassName = "Category";
            permission.AllowRead = true;

            customerPermissions.Add(permission);

            permission = new PermissionEntity();
            permission.BusinessClassName = "UserAction";
            permission.AllowUpdate = true;

            customerPermissions.Add(permission);
        }

        #endregion

        #region Static Methods

        #region Public Static Methods

        /// <summary>
        /// Crea una session única para identificar a los clietnes
        /// </summary>
        /// <param name="userName">Nombre del cliente.</param>
        /// <param name="password">Nombre del password.</param>
        /// <returns>Una cadena no vacia si la validación tuvo éxito o nulo si fallo.</returns>
        public static string CreateCustomerSession(string userName, string password)
        {
            string sessionString;

            sessionString = (userName + DateTime.Now.Ticks + password.Length).ToString();

            return sessionString;
        }

        /// <summary>
        /// Crea un identificador de sesión único para usuarios.
        /// </summary>
        /// <param name="userName">Nombre del usuario.</param>
        /// <param name="password">Password del usuario.</param>
        /// <returns>Una cadena no vacia si la validación tuvo éxito o nulo si fallo.</returns>
        public static string CreateUserSession(string userName, string password)
        {
            string sessionString;

            sessionString = (userName + DateTime.Now.Ticks + password.Length).ToString();

            return sessionString;
        }

        #endregion

        #region Protected and Private Static Methods

        static private bool IsAllowed(List<PermissionEntity> permissions, string businessEntity, string action)
        {
            bool allow = false;

            foreach (PermissionEntity item in permissions)
            {
                if (item.BusinessClassName == businessEntity)
                {
                    if (action == "delete")
                    {
                        allow = item.AllowDelete;
                    }
                    else if (action == "new")
                    {
                        allow = item.AllowNew;
                    }
                    else if (action == "read")
                    {
                        allow = item.AllowRead;
                    }
                    else if (action == "save")
                    {
                        allow = item.AllowUpdate;
                    }

                    return allow;
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Corre el SessionManager.
        /// </summary>
        public void Run()
        {
            // Crea y corre una tarea para limpiar sesiones que expiraron
            Thread cleanService = new Thread(new ThreadStart(this.RemoveTimeframeExceededMobile));
            cleanService.Start();

            // Crea y corre un hilo para escuchar por los mobiles
            DiscoveryService discoveryService = new DiscoveryService(this);
            Thread service = new Thread(new ThreadStart(discoveryService.ListenForMobileDevices));

            service.IsBackground = false;
            service.Start();
        }

        /// <summary>
        /// Devuelve un cliente asociado con el identificador de sesion proporcionado
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del cliente.</param>
        /// <returns>Un cliente si el identificador de sesión es valido. Nulo si el identificador de sesión es invalido o no identifica un cliente.</returns>
        public CustomerEntity GetCustomerFromSession(string sessionId)
        {
            IEntity entity;
            CustomerEntity customer;

            if (activeSessions.TryGetValue(sessionId, out entity))
            {
                customer = entity as CustomerEntity;
                return customer;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve un usuario asociado con el identificador de sesión provisto.
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del usuario.</param>
        /// <returns>Un usuario si el identificador de sesión es valido. Nulo si la sesión no existe o no corresponde a un usuario.</returns>
        public UserEntity GetUserFromSession(string sessionId)
        {
            IEntity entity;
            UserEntity user;

            if (activeSessions.TryGetValue(sessionId, out entity))
            {
                user = entity as UserEntity;
                return user;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Indica si el identificador de sesión pertenece a un cliente.
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del cliente.</param>
        /// <returns>Verdadero si el sessionId pertenece a un cliente, falso de lo contrario</returns>
        public bool IsCustomerSession(string sessionId)
        {
            return GetCustomerFromSession(sessionId) != null;
        }

        /// <summary>
        /// Indica si un identificador de sesión pertenece a un Usuario.
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del usuario.</param>
        /// <returns>Verdadero si el identificador de sesión pertenece a un Usuario, falso de lo contrario</returns>
        public bool IsUserSession(string sessionId)
        {
            return GetUserFromSession(sessionId) != null;
        }

        /// <summary>
        /// Devuelve la tienda asociada a una sessión de usuario o nulo si la sesión no pertenece a un usuario.
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del usuario.</param>
        /// <returns>La tienda relacionada con el usuario, o nulo si la sesión no pertenece a un usuario</returns>
        public StoreEntity StoreFromUserSession(string sessionId)
        {
            UserEntity user = GetUserFromSession(sessionId);

            if (user != null)
            {
                if (user.Store == null)
                {
                    return new StoreDataAccess().Load(user.IdStore);
                }
                else
                {
                    return user.Store;
                }
            }

            return null;
        }

        /// <summary>
        /// Valida el nombre de usuario y la clave de un cliente.
        /// </summary>
        /// <param name="userName">Nombre del cliente.</param>
        /// <param name="password">Nombre del password.</param>
        /// <returns>Una cadena no vacia si la validación tuvo éxito o nulo si fallo.</returns>
        public string ValidateCustomer(string userName, string passwordHash)
        {
            string sessionID = "";
            CustomerEntity customerEntity;
            Collection<CustomerEntity> customers;

            if (!loggedMobiles.TryGetValue(userName, out sessionID))
            {
                CustomerDataAccess dataAccessCustomer = new CustomerDataAccess();
                customers = dataAccessCustomer.LoadWhere(CustomerEntity.DBUserName, userName, true, OperatorType.Equal);

                if (customers.Count > 0)
                {
                    customerEntity = (CustomerEntity)customers[0];

                    if (customerEntity.Password == passwordHash)
                    {
                        sessionID = CreateCustomerSession(userName, passwordHash);
                        activeSessions.Add(sessionID, customerEntity);
                        connectionTimeFrames.Add(sessionID, new MobileTimestampPair(userName, DateTime.Now));

                        if (!loggedMobiles.ContainsKey(userName))
                        {
                            loggedMobiles.Add(userName, sessionID);
                        }
                    }
                }
                else
                {
                    ConsoleWriter.SetText("VALIDATE CUSTOMER ERROR");
                }
            }

            return sessionID;
        }

        /// <summary>
        /// Éste método valida si el nombre de usuario y la clave de un usuario son validas.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="password">Clave de usuario.</param>
        /// <returns>Una cadena no vacia si la validación tuvo éxito o nulo si fallo.</returns>
        public string ValidateUser(string userName, string passwordHash)
        {
            string sessionID = "";

            Collection<UserEntity> users;

            UserDataAccess userDataAccess = new UserDataAccess();

            users = userDataAccess.LoadWhere(UserEntity.DBUserName, userName, true, OperatorType.Equal);

            if (users.Count == 1)
            {
                UserEntity userEntity = (UserEntity)users[0];

                if (userEntity.Password == passwordHash)
                {
                    sessionID = CreateUserSession(userName, passwordHash);

                    if (loggedMobiles.ContainsKey(userName))
                    {
                        string oldSessionID = loggedMobiles[userName];
                        activeSessions.Remove(oldSessionID);
                        permissionDictionary.Remove(oldSessionID);
                        loggedMobiles.Remove(userName);
                    }

                    UsersPermissions(sessionID, userEntity);
                    activeSessions.Add(sessionID, userEntity);
                    loggedMobiles.Add(userName, sessionID);
                }
            }

            return sessionID;
        }

        /// <summary>
        /// Valida si un usuario puede realizar una acción en particular.
        /// </summary>
        /// <param name="id">Identificador de sesión del usuario.</param>
        /// <param name="action">Indica el tipo de acción requerida (leer, escribir, modificar, eliminar).</param>
        /// <param name="businessEntity">Entidad de negocios en la cual se requiere la acción.</param>
        /// <returns>Devuelve verdadero o falso de acuerdo a los permisos asignados al usuario.</returns>
        public bool ValidatePermission(string sessionId, string action, string businessEntity)
        {
            List<PermissionEntity> pair;

            if (ValidateSessionID(sessionId))
            {
                if (IsCustomerSession(sessionId))
                {
                    return IsAllowed(customerPermissions, businessEntity, action);
                }
                else if (permissionDictionary.TryGetValue(sessionId, out pair))
                {
                    return IsAllowed(pair, businessEntity, action);
                }
                else
                {
                    Debug.WriteLine("Validate Permission error.");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Termina la sesión del usuario o cliente.
        /// </summary>
        /// <param name="id">El identificador de sesión para el cliente o usuario.</param>
        /// <returns>Devuelve verdadero si tuvo éxito.</returns>
        public bool UserLogOff(string sessionId)
        {
            bool wasSuccesful;

            wasSuccesful = activeSessions.Remove(sessionId);

            return wasSuccesful;
        }

        /// <summary>
        /// Elimina las entradas del diccionario si las marcas de tiempo son antiguas.
        /// </summary>
        public void RemoveTimeframeExceededMobile()
        {
            // listado temporal
            Collection<string> expiredMobileIDs;

            // Iterar mientras la aplicación esta corriendo
            while (true)
            {
                // Dormirse por un tiempo
                Thread.Sleep(cleanupThreadWait);

                expiredMobileIDs = new Collection<string>();

                lock (connectionTimeFrames)
                {
                    foreach (MobileTimestampPair item in connectionTimeFrames.Values)
                    {

                        // Comparar si los minutos pasados desde el último aviso supero al máximo permitido
                        TimeSpan span = DateTime.Now - item.Timestamp;

                        if (span.TotalMinutes >= maxMinutesAllowed)
                        {
                            // Agregar el Id del mobile a la lista de moviles a ser removida
                            expiredMobileIDs.Add(this.loggedMobiles[item.User]);

                            // Eliminar los moviles expirados de la lista de conectados
                            loggedMobiles.Remove(item.User);
                        }
                    }

                    // Eliminar los moviles expirados de la lista de conectados
                    foreach (string item in expiredMobileIDs)
                    {
                        Debug.WriteLine("Removing session " + item);
                        connectionTimeFrames.Remove(item);
                        activeSessions.Remove(item);
                    }
                }
            }
        }
        
        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Obtener todos los privilegios con un usuario en particular
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del usuario.</param>
        /// <param name="userEntity">La entidad del usuario.</param>
        private void UsersPermissions(string sessionID, UserEntity userEntity)
        {
            GroupEntity group;
            Collection<UserGroupEntity> usersGroup;
            Collection<PermissionEntity> permission;

            usersGroup = userEntity.UserGroup;

            if (usersGroup.Count > 0)
            {
                foreach (UserGroupEntity userGroupItem in usersGroup)
                {
                    group = userGroupItem.Group;
                    permission = group.Permissions;

                    foreach (PermissionEntity permissionItem in permission)
                    {
                        PermissionList(sessionID, permissionItem);
                    }
                }
            }
            else
            {
                Debug.WriteLine("The user doesn't have an associated group.");
            }
        }

        /// <summary>
        /// Calcula los permisos para un usuario
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del usuario.</param>
        /// <param name="permissionItem">Representa un permiso especifico para una clase de negocios.</param>
        private void PermissionList(string sessionID, PermissionEntity permissionItem)
        {
            List<PermissionEntity> pair;
            bool isBusinessClassNotFound = true;

            if (permissionDictionary.TryGetValue(sessionID, out pair))
            {
                foreach (PermissionEntity item in pair)
                {
                    if (item.BusinessClassName == permissionItem.BusinessClassName)
                    {
                        item.AllowDelete = item.AllowDelete || permissionItem.AllowDelete;
                        item.AllowNew = item.AllowNew || permissionItem.AllowNew;
                        item.AllowRead = item.AllowRead || permissionItem.AllowRead;
                        item.AllowUpdate = item.AllowUpdate || permissionItem.AllowUpdate;
                        isBusinessClassNotFound = false;
                    }
                }
                if (isBusinessClassNotFound)
                {
                    pair.Add(permissionItem);
                }
            }
            else
            {
                pair = new List<PermissionEntity>();
                pair.Add(permissionItem);
                permissionDictionary.Add(sessionID, pair);
            }
        }
                
        /// <summary>
        /// Valida si el identificador de sesión de un usuario es valido.
        /// </summary>
        /// <param name="sessionId">Identificador de sesión del usuario.</param>
        /// <returns>Devuelve verdadero o falso de acuerdo a los permisos asignados al usuario.</returns>
        private bool ValidateSessionID(string sessionID)
        {
            bool wasSuccesful;

            if (activeSessions.ContainsKey(sessionID))
            {
                wasSuccesful = true;
            }
            else
            {
                wasSuccesful = false;
            }

            return wasSuccesful;
        }

        /// <summary>
        /// Renueva la duración de la sesión de un cliente.
        /// </summary>
        /// <param name="userName">Nombre del usuario</param>
        /// <returns>Verdadero si tiene éxito.</returns>
        internal bool RenewTimestamp(string userName)
        {
            MobileTimestampPair pair;
            bool wasSuccesful;
            string sessionID;

            wasSuccesful = loggedMobiles.TryGetValue(userName, out sessionID);

            if (wasSuccesful)
            {
                lock (connectionTimeFrames)
                {
                    // Si se elimino la conexión renovarla y notificar
                    if (connectionTimeFrames.TryGetValue(sessionID, out pair))
                    {
                        pair.Timestamp = DateTime.Now;
                        wasSuccesful = true;
                    }
                    else
                    {
                        wasSuccesful = false;
                    }
                }
            }

            return wasSuccesful;
        }

        /// <summary>
        /// Validar si un movil ya posee una sesión.
        /// </summary>
        /// <param name="userName">Nombre del usuario.</param>
        /// <returns>Devuelve verdadero o falso de acuerdo a los permisos asignados al usuario.</returns>
        internal bool MobileHasAuthenticated(string userName)
        {
            MobileTimestampPair pair;
            string sessionID;
            bool loggedMobile;

            loggedMobile = loggedMobiles.TryGetValue(userName, out sessionID);

            if (loggedMobile)
            {
                lock (connectionTimeFrames)
                {
                    loggedMobile = connectionTimeFrames.TryGetValue(sessionID, out pair);
                }
            }

            return loggedMobile;
        }

        /// <summary>
        /// Valida si un usuario/cliente usando el hash de la password
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <param name="password">El password.</param>
        /// <param name="isCustomer">Indica si se trata de un cliente.</param>
        /// <returns>Devuelve un string de session para el cliente o usuario.</returns>
        internal string ValidateLogin(string username, byte[] passwordHash, bool isCustomer)
        {
            string sessionID;

            if (isCustomer)
            {
                sessionID = ValidateCustomer(username, System.Convert.ToBase64String(passwordHash));
            }
            else
            {
                sessionID = ValidateUser(username, System.Convert.ToBase64String(passwordHash));
            }

            return sessionID;
        }
        
        #endregion

        #endregion        
    }
}
