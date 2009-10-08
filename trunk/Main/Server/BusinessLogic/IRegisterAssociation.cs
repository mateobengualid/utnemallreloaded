using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IRegisterAssociation</c> business contract to process RegisterAssociationEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IRegisterAssociation
	{
		/// <summary>
		/// Function to save a RegisterAssociationEntity to the database.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the RegisterAssociationEntity was saved successfully, the same RegisterAssociationEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[System.ServiceModel.OperationContract]
		RegisterAssociationEntity Save(RegisterAssociationEntity registerAssociationEntity, string session);
		/// <summary>
		/// Function to delete a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the RegisterAssociationEntity was deleted successfully, the same RegisterAssociationEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		RegisterAssociationEntity Delete(RegisterAssociationEntity registerAssociationEntity, string session);
		/// <summary>
		/// Get an specific registerAssociationEntity
		/// </summary>
		/// <param name="id">id of the RegisterAssociationEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		RegisterAssociationEntity GetRegisterAssociation(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all registerAssociationEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all RegisterAssociationEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<RegisterAssociationEntity> GetAllRegisterAssociation(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all registerAssociationEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of registerAssociationEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<RegisterAssociationEntity> GetRegisterAssociationWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all registerAssociationEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of registerAssociationEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<RegisterAssociationEntity> GetRegisterAssociationWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a RegisterAssociationEntity before it's saved.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the RegisterAssociationEntity was deleted successfully, the same RegisterAssociationEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(RegisterAssociationEntity registerAssociation);
	} 

}

