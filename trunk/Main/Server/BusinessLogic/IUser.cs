using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IUser</c> es un contrato de servicio web para procesar UserEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IUser
	{
		/// <summary>
		/// Función para guardar UserEntity en la base de datos.
		/// </summary>
		/// <param name="userEntity">UserEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el UserEntity se guardo con exito, el mismo UserEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[System.ServiceModel.OperationContract]
		UserEntity Save(UserEntity userEntity, string session);
		/// <summary>
		/// Función para eliminar un UserEntity de la base de datos.
		/// </summary>
		/// <param name="userEntity">UserEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el UserEntity fue eliminado con éxito, el mismo UserEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserEntity Delete(UserEntity userEntity, string session);
		/// <summary>
		/// Obtiene un userEntity específico
		/// </summary>
		/// <param name="id">id del UserEntity a cargar</param>
		/// <returns>Un UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserEntity GetUser(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de userEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserEntity> GetAllUser(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los userEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserEntity> GetUserWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los userEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserEntity> GetUserWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un UserEntity antes de ser guardado.
		/// </summary>
		/// <param name="userEntity">UserEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="userEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(UserEntity user);
	} 

}

