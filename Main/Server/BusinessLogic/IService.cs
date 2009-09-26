using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>IService</c> es un contrato de servicio web para procesar ServiceEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface IService
	{
		/// <summary>
		/// Función para guardar ServiceEntity en la base de datos.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el ServiceEntity se guardo con exito, el mismo ServiceEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		ServiceEntity Save(ServiceEntity serviceEntity, string session);
		/// <summary>
		/// Función para eliminar un ServiceEntity de la base de datos.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el ServiceEntity fue eliminado con éxito, el mismo ServiceEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		ServiceEntity Delete(ServiceEntity serviceEntity, string session);
		/// <summary>
		/// Obtiene un serviceEntity específico
		/// </summary>
		/// <param name="id">id del ServiceEntity a cargar</param>
		/// <returns>Un ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		ServiceEntity GetService(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de serviceEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<ServiceEntity> GetAllService(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de ServiceEntity</returns>
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
		Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de ServiceEntity</returns>
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
		Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un ServiceEntity antes de ser guardado.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="serviceEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(ServiceEntity service);
	} 

}

