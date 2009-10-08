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
	/// The <c>CustomerDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class CustomerDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,CustomerEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CustomerDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  CustomerDataAccess()
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

			inMemoryEntities = new Dictionary<int,CustomerEntity>();
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
		/// Function to load a CustomerEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((CustomerEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			CustomerEntity customer = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				customer = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, customer);
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

					string cmdText = "SELECT idCustomer, name, surname, address, phoneNumber, userName, password, birthday, howManyChildren, gender, civilState, idMall, timestamp FROM [Customer] WHERE idCustomer = @idCustomer";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					customer = new CustomerEntity();

					if (reader.Read())
					{
						// Load fields of entity
						customer.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							customer.Name = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							customer.Surname = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							customer.Address = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							customer.PhoneNumber = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							customer.UserName = reader.GetString(5);
						}
						if (!reader.IsDBNull(6))
						{
							customer.Password = reader.GetString(6);
						}

						customer.Birthday = reader.GetDateTime(7);
						customer.HowManyChildren = reader.GetInt32(8);
						customer.Gender = reader.GetInt32(9);
						customer.CivilState = reader.GetInt32(10);
						customer.IdMall = reader.GetInt32(11);
						// Add current object to the scope

						scope.Add(scopeKey, customer);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(customer.Id, customer);
						// Read the timestamp and set new and changed properties

						customer.Timestamp = reader.GetDateTime(12);
						customer.IsNew = false;
						customer.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationPreferences(customer, scope);
							LoadRelationDeviceProfile(customer, scope);
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
			return customer;
		} 

		/// <summary>
		/// Function to load a CustomerEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a CustomerEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a CustomerEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public CustomerEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idCustomer", "name", "surname", "address", "phoneNumber", "userName", "password", "birthday", "howManyChildren", "gender", "civilState", "idMall"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( System.DateTime ), typeof( int ), typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Customer");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Customer", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteCustomer");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveCustomer");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateCustomer");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Customer", "idCustomer");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Customer", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Customer", "idCustomer", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(CustomerEntity customer, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = customer.Name;
			if (String.IsNullOrEmpty(customer.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@surname", DbType.String);

			parameter.Value = customer.Surname;
			if (String.IsNullOrEmpty(customer.Surname))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@address", DbType.String);

			parameter.Value = customer.Address;
			if (String.IsNullOrEmpty(customer.Address))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@phoneNumber", DbType.String);

			parameter.Value = customer.PhoneNumber;
			if (String.IsNullOrEmpty(customer.PhoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@userName", DbType.String);

			parameter.Value = customer.UserName;
			if (String.IsNullOrEmpty(customer.UserName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@password", DbType.String);

			parameter.Value = customer.Password;
			if (String.IsNullOrEmpty(customer.Password))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@birthday", DbType.DateTime);

			parameter.Value = customer.Birthday;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@howManyChildren", DbType.Int32);

			parameter.Value = customer.HowManyChildren;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@gender", DbType.Int32);

			parameter.Value = customer.Gender;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@civilState", DbType.Int32);

			parameter.Value = customer.CivilState;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);

			parameter.Value = customer.IdMall;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a CustomerEntity in the database.
		/// </summary>
		/// <param name="customer">CustomerEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customer"/> is not a <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(CustomerEntity customer)
		{
			Save(customer, null);
		} 

		/// <summary>
		/// Function to Save a CustomerEntity in the database.
		/// </summary>
		/// <param name="customer">CustomerEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="customer"/> is not a <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = customer.Id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
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

				if (customer.IsNew || !DataAccessConnection.ExistsEntity(customer.Id, "Customer", "idCustomer", dbConnection, dbTransaction))
				{
					commandName = "SaveCustomer";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateCustomer";
				}
				// Create a db command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
					parameter.Value = customer.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(customer, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					customer.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = customer.Id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
				// Add entity to current internal scope

				scope.Add(scopeKey, customer);
				// Save collections of related objects to current entity
				if (customer.Preferences != null)
				{
					this.SavePreferenceCollection(new PreferenceDataAccess(), customer, customer.Preferences, customer.IsNew, scope);
				}
				if (customer.DeviceProfile != null)
				{
					this.SaveDeviceProfileCollection(new DeviceProfileDataAccess(), customer, customer.DeviceProfile, customer.IsNew, scope);
				}
				// Save objects related to current entity
				// Update
				// Close transaction if initiated by me
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				customer.IsNew = false;
				customer.Changed = false;
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
		/// Function to Delete a CustomerEntity from database.
		/// </summary>
		/// <param name="customer">CustomerEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="customer"/> is not a <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(CustomerEntity customer)
		{
			Delete(customer, null);
		} 

		/// <summary>
		/// Function to Delete a CustomerEntity from database.
		/// </summary>
		/// <param name="customer">CustomerEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="customer"/> is not a <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
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

				customer = this.Load(customer.Id, true);
				if (customer == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteCustomer";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				parameterID.Value = customer.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				if (customer.Preferences != null)
				{
					this.DeletePreferenceCollection(new PreferenceDataAccess(), customer.Preferences, scope);
				}
				if (customer.DeviceProfile != null)
				{
					this.DeleteDeviceProfileCollection(new DeviceProfileDataAccess(), customer.DeviceProfile, scope);
				}
				// Commit transaction if is mine

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(customer.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = customer.Id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
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
			properties.Add("idCustomer", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("surname", typeof( string ));
			properties.Add("address", typeof( string ));
			properties.Add("phoneNumber", typeof( string ));
			properties.Add("userName", typeof( string ));
			properties.Add("password", typeof( string ));
			properties.Add("birthday", typeof( System.DateTime ));
			properties.Add("howManyChildren", typeof( int ));
			properties.Add("gender", typeof( int ));
			properties.Add("civilState", typeof( int ));
			properties.Add("idMall", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the CustomerEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<CustomerEntity> LoadAll(bool loadRelation)
		{
			Collection<CustomerEntity> customerList = new Collection<CustomerEntity>();

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

				string cmdText = "SELECT idCustomer FROM [Customer]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				CustomerEntity customer;
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
					customer = Load(id, loadRelation, scope);
					customerList.Add(customer);
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
			return customerList;
		} 

		/// <summary>
		/// Function to Load a CustomerEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of CustomerEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<CustomerEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<CustomerEntity> customerList;

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

				string cmdText = "SELECT idCustomer, name, surname, address, phoneNumber, userName, password, birthday, howManyChildren, gender, civilState, idMall, timestamp FROM [Customer] WHERE " + propertyName + " " + op + " @expValue";
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
				customerList = new Collection<CustomerEntity>();
				CustomerEntity customer;
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
					customer = Load(id, loadRelation, null);
					customerList.Add(customer);
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
			return customerList;
		} 

		/// <summary>
		/// Function to Load the relation Preferences from database.
		/// </summary>
		/// <param name="customer">CustomerEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customer"/> is not a <c>CustomerEntity</c>.
		/// </exception>
		public void LoadRelationPreferences(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			PreferenceDataAccess preferenceDataAccess = new PreferenceDataAccess();
			// Set connection objects to the data access

			preferenceDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			customer.Preferences = preferenceDataAccess.LoadByCustomerCollection(customer.Id, scope);
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
		private void SavePreferenceCollection(PreferenceDataAccess collectionDataAccess, CustomerEntity parent, Collection<PreferenceEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Customer = parent;
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

				string command = "SELECT idPreference FROM [Preference] WHERE idCustomer = @idCustomer AND idPreference NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<PreferenceEntity> objectsToDelete = new Collection<PreferenceEntity>();
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
					PreferenceEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					PreferenceEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [Preference] WHERE idPreference = @idPreference";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
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
		private bool DeletePreferenceCollection(PreferenceDataAccess collectionDataAccess, Collection<PreferenceEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Function to Load a list of CustomerEntity from database by idMall.
		/// </summary>
		/// <param name="idMall">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of CustomerEntity</returns>
		public Collection<CustomerEntity> LoadByMallCollection(int idMall, Dictionary<string,IEntity> scope)
		{
			Collection<CustomerEntity> customerList;
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

				string cmdText = "SELECT idCustomer FROM [Customer] WHERE idMall = @idMall";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);
				parameter.Value = idMall;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				customerList = new Collection<CustomerEntity>();
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
					customerList.Add(Load(id, scope));
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
			return customerList;
		} 

		/// <summary>
		/// Function to Load a list of CustomerEntity from database by idMall.
		/// </summary>
		/// <param name="idMall">Foreing key column</param>
		/// <returns>IList of CustomerEntity</returns>
		public Collection<CustomerEntity> LoadByMallCollection(int idMall)
		{
			return LoadByMallCollection(idMall, null);
		} 

		/// <summary>
		/// Function to Load the relation DeviceProfile from database.
		/// </summary>
		/// <param name="customer">CustomerEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customer"/> is not a <c>CustomerEntity</c>.
		/// </exception>
		public void LoadRelationDeviceProfile(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			DeviceProfileDataAccess deviceProfileDataAccess = new DeviceProfileDataAccess();
			// Set connection objects to the data access

			deviceProfileDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			customer.DeviceProfile = deviceProfileDataAccess.LoadByCustomerCollection(customer.Id, scope);
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
		private void SaveDeviceProfileCollection(DeviceProfileDataAccess collectionDataAccess, CustomerEntity parent, Collection<DeviceProfileEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Customer = parent;
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

				string command = "SELECT idDeviceProfile FROM [DeviceProfile] WHERE idCustomer = @idCustomer AND idDeviceProfile NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<DeviceProfileEntity> objectsToDelete = new Collection<DeviceProfileEntity>();
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
					DeviceProfileEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					DeviceProfileEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Create the command
						string sql = "SELECT timestamp FROM [DeviceProfile] WHERE idDeviceProfile = @idDeviceProfile";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Set the command's parameters values

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idDeviceProfile", DbType.Int32);
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
		private bool DeleteDeviceProfileCollection(DeviceProfileDataAccess collectionDataAccess, Collection<DeviceProfileEntity> collection, Dictionary<string,IEntity> scope)
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

