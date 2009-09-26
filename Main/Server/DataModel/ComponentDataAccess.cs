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
	/// El <c>ComponentDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
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
		/// Inicializa una nueva instancia de
		/// <c>ComponentDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
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
		/// Establece la conexión y la transacción en el caso de que una transacción global se este ejecutando
		/// </summary>
		/// <param name="connection">La conexión IDbConnection</param>
		/// <param name="transaction">La transacción global IDbTransaction</param>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void SetConnectionObjects(IDbConnection connection, IDbTransaction transaction)
		{
			if (connection == null)
			{
				throw new ArgumentException("The connection cannot be null");
			}
			this.dbConnection = connection;
			this.dbTransaction = transaction;
			this.isGlobalTransaction = true;
		} 

		/// <summary>
		/// Función para cargar un ComponentEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ComponentEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((ComponentEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			ComponentEntity component = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				component = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, component);
			}
			else 
			{
				bool closeConnection = false;
				try 
				{
					// Abrir una nueva conexión si no es una transaccion
					if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
					{
						closeConnection = true;
						dbConnection = dataAccess.GetNewConnection();
						dbConnection.Open();
					}

					string cmdText = "SELECT idComponent, height, width, heightFactor, widthFactor, xCoordinateRelativeToParent, yCoordinateRelativeToParent, xFactorCoordinateRelativeToParent, yFactorCoordinateRelativeToParent, bold, fontColor, fontName, fontSize, italic, underline, textAlign, backgroundColor, text, dataTypes, typeOrder, title, stringHelp, descriptiveText, componentType, finalizeService, idCustomerServiceData, idTemplateListFormDocument, idParentComponent, idInputConnectionPoint, idOutputConnectionPoint, idOutputDataContext, idInputDataContext, idRelatedTable, idFieldToOrder, idFieldAssociated, timestamp FROM [Component] WHERE idComponent = @idComponent";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					component = new ComponentEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
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
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, component);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(component.Id, component);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						component.Timestamp = reader.GetDateTime(35);
						component.IsNew = false;
						component.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

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
					// Relanza la excepcion como una excepcion personalizada
					throw new UtnEmallDataAccessException(dbException.Message, dbException);
				}
				finally 
				{
					// Cierra la conexión si fue creada dentro de la Función
					if (closeConnection)
					{
						dbConnection.Close();
					}
				}
			}
			// Retorna la entidad cargada
			return component;
		} 

		/// <summary>
		/// Función para cargar un ComponentEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ComponentEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un ComponentEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ComponentEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un ComponentEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ComponentEntity Load(int id, Dictionary<string,IEntity> scope)
		{
			return Load(id, true, scope);
		} 

		/// <summary>
		/// Función que controla y crea la tabla y los procedimientos almacenados para esta clase.
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
		/// Función que guarda un ComponentEntity en la base de datos.
		/// </summary>
		/// <param name="component">ComponentEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ComponentEntity component)
		{
			Save(component, null);
		} 

		/// <summary>
		/// Función que guarda un ComponentEntity en la base de datos.
		/// </summary>
		/// <param name="component">ComponentEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = component.Id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
			if (scope != null)
			{
				// Si se encuentra dentro del scope lo retornamos
				if (scope.ContainsKey(scopeKey))
				{
					return;
				}
			}
			else 
			{
				// Crea un nuevo scope si este no fue enviado
				scope = new Dictionary<string,IEntity>();
			}

			try 
			{
				// Crea una nueva conexion y una nueva transaccion si no hay una a nivel superior
				if (!isGlobalTransaction)
				{
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
					dbTransaction = dbConnection.BeginTransaction();
				}

				string commandName = "";
				bool isUpdate = false;
				// Verifica si se debe hacer una actualización o una inserción

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
				// Se crea un command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
					parameter.Value = component.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(component, sqlCommand);
				// Ejecutar el command
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
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, component);
				// Guarda las colecciones de objetos relacionados.
				if (component.MenuItems != null)
				{
					this.SaveComponentCollection(new ComponentDataAccess(), component, component.MenuItems, component.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
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
				// Actualizar
				Update(component);
				// Cierra la conexión si fue abierta en la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				component.IsNew = false;
				component.Changed = false;
			}
			catch (DbException dbException)
			{
				// Anula la transaccion
				if (!isGlobalTransaction)
				{
					dbTransaction.Rollback();
				}
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (!isGlobalTransaction)
				{
					dbConnection.Close();
					dbConnection = null;
					dbTransaction = null;
				}
			}
		} 

		/// <summary>
		/// Función que elimina un ComponentEntity de la base de datos.
		/// </summary>
		/// <param name="component">ComponentEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ComponentEntity component)
		{
			Delete(component, null);
		} 

		/// <summary>
		/// Función que elimina un ComponentEntity de la base de datos.
		/// </summary>
		/// <param name="component">ComponentEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			try 
			{
				// Abrir una nueva conexión e inicializar una transacción si es necesario
				if (!isGlobalTransaction)
				{
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
					dbTransaction = dbConnection.BeginTransaction();
				}
				// Carga la entidad para garantizar eliminar todos los datos antiguos.

				component = this.Load(component.Id, true);
				if (component == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				parameterID.Value = component.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
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
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(component.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = component.Id.ToString(NumberFormatInfo.InvariantInfo) + "Component";
					scope.Remove(scopeKey);
				}
			}
			catch (DbException dbException)
			{
				// Anula la transaccion
				if (!isGlobalTransaction)
				{
					dbTransaction.Rollback();
				}
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue abierta dentro de la Función
				if (!isGlobalTransaction)
				{
					dbConnection.Close();
					dbConnection = null;
					dbTransaction = null;
				}
			}
		} 

		/// <summary>
		/// Agrega al diccionario las propiedades que pueden ser usadas como primer parametro de los metodos LoadWhere
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
		/// Función que carga todos los ComponentEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<ComponentEntity> LoadAll(bool loadRelation)
		{
			Collection<ComponentEntity> componentList = new Collection<ComponentEntity>();

			bool closeConnection = false;
			try 
			{
				// Abrir una nueva conexión de ser necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Construir la consulta

				string cmdText = "SELECT idComponent FROM [Component]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				ComponentEntity component;
				// Lee los ids y los inserta en una lista

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}
				// Cierra el DataReader

				reader.Close();
				// Crea un scope

				Dictionary<string,IEntity> scope = new Dictionary<string,IEntity>();
				// Carga las entidades y las agrega a la lista a retornar

				foreach(int  id in listId)
				{
					component = Load(id, loadRelation, scope);
					componentList.Add(component);
				}
			}
			catch (DbException dbException)
			{
				// Relanza la excepcion como una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			// Retorna la entidad cargada
			return componentList;
		} 

		/// <summary>
		/// Función para cargar un ComponentEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase ComponentEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
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
				// Abrir una nueva conexión con la base de datos si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}

				string op = DataAccessConnection.GetOperatorString(operatorType);
				// Construir la consulta

				string cmdText = "SELECT idComponent, height, width, heightFactor, widthFactor, xCoordinateRelativeToParent, yCoordinateRelativeToParent, xFactorCoordinateRelativeToParent, yFactorCoordinateRelativeToParent, bold, fontColor, fontName, fontSize, italic, underline, textAlign, backgroundColor, text, dataTypes, typeOrder, title, stringHelp, descriptiveText, componentType, finalizeService, idCustomerServiceData, idTemplateListFormDocument, idParentComponent, idInputConnectionPoint, idOutputConnectionPoint, idOutputDataContext, idInputDataContext, idRelatedTable, idFieldToOrder, idFieldAssociated, timestamp FROM [Component] WHERE " + propertyName + " " + op + " @expValue";
				// Crea el command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los parametros al command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter();
				parameter.ParameterName = "@expValue";
				Type parameterType = properties[propertyName];
				parameter.DbType = DataAccessConnection.GetParameterDBType(parameterType);

				parameter.Value = expValue;
				sqlCommand.Parameters.Add(parameter);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();
				componentList = new Collection<ComponentEntity>();
				ComponentEntity component;
				List<int> listId = new List<int>();
				// Agrega los id a una lista de ids
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}
				// Cerrar el Reader

				reader.Close();
				// Carga las entidades

				foreach(int  id in listId)
				{
					component = Load(id, loadRelation, null);
					componentList.Add(component);
				}
			}
			catch (DbException dbException)
			{
				// Relanza la excepcion como una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue abierta dentro de la Función
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			return componentList;
		} 

		/// <summary>
		/// Función que carga una lista de ComponentEntity desde la base de datos por idCustomerServiceData.
		/// </summary>
		/// <param name="idCustomerServiceData">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByCustomerServiceDataCollection(int idCustomerServiceData, Dictionary<string,IEntity> scope)
		{
			Collection<ComponentEntity> componentList;
			bool closeConnection = false;
			try 
			{
				// Crea una nueva conexión
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un command

				string cmdText = "SELECT idComponent FROM [Component] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				parameter.Value = idCustomerServiceData;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				componentList = new Collection<ComponentEntity>();
				// Carga los ids de los objetos relacionados en una lista de int.

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}

				reader.Close();
				// Carga los objetos relacionados y los agrega a la coleccion

				foreach(int  id in listId)
				{
					componentList.Add(Load(id, scope));
				}
			}
			catch (DbException dbException)
			{
				// Relanzamos una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cerrar la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			// retornamos la lista de objetos relacionados
			return componentList;
		} 

		/// <summary>
		/// Función para cargar una lista de ComponentEntity desde la base de datos por idCustomerServiceData.
		/// </summary>
		/// <param name="idCustomerServiceData">columna Foreing key</param>
		/// <returns>IList de ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByCustomerServiceDataCollection(int idCustomerServiceData)
		{
			return LoadByCustomerServiceDataCollection(idCustomerServiceData, null);
		} 

		/// <summary>
		/// Función que carga la relacion TemplateListFormDocument desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idTemplateListFormDocument FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.TemplateListFormDocument = customerServiceDataDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion MenuItems desde la base de datos
		/// </summary>
		/// <param name="component">Entidad padre ComponentEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
		/// </exception>
		public void LoadRelationMenuItems(ComponentEntity component, Dictionary<string,IEntity> scope)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			ComponentDataAccess componentDataAccess = new ComponentDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			component.MenuItems = componentDataAccess.LoadByComponentCollection(component.Id, scope);
		} 

		/// <summary>
		/// Actualiza la base de datos para reflejar el estado actual de la lista.
		/// </summary>
		/// <param name="collectionDataAccess">El IDataAccess de la relación</param>
		/// <param name="parent">El objeto padre</param>
		/// <param name="collection">una colección de items</param>
		/// <param name="isNewParent">Si el padre es un objeto nuevo</param>
		/// <param name="scope">Estructura de datos interna para evitar problemas de referencia circular</param>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		private void SaveComponentCollection(ComponentDataAccess collectionDataAccess, ComponentEntity parent, Collection<ComponentEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
		{
			if (collection == null)
			{
				return;
			}
			// Establece los objetos de conexión
			collectionDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Establece la relación padre/hijo

			for (int  i = 0; i < collection.Count; i++)
			{
				bool changed = collection[i].Changed;
				collection[i].ParentComponent = parent;
				collection[i].Changed = changed;
			}
			// Si el padre es nuevo guarda todos los hijos, sino controla las diferencias con la base de datos.

			if (isNewParent)
			{
				for (int  i = 0; i < collection.Count; i++)
				{
					collectionDataAccess.Save(collection[i], scope);
				}
			}
			else 
			{
				// Controla los hijos que ya no son parte de la relación
				string idList = "0";
				if (collection.Count > 0)
				{
					idList = "" + collection[0].Id;
				}

				for (int  i = 1; i < collection.Count; i++)
				{
					idList += ", " + collection[i].Id;
				}
				// Retorna los ids que ya no existe en la colección actual

				string command = "SELECT idComponent FROM [Component] WHERE idParentComponent = @idParentComponent AND idComponent NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<ComponentEntity> objectsToDelete = new Collection<ComponentEntity>();
				// Inserta los id en una lista

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}

				reader.Close();
				// Carga los items a ser eliminados

				foreach(int  id in listId)
				{
					ComponentEntity entityToDelete = collectionDataAccess.Load(id, scope);
					objectsToDelete.Add(entityToDelete);
				}
				// Esto se realiza porque el reader debe ser cerrado despues de eliminar las entidades

				for (int  i = 0; i < objectsToDelete.Count; i++)
				{
					collectionDataAccess.Delete(objectsToDelete[i], scope);
				}

				System.DateTime timestamp;
				// Controla todas las propiedades de los items de la colección
				// para verificar si alguno cambio

				for (int  i = 0; i < collection.Count; i++)
				{
					ComponentEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [Component] WHERE idComponent = @idComponent";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
						sqlParameterIdPreference.Value = item.Id;
						sqlCommandTimestamp.Parameters.Add(sqlParameterIdPreference);

						timestamp = ((System.DateTime)sqlCommandTimestamp.ExecuteScalar());
						if (item.Timestamp != timestamp)
						{
							item.Changed = true;
						}
					}
					// Guarda el item si cambio o es nuevo

					if (item.Changed || item.IsNew)
					{
						collectionDataAccess.Save(item);
					}
				}
			}
		} 

		/// <summary>
		/// Función para eliminar una lista de entidades relacionadas desde la base de datos
		/// </summary>
		/// <param name="collectionDataAccess">IDataAccess de la relacion</param>
		/// <param name="collection">La colección de entidades a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular</param>
		/// <returns>True si la colección no es nula</returns>
		private bool DeleteComponentCollection(ComponentDataAccess collectionDataAccess, Collection<ComponentEntity> collection, Dictionary<string,IEntity> scope)
		{
			if (collection == null)
			{
				return false;
			}
			// Establece los objetos de conexión al data access de la relación.
			collectionDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Elimina los objetos relacionados

			for (int  i = 0; i < collection.Count; i++)
			{
				collectionDataAccess.Delete(collection[i], scope);
			}
			return true;
		} 

		/// <summary>
		/// Función que carga una lista de ComponentEntity desde la base de datos por idComponent.
		/// </summary>
		/// <param name="idComponent">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByComponentCollection(int idComponent, Dictionary<string,IEntity> scope)
		{
			Collection<ComponentEntity> componentList;
			bool closeConnection = false;
			try 
			{
				// Crea una nueva conexión
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un command

				string cmdText = "SELECT idComponent FROM [Component] WHERE idParentComponent = @idParentComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idParentComponent", DbType.Int32);
				parameter.Value = idComponent;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				componentList = new Collection<ComponentEntity>();
				// Carga los ids de los objetos relacionados en una lista de int.

				List<int> listId = new List<int>();
				while (reader.Read())
				{
					listId.Add(reader.GetInt32(0));
				}

				reader.Close();
				// Carga los objetos relacionados y los agrega a la coleccion

				foreach(int  id in listId)
				{
					componentList.Add(Load(id, scope));
				}
			}
			catch (DbException dbException)
			{
				// Relanzamos una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cerrar la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
			// retornamos la lista de objetos relacionados
			return componentList;
		} 

		/// <summary>
		/// Función para cargar una lista de ComponentEntity desde la base de datos por idComponent.
		/// </summary>
		/// <param name="idComponent">columna Foreing key</param>
		/// <returns>IList de ComponentEntity</returns>
		public Collection<ComponentEntity> LoadByComponentCollection(int idComponent)
		{
			return LoadByComponentCollection(idComponent, null);
		} 

		/// <summary>
		/// Función que carga la relacion ParentComponent desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idParentComponent FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.ParentComponent = componentDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion InputConnectionPoint desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idInputConnectionPoint FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.InputConnectionPoint = connectionPointDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion OutputConnectionPoint desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idOutputConnectionPoint FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.OutputConnectionPoint = connectionPointDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion OutputDataContext desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idOutputDataContext FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					TableDataAccess tableDataAccess = new TableDataAccess();
					tableDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.OutputDataContext = tableDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion InputDataContext desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idInputDataContext FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					TableDataAccess tableDataAccess = new TableDataAccess();
					tableDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.InputDataContext = tableDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion RelatedTable desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idRelatedTable FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					TableDataAccess tableDataAccess = new TableDataAccess();
					tableDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.RelatedTable = tableDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion FieldToOrder desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idFieldToOrder FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					FieldDataAccess fieldDataAccess = new FieldDataAccess();
					fieldDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.FieldToOrder = fieldDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que carga la relacion FieldAssociated desde la base de datos
		/// </summary>
		/// <param name="component">Padre: ComponentEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idFieldAssociated FROM [Component] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = component.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					FieldDataAccess fieldDataAccess = new FieldDataAccess();
					fieldDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					component.FieldAssociated = fieldDataAccess.Load(((int)idRelation), true, scope);
				}
			}
			catch (DbException dbException)
			{
				// Relanza una excepcion personalizada
				throw new UtnEmallDataAccessException(dbException.Message, dbException);
			}
			finally 
			{
				// Cierra la conexión si fue inicializada
				if (closeConnection)
				{
					dbConnection.Close();
				}
			}
		} 

		/// <summary>
		/// Función que actualiza un ComponentEntity en la base de datos.
		/// </summary>
		/// <param name="component">ComponentEntity a actualizar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="component"/> no es un <c>ComponentEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		private void Update(ComponentEntity component)
		{
			if (component == null)
			{
				throw new ArgumentException("The argument can't be null", "component");
			}
			// Construir un comando para actualizar
			string commandName = "UpdateComponent";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Establece los parametros de actualización

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
			// Ejecuta la actualización

			sqlCommand.ExecuteNonQuery();
			// Actualizar los campos new y changed

			component.IsNew = false;
			component.Changed = false;
		} 

	} 

}

