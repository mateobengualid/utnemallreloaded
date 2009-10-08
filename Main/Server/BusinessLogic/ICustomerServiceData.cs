using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>ICustomerServiceData</c> business contract to process CustomerServiceDataEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface ICustomerServiceData
	{
		/// <summary>
		/// Function to save a CustomerServiceDataEntity to the database.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerServiceDataEntity was saved successfully, the same CustomerServiceDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerServiceDataEntity Save(CustomerServiceDataEntity customerServiceDataEntity, string session);
		/// <summary>
		/// Function to delete a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerServiceDataEntity was deleted successfully, the same CustomerServiceDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerServiceDataEntity Delete(CustomerServiceDataEntity customerServiceDataEntity, string session);
		/// <summary>
		/// Get an specific customerServiceDataEntity
		/// </summary>
		/// <param name="id">id of the CustomerServiceDataEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CustomerServiceDataEntity GetCustomerServiceData(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all customerServiceDataEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CustomerServiceDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CustomerServiceDataEntity> GetAllCustomerServiceData(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all customerServiceDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerServiceDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerServiceDataEntity</returns>
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
		Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all customerServiceDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerServiceDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerServiceDataEntity</returns>
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
		Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a CustomerServiceDataEntity before it's saved.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerServiceDataEntity was deleted successfully, the same CustomerServiceDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CustomerServiceDataEntity customerServiceData);
	} 

}

