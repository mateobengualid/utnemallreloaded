using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>ITable</c> business contract to process TableEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface ITable
	{
		/// <summary>
		/// Function to save a TableEntity to the database.
		/// </summary>
		/// <param name="tableEntity">TableEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the TableEntity was saved successfully, the same TableEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[System.ServiceModel.OperationContract]
		TableEntity Save(TableEntity tableEntity, string session);
		/// <summary>
		/// Function to delete a TableEntity from database.
		/// </summary>
		/// <param name="tableEntity">TableEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the TableEntity was deleted successfully, the same TableEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		TableEntity Delete(TableEntity tableEntity, string session);
		/// <summary>
		/// Get an specific tableEntity
		/// </summary>
		/// <param name="id">id of the TableEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		TableEntity GetTable(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all tableEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all TableEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<TableEntity> GetAllTable(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all tableEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of tableEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<TableEntity> GetTableWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all tableEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of tableEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<TableEntity> GetTableWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a TableEntity before it's saved.
		/// </summary>
		/// <param name="tableEntity">TableEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the TableEntity was deleted successfully, the same TableEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(TableEntity table);
	} 

}

