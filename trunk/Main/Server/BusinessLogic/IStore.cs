using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IStore</c> es un contrato de servicio web para procesar StoreEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IStore
	{
		/// <summary>
		/// Función para guardar StoreEntity en la base de datos.
		/// </summary>
		/// <param name="storeEntity">StoreEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el StoreEntity se guardo con exito, el mismo StoreEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		StoreEntity Save(StoreEntity storeEntity, string session);
		/// <summary>
		/// Función para eliminar un StoreEntity de la base de datos.
		/// </summary>
		/// <param name="storeEntity">StoreEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el StoreEntity fue eliminado con éxito, el mismo StoreEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		StoreEntity Delete(StoreEntity storeEntity, string session);
		/// <summary>
		/// Obtiene un storeEntity específico
		/// </summary>
		/// <param name="id">id del StoreEntity a cargar</param>
		/// <returns>Un StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		StoreEntity GetStore(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de storeEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de StoreEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<StoreEntity> GetAllStore(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los storeEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del storeEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los storeEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del storeEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un StoreEntity antes de ser guardado.
		/// </summary>
		/// <param name="storeEntity">StoreEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="storeEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(StoreEntity store);
	} 

}

