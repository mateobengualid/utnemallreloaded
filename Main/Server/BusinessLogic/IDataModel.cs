using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IDataModel</c> es un contrato de servicio web para procesar DataModelEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IDataModel
	{
		/// <summary>
		/// Función para guardar DataModelEntity en la base de datos.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el DataModelEntity se guardo con exito, el mismo DataModelEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		DataModelEntity Save(DataModelEntity dataModelEntity, string session);
		/// <summary>
		/// Función para eliminar un DataModelEntity de la base de datos.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el DataModelEntity fue eliminado con éxito, el mismo DataModelEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		DataModelEntity Delete(DataModelEntity dataModelEntity, string session);
		/// <summary>
		/// Obtiene un dataModelEntity específico
		/// </summary>
		/// <param name="id">id del DataModelEntity a cargar</param>
		/// <returns>Un DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		DataModelEntity GetDataModel(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de dataModelEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de DataModelEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<DataModelEntity> GetAllDataModel(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los dataModelEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del dataModelEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de DataModelEntity</returns>
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
		Collection<DataModelEntity> GetDataModelWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los dataModelEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del dataModelEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de DataModelEntity</returns>
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
		Collection<DataModelEntity> GetDataModelWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un DataModelEntity antes de ser guardado.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="dataModelEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(DataModelEntity dataModel);
	} 

}

