using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IUserAction</c> es un contrato de servicio web para procesar UserActionEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IUserAction
	{
		/// <summary>
		/// Función para guardar UserActionEntity en la base de datos.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el UserActionEntity se guardo con exito, el mismo UserActionEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[System.ServiceModel.OperationContract]
		UserActionEntity Save(UserActionEntity userActionEntity, string session);
		/// <summary>
		/// Función para eliminar un UserActionEntity de la base de datos.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el UserActionEntity fue eliminado con éxito, el mismo UserActionEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionEntity Delete(UserActionEntity userActionEntity, string session);
		/// <summary>
		/// Obtiene un userActionEntity específico
		/// </summary>
		/// <param name="id">id del UserActionEntity a cargar</param>
		/// <returns>Un UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionEntity GetUserAction(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de userActionEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserActionEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionEntity> GetAllUserAction(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los userActionEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionEntity> GetUserActionWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los userActionEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionEntity> GetUserActionWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un UserActionEntity antes de ser guardado.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="userActionEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(UserActionEntity userAction);
	} 

}

