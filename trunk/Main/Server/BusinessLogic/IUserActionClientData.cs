using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IUserActionClientData</c> es un contrato de servicio web para procesar UserActionClientDataEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IUserActionClientData
	{
		/// <summary>
		/// Función para guardar UserActionClientDataEntity en la base de datos.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el UserActionClientDataEntity se guardo con exito, el mismo UserActionClientDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[System.ServiceModel.OperationContract]
		UserActionClientDataEntity Save(UserActionClientDataEntity userActionClientDataEntity, string session);
		/// <summary>
		/// Función para eliminar un UserActionClientDataEntity de la base de datos.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el UserActionClientDataEntity fue eliminado con éxito, el mismo UserActionClientDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionClientDataEntity Delete(UserActionClientDataEntity userActionClientDataEntity, string session);
		/// <summary>
		/// Obtiene un userActionClientDataEntity específico
		/// </summary>
		/// <param name="id">id del UserActionClientDataEntity a cargar</param>
		/// <returns>Un UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de userActionClientDataEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un UserActionClientDataEntity antes de ser guardado.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="userActionClientDataEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(UserActionClientDataEntity userActionClientData);
	} 

}

