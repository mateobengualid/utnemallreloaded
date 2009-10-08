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
	/// The <c>PreferenceDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class PreferenceDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,PreferenceEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>PreferenceDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  PreferenceDataAccess()
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

			inMemoryEntities = new Dictionary<int,PreferenceEntity>();
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
		/// Function to load a PreferenceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public PreferenceEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Preference";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((PreferenceEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			PreferenceEntity preference = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				preference = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, preference);
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

					string cmdText = "SELECT idPreference, active, level, idCustomer, idCategory, timestamp FROM [Preference] WHERE idPreference = @idPreference";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					preference = new PreferenceEntity();

					if (reader.Read())
					{
						// Load fields of entity
						preference.Id = reader.GetInt32(0);

						preference.Active = reader.GetBoolean(1);
						preference.Level = Convert.ToDouble(reader.GetDecimal(2));
						preference.IdCustomer = reader.GetInt32(3);
						preference.IdCategory = reader.GetInt32(4);
						// Add current object to the scope

						scope.Add(scopeKey, preference);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(preference.Id, preference);
						// Read the timestamp and set new and changed properties

						preference.Timestamp = reader.GetDateTime(5);
						preference.IsNew = false;
						preference.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationCategory(preference, scope);
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
			return preference;
		} 

		/// <summary>
		/// Function to load a PreferenceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public PreferenceEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a PreferenceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public PreferenceEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a PreferenceEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public PreferenceEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idPreference", "active", "level", "idCustomer", "idCategory"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( bool ), typeof( double ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Preference");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Preference", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeletePreference");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SavePreference");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdatePreference");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Preference", "idPreference");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Preference", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Preference", "idPreference", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(PreferenceEntity preference, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@active", DbType.Boolean);

			parameter.Value = preference.Active;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@level", DbType.Decimal);

			parameter.Value = preference.Level;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);

			parameter.Value = preference.IdCustomer;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);

			parameter.Value = preference.IdCategory;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a PreferenceEntity in the database.
		/// </summary>
		/// <param name="preference">PreferenceEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="preference"/> is not a <c>PreferenceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(PreferenceEntity preference)
		{
			Save(preference, null);
		} 

		/// <summary>
		/// Function to Save a PreferenceEntity in the database.
		/// </summary>
		/// <param name="preference">PreferenceEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="preference"/> is not a <c>PreferenceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(PreferenceEntity preference, Dictionary<string,IEntity> scope)
		{
			if (preference == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = preference.Id.ToString(NumberFormatInfo.InvariantInfo) + "Preference";
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

				if (preference.IsNew || !DataAccessConnection.ExistsEntity(preference.Id, "Preference", "idPreference", dbConnection, dbTransaction))
				{
					commandName = "SavePreference";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdatePreference";
				}
				// Create a db command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
					parameter.Value = preference.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(preference, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					preference.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = preference.Id.ToString(NumberFormatInfo.InvariantInfo) + "Preference";
				// Add entity to current internal scope

				scope.Add(scopeKey, preference);
				// Save collections of related objects to current entity
				// Save objects related to current entity
				// Update
				// Close transaction if initiated by me
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				preference.IsNew = false;
				preference.Changed = false;
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
		/// Function to Delete a PreferenceEntity from database.
		/// </summary>
		/// <param name="preference">PreferenceEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="preference"/> is not a <c>PreferenceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(PreferenceEntity preference)
		{
			Delete(preference, null);
		} 

		/// <summary>
		/// Function to Delete a PreferenceEntity from database.
		/// </summary>
		/// <param name="preference">PreferenceEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="preference"/> is not a <c>PreferenceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(PreferenceEntity preference, Dictionary<string,IEntity> scope)
		{
			if (preference == null)
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

				preference = this.Load(preference.Id, true);
				if (preference == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeletePreference";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
				parameterID.Value = preference.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				// Commit transaction if is mine
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(preference.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = preference.Id.ToString(NumberFormatInfo.InvariantInfo) + "Preference";
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
			properties.Add("idPreference", typeof( int ));

			properties.Add("active", typeof( bool ));
			properties.Add("level", typeof( double ));
			properties.Add("idCustomer", typeof( int ));
			properties.Add("idCategory", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the PreferenceEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<PreferenceEntity> LoadAll(bool loadRelation)
		{
			Collection<PreferenceEntity> preferenceList = new Collection<PreferenceEntity>();

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

				string cmdText = "SELECT idPreference FROM [Preference]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				PreferenceEntity preference;
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
					preference = Load(id, loadRelation, scope);
					preferenceList.Add(preference);
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
			return preferenceList;
		} 

		/// <summary>
		/// Function to Load a PreferenceEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of PreferenceEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<PreferenceEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<PreferenceEntity> preferenceList;

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

				string cmdText = "SELECT idPreference, active, level, idCustomer, idCategory, timestamp FROM [Preference] WHERE " + propertyName + " " + op + " @expValue";
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
				preferenceList = new Collection<PreferenceEntity>();
				PreferenceEntity preference;
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
					preference = Load(id, loadRelation, null);
					preferenceList.Add(preference);
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
			return preferenceList;
		} 

		/// <summary>
		/// Function to Load a list of PreferenceEntity from database by idCustomer.
		/// </summary>
		/// <param name="idCustomer">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of PreferenceEntity</returns>
		public Collection<PreferenceEntity> LoadByCustomerCollection(int idCustomer, Dictionary<string,IEntity> scope)
		{
			Collection<PreferenceEntity> preferenceList;
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

				string cmdText = "SELECT idPreference FROM [Preference] WHERE idCustomer = @idCustomer";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				parameter.Value = idCustomer;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				preferenceList = new Collection<PreferenceEntity>();
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
					preferenceList.Add(Load(id, scope));
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
			return preferenceList;
		} 

		/// <summary>
		/// Function to Load a list of PreferenceEntity from database by idCustomer.
		/// </summary>
		/// <param name="idCustomer">Foreing key column</param>
		/// <returns>IList of PreferenceEntity</returns>
		public Collection<PreferenceEntity> LoadByCustomerCollection(int idCustomer)
		{
			return LoadByCustomerCollection(idCustomer, null);
		} 

		/// <summary>
		/// Function to Load a list of PreferenceEntity from database by idCategory.
		/// </summary>
		/// <param name="idCategory">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of PreferenceEntity</returns>
		public Collection<PreferenceEntity> LoadByCategoryCollection(int idCategory, Dictionary<string,IEntity> scope)
		{
			Collection<PreferenceEntity> preferenceList;
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

				string cmdText = "SELECT idPreference FROM [Preference] WHERE idCategory = @idCategory";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);
				parameter.Value = idCategory;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				preferenceList = new Collection<PreferenceEntity>();
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
					preferenceList.Add(Load(id, scope));
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
			return preferenceList;
		} 

		/// <summary>
		/// Function to Load a list of PreferenceEntity from database by idCategory.
		/// </summary>
		/// <param name="idCategory">Foreing key column</param>
		/// <returns>IList of PreferenceEntity</returns>
		public Collection<PreferenceEntity> LoadByCategoryCollection(int idCategory)
		{
			return LoadByCategoryCollection(idCategory, null);
		} 

		/// <summary>
		/// Function to Load the relation Category from database.
		/// </summary>
		/// <param name="preference">PreferenceEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="preference"/> is not a <c>PreferenceEntity</c>.
		/// </exception>
		public void LoadRelationCategory(PreferenceEntity preference, Dictionary<string,IEntity> scope)
		{
			if (preference == null)
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

				string cmdText = "SELECT idCategory FROM [Preference] WHERE idPreference = @idPreference";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
				// Set command parameters values

				parameter.Value = preference.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					CategoryDataAccess categoryDataAccess = new CategoryDataAccess();
					categoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					preference.Category = categoryDataAccess.Load(((int)idRelation), false, scope);
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

	} 

}

