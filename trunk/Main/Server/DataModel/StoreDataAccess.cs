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
	/// The <c>StoreDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class StoreDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,StoreEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>StoreDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  StoreDataAccess()
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

			inMemoryEntities = new Dictionary<int,StoreEntity>();
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
		/// Function to load a StoreEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public StoreEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((StoreEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			StoreEntity store = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				store = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, store);
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

					string cmdText = "SELECT idStore, name, telephoneNumber, internalPhoneNumber, contactName, ownerName, email, webAddress, localNumber, idMall, idDataModel, timestamp FROM [Store] WHERE idStore = @idStore";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					store = new StoreEntity();

					if (reader.Read())
					{
						// Load fields of entity
						store.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							store.Name = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							store.TelephoneNumber = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							store.InternalPhoneNumber = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							store.ContactName = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							store.OwnerName = reader.GetString(5);
						}
						if (!reader.IsDBNull(6))
						{
							store.Email = reader.GetString(6);
						}
						if (!reader.IsDBNull(7))
						{
							store.WebAddress = reader.GetString(7);
						}
						if (!reader.IsDBNull(8))
						{
							store.LocalNumber = reader.GetString(8);
						}

						store.IdMall = reader.GetInt32(9);
						store.IdDataModel = reader.GetInt32(10);
						// Add current object to the scope

						scope.Add(scopeKey, store);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(store.Id, store);
						// Read the timestamp and set new and changed properties

						store.Timestamp = reader.GetDateTime(11);
						store.IsNew = false;
						store.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationStoreCategory(store, scope);
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
			return store;
		} 

		/// <summary>
		/// Function to load a StoreEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public StoreEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a StoreEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public StoreEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a StoreEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public StoreEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idStore", "name", "telephoneNumber", "internalPhoneNumber", "contactName", "ownerName", "email", "webAddress", "localNumber", "idMall", "idDataModel"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Store");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Store", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteStore");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveStore");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateStore");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Store", "idStore");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Store", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Store", "idStore", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(StoreEntity store, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = store.Name;
			if (String.IsNullOrEmpty(store.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@telephoneNumber", DbType.String);

			parameter.Value = store.TelephoneNumber;
			if (String.IsNullOrEmpty(store.TelephoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@internalPhoneNumber", DbType.String);

			parameter.Value = store.InternalPhoneNumber;
			if (String.IsNullOrEmpty(store.InternalPhoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@contactName", DbType.String);

			parameter.Value = store.ContactName;
			if (String.IsNullOrEmpty(store.ContactName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@ownerName", DbType.String);

			parameter.Value = store.OwnerName;
			if (String.IsNullOrEmpty(store.OwnerName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@email", DbType.String);

			parameter.Value = store.Email;
			if (String.IsNullOrEmpty(store.Email))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@webAddress", DbType.String);

			parameter.Value = store.WebAddress;
			if (String.IsNullOrEmpty(store.WebAddress))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@localNumber", DbType.String);

			parameter.Value = store.LocalNumber;
			if (String.IsNullOrEmpty(store.LocalNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);

			parameter.Value = store.IdMall;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idDataModel", DbType.Int32);

			parameter.Value = store.IdDataModel;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a StoreEntity in the database.
		/// </summary>
		/// <param name="store">StoreEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="store"/> is not a <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(StoreEntity store)
		{
			Save(store, null);
		} 

		/// <summary>
		/// Function to Save a StoreEntity in the database.
		/// </summary>
		/// <param name="store">StoreEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="store"/> is not a <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(StoreEntity store, Dictionary<string,IEntity> scope)
		{
			if (store == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = store.Id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
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

				if (store.IsNew || !DataAccessConnection.ExistsEntity(store.Id, "Store", "idStore", dbConnection, dbTransaction))
				{
					commandName = "SaveStore";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateStore";
				}
				// Create a db command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
					parameter.Value = store.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(store, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					store.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = store.Id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
				// Add entity to current internal scope

				scope.Add(scopeKey, store);
				// Save collections of related objects to current entity
				if (store.StoreCategory != null)
				{
					this.SaveStoreCategoryCollection(new StoreCategoryDataAccess(), store, store.StoreCategory, store.IsNew, scope);
				}
				// Save objects related to current entity
				// Update
				// Close transaction if initiated by me
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				store.IsNew = false;
				store.Changed = false;
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
		/// Function to Delete a StoreEntity from database.
		/// </summary>
		/// <param name="store">StoreEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="store"/> is not a <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(StoreEntity store)
		{
			Delete(store, null);
		} 

		/// <summary>
		/// Function to Delete a StoreEntity from database.
		/// </summary>
		/// <param name="store">StoreEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="store"/> is not a <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(StoreEntity store, Dictionary<string,IEntity> scope)
		{
			if (store == null)
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

				store = this.Load(store.Id, true);
				if (store == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteStore";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				parameterID.Value = store.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				if (store.StoreCategory != null)
				{
					this.DeleteStoreCategoryCollection(new StoreCategoryDataAccess(), store.StoreCategory, scope);
				}
				// Commit transaction if is mine

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(store.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = store.Id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
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
			properties.Add("idStore", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("telephoneNumber", typeof( string ));
			properties.Add("internalPhoneNumber", typeof( string ));
			properties.Add("contactName", typeof( string ));
			properties.Add("ownerName", typeof( string ));
			properties.Add("email", typeof( string ));
			properties.Add("webAddress", typeof( string ));
			properties.Add("localNumber", typeof( string ));
			properties.Add("idMall", typeof( int ));
			properties.Add("idDataModel", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the StoreEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<StoreEntity> LoadAll(bool loadRelation)
		{
			Collection<StoreEntity> storeList = new Collection<StoreEntity>();

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

				string cmdText = "SELECT idStore FROM [Store]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				StoreEntity store;
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
					store = Load(id, loadRelation, scope);
					storeList.Add(store);
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
			return storeList;
		} 

		/// <summary>
		/// Function to Load a StoreEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of StoreEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<StoreEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<StoreEntity> storeList;

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

				string cmdText = "SELECT idStore, name, telephoneNumber, internalPhoneNumber, contactName, ownerName, email, webAddress, localNumber, idMall, idDataModel, timestamp FROM [Store] WHERE " + propertyName + " " + op + " @expValue";
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
				storeList = new Collection<StoreEntity>();
				StoreEntity store;
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
					store = Load(id, loadRelation, null);
					storeList.Add(store);
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
			return storeList;
		} 

		/// <summary>
		/// Function to Load the relation StoreCategory from database.
		/// </summary>
		/// <param name="store">StoreEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="store"/> is not a <c>StoreEntity</c>.
		/// </exception>
		public void LoadRelationStoreCategory(StoreEntity store, Dictionary<string,IEntity> scope)
		{
			if (store == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			StoreCategoryDataAccess storeCategoryDataAccess = new StoreCategoryDataAccess();
			// Set connection objects to the data access

			storeCategoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			store.StoreCategory = storeCategoryDataAccess.LoadByStoreCollection(store.Id, scope);
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
		private void SaveStoreCategoryCollection(StoreCategoryDataAccess collectionDataAccess, StoreEntity parent, Collection<StoreCategoryEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Store = parent;
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

				string command = "SELECT idStoreCategory FROM [StoreCategory] WHERE idStore = @idStore AND idStoreCategory NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<StoreCategoryEntity> objectsToDelete = new Collection<StoreCategoryEntity>();
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
					StoreCategoryEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					StoreCategoryEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [StoreCategory] WHERE idStoreCategory = @idStoreCategory";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idStoreCategory", DbType.Int32);
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
		private bool DeleteStoreCategoryCollection(StoreCategoryDataAccess collectionDataAccess, Collection<StoreCategoryEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Function to Load a list of StoreEntity from database by idMall.
		/// </summary>
		/// <param name="idMall">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of StoreEntity</returns>
		public Collection<StoreEntity> LoadByMallCollection(int idMall, Dictionary<string,IEntity> scope)
		{
			Collection<StoreEntity> storeList;
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

				string cmdText = "SELECT idStore FROM [Store] WHERE idMall = @idMall";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);
				parameter.Value = idMall;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				storeList = new Collection<StoreEntity>();
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
					storeList.Add(Load(id, scope));
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
			return storeList;
		} 

		/// <summary>
		/// Function to Load a list of StoreEntity from database by idMall.
		/// </summary>
		/// <param name="idMall">Foreing key column</param>
		/// <returns>IList of StoreEntity</returns>
		public Collection<StoreEntity> LoadByMallCollection(int idMall)
		{
			return LoadByMallCollection(idMall, null);
		} 

	} 

}

