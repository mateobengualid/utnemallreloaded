using System;
using UtnEmall.Server.BusinessLogic;
using System.ServiceModel;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Interfase de administración de usuarios y clientes.
    /// </summary>
    [ServiceContract]
    interface ISessionManager
    {
        /// <summary>
        /// Finaliza la sesión de un usuario o cliente.
        /// </summary>
        /// <param name="id">El id de sesión.</param>
        /// <returns>true si el fin de la sesión se realizó con éxito.</returns>
        [OperationContract]
        bool UserLogOff(string sessionId);
        /// <summary>
        /// Valida el nombre de usuario y contraseña.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="passwordHash">Contraseña del usuario.</param>
        /// <returns>Cadena de texto no vacía si se validó con éxito.</returns>
        [OperationContract]
        string ValidateCustomer(string userName, string passwordHash);
        /// <summary>
        /// Valida el nombre de usuario y contraseña
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="passwordHash">Contraseña de usuario.</param>
        /// <returns>Cadena de texto no vacía si se validó exitosamente.</returns>
        [OperationContract]
        string ValidateUser(string userName, string passwordHash);
        /// <summary>
        /// Retorna un objeto de cliente desde la sesión
        /// </summary>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>Un objeto de cliente o null.</returns>
        [OperationContract]
        CustomerEntity GetCustomerFromSession(string sessionId);
        /// <summary>
        /// Retorna el usuario asociado a una sesión
        /// </summary>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>Un objeto de usuario o null.</returns>
        [OperationContract]
        UserEntity GetUserFromSession(string sessionId);
        /// <summary>
        /// Verifica si un id de sesión pertenece a un cliente
        /// </summary>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>true si la sesión pertenece a un cliente</returns>
        [OperationContract]
        bool IsCustomerSession(string sessionId);
        /// <summary>
        /// Verifica si e id de sesión pertenece a un usuario
        /// </summary>
        /// <param name="sessionId">Identificador de sesión de usuario.</param>
        /// <returns>true si la sesión pertenece al usuario</returns>
        [OperationContract]
        bool IsUserSession(string sessionId);
        /// <summary>
        /// Retorna una tienda asociada a una sesión de usuario.
        /// </summary>
        /// <param name="SessionId">Identificador de sesión.</param>
        /// <returns>La tienda asociada o null.</returns>
        [OperationContract]
        StoreEntity StoreFromUserSession(string sessionId);
    }
}
