using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IGroup</c> business contract to process GroupEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IGroup
	{
		/// <summary>
		/// Function to save a GroupEntity to the database.
		/// </summary>
		/// <param name="groupEntity">GroupEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the GroupEntity was saved successfully, the same GroupEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[System.ServiceModel.OperationContract]
		GroupEntity Save(GroupEntity groupEntity, string session);
		/// <summary>
		/// Function to delete a GroupEntity from database.
		/// </summary>
		/// <param name="groupEntity">GroupEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the GroupEntity was deleted successfully, the same GroupEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		GroupEntity Delete(GroupEntity groupEntity, string session);
		/// <summary>
		/// Get an specific groupEntity
		/// </summary>
		/// <param name="id">id of the GroupEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		GroupEntity GetGroup(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all groupEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all GroupEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<GroupEntity> GetAllGroup(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all groupEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of groupEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<GroupEntity> GetGroupWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all groupEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of groupEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<GroupEntity> GetGroupWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a GroupEntity before it's saved.
		/// </summary>
		/// <param name="groupEntity">GroupEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the GroupEntity was deleted successfully, the same GroupEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(GroupEntity group);
	} 

}

