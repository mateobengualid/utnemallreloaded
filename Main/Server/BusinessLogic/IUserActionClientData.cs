using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IUserActionClientData</c> business contract to process UserActionClientDataEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IUserActionClientData
	{
		/// <summary>
		/// Function to save a UserActionClientDataEntity to the database.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionClientDataEntity was saved successfully, the same UserActionClientDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[System.ServiceModel.OperationContract]
		UserActionClientDataEntity Save(UserActionClientDataEntity userActionClientDataEntity, string session);
		/// <summary>
		/// Function to delete a UserActionClientDataEntity from database.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionClientDataEntity Delete(UserActionClientDataEntity userActionClientDataEntity, string session);
		/// <summary>
		/// Get an specific userActionClientDataEntity
		/// </summary>
		/// <param name="id">id of the UserActionClientDataEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all userActionClientDataEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all userActionClientDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionClientDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all userActionClientDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionClientDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a UserActionClientDataEntity before it's saved.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(UserActionClientDataEntity userActionClientData);
	} 

}

