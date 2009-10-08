using System.Data;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Data.Common;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.DataModel
{

	/// <summary>
	/// The <c>ServiceDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class ServiceDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,ServiceEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ServiceDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  ServiceDataAccess()
		{
			dataAccess = DataAccessConnection.Instance;
			if (!dbChecked)
			{
				DbChecked();
			}

			if (properties == null)
			{
				SetProperties();
			}

			inMemoryEntities = new Dictionary<int,ServiceEntity>();
		} 

		/// <summary>
		/// set the connection and the transaction to the object, in the case
		/// that a global transaction is running.
		/// </summary>
		/// <param name="connection">The IDbConnection connection to the database</param>
		/// <param name="transaction">The global IDbTransaction transaction</param>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void SetConnectionObjects(IDbConnection connection, IDbTransaction transaction)
		{
			if (connection == null)
			{
				throw new ArgumentException("The connection cannot be null");
			}
			this.dbConnection = connection;
			this.dbTransaction = transaction;
			// FIXME : The name of this flag is not always apropiated

			this.isGlobalTransaction = true;
		} 

		/// <summary>
		/// Function to load a ServiceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ServiceEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((ServiceEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			ServiceEntity service = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				service = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, service);
			}
			else 
			{
				bool closeConnection = false;
				try 
				{
					// Open a new connection if it isn't on a transaction
					if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
					{
						closeConnection = true;
						dbConnection = dataAccess.GetNewConnection();
						dbConnection.Open();
					}

					string cmdText = "SELECT idService, name, description, webAccess, relativePathAssembly, pathAssemblyServer, active, global, image, website, deployed, updated, idMall, idStore, idCustomerServiceData, startDate, stopDate, timestamp FROM [Service] WHERE idService = @idService";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					service = new ServiceEntity();

					if (reader.Read())
					{
						// Load fields of entity
						service.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							service.Name = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							service.Description = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							service.WebAccess = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							service.RelativePathAssembly = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							service.PathAssemblyServer = reader.GetString(5);
						}

						service.Active = reader.GetBoolean(6);
						service.Global = reader.GetBoolean(7);
						if (!reader.IsDBNull(8))
						{
							service.Image = reader.GetString(8);
						}
						if (!reader.IsDBNull(9))
						{
							service.Website = reader.GetString(9);
						}

						service.Deployed = reader.GetBoolean(10);
						service.Updated = reader.GetBoolean(11);
						service.IdMall = reader.GetInt32(12);
						service.IdStore = reader.GetInt32(13);
						service.IdCustomerServiceData = reader.GetInt32(14);
						service.StartDate = reader.GetDateTime(15);
						service.StopDate = reader.GetDateTime(16);
						// Add current object to the scope

						scope.Add(scopeKey, service);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(service.Id, service);
						// Read the timestamp and set new and changed properties

						service.Timestamp = reader.GetDateTime(17);
						service.IsNew = false;
						service.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationServiceCategory(service, scope);
							LoadRelationCustomerServiceData(service, scope);
						}
					}
					else 
					{
						reader.Close();
					}
				}
				catch (DbException dbException)
				{
					// Catch DBException and rethrow as custom exception
					throw new UtnEmallDataAccessException(dbException.Message, dbException);
				}
				finally 
				{
					// Close connection if it was opened by ourself
					if (closeConnection)
					{
						dbConnection.Close();
					}
				}
			}
			// Return the loaded entity
			return service;
		} 

		/// <summary>
		/// Function to load a ServiceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ServiceEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a ServiceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ServiceEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a ServiceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ServiceEntity Load(int id, Dictionary<string,IEntity> scope)
		{
			return Load(id, true, scope);
		} 

		/// <summary>
		/// Function to check and create table and stored procedures for this class.
		/// </summary>
		private static void DbChecked()
		{
			if (dbChecked)
			{
				return;
			}
			string[] fieldsName = new string[]{"idService", "name", "description", "webAccess", "relativePathAssembly", "pathAssemblyServer", "active", "global", "image", "website", "deployed", "updated", "idMall", "idStore", "idCustomerServiceData", "startDate", "stopDate"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( bool ), typeof( bool ), typeof( string ), typeof( string ), typeof( bool ), typeof( bool ), typeof( int ), typeof( int ), typeof( int ), typeof( System.DateTime ), typeof( System.DateTime )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Service");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Service", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteService");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveService");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateService");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Service", "idService");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Service", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Service", "idService", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(ServiceEntity service, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = service.Name;
			if (String.IsNullOrEmpty(service.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@description", DbType.String);

			parameter.Value = service.Description;
			if (String.IsNullOrEmpty(service.Description))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@webAccess", DbType.String);

			parameter.Value = service.WebAccess;
			if (String.IsNullOrEmpty(service.WebAccess))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@relativePathAssembly", DbType.String);

			parameter.Value = service.RelativePathAssembly;
			if (String.IsNullOrEmpty(service.RelativePathAssembly))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@pathAssemblyServer", DbType.String);

			parameter.Value = service.PathAssemblyServer;
			if (String.IsNullOrEmpty(service.PathAssemblyServer))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@active", DbType.Boolean);

			parameter.Value = service.Active;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@global", DbType.Boolean);

			parameter.Value = service.Global;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@image", DbType.String);

			parameter.Value = service.Image;
			if (String.IsNullOrEmpty(service.Image))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@website", DbType.String);

			parameter.Value = service.Website;
			if (String.IsNullOrEmpty(service.Website))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@deployed", DbType.Boolean);

			parameter.Value = service.Deployed;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@updated", DbType.Boolean);

			parameter.Value = service.Updated;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);

			parameter.Value = service.IdMall;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);

			parameter.Value = service.IdStore;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = service.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@startDate", DbType.DateTime);

			parameter.Value = service.StartDate;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stopDate", DbType.DateTime);

			parameter.Value = service.StopDate;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a ServiceEntity in the database.
		/// </summary>
		/// <param name="service">ServiceEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(ServiceEntity service)
		{
			Save(service, null);
		} 

		/// <summary>
		/// Function to Save a ServiceEntity in the database.
		/// </summary>
		/// <param name="service">ServiceEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = service.Id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
			if (scope != null)
			{
				// If it's on the scope return it, don't save again
				if (scope.ContainsKey(scopeKey))
				{
					return;
				}
			}
			else 
			{
				// Create a new scope if it's not provided
				scope = new Dictionary<string,IEntity>();
			}

			try 
			{
				// Open a DbConnection and a new transaction if it isn't on a higher level one
				if (!isGlobalTransaction)
				{
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
					dbTransaction = dbConnection.BeginTransaction();
				}

				string commandName = "";
				bool isUpdate = false;
				// Check if it is an insert or update command

				if (service.IsNew || !DataAccessConnection.ExistsEntity(service.Id, "Service", "idService", dbConnection, dbTransaction))
				{
					commandName = "SaveService";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateService";
					ServiceEntity serviceTemp3 = new ServiceEntity();
					serviceTemp3.Id = service.Id;
					LoadRelationCustomerServiceData(serviceTemp3, scope);
					if (serviceTemp3.CustomerServiceData != null && serviceTemp3.IdCustomerServiceData != service.IdCustomerServiceData)
					{
						CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
						customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						customerServiceDataDataAccess.Delete(serviceTemp3.CustomerServiceData, scope);
					}
				}
				// Create a db command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
					parameter.Value = service.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(service, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					service.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = service.Id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
				// Add entity to current internal scope

				scope.Add(scopeKey, service);
				// Save collections of related objects to current entity
				if (service.ServiceCategory != null)
				{
					this.SaveServiceCategoryCollection(new ServiceCategoryDataAccess(), service, service.ServiceCategory, service.IsNew, scope);
				}
				// Save objects related to current entity
				if (service.CustomerServiceData != null)
				{
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					customerServiceDataDataAccess.Save(service.CustomerServiceData, scope);
				}
				// Update
				Update(service);
				// Close transaction if initiated by me

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				service.IsNew = false;
				service.Changed = false;
			}
			catch (DbException dbException)
			{
				// Rollback transaction
				if (!isGlobalTransaction)
				{
					dbTransaction.Rollback();
				}
				// Rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close connection if initiated by me
				if (!isGlobalTransaction)
				{
					dbConnection.Close();
					dbConnection = null;
					dbTransaction = null;
				}
			}
		} 

		/// <summary>
		/// Function to Delete a ServiceEntity from database.
		/// </summary>
		/// <param name="service">ServiceEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(ServiceEntity service)
		{
			Delete(service, null);
		} 

		/// <summary>
		/// Function to Delete a ServiceEntity from database.
		/// </summary>
		/// <param name="service">ServiceEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			try 
			{
				// Open connection and initialize a transaction if needed
				if (!isGlobalTransaction)
				{
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
					dbTransaction = dbConnection.BeginTransaction();
				}
				// Reload the entity to ensure deletion of older data

				service = this.Load(service.Id, true);
				if (service == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteService";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
				parameterID.Value = service.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				if (service.ServiceCategory != null)
				{
					this.DeleteServiceCategoryCollection(new ServiceCategoryDataAccess(), service.ServiceCategory, scope);
				}

				if (service.CustomerServiceData != null)
				{
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					customerServiceDataDataAccess.Delete(service.CustomerServiceData, scope);
				}
				// Commit transaction if is mine

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(service.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = service.Id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
					scope.Remove(scopeKey);
				}
			}
			catch (DbException dbException)
			{
				// Rollback transaction
				if (!isGlobalTransaction)
				{
					dbTransaction.Rollback();
				}
				// Rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close connection if it was initiated by this instance
				if (!isGlobalTransaction)
				{
					dbConnection.Close();
					dbConnection = null;
					dbTransaction = null;
				}
			}
		} 

		/// <summary>
		/// Add to the dictionary the properties that can
		/// be used as first parameter on the LoadWhere method.
		/// </summary>
		private static void SetProperties()
		{
			properties = new Dictionary<string,Type>();
			properties.Add("timestamp", typeof( System.DateTime ));
			properties.Add("idService", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("description", typeof( string ));
			properties.Add("webAccess", typeof( string ));
			properties.Add("relativePathAssembly", typeof( string ));
			properties.Add("pathAssemblyServer", typeof( string ));
			properties.Add("active", typeof( bool ));
			properties.Add("global", typeof( bool ));
			properties.Add("image", typeof( string ));
			properties.Add("website", typeof( string ));
			properties.Add("deployed", typeof( bool ));
			properties.Add("updated", typeof( bool ));
			properties.Add("idMall", typeof( int ));
			properties.Add("idStore", typeof( int ));
			properties.Add("idCustomerServiceData", typeof( int ));
			properties.Add("startDate", typeof( System.DateTime ));
			properties.Add("stopDate", typeof( System.DateTime ));
		} 

		/// <summary>
		/// Function to Load all the ServiceEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<ServiceEntity> LoadAll(bool loadRelation)
		{
			Collection<ServiceEntity> serviceList = new Collection<ServiceEntity>();

			bool closeConnection = false;
			try 
			{
				// Open a new connection if necessary
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Build the query string

				string cmdText = "SELECT idService FROM [Service]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				ServiceEntity service;
				// Read the Ids and insert on a list

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}
				// Close the DataReader

				reader.Close();
				// Create a scope

				Dictionary<string,IEntity> scope = new Dictionary<string,IEntity>();
				// Load entities and add to return list

				foreach(int  id in listId)
				{
					service = Load(id, loadRelation, scope);
					serviceList.Add(service);
				}
			}
			catch (DbException dbException)
			{
				// Catch DbException and rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close the connection
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			// Return the loaded
			return serviceList;
		} 

		/// <summary>
		/// Function to Load a ServiceEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of ServiceEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<ServiceEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<ServiceEntity> serviceList;

			bool closeConnection = false;
			try 
			{
				// Open a new connection with a database if necessary
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}

				string op = DataAccessConnection.GetOperatorString(operatorType);
				// Build the query string

				string cmdText = "SELECT idService, name, description, webAccess, relativePathAssembly, pathAssemblyServer, active, global, image, website, deployed, updated, idMall, idStore, idCustomerServiceData, startDate, stopDate, timestamp FROM [Service] WHERE " + propertyName + " " + op + " @expValue";
				// Create the command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Add parameters values to the command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter();
				parameter.ParameterName = "@expValue";
				Type parameterType = properties[propertyName];
				parameter.DbType = DataAccessConnection.GetParameterDBType(parameterType);

				parameter.Value = expValue;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				serviceList = new Collection<ServiceEntity>();
				ServiceEntity service;
				List<int> listId = new List<int>();
				// Add list of Ids to a list
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}
				// Close the reader

				reader.Close();
				// Load the entities

				foreach(int  id in listId)
				{
					service = Load(id, loadRelation, null);
					serviceList.Add(service);
				}
			}
			catch (DbException dbException)
			{
				// Catch DbException and rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close connection if it was opened by myself
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			return serviceList;
		} 

		/// <summary>
		/// Function to Load a list of ServiceEntity from database by idMall.
		/// </summary>
		/// <param name="idMall">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByMallCollection(int idMall, Dictionary<string,IEntity> scope)
		{
			Collection<ServiceEntity> serviceList;
			bool closeConnection = false;
			try 
			{
				// Create a new connection
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Create a command

				string cmdText = "SELECT idService FROM [Service] WHERE idMall = @idMall";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);
				parameter.Value = idMall;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				serviceList = new Collection<ServiceEntity>();
				// Load Ids of related objects into a list of int

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}

				reader.Close();
				// Load related objects and add to collection

				foreach(int  id in listId)
				{
					serviceList.Add(Load(id, scope));
				}
			}
			catch (DbException dbException)
			{
				// Rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close connection if initiated be me
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			// Return related objects list
			return serviceList;
		} 

		/// <summary>
		/// Function to Load a list of ServiceEntity from database by idMall.
		/// </summary>
		/// <param name="idMall">Foreing key column</param>
		/// <returns>IList of ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByMallCollection(int idMall)
		{
			return LoadByMallCollection(idMall, null);
		} 

		/// <summary>
		/// Function to Load a list of ServiceEntity from database by idStore.
		/// </summary>
		/// <param name="idStore">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByStoreCollection(int idStore, Dictionary<string,IEntity> scope)
		{
			Collection<ServiceEntity> serviceList;
			bool closeConnection = false;
			try 
			{
				// Create a new connection
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Create a command

				string cmdText = "SELECT idService FROM [Service] WHERE idStore = @idStore";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				parameter.Value = idStore;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				serviceList = new Collection<ServiceEntity>();
				// Load Ids of related objects into a list of int

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}

				reader.Close();
				// Load related objects and add to collection

				foreach(int  id in listId)
				{
					serviceList.Add(Load(id, scope));
				}
			}
			catch (DbException dbException)
			{
				// Rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close connection if initiated be me
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			// Return related objects list
			return serviceList;
		} 

		/// <summary>
		/// Function to Load a list of ServiceEntity from database by idStore.
		/// </summary>
		/// <param name="idStore">Foreing key column</param>
		/// <returns>IList of ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByStoreCollection(int idStore)
		{
			return LoadByStoreCollection(idStore, null);
		} 

		/// <summary>
		/// Function to Load the relation ServiceCategory from database.
		/// </summary>
		/// <param name="service">ServiceEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		public void LoadRelationServiceCategory(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			ServiceCategoryDataAccess serviceCategoryDataAccess = new ServiceCategoryDataAccess();
			// Set connection objects to the data access

			serviceCategoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			service.ServiceCategory = serviceCategoryDataAccess.LoadByServiceCollection(service.Id, scope);
		} 

		/// <summary>
		/// Updates the database to reflect the current state of the list.
		/// </summary>
		/// <param name="collectionDataAccess">the IDataAccess of the relation</param>
		/// <param name="parent">the parent of the object</param>
		/// <param name="collection">a collection of items</param>
		/// <param name="isNewParent">if the parent is a new object</param>
		/// <param name="scope">internal data structure to aviod problems with circular referencies on entities</param>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		private void SaveServiceCategoryCollection(ServiceCategoryDataAccess collectionDataAccess, ServiceEntity parent, Collection<ServiceCategoryEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
		{
			if (collection == null)
			{
				return;
			}
			// Set connection objects on collection data access
			collectionDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Set the child/parent relation

			for (int  i = 0; i < collection.Count; i++)
			{
				bool changed = collection[i].Changed;
				collection[i].Service = parent;
				collection[i].Changed = changed;
			}
			// If the parent is new save all childs, else check diferencies with db

			if (isNewParent)
			{
				for (int  i = 0; i < collection.Count; i++)
				{
					collectionDataAccess.Save(collection[i], scope);
				}
			}
			else 
			{
				// Check the childs that are not part of the parent any more
				string idList = "0";
				if (collection.Count > 0)
				{
					idList = "" + collection[0].Id;
				}

				for (int  i = 1; i < collection.Count; i++)
				{
					idList += ", " + collection[i].Id;
				}
				// Returns the ids that doesn't exists in the current collection

				string command = "SELECT idServiceCategory FROM [ServiceCategory] WHERE idService = @idService AND idServiceCategory NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<ServiceCategoryEntity> objectsToDelete = new Collection<ServiceCategoryEntity>();
				// Insert Ids on a list

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}

				reader.Close();
				// Load items to be removed

				foreach(int  id in listId)
				{
					ServiceCategoryEntity entityToDelete = collectionDataAccess.Load(id, scope);
					objectsToDelete.Add(entityToDelete);
				}
				// Have to do this because the reader must be closed before
				// deletion of entities

				for (int  i = 0; i < objectsToDelete.Count; i++)
				{
					collectionDataAccess.Delete(objectsToDelete[i], scope);
				}

				System.DateTime timestamp;
				// Check all the properties of the collection items
				// to see if they have changed (timestamp)

				for (int  i = 0; i < collection.Count; i++)
				{
					ServiceCategoryEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [ServiceCategory] WHERE idServiceCategory = @idServiceCategory";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idServiceCategory", DbType.Int32);
						sqlParameterIdPreference.Value = item.Id;
						sqlCommandTimestamp.Parameters.Add(sqlParameterIdPreference);

						timestamp = ((System.DateTime)sqlCommandTimestamp.ExecuteScalar());
						if (item.Timestamp != timestamp)
						{
							item.Changed = true;
						}
					}
					// Save the item if it changed or is new

					if (item.Changed || item.IsNew)
					{
						collectionDataAccess.Save(item);
					}
				}
			}
		} 

		/// <summary>
		/// Function to Delete a list of related entities from database.
		/// </summary>
		/// <param name="collectionDataAccess">IDataAccess of the relation</param>
		/// <param name="collection">The collection of entities to delete</param>
		/// <param name="scope">Internal structure to keep safe circular referencies</param>
		/// <returns>True if collection not null</returns>
		private bool DeleteServiceCategoryCollection(ServiceCategoryDataAccess collectionDataAccess, Collection<ServiceCategoryEntity> collection, Dictionary<string,IEntity> scope)
		{
			if (collection == null)
			{
				return false;
			}
			// Set connection objects of related data access object
			collectionDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Delete related objects

			for (int  i = 0; i < collection.Count; i++)
			{
				collectionDataAccess.Delete(collection[i], scope);
			}
			return true;
		} 

		/// <summary>
		/// Function to Load the relation CustomerServiceData from database.
		/// </summary>
		/// <param name="service">ServiceEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		public void LoadRelationCustomerServiceData(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			bool closeConnection = false;
			try 
			{
				// Create a new connection if needed
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Create a command

				string cmdText = "SELECT idCustomerServiceData FROM [Service] WHERE idService = @idService";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
				// Set command parameters values

				parameter.Value = service.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					service.CustomerServiceData = customerServiceDataDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Catch and rethrow as custom exception
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Close connection if initiated by me
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Function to Update a ServiceEntity from database.
		/// </summary>
		/// <param name="service">ServiceEntity to update</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="service"/> is not a <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		private void Update(ServiceEntity service)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null", "service");
			}
			// Build update command
			string commandName = "UpdateService";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Set update parameters values

			IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
			parameter.Value = service.Id;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = service.Name;
			if (String.IsNullOrEmpty(service.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@description", DbType.String);

			parameter.Value = service.Description;
			if (String.IsNullOrEmpty(service.Description))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@webAccess", DbType.String);

			parameter.Value = service.WebAccess;
			if (String.IsNullOrEmpty(service.WebAccess))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@relativePathAssembly", DbType.String);

			parameter.Value = service.RelativePathAssembly;
			if (String.IsNullOrEmpty(service.RelativePathAssembly))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@pathAssemblyServer", DbType.String);

			parameter.Value = service.PathAssemblyServer;
			if (String.IsNullOrEmpty(service.PathAssemblyServer))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@active", DbType.Boolean);

			parameter.Value = service.Active;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@global", DbType.Boolean);

			parameter.Value = service.Global;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@image", DbType.String);

			parameter.Value = service.Image;
			if (String.IsNullOrEmpty(service.Image))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@website", DbType.String);

			parameter.Value = service.Website;
			if (String.IsNullOrEmpty(service.Website))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@deployed", DbType.Boolean);

			parameter.Value = service.Deployed;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@updated", DbType.Boolean);

			parameter.Value = service.Updated;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);

			parameter.Value = service.IdMall;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);

			parameter.Value = service.IdStore;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = service.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@startDate", DbType.DateTime);

			parameter.Value = service.StartDate;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stopDate", DbType.DateTime);

			parameter.Value = service.StopDate;
			sqlCommand.Parameters.Add(parameter);
			// Execute the update

			sqlCommand.ExecuteNonQuery();
			// Update new and changed flags

			service.IsNew = false;
			service.Changed = false;
		} 

	} 

}

