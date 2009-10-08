﻿using System.Data;
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
	/// El <c>UserGroupDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class UserGroupDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,UserGroupEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>UserGroupDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  UserGroupDataAccess()
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

			inMemoryEntities = new Dictionary<int,UserGroupEntity>();
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
		/// Función para cargar un UserGroupEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserGroupEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "UserGroup";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((UserGroupEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			UserGroupEntity userGroup = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				userGroup = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, userGroup);
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

					string cmdText = "SELECT idUserGroup, idGroup, idUser, timestamp FROM [UserGroup] WHERE idUserGroup = @idUserGroup";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idUserGroup", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					userGroup = new UserGroupEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						userGroup.Id = reader.GetInt32(0);

						userGroup.IdGroup = reader.GetInt32(1);
						userGroup.IdUser = reader.GetInt32(2);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, userGroup);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(userGroup.Id, userGroup);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						userGroup.Timestamp = reader.GetDateTime(3);
						userGroup.IsNew = false;
						userGroup.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationGroup(userGroup, scope);
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
			return userGroup;
		} 

		/// <summary>
		/// Función para cargar un UserGroupEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserGroupEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un UserGroupEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserGroupEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un UserGroupEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public UserGroupEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idUserGroup", "idGroup", "idUser"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("UserGroup");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("UserGroup", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteUserGroup");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveUserGroup");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateUserGroup");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("UserGroup", "idUserGroup");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("UserGroup", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("UserGroup", "idUserGroup", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(UserGroupEntity userGroup, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@idGroup", DbType.Int32);

			parameter.Value = userGroup.IdGroup;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);

			parameter.Value = userGroup.IdUser;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un UserGroupEntity en la base de datos.
		/// </summary>
		/// <param name="userGroup">UserGroupEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userGroup"/> no es un <c>UserGroupEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(UserGroupEntity userGroup)
		{
			Save(userGroup, null);
		} 

		/// <summary>
		/// Función que guarda un UserGroupEntity en la base de datos.
		/// </summary>
		/// <param name="userGroup">UserGroupEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userGroup"/> no es un <c>UserGroupEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(UserGroupEntity userGroup, Dictionary<string,IEntity> scope)
		{
			if (userGroup == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = userGroup.Id.ToString(NumberFormatInfo.InvariantInfo) + "UserGroup";
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

				if (userGroup.IsNew || !DataAccessConnection.ExistsEntity(userGroup.Id, "UserGroup", "idUserGroup", dbConnection, dbTransaction))
				{
					commandName = "SaveUserGroup";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateUserGroup";
				}
				// Se crea un command
				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idUserGroup", DbType.Int32);
					parameter.Value = userGroup.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(userGroup, sqlCommand);
				// Ejecutar el command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idUserGroup", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					userGroup.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = userGroup.Id.ToString(NumberFormatInfo.InvariantInfo) + "UserGroup";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, userGroup);
				// Guarda las colecciones de objetos relacionados.
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				userGroup.IsNew = false;
				userGroup.Changed = false;
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
		/// Función que elimina un UserGroupEntity de la base de datos.
		/// </summary>
		/// <param name="userGroup">UserGroupEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userGroup"/> no es un <c>UserGroupEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(UserGroupEntity userGroup)
		{
			Delete(userGroup, null);
		} 

		/// <summary>
		/// Función que elimina un UserGroupEntity de la base de datos.
		/// </summary>
		/// <param name="userGroup">UserGroupEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userGroup"/> no es un <c>UserGroupEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(UserGroupEntity userGroup, Dictionary<string,IEntity> scope)
		{
			if (userGroup == null)
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

				userGroup = this.Load(userGroup.Id, true);
				if (userGroup == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteUserGroup";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idUserGroup", DbType.Int32);
				parameterID.Value = userGroup.Id;
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

				inMemoryEntities.Remove(userGroup.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = userGroup.Id.ToString(NumberFormatInfo.InvariantInfo) + "UserGroup";
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
			properties.Add("idUserGroup", typeof( int ));

			properties.Add("idGroup", typeof( int ));
			properties.Add("idUser", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los UserGroupEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<UserGroupEntity> LoadAll(bool loadRelation)
		{
			Collection<UserGroupEntity> userGroupList = new Collection<UserGroupEntity>();

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

				string cmdText = "SELECT idUserGroup FROM [UserGroup]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				UserGroupEntity userGroup;
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
					userGroup = Load(id, loadRelation, scope);
					userGroupList.Add(userGroup);
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
			return userGroupList;
		} 

		/// <summary>
		/// Función para cargar un UserGroupEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase UserGroupEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<UserGroupEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<UserGroupEntity> userGroupList;

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

				string cmdText = "SELECT idUserGroup, idGroup, idUser, timestamp FROM [UserGroup] WHERE " + propertyName + " " + op + " @expValue";
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
				userGroupList = new Collection<UserGroupEntity>();
				UserGroupEntity userGroup;
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
					userGroup = Load(id, loadRelation, null);
					userGroupList.Add(userGroup);
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
			return userGroupList;
		} 

		/// <summary>
		/// Función que carga una lista de UserGroupEntity desde la base de datos por idGroup.
		/// </summary>
		/// <param name="idGroup">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of UserGroupEntity</returns>
		public Collection<UserGroupEntity> LoadByGroupCollection(int idGroup, Dictionary<string,IEntity> scope)
		{
			Collection<UserGroupEntity> userGroupList;
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

				string cmdText = "SELECT idUserGroup FROM [UserGroup] WHERE idGroup = @idGroup";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idGroup", DbType.Int32);
				parameter.Value = idGroup;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				userGroupList = new Collection<UserGroupEntity>();
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
					userGroupList.Add(Load(id, scope));
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
			return userGroupList;
		} 

		/// <summary>
		/// Función para cargar una lista de UserGroupEntity desde la base de datos por idGroup.
		/// </summary>
		/// <param name="idGroup">columna Foreing key</param>
		/// <returns>IList de UserGroupEntity</returns>
		public Collection<UserGroupEntity> LoadByGroupCollection(int idGroup)
		{
			return LoadByGroupCollection(idGroup, null);
		} 

		/// <summary>
		/// Función que carga la relacion Group desde la base de datos
		/// </summary>
		/// <param name="userGroup">Padre: UserGroupEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userGroup"/> no es un <c>UserGroupEntity</c>.
		/// </exception>
		public void LoadRelationGroup(UserGroupEntity userGroup, Dictionary<string,IEntity> scope)
		{
			if (userGroup == null)
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

				string cmdText = "SELECT idGroup FROM [UserGroup] WHERE idUserGroup = @idUserGroup";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idUserGroup", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = userGroup.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					GroupDataAccess groupDataAccess = new GroupDataAccess();
					groupDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					userGroup.Group = groupDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que carga una lista de UserGroupEntity desde la base de datos por idUser.
		/// </summary>
		/// <param name="idUser">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of UserGroupEntity</returns>
		public Collection<UserGroupEntity> LoadByUserCollection(int idUser, Dictionary<string,IEntity> scope)
		{
			Collection<UserGroupEntity> userGroupList;
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

				string cmdText = "SELECT idUserGroup FROM [UserGroup] WHERE idUser = @idUser";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idUser", DbType.Int32);
				parameter.Value = idUser;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				userGroupList = new Collection<UserGroupEntity>();
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
					userGroupList.Add(Load(id, scope));
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
			return userGroupList;
		} 

		/// <summary>
		/// Función para cargar una lista de UserGroupEntity desde la base de datos por idUser.
		/// </summary>
		/// <param name="idUser">columna Foreing key</param>
		/// <returns>IList de UserGroupEntity</returns>
		public Collection<UserGroupEntity> LoadByUserCollection(int idUser)
		{
			return LoadByUserCollection(idUser, null);
		} 

	} 

}
