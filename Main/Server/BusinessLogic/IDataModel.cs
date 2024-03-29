﻿using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>IDataModel</c> business contract to process DataModelEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface IDataModel
	{
		/// <summary>
		/// Function to save a DataModelEntity to the database.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the DataModelEntity was saved successfully, the same DataModelEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		DataModelEntity Save(DataModelEntity dataModelEntity, string session);
		/// <summary>
		/// Function to delete a DataModelEntity from database.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the DataModelEntity was deleted successfully, the same DataModelEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		DataModelEntity Delete(DataModelEntity dataModelEntity, string session);
		/// <summary>
		/// Get an specific dataModelEntity
		/// </summary>
		/// <param name="id">id of the DataModelEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		DataModelEntity GetDataModel(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all dataModelEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all DataModelEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<DataModelEntity> GetAllDataModel(bool loadRelation, string session);
		/// <summary>
		/// Get collection of all dataModelEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of dataModelEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of DataModelEntity</returns>
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
		Collection<DataModelEntity> GetDataModelWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Get collection of all dataModelEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of dataModelEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of DataModelEntity</returns>
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
		Collection<DataModelEntity> GetDataModelWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Function to validate a DataModelEntity before it's saved.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the DataModelEntity was deleted successfully, the same DataModelEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(DataModelEntity dataModel);
	} 

}

