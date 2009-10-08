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
	/// The <c>CustomerServiceDataDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class CustomerServiceDataDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,CustomerServiceDataEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CustomerServiceDataDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  CustomerServiceDataDataAccess()
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

			inMemoryEntities = new Dictionary<int,CustomerServiceDataEntity>();
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
		/// Function to load a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerServiceDataEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((CustomerServiceDataEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			CustomerServiceDataEntity customerServiceData = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				customerServiceData = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, customerServiceData);
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

					string cmdText = "SELECT idCustomerServiceData, customerServiceDataType, idDataModel, idInitComponent, idService, timestamp FROM [CustomerServiceData] WHERE idCustomerServiceData = @idCustomerServiceData";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					customerServiceData = new CustomerServiceDataEntity();

					if (reader.Read())
					{
						// Load fields of entity
						customerServiceData.Id = reader.GetInt32(0);

						customerServiceData.CustomerServiceDataType = reader.GetInt32(1);
						customerServiceData.IdDataModel = reader.GetInt32(2);
						customerServiceData.IdInitComponent = reader.GetInt32(3);
						customerServiceData.IdService = reader.GetInt32(4);
						// Add current object to the scope

						scope.Add(scopeKey, customerServiceData);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(customerServiceData.Id, customerServiceData);
						// Read the timestamp and set new and changed properties

						customerServiceData.Timestamp = reader.GetDateTime(5);
						customerServiceData.IsNew = false;
						customerServiceData.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationComponents(customerServiceData, scope);
							LoadRelationConnections(customerServiceData, scope);
							LoadRelationDataModel(customerServiceData, scope);
							LoadRelationInitComponent(customerServiceData, scope);
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
			return customerServiceData;
		} 

		/// <summary>
		/// Function to load a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerServiceDataEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerServiceDataEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerServiceDataEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idCustomerServiceData", "customerServiceDataType", "idDataModel", "idInitComponent", "idService"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("CustomerServiceData");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("CustomerServiceData", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteCustomerServiceData");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveCustomerServiceData");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateCustomerServiceData");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("CustomerServiceData", "idCustomerServiceData");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("CustomerServiceData", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("CustomerServiceData", "idCustomerServiceData", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(CustomerServiceDataEntity customerServiceData, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@customerServiceDataType", DbType.Int32);

			parameter.Value = customerServiceData.CustomerServiceDataType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idDataModel", DbType.Int32);

			parameter.Value = customerServiceData.IdDataModel;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idInitComponent", DbType.Int32);

			parameter.Value = customerServiceData.IdInitComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);

			parameter.Value = customerServiceData.IdService;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a CustomerServiceDataEntity in the database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(CustomerServiceDataEntity customerServiceData)
		{
			Save(customerServiceData, null);
		} 

		/// <summary>
		/// Function to Save a CustomerServiceDataEntity in the database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = customerServiceData.Id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
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

				if (customerServiceData.IsNew || !DataAccessConnection.ExistsEntity(customerServiceData.Id, "CustomerServiceData", "idCustomerServiceData", dbConnection, dbTransaction))
				{
					commandName = "SaveCustomerServiceData";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateCustomerServiceData";
					CustomerServiceDataEntity customerServiceDataTemp3 = new CustomerServiceDataEntity();
					customerServiceDataTemp3.Id = customerServiceData.Id;
					LoadRelationInitComponent(customerServiceDataTemp3, scope);
					if (customerServiceDataTemp3.InitComponent != null && customerServiceDataTemp3.IdInitComponent != customerServiceData.IdInitComponent)
					{
						ComponentDataAccess componentDataAccess = new ComponentDataAccess();
						componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						componentDataAccess.Delete(customerServiceDataTemp3.InitComponent, scope);
					}
				}
				// Create a db command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
					parameter.Value = customerServiceData.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(customerServiceData, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					customerServiceData.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = customerServiceData.Id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
				// Add entity to current internal scope

				scope.Add(scopeKey, customerServiceData);
				// Save collections of related objects to current entity
				if (customerServiceData.Components != null)
				{
					this.SaveComponentCollection(new ComponentDataAccess(), customerServiceData, customerServiceData.Components, customerServiceData.IsNew, scope);
				}
				if (customerServiceData.Connections != null)
				{
					this.SaveConnectionWidgetCollection(new ConnectionWidgetDataAccess(), customerServiceData, customerServiceData.Connections, customerServiceData.IsNew, scope);
				}
				// Save objects related to current entity
				if (customerServiceData.InitComponent != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Save(customerServiceData.InitComponent, scope);
				}
				// Update
				Update(customerServiceData);
				// Close transaction if initiated by me

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				customerServiceData.IsNew = false;
				customerServiceData.Changed = false;
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
		/// Function to Delete a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(CustomerServiceDataEntity customerServiceData)
		{
			Delete(customerServiceData, null);
		} 

		/// <summary>
		/// Function to Delete a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
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

				customerServiceData = this.Load(customerServiceData.Id, true);
				if (customerServiceData == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				parameterID.Value = customerServiceData.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				if (customerServiceData.Components != null)
				{
					this.DeleteComponentCollection(new ComponentDataAccess(), customerServiceData.Components, scope);
				}
				if (customerServiceData.Connections != null)
				{
					this.DeleteConnectionWidgetCollection(new ConnectionWidgetDataAccess(), customerServiceData.Connections, scope);
				}
				// Commit transaction if is mine

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(customerServiceData.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = customerServiceData.Id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
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
			properties.Add("idCustomerServiceData", typeof( int ));

			properties.Add("customerServiceDataType", typeof( int ));
			properties.Add("idDataModel", typeof( int ));
			properties.Add("idInitComponent", typeof( int ));
			properties.Add("idService", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<CustomerServiceDataEntity> LoadAll(bool loadRelation)
		{
			Collection<CustomerServiceDataEntity> customerServiceDataList = new Collection<CustomerServiceDataEntity>();

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

				string cmdText = "SELECT idCustomerServiceData FROM [CustomerServiceData]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				CustomerServiceDataEntity customerServiceData;
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
					customerServiceData = Load(id, loadRelation, scope);
					customerServiceDataList.Add(customerServiceData);
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
			return customerServiceDataList;
		} 

		/// <summary>
		/// Function to Load a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of CustomerServiceDataEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<CustomerServiceDataEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<CustomerServiceDataEntity> customerServiceDataList;

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

				string cmdText = "SELECT idCustomerServiceData, customerServiceDataType, idDataModel, idInitComponent, idService, timestamp FROM [CustomerServiceData] WHERE " + propertyName + " " + op + " @expValue";
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
				customerServiceDataList = new Collection<CustomerServiceDataEntity>();
				CustomerServiceDataEntity customerServiceData;
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
					customerServiceData = Load(id, loadRelation, null);
					customerServiceDataList.Add(customerServiceData);
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
			return customerServiceDataList;
		} 

		/// <summary>
		/// Function to Load the relation Components from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		public void LoadRelationComponents(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			ComponentDataAccess componentDataAccess = new ComponentDataAccess();
			// Set connection objects to the data access

			componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			customerServiceData.Components = componentDataAccess.LoadByCustomerServiceDataCollection(customerServiceData.Id, scope);
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
		private void SaveComponentCollection(ComponentDataAccess collectionDataAccess, CustomerServiceDataEntity parent, Collection<ComponentEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].CustomerServiceData = parent;
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

				string command = "SELECT idComponent FROM [Component] WHERE idCustomerServiceData = @idCustomerServiceData AND idComponent NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<ComponentEntity> objectsToDelete = new Collection<ComponentEntity>();
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
					ComponentEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					ComponentEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [Component] WHERE idComponent = @idComponent";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
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
		private bool DeleteComponentCollection(ComponentDataAccess collectionDataAccess, Collection<ComponentEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Function to Load the relation Connections from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		public void LoadRelationConnections(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			ConnectionWidgetDataAccess connectionWidgetDataAccess = new ConnectionWidgetDataAccess();
			// Set connection objects to the data access

			connectionWidgetDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			customerServiceData.Connections = connectionWidgetDataAccess.LoadByCustomerServiceDataCollection(customerServiceData.Id, scope);
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
		private void SaveConnectionWidgetCollection(ConnectionWidgetDataAccess collectionDataAccess, CustomerServiceDataEntity parent, Collection<ConnectionWidgetEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].CustomerServiceData = parent;
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

				string command = "SELECT idConnectionWidget FROM [ConnectionWidget] WHERE idCustomerServiceData = @idCustomerServiceData AND idConnectionWidget NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<ConnectionWidgetEntity> objectsToDelete = new Collection<ConnectionWidgetEntity>();
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
					ConnectionWidgetEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					ConnectionWidgetEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [ConnectionWidget] WHERE idConnectionWidget = @idConnectionWidget";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
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
		private bool DeleteConnectionWidgetCollection(ConnectionWidgetDataAccess collectionDataAccess, Collection<ConnectionWidgetEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Function to Load the relation DataModel from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		public void LoadRelationDataModel(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
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

				string cmdText = "SELECT idDataModel FROM [CustomerServiceData] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				// Set command parameters values

				parameter.Value = customerServiceData.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					DataModelDataAccess dataModelDataAccess = new DataModelDataAccess();
					dataModelDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					customerServiceData.DataModel = dataModelDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation InitComponent from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		public void LoadRelationInitComponent(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
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

				string cmdText = "SELECT idInitComponent FROM [CustomerServiceData] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				// Set command parameters values

				parameter.Value = customerServiceData.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					customerServiceData.InitComponent = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Update a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity to update</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceData"/> is not a <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		private void Update(CustomerServiceDataEntity customerServiceData)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null", "customerServiceData");
			}
			// Build update command
			string commandName = "UpdateCustomerServiceData";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Set update parameters values

			IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
			parameter.Value = customerServiceData.Id;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@customerServiceDataType", DbType.Int32);

			parameter.Value = customerServiceData.CustomerServiceDataType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idDataModel", DbType.Int32);

			parameter.Value = customerServiceData.IdDataModel;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idInitComponent", DbType.Int32);

			parameter.Value = customerServiceData.IdInitComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);

			parameter.Value = customerServiceData.IdService;
			sqlCommand.Parameters.Add(parameter);
			// Execute the update

			sqlCommand.ExecuteNonQuery();
			// Update new and changed flags

			customerServiceData.IsNew = false;
			customerServiceData.Changed = false;
		} 

	} 

}

