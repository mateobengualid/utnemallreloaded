using System.Data;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Data.Common;
using UtnEmall.Server.Base;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.DataModel
{

	/// <summary>
	/// The <c>CampaignDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class CampaignDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,CampaignEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CampaignDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  CampaignDataAccess()
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

			inMemoryEntities = new Dictionary<int,CampaignEntity>();
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
		/// Function to load a CampaignEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CampaignEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Campaign";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((CampaignEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			CampaignEntity campaign = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				campaign = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, campaign);
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

					string cmdText = "SELECT idCampaign, description, name, startDate, stopDate, idUser, timestamp FROM [Campaign] WHERE idCampaign = @idCampaign";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCampaign", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					campaign = new CampaignEntity();

					if (reader.Read())
					{
						// Load fields of entity
						campaign.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							campaign.Description = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							campaign.Name = reader.GetString(2);
						}

						campaign.StartDate = reader.GetDateTime(3);
						campaign.StopDate = reader.GetDateTime(4);
						campaign.IdUser = reader.GetInt32(5);
						// Add current object to the scope

						scope.Add(scopeKey, campaign);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(campaign.Id, campaign);
						// Read the timestamp and set new and changed properties

						campaign.Timestamp = reader.GetDateTime(6);
						campaign.IsNew = false;
						campaign.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationUser(campaign, scope);
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
			return campaign;
		} 

		/// <summary>
		/// Function to load a CampaignEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CampaignEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a CampaignEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CampaignEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a CampaignEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CampaignEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idCampaign", "description", "name", "startDate", "stopDate", "idUser"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( System.DateTime ), typeof( System.DateTime ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Campaign");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Campaign", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteCampaign");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveCampaign");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateCampaign");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Campaign", "idCampaign");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Campaign", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Campaign", "idCampaign", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(CampaignEntity campaign, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@description", DbType.String);

			parameter.Value = campaign.Description;
			if (String.IsNullOrEmpty(campaign.Description))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = campaign.Name;
			if (String.IsNullOrEmpty(campaign.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@startDate", DbType.DateTime);

			parameter.Value = campaign.StartDate;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stopDate", DbType.DateTime);

			parameter.Value = campaign.StopDate;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);

			parameter.Value = campaign.IdUser;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a CampaignEntity in the database.
		/// </summary>
		/// <param name="campaign">CampaignEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaign"/> is not a <c>CampaignEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(CampaignEntity campaign)
		{
			Save(campaign, null);
		} 

		/// <summary>
		/// Function to Save a CampaignEntity in the database.
		/// </summary>
		/// <param name="campaign">CampaignEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="campaign"/> is not a <c>CampaignEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(CampaignEntity campaign, Dictionary<string,IEntity> scope)
		{
			if (campaign == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = campaign.Id.ToString(NumberFormatInfo.InvariantInfo) + "Campaign";
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

				if (campaign.IsNew || !DataAccessConnection.ExistsEntity(campaign.Id, "Campaign", "idCampaign", dbConnection, dbTransaction))
				{
					commandName = "SaveCampaign";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateCampaign";
				}
				// Create a db command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idCampaign", DbType.Int32);
					parameter.Value = campaign.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(campaign, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idCampaign", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					campaign.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = campaign.Id.ToString(NumberFormatInfo.InvariantInfo) + "Campaign";
				// Add entity to current internal scope

				scope.Add(scopeKey, campaign);
				// Save collections of related objects to current entity
				// Save objects related to current entity
				// Update
				// Close transaction if initiated by me
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				campaign.IsNew = false;
				campaign.Changed = false;
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
		/// Function to Delete a CampaignEntity from database.
		/// </summary>
		/// <param name="campaign">CampaignEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="campaign"/> is not a <c>CampaignEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(CampaignEntity campaign)
		{
			Delete(campaign, null);
		} 

		/// <summary>
		/// Function to Delete a CampaignEntity from database.
		/// </summary>
		/// <param name="campaign">CampaignEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="campaign"/> is not a <c>CampaignEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(CampaignEntity campaign, Dictionary<string,IEntity> scope)
		{
			if (campaign == null)
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

				campaign = this.Load(campaign.Id, true);
				if (campaign == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Check for related data
				CheckForDelete(campaign);
				// Create a command for delete

				string cmdText = "DeleteCampaign";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idCampaign", DbType.Int32);
				parameterID.Value = campaign.Id;
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

				inMemoryEntities.Remove(campaign.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = campaign.Id.ToString(NumberFormatInfo.InvariantInfo) + "Campaign";
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

		private static void CheckForDelete(CampaignEntity entity)
		{
			ServiceCampaignDataAccess varServiceCampaignDataAccess = new ServiceCampaignDataAccess();

			if (varServiceCampaignDataAccess.LoadWhere(ServiceCampaignEntity.DBIdCampaign, entity.Id, false, OperatorType.Equal).Count > 0)
			{
				throw new UtnEmallDataAccessException("There are services associated to this campaign.");
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
			properties.Add("idCampaign", typeof( int ));

			properties.Add("description", typeof( string ));
			properties.Add("name", typeof( string ));
			properties.Add("startDate", typeof( System.DateTime ));
			properties.Add("stopDate", typeof( System.DateTime ));
			properties.Add("idUser", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the CampaignEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<CampaignEntity> LoadAll(bool loadRelation)
		{
			Collection<CampaignEntity> campaignList = new Collection<CampaignEntity>();

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

				string cmdText = "SELECT idCampaign FROM [Campaign]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				CampaignEntity campaign;
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
					campaign = Load(id, loadRelation, scope);
					campaignList.Add(campaign);
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
			return campaignList;
		} 

		/// <summary>
		/// Function to Load a CampaignEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of CampaignEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<CampaignEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<CampaignEntity> campaignList;

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

				string cmdText = "SELECT idCampaign, description, name, startDate, stopDate, idUser, timestamp FROM [Campaign] WHERE " + propertyName + " " + op + " @expValue";
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
				campaignList = new Collection<CampaignEntity>();
				CampaignEntity campaign;
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
					campaign = Load(id, loadRelation, null);
					campaignList.Add(campaign);
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
			return campaignList;
		} 

		/// <summary>
		/// Function to Load a list of CampaignEntity from database by idUser.
		/// </summary>
		/// <param name="idUser">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of CampaignEntity</returns>
		public Collection<CampaignEntity> LoadByUserCollection(int idUser, Dictionary<string,IEntity> scope)
		{
			Collection<CampaignEntity> campaignList;
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

				string cmdText = "SELECT idCampaign FROM [Campaign] WHERE idUser = @idUser";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
				parameter.Value = idUser;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				campaignList = new Collection<CampaignEntity>();
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
					campaignList.Add(Load(id, scope));
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
			return campaignList;
		} 

		/// <summary>
		/// Function to Load a list of CampaignEntity from database by idUser.
		/// </summary>
		/// <param name="idUser">Foreing key column</param>
		/// <returns>IList of CampaignEntity</returns>
		public Collection<CampaignEntity> LoadByUserCollection(int idUser)
		{
			return LoadByUserCollection(idUser, null);
		} 

		/// <summary>
		/// Function to Load the relation User from database.
		/// </summary>
		/// <param name="campaign">CampaignEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaign"/> is not a <c>CampaignEntity</c>.
		/// </exception>
		public void LoadRelationUser(CampaignEntity campaign, Dictionary<string,IEntity> scope)
		{
			if (campaign == null)
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

				string cmdText = "SELECT idUser FROM [Campaign] WHERE idCampaign = @idCampaign";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCampaign", DbType.Int32);
				// Set command parameters values

				parameter.Value = campaign.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					UserDataAccess userDataAccess = new UserDataAccess();
					userDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					campaign.User = userDataAccess.Load(((int)idRelation), true, scope);
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

