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
	/// The <c>RegisterAssociationDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class RegisterAssociationDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,RegisterAssociationEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>RegisterAssociationDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  RegisterAssociationDataAccess()
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

			inMemoryEntities = new Dictionary<int,RegisterAssociationEntity>();
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
		/// Function to load a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public RegisterAssociationEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "RegisterAssociation";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((RegisterAssociationEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			RegisterAssociationEntity registerAssociation = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				registerAssociation = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, registerAssociation);
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

					string cmdText = "SELECT idRegisterAssociation, idRegister, idTable, timestamp FROM [RegisterAssociation] WHERE idRegisterAssociation = @idRegisterAssociation";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idRegisterAssociation", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					registerAssociation = new RegisterAssociationEntity();

					if (reader.Read())
					{
						// Load fields of entity
						registerAssociation.Id = reader.GetInt32(0);

						registerAssociation.IdRegister = reader.GetInt32(1);
						registerAssociation.IdTable = reader.GetInt32(2);
						// Add current object to the scope

						scope.Add(scopeKey, registerAssociation);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(registerAssociation.Id, registerAssociation);
						// Read the timestamp and set new and changed properties

						registerAssociation.Timestamp = reader.GetDateTime(3);
						registerAssociation.IsNew = false;
						registerAssociation.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationRegisterAssociationCategories(registerAssociation, scope);
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
			return registerAssociation;
		} 

		/// <summary>
		/// Function to load a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public RegisterAssociationEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public RegisterAssociationEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public RegisterAssociationEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idRegisterAssociation", "idRegister", "idTable"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("RegisterAssociation");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("RegisterAssociation", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteRegisterAssociation");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveRegisterAssociation");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateRegisterAssociation");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("RegisterAssociation", "idRegisterAssociation");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("RegisterAssociation", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("RegisterAssociation", "idRegisterAssociation", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(RegisterAssociationEntity registerAssociation, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@idRegister", DbType.Int32);

			parameter.Value = registerAssociation.IdRegister;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);

			parameter.Value = registerAssociation.IdTable;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a RegisterAssociationEntity in the database.
		/// </summary>
		/// <param name="registerAssociation">RegisterAssociationEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociation"/> is not a <c>RegisterAssociationEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(RegisterAssociationEntity registerAssociation)
		{
			Save(registerAssociation, null);
		} 

		/// <summary>
		/// Function to Save a RegisterAssociationEntity in the database.
		/// </summary>
		/// <param name="registerAssociation">RegisterAssociationEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="registerAssociation"/> is not a <c>RegisterAssociationEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(RegisterAssociationEntity registerAssociation, Dictionary<string,IEntity> scope)
		{
			if (registerAssociation == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = registerAssociation.Id.ToString(NumberFormatInfo.InvariantInfo) + "RegisterAssociation";
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

				if (registerAssociation.IsNew || !DataAccessConnection.ExistsEntity(registerAssociation.Id, "RegisterAssociation", "idRegisterAssociation", dbConnection, dbTransaction))
				{
					commandName = "SaveRegisterAssociation";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateRegisterAssociation";
				}
				// Create a db command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idRegisterAssociation", DbType.Int32);
					parameter.Value = registerAssociation.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(registerAssociation, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idRegisterAssociation", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					registerAssociation.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = registerAssociation.Id.ToString(NumberFormatInfo.InvariantInfo) + "RegisterAssociation";
				// Add entity to current internal scope

				scope.Add(scopeKey, registerAssociation);
				// Save collections of related objects to current entity
				if (registerAssociation.RegisterAssociationCategories != null)
				{
					this.SaveRegisterAssociationCategoriesCollection(new RegisterAssociationCategoriesDataAccess(), registerAssociation, registerAssociation.RegisterAssociationCategories, registerAssociation.IsNew, scope);
				}
				// Save objects related to current entity
				// Update
				// Close transaction if initiated by me
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				registerAssociation.IsNew = false;
				registerAssociation.Changed = false;
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
		/// Function to Delete a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="registerAssociation">RegisterAssociationEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="registerAssociation"/> is not a <c>RegisterAssociationEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(RegisterAssociationEntity registerAssociation)
		{
			Delete(registerAssociation, null);
		} 

		/// <summary>
		/// Function to Delete a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="registerAssociation">RegisterAssociationEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="registerAssociation"/> is not a <c>RegisterAssociationEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(RegisterAssociationEntity registerAssociation, Dictionary<string,IEntity> scope)
		{
			if (registerAssociation == null)
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

				registerAssociation = this.Load(registerAssociation.Id, true);
				if (registerAssociation == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteRegisterAssociation";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idRegisterAssociation", DbType.Int32);
				parameterID.Value = registerAssociation.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				if (registerAssociation.RegisterAssociationCategories != null)
				{
					this.DeleteRegisterAssociationCategoriesCollection(new RegisterAssociationCategoriesDataAccess(), registerAssociation.RegisterAssociationCategories, scope);
				}
				// Commit transaction if is mine

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(registerAssociation.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = registerAssociation.Id.ToString(NumberFormatInfo.InvariantInfo) + "RegisterAssociation";
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
			properties.Add("idRegisterAssociation", typeof( int ));

			properties.Add("idRegister", typeof( int ));
			properties.Add("idTable", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<RegisterAssociationEntity> LoadAll(bool loadRelation)
		{
			Collection<RegisterAssociationEntity> registerAssociationList = new Collection<RegisterAssociationEntity>();

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

				string cmdText = "SELECT idRegisterAssociation FROM [RegisterAssociation]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				RegisterAssociationEntity registerAssociation;
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
					registerAssociation = Load(id, loadRelation, scope);
					registerAssociationList.Add(registerAssociation);
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
			return registerAssociationList;
		} 

		/// <summary>
		/// Function to Load a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of RegisterAssociationEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<RegisterAssociationEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<RegisterAssociationEntity> registerAssociationList;

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

				string cmdText = "SELECT idRegisterAssociation, idRegister, idTable, timestamp FROM [RegisterAssociation] WHERE " + propertyName + " " + op + " @expValue";
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
				registerAssociationList = new Collection<RegisterAssociationEntity>();
				RegisterAssociationEntity registerAssociation;
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
					registerAssociation = Load(id, loadRelation, null);
					registerAssociationList.Add(registerAssociation);
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
			return registerAssociationList;
		} 

		/// <summary>
		/// Function to Load a list of RegisterAssociationEntity from database by idTable.
		/// </summary>
		/// <param name="idTable">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of RegisterAssociationEntity</returns>
		public Collection<RegisterAssociationEntity> LoadByTableCollection(int idTable, Dictionary<string,IEntity> scope)
		{
			Collection<RegisterAssociationEntity> registerAssociationList;
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

				string cmdText = "SELECT idRegisterAssociation FROM [RegisterAssociation] WHERE idTable = @idTable";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
				parameter.Value = idTable;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				registerAssociationList = new Collection<RegisterAssociationEntity>();
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
					registerAssociationList.Add(Load(id, scope));
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
			return registerAssociationList;
		} 

		/// <summary>
		/// Function to Load a list of RegisterAssociationEntity from database by idTable.
		/// </summary>
		/// <param name="idTable">Foreing key column</param>
		/// <returns>IList of RegisterAssociationEntity</returns>
		public Collection<RegisterAssociationEntity> LoadByTableCollection(int idTable)
		{
			return LoadByTableCollection(idTable, null);
		} 

		/// <summary>
		/// Function to Load the relation RegisterAssociationCategories from database.
		/// </summary>
		/// <param name="registerAssociation">RegisterAssociationEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociation"/> is not a <c>RegisterAssociationEntity</c>.
		/// </exception>
		public void LoadRelationRegisterAssociationCategories(RegisterAssociationEntity registerAssociation, Dictionary<string,IEntity> scope)
		{
			if (registerAssociation == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			RegisterAssociationCategoriesDataAccess registerAssociationCategoriesDataAccess = new RegisterAssociationCategoriesDataAccess();
			// Set connection objects to the data access

			registerAssociationCategoriesDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			registerAssociation.RegisterAssociationCategories = registerAssociationCategoriesDataAccess.LoadByRegisterAssociationCollection(registerAssociation.Id, scope);
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
		private void SaveRegisterAssociationCategoriesCollection(RegisterAssociationCategoriesDataAccess collectionDataAccess, RegisterAssociationEntity parent, Collection<RegisterAssociationCategoriesEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].RegisterAssociation = parent;
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

				string command = "SELECT idRegisterAssociationCategories FROM [RegisterAssociationCategories] WHERE idRegisterAssociation = @idRegisterAssociation AND idRegisterAssociationCategories NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idRegisterAssociation", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<RegisterAssociationCategoriesEntity> objectsToDelete = new Collection<RegisterAssociationCategoriesEntity>();
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
					RegisterAssociationCategoriesEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					RegisterAssociationCategoriesEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [RegisterAssociationCategories] WHERE idRegisterAssociationCategories = @idRegisterAssociationCategories";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idRegisterAssociationCategories", DbType.Int32);
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
		private bool DeleteRegisterAssociationCategoriesCollection(RegisterAssociationCategoriesDataAccess collectionDataAccess, Collection<RegisterAssociationCategoriesEntity> collection, Dictionary<string,IEntity> scope)
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

	} 

}

