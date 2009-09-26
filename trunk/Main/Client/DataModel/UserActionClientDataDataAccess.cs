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

	/// <summary>
	/// El <c>UserActionClientDataDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class UserActionClientDataDataAccess
	{
		private bool isGlobalTransaction; 
		private SqlCeConnection dbConnection; 
		private SqlCeTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,UserActionClientDataEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>UserActionClientDataDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  UserActionClientDataDataAccess()
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

			inMemoryEntities = new Dictionary<int,UserActionClientDataEntity>();
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
		/// Función para cargar un UserActionClientDataEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserActionClientDataEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "UserActionClientData";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((UserActionClientDataEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			UserActionClientDataEntity userActionClientData = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				userActionClientData = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, userActionClientData);
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

					string cmdText = "SELECT idUserActionClientData, actionType, start, stop, idTable, idRegister, idComponent, idService, timestamp FROM [UserActionClientData] WHERE idUserActionClientData = @idUserActionClientData";
					// Crea el command

					SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idUserActionClientData", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					userActionClientData = new UserActionClientDataEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						userActionClientData.Id = reader.GetInt32(0);

						userActionClientData.ActionType = reader.GetInt32(1);
						userActionClientData.Start = reader.GetDateTime(2);
						userActionClientData.Stop = reader.GetDateTime(3);
						userActionClientData.IdTable = reader.GetInt32(4);
						userActionClientData.IdRegister = reader.GetInt32(5);
						userActionClientData.IdComponent = reader.GetInt32(6);
						userActionClientData.IdService = reader.GetInt32(7);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, userActionClientData);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(userActionClientData.Id, userActionClientData);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						userActionClientData.Timestamp = reader.GetDateTime(8);
						userActionClientData.IsNew = false;
						userActionClientData.Changed = false;
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
			return userActionClientData;
		} 

		/// <summary>
		/// Función para cargar un UserActionClientDataEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserActionClientDataEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un UserActionClientDataEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserActionClientDataEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un UserActionClientDataEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserActionClientDataEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idUserActionClientData", "actionType", "start", "stop", "idTable", "idRegister", "idComponent", "idService"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( int ), typeof( System.DateTime ), typeof( System.DateTime ), typeof( int ), typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("UserActionClientData");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("UserActionClientData", fieldsName, false, fieldsType);
			}
			dbChecked = true;
		} 

		private void FillSaveParameters(UserActionClientDataEntity userActionClientData, SqlCeCommand sqlCommand)
		{
			SqlCeParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@actionType", DbType.Int32);

			parameter.Value = userActionClientData.ActionType;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@start", DbType.DateTime);

			parameter.Value = userActionClientData.Start;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stop", DbType.DateTime);

			parameter.Value = userActionClientData.Stop;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);

			parameter.Value = userActionClientData.IdTable;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idRegister", DbType.Int32);

			parameter.Value = userActionClientData.IdRegister;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);

			parameter.Value = userActionClientData.IdComponent;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);

			parameter.Value = userActionClientData.IdService;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un UserActionClientDataEntity en la base de datos.
		/// </summary>
		/// <param name="userActionClientData">UserActionClientDataEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientData"/> no es un <c>UserActionClientDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(UserActionClientDataEntity userActionClientData)
		{
			Save(userActionClientData, null);
		} 

		/// <summary>
		/// Función que guarda un UserActionClientDataEntity en la base de datos.
		/// </summary>
		/// <param name="userActionClientData">UserActionClientDataEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientData"/> no es un <c>UserActionClientDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(UserActionClientDataEntity userActionClientData, Dictionary<string,IEntity> scope)
		{
			if (userActionClientData == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = userActionClientData.Id.ToString(NumberFormatInfo.InvariantInfo) + "UserActionClientData";
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

				if (userActionClientData.IsNew || !DataAccessConnection.ExistsEntity(userActionClientData.Id, "UserActionClientData", "idUserActionClientData", dbConnection, dbTransaction))
				{
					commandName = "INSERT INTO [UserActionClientData] (idUserActionClientData, ACTIONTYPE, START, STOP, IDTABLE, IDREGISTER, IDCOMPONENT, IDSERVICE, [TIMESTAMP] ) VALUES( @idUserActionClientData,  @actionType,@start,@stop,@idTable,@idRegister,@idComponent,@idService, GETDATE()); ";
				}
				else 
				{
					isUpdate = true;
					commandName = "UPDATE [UserActionClientData] SET actionType = @actionType, start = @start, stop = @stop, idTable = @idTable, idRegister = @idRegister, idComponent = @idComponent, idService = @idService , timestamp=GETDATE() WHERE idUserActionClientData = @idUserActionClientData";
				}
				// Se crea un command
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				// Agregar los parametros del command .
				SqlCeParameter parameter;
				if (!isUpdate && userActionClientData.Id == 0)
				{
					userActionClientData.Id = DataAccessConnection.GetNextId("idUserActionClientData", "UserActionClientData", dbConnection, dbTransaction);
				}

				parameter = dataAccess.GetNewDataParameter("@idUserActionClientData", DbType.Int32);
				parameter.Value = userActionClientData.Id;
				sqlCommand.Parameters.Add(parameter);

				FillSaveParameters(userActionClientData, sqlCommand);
				// Ejecutar el command
				sqlCommand.ExecuteNonQuery();

				scopeKey = userActionClientData.Id.ToString(NumberFormatInfo.InvariantInfo) + "UserActionClientData";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, userActionClientData);
				// Guarda las colecciones de objetos relacionados.
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				userActionClientData.IsNew = false;
				userActionClientData.Changed = false;
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
		/// Función que elimina un UserActionClientDataEntity de la base de datos.
		/// </summary>
		/// <param name="userActionClientData">UserActionClientDataEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientData"/> no es un <c>UserActionClientDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(UserActionClientDataEntity userActionClientData)
		{
			Delete(userActionClientData, null);
		} 

		/// <summary>
		/// Función que elimina un UserActionClientDataEntity de la base de datos.
		/// </summary>
		/// <param name="userActionClientData">UserActionClientDataEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientData"/> no es un <c>UserActionClientDataEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(UserActionClientDataEntity userActionClientData, Dictionary<string,IEntity> scope)
		{
			if (userActionClientData == null)
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

				userActionClientData = this.Load(userActionClientData.Id, true);
				if (userActionClientData == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DELETE FROM [UserActionClientData] WHERE idUserActionClientData = @idUserActionClientData";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los valores de los parametros
				SqlCeParameter parameterID = dataAccess.GetNewDataParameter("@idUserActionClientData", DbType.Int32);
				parameterID.Value = userActionClientData.Id;
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

				inMemoryEntities.Remove(userActionClientData.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = userActionClientData.Id.ToString(NumberFormatInfo.InvariantInfo) + "UserActionClientData";
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
			properties.Add("idUserActionClientData", typeof( int ));

			properties.Add("actionType", typeof( int ));
			properties.Add("start", typeof( System.DateTime ));
			properties.Add("stop", typeof( System.DateTime ));
			properties.Add("idTable", typeof( int ));
			properties.Add("idRegister", typeof( int ));
			properties.Add("idComponent", typeof( int ));
			properties.Add("idService", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los UserActionClientDataEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<UserActionClientDataEntity> LoadAll(bool loadRelation)
		{
			Collection<UserActionClientDataEntity> userActionClientDataList = new Collection<UserActionClientDataEntity>();

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

				string cmdText = "SELECT idUserActionClientData FROM [UserActionClientData]";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				UserActionClientDataEntity userActionClientData;
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
					userActionClientData = Load(id, loadRelation, scope);
					userActionClientDataList.Add(userActionClientData);
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
			return userActionClientDataList;
		} 

		/// <summary>
		/// Función para cargar un UserActionClientDataEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase UserActionClientDataEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<UserActionClientDataEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<UserActionClientDataEntity> userActionClientDataList;

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

				string cmdText = "SELECT idUserActionClientData, actionType, start, stop, idTable, idRegister, idComponent, idService, timestamp FROM [UserActionClientData] WHERE " + propertyName + " " + op + " @expValue";
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
				userActionClientDataList = new Collection<UserActionClientDataEntity>();
				UserActionClientDataEntity userActionClientData;
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
					userActionClientData = Load(id, loadRelation, null);
					userActionClientDataList.Add(userActionClientData);
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
			return userActionClientDataList;
		} 

	} 

}

