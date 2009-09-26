using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>ICustomer</c> es un contrato de servicio web para procesar CustomerEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface ICustomer
	{
		/// <summary>
		/// Función para guardar CustomerEntity en la base de datos.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el CustomerEntity se guardo con exito, el mismo CustomerEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerEntity Save(CustomerEntity customerEntity, string session);
		/// <summary>
		/// Función para eliminar un CustomerEntity de la base de datos.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el CustomerEntity fue eliminado con éxito, el mismo CustomerEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerEntity Delete(CustomerEntity customerEntity, string session);
		/// <summary>
		/// Obtiene un customerEntity específico
		/// </summary>
		/// <param name="id">id del CustomerEntity a cargar</param>
		/// <returns>Un CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerEntity GetCustomer(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de customerEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CustomerEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CustomerEntity> GetAllCustomer(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los customerEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del customerEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de CustomerEntity</returns>
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
		Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los customerEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del customerEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de CustomerEntity</returns>
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
		Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un CustomerEntity antes de ser guardado.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="customerEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CustomerEntity customer);
	} 

}

