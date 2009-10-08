using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>ICustomer</c> business contract to process CustomerEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface ICustomer
	{
		/// <summary>
		/// Function to save a CustomerEntity to the database.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerEntity was saved successfully, the same CustomerEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerEntity Save(CustomerEntity customerEntity, string session);
		/// <summary>
		/// Function to delete a CustomerEntity from database.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerEntity Delete(CustomerEntity customerEntity, string session);
		/// <summary>
		/// Get an specific customerEntity
		/// </summary>
		/// <param name="id">id of the CustomerEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerEntity GetCustomer(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all customerEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CustomerEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CustomerEntity> GetAllCustomer(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all customerEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all customerEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a CustomerEntity before it's saved.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CustomerEntity customer);
	} 

}

