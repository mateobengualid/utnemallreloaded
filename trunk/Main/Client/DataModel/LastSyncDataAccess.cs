using System.Data.SqlServerCe;
using UtnEmall.Client.DataModel;
using UtnEmall.Client.EntityModel;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.DataModel
{

	/// Definiciones para modelo
	/// Definiciones para Business
	/// Definiciones para SmartClient layer
	/// Seek stadistical data about services usage. Used on client and for interface
	/// between Server and Client
	/// <summary>
	/// El <c>LastSyncDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class LastSyncDataAccess
	{
		private bool isGlobalTransaction; 
		private SqlCeConnection dbConnection; 
		private SqlCeTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,LastSyncEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>LastSyncDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  LastSyncDataAccess()
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

			inMemoryEntities = new Dictionary<int,LastSyncEntity>();
		} 

		/// <summary>
		/// Establece la conexión y la transacción en el caso de que una transacción global se este ejecutando
		/// </summary>
		/// <param name="connection">La conexión SqlCeConnection</param>
		/// <param name="transaction">La transacción global SqlCeTransaction</param>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void SetConnectionObjects(SqlCeConnection connection, SqlCeTransaction transaction)
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
		/// Función para cargar un LastSyncEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public LastSyncEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "LastSync";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((LastSyncEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			LastSyncEntity lastSync = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				lastSync = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, lastSync);
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

					string cmdText = "SELECT idLastSync, entityName, lastTimestamp, timestamp FROM [LastSync] WHERE idLastSync = @idLastSync";
					// Crea el command

					SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idLastSync", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					lastSync = new LastSyncEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						lastSync.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							lastSync.EntityName = reader.GetString(1);
						}

						lastSync.LastTimestamp = reader.GetDateTime(2);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, lastSync);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(lastSync.Id, lastSync);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						lastSync.Timestamp = reader.GetDateTime(3);
						lastSync.IsNew = false;
						lastSync.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
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
			return lastSync;
		} 

		/// <summary>
		/// Función para cargar un LastSyncEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public LastSyncEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un LastSyncEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public LastSyncEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un LastSyncEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public LastSyncEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idLastSync", "entityName", "lastTimestamp"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( System.DateTime )};

			bool existsTable = DataAccessConnection.DBCheckedTable("LastSync");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("LastSync", fieldsName, false, fieldsType);
			}
			dbChecked = true;
		} 

		private void FillSaveParameters(LastSyncEntity lastSync, SqlCeCommand sqlCommand)
		{
			SqlCeParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@entityName", DbType.String);

			parameter.Value = lastSync.EntityName;
			if (String.IsNullOrEmpty(lastSync.EntityName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@lastTimestamp", DbType.DateTime);

			parameter.Value = lastSync.LastTimestamp;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un LastSyncEntity en la base de datos.
		/// </summary>
		/// <param name="lastSync">LastSyncEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="lastSync"/> no es un <c>LastSyncEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(LastSyncEntity lastSync)
		{
			Save(lastSync, null);
		} 

		/// <summary>
		/// Función que guarda un LastSyncEntity en la base de datos.
		/// </summary>
		/// <param name="lastSync">LastSyncEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="lastSync"/> no es un <c>LastSyncEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(LastSyncEntity lastSync, Dictionary<string,IEntity> scope)
		{
			if (lastSync == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = lastSync.Id.ToString(NumberFormatInfo.InvariantInfo) + "LastSync";
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

				if (lastSync.IsNew || !DataAccessConnection.ExistsEntity(lastSync.Id, "LastSync", "idLastSync", dbConnection, dbTransaction))
				{
					commandName = "INSERT INTO [LastSync] (idLastSync, ENTITYNAME, LASTTIMESTAMP, [TIMESTAMP] ) VALUES( @idLastSync,  @entityName,@lastTimestamp, GETDATE()); ";
				}
				else 
				{
					isUpdate = true;
					commandName = "UPDATE [LastSync] SET entityName = @entityName, lastTimestamp = @lastTimestamp , timestamp=GETDATE() WHERE idLastSync = @idLastSync";
				}
				// Se crea un command
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				// Agregar los parametros del command .
				SqlCeParameter parameter;
				if (!isUpdate && lastSync.Id == 0)
				{
					lastSync.Id = DataAccessConnection.GetNextId("idLastSync", "LastSync", dbConnection, dbTransaction);
				}

				parameter = dataAccess.GetNewDataParameter("@idLastSync", DbType.Int32);
				parameter.Value = lastSync.Id;
				sqlCommand.Parameters.Add(parameter);

				FillSaveParameters(lastSync, sqlCommand);
				// Ejecutar el command
				sqlCommand.ExecuteNonQuery();

				scopeKey = lastSync.Id.ToString(NumberFormatInfo.InvariantInfo) + "LastSync";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, lastSync);
				// Guarda las colecciones de objetos relacionados.
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				lastSync.IsNew = false;
				lastSync.Changed = false;
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
		/// Función que elimina un LastSyncEntity de la base de datos.
		/// </summary>
		/// <param name="lastSync">LastSyncEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="lastSync"/> no es un <c>LastSyncEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(LastSyncEntity lastSync)
		{
			Delete(lastSync, null);
		} 

		/// <summary>
		/// Función que elimina un LastSyncEntity de la base de datos.
		/// </summary>
		/// <param name="lastSync">LastSyncEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="lastSync"/> no es un <c>LastSyncEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(LastSyncEntity lastSync, Dictionary<string,IEntity> scope)
		{
			if (lastSync == null)
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

				lastSync = this.Load(lastSync.Id, true);
				if (lastSync == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DELETE FROM [LastSync] WHERE idLastSync = @idLastSync";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los valores de los parametros
				SqlCeParameter parameterID = dataAccess.GetNewDataParameter("@idLastSync", DbType.Int32);
				parameterID.Value = lastSync.Id;
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

				inMemoryEntities.Remove(lastSync.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = lastSync.Id.ToString(NumberFormatInfo.InvariantInfo) + "LastSync";
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
			properties.Add("idLastSync", typeof( int ));

			properties.Add("entityName", typeof( string ));
			properties.Add("lastTimestamp", typeof( System.DateTime ));
		} 

		/// <summary>
		/// Función que carga todos los LastSyncEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<LastSyncEntity> LoadAll(bool loadRelation)
		{
			Collection<LastSyncEntity> lastSyncList = new Collection<LastSyncEntity>();

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

				string cmdText = "SELECT idLastSync FROM [LastSync]";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				LastSyncEntity lastSync;
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
					lastSync = Load(id, loadRelation, scope);
					lastSyncList.Add(lastSync);
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
			return lastSyncList;
		} 

		/// <summary>
		/// Función para cargar un LastSyncEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase LastSyncEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<LastSyncEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<LastSyncEntity> lastSyncList;

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

				string cmdText = "SELECT idLastSync, entityName, lastTimestamp, timestamp FROM [LastSync] WHERE " + propertyName + " " + op + " @expValue";
				// Crea el command

				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los parametros al command

				SqlCeParameter parameter = dataAccess.GetNewDataParameter();
				parameter.ParameterName = "@expValue";
				Type parameterType = properties[propertyName];
				parameter.DbType = DataAccessConnection.GetParameterDBType(parameterType);

				parameter.Value = expValue;
				sqlCommand.Parameters.Add(parameter);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();
				lastSyncList = new Collection<LastSyncEntity>();
				LastSyncEntity lastSync;
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
					lastSync = Load(id, loadRelation, null);
					lastSyncList.Add(lastSync);
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
			return lastSyncList;
		} 

	} 

}

