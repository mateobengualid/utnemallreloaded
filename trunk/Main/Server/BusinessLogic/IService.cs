using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IService</c> business contract to process ServiceEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IService
	{
		/// <summary>
		/// Function to save a ServiceEntity to the database.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the ServiceEntity was saved successfully, the same ServiceEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		ServiceEntity Save(ServiceEntity serviceEntity, string session);
		/// <summary>
		/// Function to delete a ServiceEntity from database.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		ServiceEntity Delete(ServiceEntity serviceEntity, string session);
		/// <summary>
		/// Get an specific serviceEntity
		/// </summary>
		/// <param name="id">id of the ServiceEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		ServiceEntity GetService(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all serviceEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<ServiceEntity> GetAllService(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all serviceEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of serviceEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of ServiceEntity</returns>
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
		Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all serviceEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of serviceEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of ServiceEntity</returns>
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
		Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a ServiceEntity before it's saved.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(ServiceEntity service);
	} 

}

