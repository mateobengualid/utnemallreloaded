using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IStore</c> business contract to process StoreEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IStore
	{
		/// <summary>
		/// Function to save a StoreEntity to the database.
		/// </summary>
		/// <param name="storeEntity">StoreEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the StoreEntity was saved successfully, the same StoreEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		StoreEntity Save(StoreEntity storeEntity, string session);
		/// <summary>
		/// Function to delete a StoreEntity from database.
		/// </summary>
		/// <param name="storeEntity">StoreEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		StoreEntity Delete(StoreEntity storeEntity, string session);
		/// <summary>
		/// Get an specific storeEntity
		/// </summary>
		/// <param name="id">id of the StoreEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		StoreEntity GetStore(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all storeEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all StoreEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<StoreEntity> GetAllStore(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all storeEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of storeEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of StoreEntity</returns>
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
		Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all storeEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of storeEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of StoreEntity</returns>
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
		Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a StoreEntity before it's saved.
		/// </summary>
		/// <param name="storeEntity">StoreEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(StoreEntity store);
	} 

}

