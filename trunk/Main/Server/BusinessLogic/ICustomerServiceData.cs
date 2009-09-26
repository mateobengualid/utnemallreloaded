using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>ICustomerServiceData</c> es un contrato de servicio web para procesar CustomerServiceDataEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface ICustomerServiceData
	{
		/// <summary>
		/// Función para guardar CustomerServiceDataEntity en la base de datos.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el CustomerServiceDataEntity se guardo con exito, el mismo CustomerServiceDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerServiceDataEntity Save(CustomerServiceDataEntity customerServiceDataEntity, string session);
		/// <summary>
		/// Función para eliminar un CustomerServiceDataEntity de la base de datos.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el CustomerServiceDataEntity fue eliminado con éxito, el mismo CustomerServiceDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerServiceDataEntity Delete(CustomerServiceDataEntity customerServiceDataEntity, string session);
		/// <summary>
		/// Obtiene un customerServiceDataEntity específico
		/// </summary>
		/// <param name="id">id del CustomerServiceDataEntity a cargar</param>
		/// <returns>Un CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerServiceDataEntity GetCustomerServiceData(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de customerServiceDataEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CustomerServiceDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CustomerServiceDataEntity> GetAllCustomerServiceData(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los customerServiceDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del customerServiceDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de CustomerServiceDataEntity</returns>
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
		Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los customerServiceDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del customerServiceDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de CustomerServiceDataEntity</returns>
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
		Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un CustomerServiceDataEntity antes de ser guardado.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="customerServiceDataEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CustomerServiceDataEntity customerServiceData);
	} 

}

