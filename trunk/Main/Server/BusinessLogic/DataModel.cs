using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>DataModel</c> implement business logic to process DataModelEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class DataModel: UtnEmall.Server.BusinessLogic.IDataModel
	{
		private DataModelDataAccess datamodelDataAccess; 
		public  DataModel()
		{
			datamodelDataAccess = new DataModelDataAccess();
		} 

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
		public DataModelEntity Save(DataModelEntity dataModelEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(dataModelEntity))
			{
				return dataModelEntity;
			}
			try 
			{
				// Check that the service is not deployed
				if (dataModelEntity.Deployed)
				{
					dataModelEntity.Errors.Add(new Error("DataModel Deployed", "", "The data model is already deployed. Can not be saved."));
					return dataModelEntity;
				}
				// Check that there isn't related custom services

				if (dataModelEntity.Id > 0)
				{
					CustomerServiceDataDataAccess customerServiceData = new CustomerServiceDataDataAccess();
					// Get all customer services where IdDataModel is the same as us

					int referencedServices = customerServiceData.LoadWhere(CustomerServiceDataEntity.DBIdDataModel, dataModelEntity.Id, false, OperatorType.Equal).Count;
					// If there are customer services it is an error

					if (referencedServices > 0)
					{
						dataModelEntity.Errors.Add(new Error("DataModel Deployed", "", "The data model has related customer services. Can not be updated."));
						return dataModelEntity;
					}
				}
				// Save dataModelEntity using data access object

				datamodelDataAccess.Save(dataModelEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

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
		public DataModelEntity Delete(DataModelEntity dataModelEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (dataModelEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Check that the service is not deployed
				if (dataModelEntity.Deployed)
				{
					dataModelEntity.Errors.Add(new Error("DataModel Deployed", "", "The data model is already deployed. Can not be deleted."));
					return dataModelEntity;
				}
				// Delete dataModelEntity using data access object

				datamodelDataAccess.Delete(dataModelEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific dataModelEntity
		/// </summary>
		/// <param name="id">id of the DataModelEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public DataModelEntity GetDataModel(int id, string session)
		{
			return GetDataModel(id, true, session);
		} 

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
		public DataModelEntity GetDataModel(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return datamodelDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all dataModelEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all DataModelEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<DataModelEntity> GetAllDataModel(string session)
		{
			return GetAllDataModel(true, session);
		} 

		/// <summary>
		/// Get collection of all dataModelEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all DataModelEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<DataModelEntity> GetAllDataModel(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return datamodelDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all dataModelEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of dataModelEntity</param>
		/// <param name="expValue">pattern</param>
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
		public Collection<DataModelEntity> GetDataModelWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetDataModelWhere(propertyName, expValue, true, operatorType, session);
		} 

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
		public Collection<DataModelEntity> GetDataModelWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return datamodelDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all dataModelEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of dataModelEntity</param>
		/// <param name="expValue">pattern</param>
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
		public Collection<DataModelEntity> GetDataModelWhereEqual(string propertyName, object expValue, string session)
		{
			return GetDataModelWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

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
		public Collection<DataModelEntity> GetDataModelWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetDataModelWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

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
		public bool Validate(DataModelEntity dataModel)
		{
			bool result = true;

			if (dataModel == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
			if (dataModel.Tables == null)
			{
				dataModel.Errors.Add(new Error("Tables", "Tables", "El nombre de la tabla no puede estar vacío"));
				result = false;
			}
			if (!ValidateTables(dataModel.Tables))
			{
				result = false;
			}
			if (dataModel.Relations == null)
			{
				dataModel.Errors.Add(new Error("Relations", "Relations", "Las relaciones no pueden estar vacías"));
				result = false;
			}
			if (!ValidateRelations(dataModel.Relations))
			{
				result = false;
			}
			return result;
		} 

		private static bool ValidateTables(Collection<TableEntity> Tables)
		{
			bool result = true;

			for (int  i = 0; i < Tables.Count; i++)
			{
				TableEntity item = Tables[i];
				if (String.IsNullOrEmpty(item.Name))
				{
					item.Errors.Add(new Error("Name", "Name", "El nombre de tabla no puede estar vacío"));
					result = false;
				}

				if (item.Fields == null)
				{
					item.Errors.Add(new Error("Fields", "Fields", "El nombre de campo no puede estar vacío"));
					result = false;
				}
				if (!ValidateFields(item.Fields))
				{
					result = false;
				}
			}
			return result;
		} 

		private static bool ValidateRelations(Collection<RelationEntity> Relations)
		{
			bool result = true;

			for (int  i = 0; i < Relations.Count; i++)
			{
				RelationEntity item = Relations[i];
				if (item.RelationType < 0)
				{
					item.Errors.Add(new Error("RelationType", "RelationType", "El tipo de relación no puede ser cero"));
					result = false;
				}
				if (item.IdTarget < 0)
				{
					item.Errors.Add(new Error("IdTarget", "IdTarget", "IdTarget no puede estar vacío"));
					result = false;
				}
				if (item.IdSource < 0)
				{
					item.Errors.Add(new Error("IdSource", "IdSource", "IdSource no puede estar vacío"));
					result = false;
				}
			}
			return result;
		} 

		private static bool ValidateFields(Collection<FieldEntity> Fields)
		{
			bool result = true;

			for (int  i = 0; i < Fields.Count; i++)
			{
				FieldEntity item = Fields[i];
				if (String.IsNullOrEmpty(item.Name))
				{
					item.Errors.Add(new Error("Name", "Name", "El nombre de campo no puede estar vacío"));
					result = false;
				}

				if (item.DataType < 0)
				{
					item.Errors.Add(new Error("DataType", "DataType", "El tipo de dato no puede estar vacío"));
					result = false;
				}
			}
			return result;
		} 

	} 

}

