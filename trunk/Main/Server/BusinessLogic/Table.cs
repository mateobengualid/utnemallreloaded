using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>Table</c> implement business logic to process TableEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class Table: UtnEmall.Server.BusinessLogic.ITable
	{
		private TableDataAccess tableDataAccess; 
		public  Table()
		{
			tableDataAccess = new TableDataAccess();
		} 

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
		public TableEntity Save(TableEntity tableEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(tableEntity))
			{
				return tableEntity;
			}
			try 
			{
				// Save tableEntity using data access object
				tableDataAccess.Save(tableEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

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
		public TableEntity Delete(TableEntity tableEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (tableEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete tableEntity using data access object
				tableDataAccess.Delete(tableEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific tableEntity
		/// </summary>
		/// <param name="id">id of the TableEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public TableEntity GetTable(int id, string session)
		{
			return GetTable(id, true, session);
		} 

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
		public TableEntity GetTable(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return tableDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all tableEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all TableEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<TableEntity> GetAllTable(string session)
		{
			return GetAllTable(true, session);
		} 

		/// <summary>
		/// Get collection of all tableEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all TableEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<TableEntity> GetAllTable(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return tableDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all tableEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of tableEntity</param>
		/// <param name="expValue">pattern</param>
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
		public Collection<TableEntity> GetTableWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetTableWhere(propertyName, expValue, true, operatorType, session);
		} 

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
		public Collection<TableEntity> GetTableWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return tableDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all tableEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of tableEntity</param>
		/// <param name="expValue">pattern</param>
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
		public Collection<TableEntity> GetTableWhereEqual(string propertyName, object expValue, string session)
		{
			return GetTableWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

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
		public Collection<TableEntity> GetTableWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetTableWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

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
		public bool Validate(TableEntity table)
		{
			bool result = true;

			if (table == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
			if (String.IsNullOrEmpty(table.Name))
			{
				table.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			return result;
		} 

	} 

}

