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
	/// El <c>CustomerServiceDataDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
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
		/// Inicializa una nueva instancia de
		/// <c>CustomerServiceDataDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
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
		/// Función para cargar un CustomerServiceDataEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerServiceDataEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((CustomerServiceDataEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			CustomerServiceDataEntity customerServiceData = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				customerServiceData = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, customerServiceData);
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

					string cmdText = "SELECT idCustomerServiceData, customerServiceDataType, idDataModel, idInitComponent, idService, timestamp FROM [CustomerServiceData] WHERE idCustomerServiceData = @idCustomerServiceData";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					customerServiceData = new CustomerServiceDataEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						customerServiceData.Id = reader.GetInt32(0);

						customerServiceData.CustomerServiceDataType = reader.GetInt32(1);
						customerServiceData.IdDataModel = reader.GetInt32(2);
						customerServiceData.IdInitComponent = reader.GetInt32(3);
						customerServiceData.IdService = reader.GetInt32(4);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, customerServiceData);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(customerServiceData.Id, customerServiceData);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						customerServiceData.Timestamp = reader.GetDateTime(5);
						customerServiceData.IsNew = false;
						customerServiceData.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

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
			return customerServiceData;
		} 

		/// <summary>
		/// Función para cargar un CustomerServiceDataEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerServiceDataEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un CustomerServiceDataEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerServiceDataEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un CustomerServiceDataEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerServiceDataEntity Load(int id, Dictionary<string,IEntity> scope)
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
		/// Función que guarda un CustomerServiceDataEntity en la base de datos.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(CustomerServiceDataEntity customerServiceData)
		{
			Save(customerServiceData, null);
		} 

		/// <summary>
		/// Función que guarda un CustomerServiceDataEntity en la base de datos.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = customerServiceData.Id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
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
				// Se crea un command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
					parameter.Value = customerServiceData.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(customerServiceData, sqlCommand);
				// Ejecutar el command
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
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, customerServiceData);
				// Guarda las colecciones de objetos relacionados.
				if (customerServiceData.Components != null)
				{
					this.SaveComponentCollection(new ComponentDataAccess(), customerServiceData, customerServiceData.Components, customerServiceData.IsNew, scope);
				}
				if (customerServiceData.Connections != null)
				{
					this.SaveConnectionWidgetCollection(new ConnectionWidgetDataAccess(), customerServiceData, customerServiceData.Connections, customerServiceData.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				if (customerServiceData.InitComponent != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Save(customerServiceData.InitComponent, scope);
				}
				// Actualizar
				Update(customerServiceData);
				// Cierra la conexión si fue abierta en la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				customerServiceData.IsNew = false;
				customerServiceData.Changed = false;
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
		/// Función que elimina un CustomerServiceDataEntity de la base de datos.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(CustomerServiceDataEntity customerServiceData)
		{
			Delete(customerServiceData, null);
		} 

		/// <summary>
		/// Función que elimina un CustomerServiceDataEntity de la base de datos.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
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

				customerServiceData = this.Load(customerServiceData.Id, true);
				if (customerServiceData == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				parameterID.Value = customerServiceData.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (customerServiceData.Components != null)
				{
					this.DeleteComponentCollection(new ComponentDataAccess(), customerServiceData.Components, scope);
				}
				if (customerServiceData.Connections != null)
				{
					this.DeleteConnectionWidgetCollection(new ConnectionWidgetDataAccess(), customerServiceData.Connections, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(customerServiceData.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = customerServiceData.Id.ToString(NumberFormatInfo.InvariantInfo) + "CustomerServiceData";
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
			properties.Add("idCustomerServiceData", typeof( int ));

			properties.Add("customerServiceDataType", typeof( int ));
			properties.Add("idDataModel", typeof( int ));
			properties.Add("idInitComponent", typeof( int ));
			properties.Add("idService", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los CustomerServiceDataEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<CustomerServiceDataEntity> LoadAll(bool loadRelation)
		{
			Collection<CustomerServiceDataEntity> customerServiceDataList = new Collection<CustomerServiceDataEntity>();

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

				string cmdText = "SELECT idCustomerServiceData FROM [CustomerServiceData]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				CustomerServiceDataEntity customerServiceData;
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
					customerServiceData = Load(id, loadRelation, scope);
					customerServiceDataList.Add(customerServiceData);
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
			return customerServiceDataList;
		} 

		/// <summary>
		/// Función para cargar un CustomerServiceDataEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase CustomerServiceDataEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
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
				// Abrir una nueva conexión con la base de datos si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}

				string op = DataAccessConnection.GetOperatorString(operatorType);
				// Construir la consulta

				string cmdText = "SELECT idCustomerServiceData, customerServiceDataType, idDataModel, idInitComponent, idService, timestamp FROM [CustomerServiceData] WHERE " + propertyName + " " + op + " @expValue";
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
				customerServiceDataList = new Collection<CustomerServiceDataEntity>();
				CustomerServiceDataEntity customerServiceData;
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
					customerServiceData = Load(id, loadRelation, null);
					customerServiceDataList.Add(customerServiceData);
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
			return customerServiceDataList;
		} 

		/// <summary>
		/// Función que carga la relacion Components desde la base de datos
		/// </summary>
		/// <param name="customerServiceData">Entidad padre CustomerServiceDataEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		public void LoadRelationComponents(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			ComponentDataAccess componentDataAccess = new ComponentDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			customerServiceData.Components = componentDataAccess.LoadByCustomerServiceDataCollection(customerServiceData.Id, scope);
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
		private void SaveComponentCollection(ComponentDataAccess collectionDataAccess, CustomerServiceDataEntity parent, Collection<ComponentEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].CustomerServiceData = parent;
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

				string command = "SELECT idComponent FROM [Component] WHERE idCustomerServiceData = @idCustomerServiceData AND idComponent NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
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
		/// Función que carga la relacion Connections desde la base de datos
		/// </summary>
		/// <param name="customerServiceData">Entidad padre CustomerServiceDataEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		public void LoadRelationConnections(CustomerServiceDataEntity customerServiceData, Dictionary<string,IEntity> scope)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			ConnectionWidgetDataAccess connectionWidgetDataAccess = new ConnectionWidgetDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			connectionWidgetDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			customerServiceData.Connections = connectionWidgetDataAccess.LoadByCustomerServiceDataCollection(customerServiceData.Id, scope);
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
		private void SaveConnectionWidgetCollection(ConnectionWidgetDataAccess collectionDataAccess, CustomerServiceDataEntity parent, Collection<ConnectionWidgetEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].CustomerServiceData = parent;
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

				string command = "SELECT idConnectionWidget FROM [ConnectionWidget] WHERE idCustomerServiceData = @idCustomerServiceData AND idConnectionWidget NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<ConnectionWidgetEntity> objectsToDelete = new Collection<ConnectionWidgetEntity>();
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
					ConnectionWidgetEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					ConnectionWidgetEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [ConnectionWidget] WHERE idConnectionWidget = @idConnectionWidget";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
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
		private bool DeleteConnectionWidgetCollection(ConnectionWidgetDataAccess collectionDataAccess, Collection<ConnectionWidgetEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Función que carga la relacion DataModel desde la base de datos
		/// </summary>
		/// <param name="customerServiceData">Padre: CustomerServiceDataEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idDataModel FROM [CustomerServiceData] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = customerServiceData.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					DataModelDataAccess dataModelDataAccess = new DataModelDataAccess();
					dataModelDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					customerServiceData.DataModel = dataModelDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que carga la relacion InitComponent desde la base de datos
		/// </summary>
		/// <param name="customerServiceData">Padre: CustomerServiceDataEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idInitComponent FROM [CustomerServiceData] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = customerServiceData.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					customerServiceData.InitComponent = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que actualiza un CustomerServiceDataEntity en la base de datos.
		/// </summary>
		/// <param name="customerServiceData">CustomerServiceDataEntity a actualizar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceData"/> no es un <c>CustomerServiceDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		private void Update(CustomerServiceDataEntity customerServiceData)
		{
			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can't be null", "customerServiceData");
			}
			// Construir un comando para actualizar
			string commandName = "UpdateCustomerServiceData";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Establece los parametros de actualización

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
			// Ejecuta la actualización

			sqlCommand.ExecuteNonQuery();
			// Actualizar los campos new y changed

			customerServiceData.IsNew = false;
			customerServiceData.Changed = false;
		} 

	} 

}

