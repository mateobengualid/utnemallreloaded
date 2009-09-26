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
	/// El <c>ConnectionPointDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
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
		/// Inicializa una nueva instancia de
		/// <c>ConnectionPointDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
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
		/// Función para cargar un ConnectionPointEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionPointEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((ConnectionPointEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			ConnectionPointEntity connectionPoint = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				connectionPoint = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, connectionPoint);
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

					string cmdText = "SELECT idConnectionPoint, connectionType, xCoordinateRelativeToParent, yCoordinateRelativeToParent, idParentComponent, idComponent, idConnectionWidget, timestamp FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					connectionPoint = new ConnectionPointEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						connectionPoint.Id = reader.GetInt32(0);

						connectionPoint.ConnectionType = reader.GetInt32(1);
						connectionPoint.XCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(2));
						connectionPoint.YCoordinateRelativeToParent = Convert.ToDouble(reader.GetDecimal(3));
						connectionPoint.IdParentComponent = reader.GetInt32(4);
						connectionPoint.IdComponent = reader.GetInt32(5);
						connectionPoint.IdConnectionWidget = reader.GetInt32(6);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, connectionPoint);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(connectionPoint.Id, connectionPoint);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						connectionPoint.Timestamp = reader.GetDateTime(7);
						connectionPoint.IsNew = false;
						connectionPoint.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

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
			return connectionPoint;
		} 

		/// <summary>
		/// Función para cargar un ConnectionPointEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionPointEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un ConnectionPointEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionPointEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un ConnectionPointEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ConnectionPointEntity Load(int id, Dictionary<string,IEntity> scope)
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
		/// Función que guarda un ConnectionPointEntity en la base de datos.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ConnectionPointEntity connectionPoint)
		{
			Save(connectionPoint, null);
		} 

		/// <summary>
		/// Función que guarda un ConnectionPointEntity en la base de datos.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = connectionPoint.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
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
				// Se crea un command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
					parameter.Value = connectionPoint.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(connectionPoint, sqlCommand);
				// Ejecutar el command
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
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, connectionPoint);
				// Guarda las colecciones de objetos relacionados.
				// Guardar objetos relacionados con la entidad actual
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
				// Actualizar
				Update(connectionPoint);
				// Cierra la conexión si fue abierta en la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				connectionPoint.IsNew = false;
				connectionPoint.Changed = false;
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
		/// Función que elimina un ConnectionPointEntity de la base de datos.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ConnectionPointEntity connectionPoint)
		{
			Delete(connectionPoint, null);
		} 

		/// <summary>
		/// Función que elimina un ConnectionPointEntity de la base de datos.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ConnectionPointEntity connectionPoint, Dictionary<string,IEntity> scope)
		{
			if (connectionPoint == null)
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

				connectionPoint = this.Load(connectionPoint.Id, true);
				if (connectionPoint == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				parameterID.Value = connectionPoint.Id;
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

				inMemoryEntities.Remove(connectionPoint.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = connectionPoint.Id.ToString(NumberFormatInfo.InvariantInfo) + "ConnectionPoint";
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
			properties.Add("idConnectionPoint", typeof( int ));

			properties.Add("connectionType", typeof( int ));
			properties.Add("xCoordinateRelativeToParent", typeof( double ));
			properties.Add("yCoordinateRelativeToParent", typeof( double ));
			properties.Add("idParentComponent", typeof( int ));
			properties.Add("idComponent", typeof( int ));
			properties.Add("idConnectionWidget", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los ConnectionPointEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<ConnectionPointEntity> LoadAll(bool loadRelation)
		{
			Collection<ConnectionPointEntity> connectionPointList = new Collection<ConnectionPointEntity>();

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

				string cmdText = "SELECT idConnectionPoint FROM [ConnectionPoint]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				ConnectionPointEntity connectionPoint;
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
					connectionPoint = Load(id, loadRelation, scope);
					connectionPointList.Add(connectionPoint);
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
			return connectionPointList;
		} 

		/// <summary>
		/// Función para cargar un ConnectionPointEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase ConnectionPointEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
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
				// Abrir una nueva conexión con la base de datos si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}

				string op = DataAccessConnection.GetOperatorString(operatorType);
				// Construir la consulta

				string cmdText = "SELECT idConnectionPoint, connectionType, xCoordinateRelativeToParent, yCoordinateRelativeToParent, idParentComponent, idComponent, idConnectionWidget, timestamp FROM [ConnectionPoint] WHERE " + propertyName + " " + op + " @expValue";
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
				connectionPointList = new Collection<ConnectionPointEntity>();
				ConnectionPointEntity connectionPoint;
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
					connectionPoint = Load(id, loadRelation, null);
					connectionPointList.Add(connectionPoint);
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
			return connectionPointList;
		} 

		/// <summary>
		/// Función que carga la relacion ParentComponent desde la base de datos
		/// </summary>
		/// <param name="connectionPoint">Padre: ConnectionPointEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idParentComponent FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = connectionPoint.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					connectionPoint.ParentComponent = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que carga una lista de ConnectionPointEntity desde la base de datos por idComponent.
		/// </summary>
		/// <param name="idComponent">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of ConnectionPointEntity</returns>
		public Collection<ConnectionPointEntity> LoadByComponentCollection(int idComponent, Dictionary<string,IEntity> scope)
		{
			Collection<ConnectionPointEntity> connectionPointList;
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

				string cmdText = "SELECT idConnectionPoint FROM [ConnectionPoint] WHERE idComponent = @idComponent";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);
				parameter.Value = idComponent;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				connectionPointList = new Collection<ConnectionPointEntity>();
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
					connectionPointList.Add(Load(id, scope));
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
			return connectionPointList;
		} 

		/// <summary>
		/// Función para cargar una lista de ConnectionPointEntity desde la base de datos por idComponent.
		/// </summary>
		/// <param name="idComponent">columna Foreing key</param>
		/// <returns>IList de ConnectionPointEntity</returns>
		public Collection<ConnectionPointEntity> LoadByComponentCollection(int idComponent)
		{
			return LoadByComponentCollection(idComponent, null);
		} 

		/// <summary>
		/// Función que carga la relacion Component desde la base de datos
		/// </summary>
		/// <param name="connectionPoint">Padre: ConnectionPointEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idComponent FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = connectionPoint.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					connectionPoint.Component = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que carga la relacion ConnectionWidget desde la base de datos
		/// </summary>
		/// <param name="connectionPoint">Padre: ConnectionPointEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
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
				// Crea una nueva conexión si es necesario
				if (dbConnection == null || dbConnection.State.CompareTo(ConnectionState.Closed) == 0)
				{
					closeConnection = true;
					dbConnection = dataAccess.GetNewConnection();
					dbConnection.Open();
				}
				// Crea un nuevo command

				string cmdText = "SELECT idConnectionWidget FROM [ConnectionPoint] WHERE idConnectionPoint = @idConnectionPoint";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idConnectionPoint", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = connectionPoint.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ConnectionWidgetDataAccess connectionWidgetDataAccess = new ConnectionWidgetDataAccess();
					connectionWidgetDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					connectionPoint.ConnectionWidget = connectionWidgetDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que actualiza un ConnectionPointEntity en la base de datos.
		/// </summary>
		/// <param name="connectionPoint">ConnectionPointEntity a actualizar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="connectionPoint"/> no es un <c>ConnectionPointEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		private void Update(ConnectionPointEntity connectionPoint)
		{
			if (connectionPoint == null)
			{
				throw new ArgumentException("The argument can't be null", "connectionPoint");
			}
			// Construir un comando para actualizar
			string commandName = "UpdateConnectionPoint";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Establece los parametros de actualización

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
			// Ejecuta la actualización

			sqlCommand.ExecuteNonQuery();
			// Actualizar los campos new y changed

			connectionPoint.IsNew = false;
			connectionPoint.Changed = false;
		} 

	} 

}

