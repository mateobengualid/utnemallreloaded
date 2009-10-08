using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IUserAction</c> business contract to process UserActionEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IUserAction
	{
		/// <summary>
		/// Function to save a UserActionEntity to the database.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionEntity was saved successfully, the same UserActionEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[System.ServiceModel.OperationContract]
		UserActionEntity Save(UserActionEntity userActionEntity, string session);
		/// <summary>
		/// Function to delete a UserActionEntity from database.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionEntity was deleted successfully, the same UserActionEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionEntity Delete(UserActionEntity userActionEntity, string session);
		/// <summary>
		/// Get an specific userActionEntity
		/// </summary>
		/// <param name="id">id of the UserActionEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserActionEntity GetUserAction(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all userActionEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserActionEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionEntity> GetAllUserAction(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all userActionEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionEntity> GetUserActionWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all userActionEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserActionEntity> GetUserActionWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a UserActionEntity before it's saved.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionEntity was deleted successfully, the same UserActionEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(UserActionEntity userAction);
	} 

}

