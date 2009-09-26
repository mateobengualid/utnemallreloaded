using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IRegisterAssociation</c> es un contrato de servicio web para procesar RegisterAssociationEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IRegisterAssociation
	{
		/// <summary>
		/// Función para guardar RegisterAssociationEntity en la base de datos.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el RegisterAssociationEntity se guardo con exito, el mismo RegisterAssociationEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[System.ServiceModel.OperationContract]
		RegisterAssociationEntity Save(RegisterAssociationEntity registerAssociationEntity, string session);
		/// <summary>
		/// Función para eliminar un RegisterAssociationEntity de la base de datos.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el RegisterAssociationEntity fue eliminado con éxito, el mismo RegisterAssociationEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		RegisterAssociationEntity Delete(RegisterAssociationEntity registerAssociationEntity, string session);
		/// <summary>
		/// Obtiene un registerAssociationEntity específico
		/// </summary>
		/// <param name="id">id del RegisterAssociationEntity a cargar</param>
		/// <returns>Un RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		RegisterAssociationEntity GetRegisterAssociation(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de registerAssociationEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de RegisterAssociationEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<RegisterAssociationEntity> GetAllRegisterAssociation(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los registerAssociationEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del registerAssociationEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<RegisterAssociationEntity> GetRegisterAssociationWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los registerAssociationEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del registerAssociationEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<RegisterAssociationEntity> GetRegisterAssociationWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un RegisterAssociationEntity antes de ser guardado.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="registerAssociationEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(RegisterAssociationEntity registerAssociation);
	} 

}

