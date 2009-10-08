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
	/// The <c>ConnectionPointDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class ConnectionPointDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,ConnectionPointEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ConnectionPointDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  ConnectionPointDataAccess()
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

			inMemoryEntities = new Dictionary<int,ConnectionPointEntity>();
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
		/// Function to load a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ConnectionPointEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((ConnectionPointEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			ConnectionPointEntity connectionPoint = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				connectionPoint = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, connectionPoint);
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

					string cmdText = "SELECT idConnectionPoint, connectionType, xCoordinateRelativeToParent, yCoordinateRelativeToParent, idParentComponent, idComponent, idConnectionWidget, timestamp FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					connectionPoint = new ConnectionPointEntity();

					if (reader.Read())
					{
						// Load fields of entity
						connectionPoint.Id = reader.GetInt32(0);

						connectionPoint.ConnectionType = reader.GetInt32(1);
						connectionPoint.XCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(2));
						connectionPoint.YCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(3));
						connectionPoint.IdParentComponent = reader.GetInt32(4);
						connectionPoint.IdComponent = reader.GetInt32(5);
						connectionPoint.IdConnectionWidget = reader.GetInt32(6);
						// Add current object to the scope

						scope.Add(scopeKey, connectionPoint);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(connectionPoint.Id, connectionPoint);
						// Read the timestamp and set new and changed properties

						connectionPoint.Timestamp = reader.GetDateTime(7);
						connectionPoint.IsNew = false;
						connectionPoint.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationParentComponent(connectionPoint, scope);
							LoadRelationComponent(connectionPoint, scope);
							LoadRelationConnectionWidget(connectionPoint, scope);
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
			return connectionPoint;
		} 

		/// <summary>
		/// Function to load a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ConnectionPointEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ConnectionPointEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ConnectionPointEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idConnectionPoint", "connectionType", "xCoordinateRelativeToParent", "yCoordinateRelativeToParent", "idParentComponent", "idComponent", "idConnectionWidget"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( int ), typeof( double ), typeof( double ), typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("ConnectionPoint");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("ConnectionPoint", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteConnectionPoint");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveConnectionPoint");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateConnectionPoint");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("ConnectionPoint", "idConnectionPoint");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("ConnectionPoint", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("ConnectionPoint", "idConnectionPoint", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(ConnectionPointEntity connectionPoint, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@connectionType", DbType.Int32);

			parameter.Value = connectionPoint.ConnectionType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@xCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = connectionPoint.XCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@yCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = connectionPoint.YCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);

			parameter.Value = connectionPoint.IdParentComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);

			parameter.Value = connectionPoint.IdComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);

			parameter.Value = connectionPoint.IdConnectionWidget;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a ConnectionPointEntity in the database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(ConnectionPointEntity connectionPoint)
		{
			Save(connectionPoint, null);
		} 

		/// <summary>
		/// Function to Save a ConnectionPointEntity in the database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = connectionPoint.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
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

				if (connectionPoint.IsNew || !DataAccessConnection.ExistsEntity(connectionPoint.Id, "ConnectionPoint", "idConnectionPoint", dbConnection, dbTransaction))
				{
					commandName = "SaveConnectionPoint";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateConnectionPoint";
					ConnectionPointEntity connectionPointTemp0 = new ConnectionPointEntity();
					connectionPointTemp0.Id = connectionPoint.Id;
					LoadRelationParentComponent(connectionPointTemp0, scope);
					if (connectionPointTemp0.ParentComponent != null && connectionPointTemp0.IdParentComponent != connectionPoint.IdParentComponent)
					{
						ComponentDataAccess componentDataAccess = new ComponentDataAccess();
						componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						componentDataAccess.Delete(connectionPointTemp0.ParentComponent, scope);
					}
				}
				// Create a db command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
					parameter.Value = connectionPoint.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(connectionPoint, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					connectionPoint.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = connectionPoint.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
				// Add entity to current internal scope

				scope.Add(scopeKey, connectionPoint);
				// Save collections of related objects to current entity
				// Save objects related to current entity
				if (connectionPoint.ParentComponent != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Save(connectionPoint.ParentComponent, scope);
				}
				if (connectionPoint.Component != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Save(connectionPoint.Component, scope);
				}
				// Update
				Update(connectionPoint);
				// Close transaction if initiated by me

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				connectionPoint.IsNew = false;
				connectionPoint.Changed = false;
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
		/// Function to Delete a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(ConnectionPointEntity connectionPoint)
		{
			Delete(connectionPoint, null);
		} 

		/// <summary>
		/// Function to Delete a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
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

				connectionPoint = this.Load(connectionPoint.Id, true);
				if (connectionPoint == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				parameterID.Value = connectionPoint.Id;
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

				inMemoryEntities.Remove(connectionPoint.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = connectionPoint.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
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
			properties.Add("idConnectionPoint", typeof( int ));

			properties.Add("connectionType", typeof( int ));
			properties.Add("xCoordinateRelativeToParent", typeof( double ));
			properties.Add("yCoordinateRelativeToParent", typeof( double ));
			properties.Add("idParentComponent", typeof( int ));
			properties.Add("idComponent", typeof( int ));
			properties.Add("idConnectionWidget", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the ConnectionPointEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<ConnectionPointEntity> LoadAll(bool loadRelation)
		{
			Collection<ConnectionPointEntity> connectionPointList = new Collection<ConnectionPointEntity>();

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

				string cmdText = "SELECT idConnectionPoint FROM [ConnectionPoint]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				ConnectionPointEntity connectionPoint;
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
					connectionPoint = Load(id, loadRelation, scope);
					connectionPointList.Add(connectionPoint);
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
			return connectionPointList;
		} 

		/// <summary>
		/// Function to Load a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of ConnectionPointEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<ConnectionPointEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<ConnectionPointEntity> connectionPointList;

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

				string cmdText = "SELECT idConnectionPoint, connectionType, xCoordinateRelativeToParent, yCoordinateRelativeToParent, idParentComponent, idComponent, idConnectionWidget, timestamp FROM [ConnectionPoint] WHERE " + propertyName + " " + op + " @expValue";
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
				connectionPointList = new Collection<ConnectionPointEntity>();
				ConnectionPointEntity connectionPoint;
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
					connectionPoint = Load(id, loadRelation, null);
					connectionPointList.Add(connectionPoint);
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
			return connectionPointList;
		} 

		/// <summary>
		/// Function to Load the relation ParentComponent from database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		public void LoadRelationParentComponent(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
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

				string cmdText = "SELECT idParentComponent FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				// Set command parameters values

				parameter.Value = connectionPoint.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					connectionPoint.ParentComponent = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load a list of ConnectionPointEntity from database by idComponent.
		/// </summary>
		/// <param name="idComponent">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of ConnectionPointEntity</returns>
		public Collection<ConnectionPointEntity> LoadByComponentCollection(int idComponent, Dictionary<string,IEntity> scope)
		{
			Collection<ConnectionPointEntity> connectionPointList;
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

				string cmdText = "SELECT idConnectionPoint FROM [ConnectionPoint] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				parameter.Value = idComponent;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				connectionPointList = new Collection<ConnectionPointEntity>();
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
					connectionPointList.Add(Load(id, scope));
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
			return connectionPointList;
		} 

		/// <summary>
		/// Function to Load a list of ConnectionPointEntity from database by idComponent.
		/// </summary>
		/// <param name="idComponent">Foreing key column</param>
		/// <returns>IList of ConnectionPointEntity</returns>
		public Collection<ConnectionPointEntity> LoadByComponentCollection(int idComponent)
		{
			return LoadByComponentCollection(idComponent, null);
		} 

		/// <summary>
		/// Function to Load the relation Component from database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		public void LoadRelationComponent(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
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

				string cmdText = "SELECT idComponent FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				// Set command parameters values

				parameter.Value = connectionPoint.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					connectionPoint.Component = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation ConnectionWidget from database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		public void LoadRelationConnectionWidget(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
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

				string cmdText = "SELECT idConnectionWidget FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				// Set command parameters values

				parameter.Value = connectionPoint.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionWidgetDataAccess connectionWidgetDataAccess = new ConnectionWidgetDataAccess();
					connectionWidgetDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					connectionPoint.ConnectionWidget = connectionWidgetDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Update a ConnectionPointEntity from database.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity to update</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="connectionPoint"/> is not a <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		private void Update(ConnectionPointEntity connectionPoint)
		{
			if (connectionPoint == null)
			{
				throw new ArgumentException("The argument can't be null", "connectionPoint");
			}
			// Build update command
			string commandName = "UpdateConnectionPoint";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Set update parameters values

			IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
			parameter.Value = connectionPoint.Id;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@connectionType", DbType.Int32);

			parameter.Value = connectionPoint.ConnectionType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@xCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = connectionPoint.XCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@yCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = connectionPoint.YCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);

			parameter.Value = connectionPoint.IdParentComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);

			parameter.Value = connectionPoint.IdComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);

			parameter.Value = connectionPoint.IdConnectionWidget;
			sqlCommand.Parameters.Add(parameter);
			// Execute the update

			sqlCommand.ExecuteNonQuery();
			// Update new and changed flags

			connectionPoint.IsNew = false;
			connectionPoint.Changed = false;
		} 

	} 

}

