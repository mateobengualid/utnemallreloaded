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
	/// El <c>UserDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class UserDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,UserEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>UserDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  UserDataAccess()
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

			inMemoryEntities = new Dictionary<int,UserEntity>();
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
		/// Función para cargar un UserEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "User";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((UserEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			UserEntity user = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				user = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, user);
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

					string cmdText = "SELECT idUser, userName, password, name, surname, phoneNumber, isUserActive, charge, idStore, timestamp FROM [User] WHERE idUser = @idUser";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					user = new UserEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						user.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							user.UserName = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							user.Password = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							user.Name = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							user.Surname = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							user.PhoneNumber = reader.GetString(5);
						}

						user.IsUserActive = reader.GetBoolean(6);
						if (!reader.IsDBNull(7))
						{
							user.Charge = reader.GetString(7);
						}

						user.IdStore = reader.GetInt32(8);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, user);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(user.Id, user);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						user.Timestamp = reader.GetDateTime(9);
						user.IsNew = false;
						user.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationUserGroup(user, scope);
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
			return user;
		} 

		/// <summary>
		/// Función para cargar un UserEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un UserEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un UserEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idUser", "userName", "password", "name", "surname", "phoneNumber", "isUserActive", "charge", "idStore"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( bool ), typeof( string ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("User");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("User", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteUser");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveUser");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateUser");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("User", "idUser");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("User", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("User", "idUser", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(UserEntity user, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@userName", DbType.String);

			parameter.Value = user.UserName;
			if (String.IsNullOrEmpty(user.UserName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@password", DbType.String);

			parameter.Value = user.Password;
			if (String.IsNullOrEmpty(user.Password))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = user.Name;
			if (String.IsNullOrEmpty(user.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@surname", DbType.String);

			parameter.Value = user.Surname;
			if (String.IsNullOrEmpty(user.Surname))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@phoneNumber", DbType.String);

			parameter.Value = user.PhoneNumber;
			if (String.IsNullOrEmpty(user.PhoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@isUserActive", DbType.Boolean);

			parameter.Value = user.IsUserActive;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@charge", DbType.String);

			parameter.Value = user.Charge;
			if (String.IsNullOrEmpty(user.Charge))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);

			parameter.Value = user.IdStore;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un UserEntity en la base de datos.
		/// </summary>
		/// <param name="user">UserEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="user"/> no es un <c>UserEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(UserEntity user)
		{
			Save(user, null);
		} 

		/// <summary>
		/// Función que guarda un UserEntity en la base de datos.
		/// </summary>
		/// <param name="user">UserEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="user"/> no es un <c>UserEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(UserEntity user, Dictionary<string,IEntity> scope)
		{
			if (user == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = user.Id.ToString(NumberFormatInfo.InvariantInfo) + "User";
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

				if (user.IsNew || !DataAccessConnection.ExistsEntity(user.Id, "User", "idUser", dbConnection, dbTransaction))
				{
					commandName = "SaveUser";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateUser";
				}
				// Se crea un command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
					parameter.Value = user.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(user, sqlCommand);
				// Ejecutar el command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					user.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = user.Id.ToString(NumberFormatInfo.InvariantInfo) + "User";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, user);
				// Guarda las colecciones de objetos relacionados.
				if (user.UserGroup != null)
				{
					this.SaveUserGroupCollection(new UserGroupDataAccess(), user, user.UserGroup, user.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				user.IsNew = false;
				user.Changed = false;
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
		/// Función que elimina un UserEntity de la base de datos.
		/// </summary>
		/// <param name="user">UserEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="user"/> no es un <c>UserEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(UserEntity user)
		{
			Delete(user, null);
		} 

		/// <summary>
		/// Función que elimina un UserEntity de la base de datos.
		/// </summary>
		/// <param name="user">UserEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="user"/> no es un <c>UserEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(UserEntity user, Dictionary<string,IEntity> scope)
		{
			if (user == null)
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

				user = this.Load(user.Id, true);
				if (user == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteUser";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
				parameterID.Value = user.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (user.UserGroup != null)
				{
					this.DeleteUserGroupCollection(new UserGroupDataAccess(), user.UserGroup, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(user.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = user.Id.ToString(NumberFormatInfo.InvariantInfo) + "User";
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
			properties.Add("idUser", typeof( int ));

			properties.Add("userName", typeof( string ));
			properties.Add("password", typeof( string ));
			properties.Add("name", typeof( string ));
			properties.Add("surname", typeof( string ));
			properties.Add("phoneNumber", typeof( string ));
			properties.Add("isUserActive", typeof( bool ));
			properties.Add("charge", typeof( string ));
			properties.Add("idStore", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los UserEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<UserEntity> LoadAll(bool loadRelation)
		{
			Collection<UserEntity> userList = new Collection<UserEntity>();

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

				string cmdText = "SELECT idUser FROM [User]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				UserEntity user;
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
					user = Load(id, loadRelation, scope);
					userList.Add(user);
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
			return userList;
		} 

		/// <summary>
		/// Función para cargar un UserEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase UserEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<UserEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<UserEntity> userList;

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

				string cmdText = "SELECT idUser, userName, password, name, surname, phoneNumber, isUserActive, charge, idStore, timestamp FROM [User] WHERE " + propertyName + " " + op + " @expValue";
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
				userList = new Collection<UserEntity>();
				UserEntity user;
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
					user = Load(id, loadRelation, null);
					userList.Add(user);
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
			return userList;
		} 

		/// <summary>
		/// Función que carga la relacion UserGroup desde la base de datos
		/// </summary>
		/// <param name="user">Entidad padre UserEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="user"/> no es un <c>UserEntity</c>.
		/// </exception>
		public void LoadRelationUserGroup(UserEntity user, Dictionary<string,IEntity> scope)
		{
			if (user == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			UserGroupDataAccess userGroupDataAccess = new UserGroupDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			userGroupDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			user.UserGroup = userGroupDataAccess.LoadByUserCollection(user.Id, scope);
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
		private void SaveUserGroupCollection(UserGroupDataAccess collectionDataAccess, UserEntity parent, Collection<UserGroupEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].User = parent;
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

				string command = "SELECT idUserGroup FROM [UserGroup] WHERE idUser = @idUser AND idUserGroup NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<UserGroupEntity> objectsToDelete = new Collection<UserGroupEntity>();
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
					UserGroupEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					UserGroupEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [UserGroup] WHERE idUserGroup = @idUserGroup";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idUserGroup", DbType.Int32);
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
		private bool DeleteUserGroupCollection(UserGroupDataAccess collectionDataAccess, Collection<UserGroupEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Función que carga una lista de UserEntity desde la base de datos por idStore.
		/// </summary>
		/// <param name="idStore">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of UserEntity</returns>
		public Collection<UserEntity> LoadByStoreCollection(int idStore, Dictionary<string,IEntity> scope)
		{
			Collection<UserEntity> userList;
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

				string cmdText = "SELECT idUser FROM [User] WHERE idStore = @idStore";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				parameter.Value = idStore;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				userList = new Collection<UserEntity>();
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
					userList.Add(Load(id, scope));
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
			return userList;
		} 

		/// <summary>
		/// Función para cargar una lista de UserEntity desde la base de datos por idStore.
		/// </summary>
		/// <param name="idStore">columna Foreing key</param>
		/// <returns>IList de UserEntity</returns>
		public Collection<UserEntity> LoadByStoreCollection(int idStore)
		{
			return LoadByStoreCollection(idStore, null);
		} 

	} 

}

