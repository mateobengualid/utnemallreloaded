using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IUser</c> business contract to process UserEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IUser
	{
		/// <summary>
		/// Function to save a UserEntity to the database.
		/// </summary>
		/// <param name="userEntity">UserEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserEntity was saved successfully, the same UserEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[System.ServiceModel.OperationContract]
		UserEntity Save(UserEntity userEntity, string session);
		/// <summary>
		/// Function to delete a UserEntity from database.
		/// </summary>
		/// <param name="userEntity">UserEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserEntity was deleted successfully, the same UserEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserEntity Delete(UserEntity userEntity, string session);
		/// <summary>
		/// Get an specific userEntity
		/// </summary>
		/// <param name="id">id of the UserEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		UserEntity GetUser(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all userEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserEntity> GetAllUser(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all userEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserEntity> GetUserWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all userEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<UserEntity> GetUserWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a UserEntity before it's saved.
		/// </summary>
		/// <param name="userEntity">UserEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserEntity was deleted successfully, the same UserEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(UserEntity user);
	} 

}

