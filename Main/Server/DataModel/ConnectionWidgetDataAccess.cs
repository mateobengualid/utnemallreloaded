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
	/// El <c>ConnectionWidgetDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class ConnectionWidgetDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,ConnectionWidgetEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>ConnectionWidgetDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  ConnectionWidgetDataAccess()
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

			inMemoryEntities = new Dictionary<int,ConnectionWidgetEntity>();
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
		/// Función para cargar un ConnectionWidgetEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionWidgetEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionWidget";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((ConnectionWidgetEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			ConnectionWidgetEntity connectionWidget = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				connectionWidget = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, connectionWidget);
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

					string cmdText = "SELECT idConnectionWidget, idTarget, idSource, idCustomerServiceData, timestamp FROM [ConnectionWidget] WHERE idConnectionWidget = @idConnectionWidget";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					connectionWidget = new ConnectionWidgetEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						connectionWidget.Id = reader.GetInt32(0);

						connectionWidget.IdTarget = reader.GetInt32(1);
						connectionWidget.IdSource = reader.GetInt32(2);
						connectionWidget.IdCustomerServiceData = reader.GetInt32(3);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, connectionWidget);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(connectionWidget.Id, connectionWidget);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						connectionWidget.Timestamp = reader.GetDateTime(4);
						connectionWidget.IsNew = false;
						connectionWidget.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationTarget(connectionWidget, scope);
							LoadRelationSource(connectionWidget, scope);
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
			return connectionWidget;
		} 

		/// <summary>
		/// Función para cargar un ConnectionWidgetEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionWidgetEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un ConnectionWidgetEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionWidgetEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un ConnectionWidgetEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionWidgetEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idConnectionWidget", "idTarget", "idSource", "idCustomerServiceData"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("ConnectionWidget");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("ConnectionWidget", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteConnectionWidget");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveConnectionWidget");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateConnectionWidget");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("ConnectionWidget", "idConnectionWidget");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("ConnectionWidget", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("ConnectionWidget", "idConnectionWidget", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(ConnectionWidgetEntity connectionWidget, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@idTarget", DbType.Int32);

			parameter.Value = connectionWidget.IdTarget;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idSource", DbType.Int32);

			parameter.Value = connectionWidget.IdSource;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = connectionWidget.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un ConnectionWidgetEntity en la base de datos.
		/// </summary>
		/// <param name="connectionWidget">ConnectionWidgetEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionWidget"/> no es un <c>ConnectionWidgetEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ConnectionWidgetEntity connectionWidget)
		{
			Save(connectionWidget, null);
		} 

		/// <summary>
		/// Función que guarda un ConnectionWidgetEntity en la base de datos.
		/// </summary>
		/// <param name="connectionWidget">ConnectionWidgetEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionWidget"/> no es un <c>ConnectionWidgetEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ConnectionWidgetEntity connectionWidget, Dictionary<string,IEntity> scope)
		{
			if (connectionWidget == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = connectionWidget.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionWidget";
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

				if (connectionWidget.IsNew || !DataAccessConnection.ExistsEntity(connectionWidget.Id, "ConnectionWidget", "idConnectionWidget", dbConnection, dbTransaction))
				{
					commandName = "SaveConnectionWidget";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateConnectionWidget";
				}
				// Se crea un command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
					parameter.Value = connectionWidget.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(connectionWidget, sqlCommand);
				// Ejecutar el command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					connectionWidget.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = connectionWidget.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionWidget";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, connectionWidget);
				// Guarda las colecciones de objetos relacionados.
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				connectionWidget.IsNew = false;
				connectionWidget.Changed = false;
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
		/// Función que elimina un ConnectionWidgetEntity de la base de datos.
		/// </summary>
		/// <param name="connectionWidget">ConnectionWidgetEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionWidget"/> no es un <c>ConnectionWidgetEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ConnectionWidgetEntity connectionWidget)
		{
			Delete(connectionWidget, null);
		} 

		/// <summary>
		/// Función que elimina un ConnectionWidgetEntity de la base de datos.
		/// </summary>
		/// <param name="connectionWidget">ConnectionWidgetEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionWidget"/> no es un <c>ConnectionWidgetEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ConnectionWidgetEntity connectionWidget, Dictionary<string,IEntity> scope)
		{
			if (connectionWidget == null)
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

				connectionWidget = this.Load(connectionWidget.Id, true);
				if (connectionWidget == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteConnectionWidget";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
				parameterID.Value = connectionWidget.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				// Confirma la transacción si se inicio dentro de la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(connectionWidget.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = connectionWidget.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionWidget";
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
			properties.Add("idConnectionWidget", typeof( int ));

			properties.Add("idTarget", typeof( int ));
			properties.Add("idSource", typeof( int ));
			properties.Add("idCustomerServiceData", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los ConnectionWidgetEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<ConnectionWidgetEntity> LoadAll(bool loadRelation)
		{
			Collection<ConnectionWidgetEntity> connectionWidgetList = new Collection<ConnectionWidgetEntity>();

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

				string cmdText = "SELECT idConnectionWidget FROM [ConnectionWidget]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				ConnectionWidgetEntity connectionWidget;
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
					connectionWidget = Load(id, loadRelation, scope);
					connectionWidgetList.Add(connectionWidget);
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
			return connectionWidgetList;
		} 

		/// <summary>
		/// Función para cargar un ConnectionWidgetEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase ConnectionWidgetEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<ConnectionWidgetEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<ConnectionWidgetEntity> connectionWidgetList;

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

				string cmdText = "SELECT idConnectionWidget, idTarget, idSource, idCustomerServiceData, timestamp FROM [ConnectionWidget] WHERE " + propertyName + " " + op + " @expValue";
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
				connectionWidgetList = new Collection<ConnectionWidgetEntity>();
				ConnectionWidgetEntity connectionWidget;
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
					connectionWidget = Load(id, loadRelation, null);
					connectionWidgetList.Add(connectionWidget);
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
			return connectionWidgetList;
		} 

		/// <summary>
		/// Función que carga la relacion Target desde la base de datos
		/// </summary>
		/// <param name="connectionWidget">Padre: ConnectionWidgetEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionWidget"/> no es un <c>ConnectionWidgetEntity</c>.
		/// </exception>
		public void LoadRelationTarget(ConnectionWidgetEntity connectionWidget, Dictionary<string,IEntity> scope)
		{
			if (connectionWidget == null)
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

				string cmdText = "SELECT idTarget FROM [ConnectionWidget] WHERE idConnectionWidget = @idConnectionWidget";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = connectionWidget.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					connectionWidget.Target = connectionPointDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que carga la relacion Source desde la base de datos
		/// </summary>
		/// <param name="connectionWidget">Padre: ConnectionWidgetEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionWidget"/> no es un <c>ConnectionWidgetEntity</c>.
		/// </exception>
		public void LoadRelationSource(ConnectionWidgetEntity connectionWidget, Dictionary<string,IEntity> scope)
		{
			if (connectionWidget == null)
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

				string cmdText = "SELECT idSource FROM [ConnectionWidget] WHERE idConnectionWidget = @idConnectionWidget";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionWidget", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = connectionWidget.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionPointDataAccess connectionPointDataAccess = new ConnectionPointDataAccess();
					connectionPointDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					connectionWidget.Source = connectionPointDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que carga una lista de ConnectionWidgetEntity desde la base de datos por idCustomerServiceData.
		/// </summary>
		/// <param name="idCustomerServiceData">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of ConnectionWidgetEntity</returns>
		public Collection<ConnectionWidgetEntity> LoadByCustomerServiceDataCollection(int idCustomerServiceData, Dictionary<string,IEntity> scope)
		{
			Collection<ConnectionWidgetEntity> connectionWidgetList;
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

				string cmdText = "SELECT idConnectionWidget FROM [ConnectionWidget] WHERE idCustomerServiceData = @idCustomerServiceData";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);
				parameter.Value = idCustomerServiceData;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				connectionWidgetList = new Collection<ConnectionWidgetEntity>();
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
					connectionWidgetList.Add(Load(id, scope));
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
			return connectionWidgetList;
		} 

		/// <summary>
		/// Función para cargar una lista de ConnectionWidgetEntity desde la base de datos por idCustomerServiceData.
		/// </summary>
		/// <param name="idCustomerServiceData">columna Foreing key</param>
		/// <returns>IList de ConnectionWidgetEntity</returns>
		public Collection<ConnectionWidgetEntity> LoadByCustomerServiceDataCollection(int idCustomerServiceData)
		{
			return LoadByCustomerServiceDataCollection(idCustomerServiceData, null);
		} 

	} 

}

