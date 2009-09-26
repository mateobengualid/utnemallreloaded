using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IGroup</c> es un contrato de servicio web para procesar GroupEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IGroup
	{
		/// <summary>
		/// Función para guardar GroupEntity en la base de datos.
		/// </summary>
		/// <param name="groupEntity">GroupEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el GroupEntity se guardo con exito, el mismo GroupEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[System.ServiceModel.OperationContract]
		GroupEntity Save(GroupEntity groupEntity, string session);
		/// <summary>
		/// Función para eliminar un GroupEntity de la base de datos.
		/// </summary>
		/// <param name="groupEntity">GroupEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el GroupEntity fue eliminado con éxito, el mismo GroupEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		GroupEntity Delete(GroupEntity groupEntity, string session);
		/// <summary>
		/// Obtiene un groupEntity específico
		/// </summary>
		/// <param name="id">id del GroupEntity a cargar</param>
		/// <returns>Un GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		GroupEntity GetGroup(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de groupEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de GroupEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<GroupEntity> GetAllGroup(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los groupEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del groupEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<GroupEntity> GetGroupWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los groupEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del groupEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<GroupEntity> GetGroupWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un GroupEntity antes de ser guardado.
		/// </summary>
		/// <param name="groupEntity">GroupEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="groupEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(GroupEntity group);
	} 

}

