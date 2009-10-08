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

	/// Represents a Menu, ItemTemplate
	/// Seek stadistical data about services usage
	/// Seek stadistical data about services usage. Used on client and for interface
	/// between Server and Client
	/// Relates registers on tables of datamodels with categories
	/// Bloque custom para el save de Category
	/// Bloque custom para el delete de Category
	/// Bloque custom para el save de Customer
	/// Bloque custom para el save de Category
	/// Bloque custom delete para el Data Model
	/// Bloque custom save para el Data Model
	/// <summary>
	/// The <c>ComponentDataAccess</c> is a class
	/// that provides access to the modelName stored on
	/// the database.
	/// </summary>
	public class ComponentDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,ComponentEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ComponentDataAccess</c> type.
		/// It checks if the table and stored procedure
		/// are already on the database, if not, it creates
		/// them.
		/// Sets the properties that allows to make queries
		/// by calling the LoadWhere method.
		/// </summary>
		public  ComponentDataAccess()
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

			inMemoryEntities = new Dictionary<int,ComponentEntity>();
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
		/// Function to load a ComponentEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>The entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ComponentEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Build a key for internal scope object
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
			if (scope != null)
			{
				// If scope contains the object it was already loaded,
				// return it to avoid circular references
				if (scope.ContainsKey(scopeKey))
				{
					return ((ComponentEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// If there isn't a current scope create one
				scope = new Dictionary<string,IEntity>();
			}

			ComponentEntity component = null;
			// Check if the entity was already loaded by current data access object
			// and return it if that is the case

			if (inMemoryEntities.ContainsKey(id))
			{
				component = inMemoryEntities[id];
				// Add current object to current load scope

				scope.Add(scopeKey, component);
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

					string cmdText = "SELECT idComponent, height, width, heightFactor, widthFactor, xCoordinateRelativeToParent, yCoordinateRelativeToParent, xFactorCoordinateRelativeToParent, yFactorCoordinateRelativeToParent, bold, fontColor, fontName, fontSize, italic, underline, textAlign, backgroundColor, text, dataTypes, typeOrder, title, stringHelp, descriptiveText, componentType, finalizeService, idCustomerServiceData, idTemplateListFormDocument, idParentComponent, idInputConnectionPoint, idOutputConnectionPoint, idOutputDataContext, idInputDataContext, idRelatedTable, idFieldToOrder, idFieldAssociated, timestamp FROM [Component] WHERE idComponent = @idComponent";
					// Create the command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Create the Id parameter for the query

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Use a DataReader to get data from db

					IDataReader reader = sqlCommand.ExecuteReader();
					component = new ComponentEntity();

					if (reader.Read())
					{
						// Load fields of entity
						component.Id = reader.GetInt32(0);

						component.Height = Convert.ToDouble(reader.GetDecimal(1));
						component.Width = Convert.ToDouble(reader.GetDecimal(2));
						component.HeightFactor = Convert.ToDouble(reader.GetDecimal(3));
						component.WidthFactor = Convert.ToDouble(reader.GetDecimal(4));
						component.XCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(5));
						component.YCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(6));
						component.XFactorCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(7));
						component.YFactorCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(8));
						component.Bold = reader.GetBoolean(9);
						if (!reader.IsDBNull(10))
						{
							component.FontColor = reader.GetString(10);
						}

						component.FontName = reader.GetInt32(11);
						component.FontSize = reader.GetInt32(12);
						component.Italic = reader.GetBoolean(13);
						component.Underline = reader.GetBoolean(14);
						component.TextAlign = reader.GetInt32(15);
						if (!reader.IsDBNull(16))
						{
							component.BackgroundColor = reader.GetString(16);
						}
						if (!reader.IsDBNull(17))
						{
							component.Text = reader.GetString(17);
						}

						component.DataTypes = reader.GetInt32(18);
						component.TypeOrder = reader.GetInt32(19);
						if (!reader.IsDBNull(20))
						{
							component.Title = reader.GetString(20);
						}
						if (!reader.IsDBNull(21))
						{
							component.StringHelp = reader.GetString(21);
						}
						if (!reader.IsDBNull(22))
						{
							component.DescriptiveText = reader.GetString(22);
						}

						component.ComponentType = reader.GetInt32(23);
						component.FinalizeService = reader.GetBoolean(24);
						component.IdCustomerServiceData = reader.GetInt32(25);
						component.IdTemplateListFormDocument = reader.GetInt32(26);
						component.IdParentComponent = reader.GetInt32(27);
						component.IdInputConnectionPoint = reader.GetInt32(28);
						component.IdOutputConnectionPoint = reader.GetInt32(29);
						component.IdOutputDataContext = reader.GetInt32(30);
						component.IdInputDataContext = reader.GetInt32(31);
						component.IdRelatedTable = reader.GetInt32(32);
						component.IdFieldToOrder = reader.GetInt32(33);
						component.IdFieldAssociated = reader.GetInt32(34);
						// Add current object to the scope

						scope.Add(scopeKey, component);
						// Add current object to cache of loaded entities

						inMemoryEntities.Add(component.Id, component);
						// Read the timestamp and set new and changed properties

						component.Timestamp = reader.GetDateTime(35);
						component.IsNew = false;
						component.Changed = false;
						// Close the reader

						reader.Close();
						// Load related objects if required

						if (loadRelation)
						{
							LoadRelationTemplateListFormDocument(component, scope);
							LoadRelationMenuItems(component, scope);
							LoadRelationParentComponent(component, scope);
							LoadRelationInputConnectionPoint(component, scope);
							LoadRelationOutputConnectionPoint(component, scope);
							LoadRelationOutputDataContext(component, scope);
							LoadRelationInputDataContext(component, scope);
							LoadRelationRelatedTable(component, scope);
							LoadRelationFieldToOrder(component, scope);
							LoadRelationFieldAssociated(component, scope);
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
			return component;
		} 

		/// <summary>
		/// Function to load a ComponentEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ComponentEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Function to load a ComponentEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="loadRelation">if is true load the relation</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ComponentEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Function to load a ComponentEntity from database.
		/// </summary>
		/// <param name="id">The ID of the record to load</param>
		/// <param name="scope">Internal structure used to avoid circular reference locks, must be provided if calling from other data access object</param>
		/// <returns>the entity instance</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs while accessing the database.
		/// </exception>
		public ComponentEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idComponent", "height", "width", "heightFactor", "widthFactor", "xCoordinateRelativeToParent", "yCoordinateRelativeToParent", "xFactorCoordinateRelativeToParent", "yFactorCoordinateRelativeToParent", "bold", "fontColor", "fontName", "fontSize", "italic", "underline", "textAlign", "backgroundColor", "text", "dataTypes", "typeOrder", "title", "stringHelp", "descriptiveText", "componentType", "finalizeService", "idCustomerServiceData", "idTemplateListFormDocument", "idParentComponent", "idInputConnectionPoint", "idOutputConnectionPoint", "idOutputDataContext", "idInputDataContext", "idRelatedTable", "idFieldToOrder", "idFieldAssociated"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( double ), typeof( double ), typeof( double ), typeof( double ), typeof( double ), typeof( double ), typeof( double ), typeof( double ), typeof( bool ), typeof( string ), typeof( int ), typeof( int ), typeof( bool ), typeof( bool ), typeof( int ), typeof( string ), typeof( string ), typeof( int ), typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( int ), typeof( bool ), typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Component");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Component", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteComponent");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveComponent");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateComponent");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Component", "idComponent");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Component", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Component", "idComponent", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(ComponentEntity component, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@height", DbType.Decimal);

			parameter.Value = component.Height;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@width", DbType.Decimal);

			parameter.Value = component.Width;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@heightFactor", DbType.Decimal);

			parameter.Value = component.HeightFactor;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@widthFactor", DbType.Decimal);

			parameter.Value = component.WidthFactor;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@xCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.XCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@yCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.YCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@xFactorCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.XFactorCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@yFactorCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.YFactorCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@bold", DbType.Boolean);

			parameter.Value = component.Bold;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@fontColor", DbType.String);

			parameter.Value = component.FontColor;
			if (String.IsNullOrEmpty(component.FontColor))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@fontName", DbType.Int32);

			parameter.Value = component.FontName;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@fontSize", DbType.Int32);

			parameter.Value = component.FontSize;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@italic", DbType.Boolean);

			parameter.Value = component.Italic;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@underline", DbType.Boolean);

			parameter.Value = component.Underline;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@textAlign", DbType.Int32);

			parameter.Value = component.TextAlign;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@backgroundColor", DbType.String);

			parameter.Value = component.BackgroundColor;
			if (String.IsNullOrEmpty(component.BackgroundColor))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@text", DbType.String);

			parameter.Value = component.Text;
			if (String.IsNullOrEmpty(component.Text))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@dataTypes", DbType.Int32);

			parameter.Value = component.DataTypes;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@typeOrder", DbType.Int32);

			parameter.Value = component.TypeOrder;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@title", DbType.String);

			parameter.Value = component.Title;
			if (String.IsNullOrEmpty(component.Title))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stringHelp", DbType.String);

			parameter.Value = component.StringHelp;
			if (String.IsNullOrEmpty(component.StringHelp))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@descriptiveText", DbType.String);

			parameter.Value = component.DescriptiveText;
			if (String.IsNullOrEmpty(component.DescriptiveText))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@componentType", DbType.Int32);

			parameter.Value = component.ComponentType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@finalizeService", DbType.Boolean);

			parameter.Value = component.FinalizeService;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = component.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idTemplateListFormDocument", DbType.Int32);

			parameter.Value = component.IdTemplateListFormDocument;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);

			parameter.Value = component.IdParentComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idInputConnectionPoint", DbType.Int32);

			parameter.Value = component.IdInputConnectionPoint;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idOutputConnectionPoint", DbType.Int32);

			parameter.Value = component.IdOutputConnectionPoint;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idOutputDataContext", DbType.Int32);

			parameter.Value = component.IdOutputDataContext;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idInputDataContext", DbType.Int32);

			parameter.Value = component.IdInputDataContext;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idRelatedTable", DbType.Int32);

			parameter.Value = component.IdRelatedTable;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idFieldToOrder", DbType.Int32);

			parameter.Value = component.IdFieldToOrder;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idFieldAssociated", DbType.Int32);

			parameter.Value = component.IdFieldAssociated;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Function to Save a ComponentEntity in the database.
		/// </summary>
		/// <param name="component">ComponentEntity to save</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(ComponentEntity component)
		{
			Save(component, null);
		} 

		/// <summary>
		/// Function to Save a ComponentEntity in the database.
		/// </summary>
		/// <param name="component">ComponentEntity to save</param>
		/// <param name="scope">Interna structure to avoid circular reference locks. Provide an instance when calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Save(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create a unique key to identify the object in the internal scope
			string scopeKey = component.Id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
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

				if (component.IsNew || !DataAccessConnection.ExistsEntity(component.Id, "Component", "idComponent", dbConnection, dbTransaction))
				{
					commandName = "SaveComponent";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateComponent";
					ComponentEntity componentTemp1 = new ComponentEntity();
					componentTemp1.Id = component.Id;
					LoadRelationTemplateListFormDocument(componentTemp1, scope);
					if (componentTemp1.TemplateListFormDocument != null && componentTemp1.IdTemplateListFormDocument != component.IdTemplateListFormDocument)
					{
						CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
						customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						customerServiceDataDataAccess.Delete(componentTemp1.TemplateListFormDocument, scope);
					}
					ComponentEntity componentTemp4 = new ComponentEntity();
					componentTemp4.Id = component.Id;
					LoadRelationInputConnectionPoint(componentTemp4, scope);
					if (componentTemp4.InputConnectionPoint != null && componentTemp4.IdInputConnectionPoint != component.IdInputConnectionPoint)
					{
						ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
						connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						connectionPointDataAccess.Delete(componentTemp4.InputConnectionPoint, scope);
					}
					ComponentEntity componentTemp5 = new ComponentEntity();
					componentTemp5.Id = component.Id;
					LoadRelationOutputConnectionPoint(componentTemp5, scope);
					if (componentTemp5.OutputConnectionPoint != null && componentTemp5.IdOutputConnectionPoint != component.IdOutputConnectionPoint)
					{
						ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
						connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						connectionPointDataAccess.Delete(componentTemp5.OutputConnectionPoint, scope);
					}
				}
				// Create a db command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add parameters values to current command

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
					parameter.Value = component.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(component, sqlCommand);
				// Execute the command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					component.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = component.Id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
				// Add entity to current internal scope

				scope.Add(scopeKey, component);
				// Save collections of related objects to current entity
				if (component.MenuItems != null)
				{
					this.SaveComponentCollection(new ComponentDataAccess(), component, component.MenuItems, component.IsNew, scope);
				}
				// Save objects related to current entity
				if (component.TemplateListFormDocument != null)
				{
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					customerServiceDataDataAccess.Save(component.TemplateListFormDocument, scope);
				}
				if (component.ParentComponent != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Save(component.ParentComponent, scope);
				}
				if (component.InputConnectionPoint != null)
				{
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					connectionPointDataAccess.Save(component.InputConnectionPoint, scope);
				}
				if (component.OutputConnectionPoint != null)
				{
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					connectionPointDataAccess.Save(component.OutputConnectionPoint, scope);
				}
				// Update
				Update(component);
				// Close transaction if initiated by me

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Update new and changed flags

				component.IsNew = false;
				component.Changed = false;
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
		/// Function to Delete a ComponentEntity from database.
		/// </summary>
		/// <param name="component">ComponentEntity to delete</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(ComponentEntity component)
		{
			Delete(component, null);
		} 

		/// <summary>
		/// Function to Delete a ComponentEntity from database.
		/// </summary>
		/// <param name="component">ComponentEntity to delete</param>
		/// <param name="scope">Internal structure to avoid circular reference locks. Must provide an instance while calling from other data access object.</param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public void Delete(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				component = this.Load(component.Id, true);
				if (component == null)
				{
					throw new UtnEmallDataAccessException("Error retrieving data while trying to delete.");
				}
				// Create a command for delete
				string cmdText = "DeleteComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Add values to parameters

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				parameterID.Value = component.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Execute the command

				sqlCommand.ExecuteNonQuery();
				// Delete related objects
				if (component.TemplateListFormDocument != null)
				{
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					customerServiceDataDataAccess.Delete(component.TemplateListFormDocument, scope);
				}
				if (component.MenuItems != null)
				{
					this.DeleteComponentCollection(new ComponentDataAccess(), component.MenuItems, scope);
				}

				if (component.InputConnectionPoint != null)
				{
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					connectionPointDataAccess.Delete(component.InputConnectionPoint, scope);
				}
				if (component.OutputConnectionPoint != null)
				{
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					connectionPointDataAccess.Delete(component.OutputConnectionPoint, scope);
				}
				// Commit transaction if is mine

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Remove entity from loaded objects

				inMemoryEntities.Remove(component.Id);
				// Remove entity from current internal scope

				if (scope != null)
				{
					string scopeKey = component.Id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
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
			properties.Add("idComponent", typeof( int ));

			properties.Add("height", typeof( double ));
			properties.Add("width", typeof( double ));
			properties.Add("heightFactor", typeof( double ));
			properties.Add("widthFactor", typeof( double ));
			properties.Add("xCoordinateRelativeToParent", typeof( double ));
			properties.Add("yCoordinateRelativeToParent", typeof( double ));
			properties.Add("xFactorCoordinateRelativeToParent", typeof( double ));
			properties.Add("yFactorCoordinateRelativeToParent", typeof( double ));
			properties.Add("bold", typeof( bool ));
			properties.Add("fontColor", typeof( string ));
			properties.Add("fontName", typeof( int ));
			properties.Add("fontSize", typeof( int ));
			properties.Add("italic", typeof( bool ));
			properties.Add("underline", typeof( bool ));
			properties.Add("textAlign", typeof( int ));
			properties.Add("backgroundColor", typeof( string ));
			properties.Add("text", typeof( string ));
			properties.Add("dataTypes", typeof( int ));
			properties.Add("typeOrder", typeof( int ));
			properties.Add("title", typeof( string ));
			properties.Add("stringHelp", typeof( string ));
			properties.Add("descriptiveText", typeof( string ));
			properties.Add("componentType", typeof( int ));
			properties.Add("finalizeService", typeof( bool ));
			properties.Add("idCustomerServiceData", typeof( int ));
			properties.Add("idTemplateListFormDocument", typeof( int ));
			properties.Add("idParentComponent", typeof( int ));
			properties.Add("idInputConnectionPoint", typeof( int ));
			properties.Add("idOutputConnectionPoint", typeof( int ));
			properties.Add("idOutputDataContext", typeof( int ));
			properties.Add("idInputDataContext", typeof( int ));
			properties.Add("idRelatedTable", typeof( int ));
			properties.Add("idFieldToOrder", typeof( int ));
			properties.Add("idFieldAssociated", typeof( int ));
		} 

		/// <summary>
		/// Function to Load all the ComponentEntity from database.
		/// </summary>
		/// <param name="loadRelation">If is true load the relation</param>
		/// <returns>A list of all the entities</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// If a DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<ComponentEntity> LoadAll(bool loadRelation)
		{
			Collection<ComponentEntity> componentList = new Collection<ComponentEntity>();

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

				string cmdText = "SELECT idComponent FROM [Component]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();

				ComponentEntity component;
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
					component = Load(id, loadRelation, scope);
					componentList.Add(component);
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
			return componentList;
		} 

		/// <summary>
		/// Function to Load a ComponentEntity from database.
		/// </summary>
		/// <param name="propertyName">A string with the name of the field or a
		/// constant from the class that represent that field</param>
		/// <param name="expValue">The value that will be inserted on the where
		/// clause of the sql query</param>
		/// <param name="loadRelation">If is true load the relations</param>
		/// <returns>A list containing all the entities that match the where clause</returns>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="propertyName"/> is null or empty.
		/// If <paramref name="propertyName"/> is not a property of ComponentEntity class.
		/// If <paramref name="expValue"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		public Collection<ComponentEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<ComponentEntity> componentList;

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

				string cmdText = "SELECT idComponent, height, width, heightFactor, widthFactor, xCoordinateRelativeToParent, yCoordinateRelativeToParent, xFactorCoordinateRelativeToParent, yFactorCoordinateRelativeToParent, bold, fontColor, fontName, fontSize, italic, underline, textAlign, backgroundColor, text, dataTypes, typeOrder, title, stringHelp, descriptiveText, componentType, finalizeService, idCustomerServiceData, idTemplateListFormDocument, idParentComponent, idInputConnectionPoint, idOutputConnectionPoint, idOutputDataContext, idInputDataContext, idRelatedTable, idFieldToOrder, idFieldAssociated, timestamp FROM [Component] WHERE " + propertyName + " " + op + " @expValue";
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
				componentList = new Collection<ComponentEntity>();
				ComponentEntity component;
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
					component = Load(id, loadRelation, null);
					componentList.Add(component);
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
			return componentList;
		} 

		/// <summary>
		/// Function to Load a list of ComponentEntity from database by idCustomerServiceData.
		/// </summary>
		/// <param name="idCustomerServiceData">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByCustomerServiceDataCollection(int idCustomerServiceData, Dictionary<string,IEntity> scope)
		{
			Collection<ComponentEntity> componentList;
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

				string cmdText = "SELECT idComponent FROM [Component] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				parameter.Value = idCustomerServiceData;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				componentList = new Collection<ComponentEntity>();
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
					componentList.Add(Load(id, scope));
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
			return componentList;
		} 

		/// <summary>
		/// Function to Load a list of ComponentEntity from database by idCustomerServiceData.
		/// </summary>
		/// <param name="idCustomerServiceData">Foreing key column</param>
		/// <returns>IList of ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByCustomerServiceDataCollection(int idCustomerServiceData)
		{
			return LoadByCustomerServiceDataCollection(idCustomerServiceData, null);
		} 

		/// <summary>
		/// Function to Load the relation TemplateListFormDocument from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationTemplateListFormDocument(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idTemplateListFormDocument FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.TemplateListFormDocument = customerServiceDataDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation MenuItems from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <param name="scope">Internal structure to avoid problems with circular referencies</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationMenuItems(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Create data access object for related object
			ComponentDataAccess componentDataAccess = new ComponentDataAccess();
			// Set connection objects to the data access

			componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Load related objects

			component.MenuItems = componentDataAccess.LoadByComponentCollection(component.Id, scope);
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
		private void SaveComponentCollection(ComponentDataAccess collectionDataAccess, ComponentEntity parent, Collection<ComponentEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].ParentComponent = parent;
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

				string command = "SELECT idComponent FROM [Component] WHERE idParentComponent = @idParentComponent AND idComponent NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);
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
		/// Function to Load a list of ComponentEntity from database by idComponent.
		/// </summary>
		/// <param name="idComponent">Foreing key column</param>
		/// <param name="scope">Internal data structure to avoid circular reference problems</param>
		/// <returns>List of ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByComponentCollection(int idComponent, Dictionary<string,IEntity> scope)
		{
			Collection<ComponentEntity> componentList;
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

				string cmdText = "SELECT idComponent FROM [Component] WHERE idParentComponent = @idParentComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Set command parameters values

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);
				parameter.Value = idComponent;
				sqlCommand.Parameters.Add(parameter);
				// Create a DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				componentList = new Collection<ComponentEntity>();
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
					componentList.Add(Load(id, scope));
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
			return componentList;
		} 

		/// <summary>
		/// Function to Load a list of ComponentEntity from database by idComponent.
		/// </summary>
		/// <param name="idComponent">Foreing key column</param>
		/// <returns>IList of ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByComponentCollection(int idComponent)
		{
			return LoadByComponentCollection(idComponent, null);
		} 

		/// <summary>
		/// Function to Load the relation ParentComponent from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationParentComponent(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idParentComponent FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.ParentComponent = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation InputConnectionPoint from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationInputConnectionPoint(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idInputConnectionPoint FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.InputConnectionPoint = connectionPointDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation OutputConnectionPoint from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationOutputConnectionPoint(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idOutputConnectionPoint FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.OutputConnectionPoint = connectionPointDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation OutputDataContext from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationOutputDataContext(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idOutputDataContext FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					TableDataAccess tableDataAccess = new TableDataAccess();
					tableDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.OutputDataContext = tableDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation InputDataContext from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationInputDataContext(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idInputDataContext FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					TableDataAccess tableDataAccess = new TableDataAccess();
					tableDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.InputDataContext = tableDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation RelatedTable from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationRelatedTable(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idRelatedTable FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					TableDataAccess tableDataAccess = new TableDataAccess();
					tableDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.RelatedTable = tableDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation FieldToOrder from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationFieldToOrder(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idFieldToOrder FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					FieldDataAccess fieldDataAccess = new FieldDataAccess();
					fieldDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.FieldToOrder = fieldDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Load the relation FieldAssociated from database.
		/// </summary>
		/// <param name="component">ComponentEntity parent</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationFieldAssociated(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
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

				string cmdText = "SELECT idFieldAssociated FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Set command parameters values

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					FieldDataAccess fieldDataAccess = new FieldDataAccess();
					fieldDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Load related object

					component.FieldAssociated = fieldDataAccess.Load(((int)idRelation), true, scope);
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
		/// Function to Update a ComponentEntity from database.
		/// </summary>
		/// <param name="component">ComponentEntity to update</param>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="component"/> is not a <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// If an DbException occurs in the try block while accessing the database.
		/// </exception>
		private void Update(ComponentEntity component)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null", "component");
			}
			// Build update command
			string commandName = "UpdateComponent";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Set update parameters values

			IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
			parameter.Value = component.Id;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@height", DbType.Decimal);

			parameter.Value = component.Height;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@width", DbType.Decimal);

			parameter.Value = component.Width;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@heightFactor", DbType.Decimal);

			parameter.Value = component.HeightFactor;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@widthFactor", DbType.Decimal);

			parameter.Value = component.WidthFactor;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@xCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.XCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@yCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.YCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@xFactorCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.XFactorCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@yFactorCoordinateRelativeToParent", DbType.Decimal);

			parameter.Value = component.YFactorCoordinateRelativeToParent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@bold", DbType.Boolean);

			parameter.Value = component.Bold;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@fontColor", DbType.String);

			parameter.Value = component.FontColor;
			if (String.IsNullOrEmpty(component.FontColor))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@fontName", DbType.Int32);

			parameter.Value = component.FontName;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@fontSize", DbType.Int32);

			parameter.Value = component.FontSize;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@italic", DbType.Boolean);

			parameter.Value = component.Italic;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@underline", DbType.Boolean);

			parameter.Value = component.Underline;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@textAlign", DbType.Int32);

			parameter.Value = component.TextAlign;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@backgroundColor", DbType.String);

			parameter.Value = component.BackgroundColor;
			if (String.IsNullOrEmpty(component.BackgroundColor))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@text", DbType.String);

			parameter.Value = component.Text;
			if (String.IsNullOrEmpty(component.Text))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@dataTypes", DbType.Int32);

			parameter.Value = component.DataTypes;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@typeOrder", DbType.Int32);

			parameter.Value = component.TypeOrder;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@title", DbType.String);

			parameter.Value = component.Title;
			if (String.IsNullOrEmpty(component.Title))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stringHelp", DbType.String);

			parameter.Value = component.StringHelp;
			if (String.IsNullOrEmpty(component.StringHelp))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@descriptiveText", DbType.String);

			parameter.Value = component.DescriptiveText;
			if (String.IsNullOrEmpty(component.DescriptiveText))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@componentType", DbType.Int32);

			parameter.Value = component.ComponentType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@finalizeService", DbType.Boolean);

			parameter.Value = component.FinalizeService;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = component.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idTemplateListFormDocument", DbType.Int32);

			parameter.Value = component.IdTemplateListFormDocument;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);

			parameter.Value = component.IdParentComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idInputConnectionPoint", DbType.Int32);

			parameter.Value = component.IdInputConnectionPoint;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idOutputConnectionPoint", DbType.Int32);

			parameter.Value = component.IdOutputConnectionPoint;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idOutputDataContext", DbType.Int32);

			parameter.Value = component.IdOutputDataContext;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idInputDataContext", DbType.Int32);

			parameter.Value = component.IdInputDataContext;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idRelatedTable", DbType.Int32);

			parameter.Value = component.IdRelatedTable;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idFieldToOrder", DbType.Int32);

			parameter.Value = component.IdFieldToOrder;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idFieldAssociated", DbType.Int32);

			parameter.Value = component.IdFieldAssociated;
			sqlCommand.Parameters.Add(parameter);
			// Execute the update

			sqlCommand.ExecuteNonQuery();
			// Update new and changed flags

			component.IsNew = false;
			component.Changed = false;
		} 

	} 

}

