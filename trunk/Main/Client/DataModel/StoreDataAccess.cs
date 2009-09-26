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
	/// El <c>StoreDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class StoreDataAccess
	{
		private bool isGlobalTransaction; 
		private SqlCeConnection dbConnection; 
		private SqlCeTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,StoreEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>StoreDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  StoreDataAccess()
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

			inMemoryEntities = new Dictionary<int,StoreEntity>();
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
		/// Función para cargar un StoreEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public StoreEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((StoreEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			StoreEntity store = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				store = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, store);
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

					string cmdText = "SELECT idStore, name, telephoneNumber, internalPhoneNumber, contactName, ownerName, email, webAddress, localNumber, timestamp FROM [Store] WHERE idStore = @idStore";
					// Crea el command

					SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					store = new StoreEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						store.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							store.Name = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							store.TelephoneNumber = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							store.InternalPhoneNumber = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							store.ContactName = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							store.OwnerName = reader.GetString(5);
						}
						if (!reader.IsDBNull(6))
						{
							store.Email = reader.GetString(6);
						}
						if (!reader.IsDBNull(7))
						{
							store.WebAddress = reader.GetString(7);
						}
						if (!reader.IsDBNull(8))
						{
							store.LocalNumber = reader.GetString(8);
						}
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, store);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(store.Id, store);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						store.Timestamp = reader.GetDateTime(9);
						store.IsNew = false;
						store.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationStoreCategory(store, scope);
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
			return store;
		} 

		/// <summary>
		/// Función para cargar un StoreEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public StoreEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un StoreEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public StoreEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un StoreEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public StoreEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idStore", "name", "telephoneNumber", "internalPhoneNumber", "contactName", "ownerName", "email", "webAddress", "localNumber"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Store");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Store", fieldsName, false, fieldsType);
			}
			dbChecked = true;
		} 

		private void FillSaveParameters(StoreEntity store, SqlCeCommand sqlCommand)
		{
			SqlCeParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = store.Name;
			if (String.IsNullOrEmpty(store.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@telephoneNumber", DbType.String);

			parameter.Value = store.TelephoneNumber;
			if (String.IsNullOrEmpty(store.TelephoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@internalPhoneNumber", DbType.String);

			parameter.Value = store.InternalPhoneNumber;
			if (String.IsNullOrEmpty(store.InternalPhoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@contactName", DbType.String);

			parameter.Value = store.ContactName;
			if (String.IsNullOrEmpty(store.ContactName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@ownerName", DbType.String);

			parameter.Value = store.OwnerName;
			if (String.IsNullOrEmpty(store.OwnerName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@email", DbType.String);

			parameter.Value = store.Email;
			if (String.IsNullOrEmpty(store.Email))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@webAddress", DbType.String);

			parameter.Value = store.WebAddress;
			if (String.IsNullOrEmpty(store.WebAddress))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@localNumber", DbType.String);

			parameter.Value = store.LocalNumber;
			if (String.IsNullOrEmpty(store.LocalNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un StoreEntity en la base de datos.
		/// </summary>
		/// <param name="store">StoreEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="store"/> no es un <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(StoreEntity store)
		{
			Save(store, null);
		} 

		/// <summary>
		/// Función que guarda un StoreEntity en la base de datos.
		/// </summary>
		/// <param name="store">StoreEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="store"/> no es un <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(StoreEntity store, Dictionary<string,IEntity> scope)
		{
			if (store == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = store.Id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
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

				if (store.IsNew || !DataAccessConnection.ExistsEntity(store.Id, "Store", "idStore", dbConnection, dbTransaction))
				{
					commandName = "INSERT INTO [Store] (idStore, NAME, TELEPHONENUMBER, INTERNALPHONENUMBER, CONTACTNAME, OWNERNAME, EMAIL, WEBADDRESS, LOCALNUMBER, [TIMESTAMP] ) VALUES( @idStore,  @name,@telephoneNumber,@internalPhoneNumber,@contactName,@ownerName,@email,@webAddress,@localNumber, GETDATE()); ";
				}
				else 
				{
					isUpdate = true;
					commandName = "UPDATE [Store] SET name = @name, telephoneNumber = @telephoneNumber, internalPhoneNumber = @internalPhoneNumber, contactName = @contactName, ownerName = @ownerName, email = @email, webAddress = @webAddress, localNumber = @localNumber , timestamp=GETDATE() WHERE idStore = @idStore";
				}
				// Se crea un command
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				// Agregar los parametros del command .
				SqlCeParameter parameter;
				if (!isUpdate && store.Id == 0)
				{
					store.Id = DataAccessConnection.GetNextId("idStore", "Store", dbConnection, dbTransaction);
				}

				parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				parameter.Value = store.Id;
				sqlCommand.Parameters.Add(parameter);

				FillSaveParameters(store, sqlCommand);
				// Ejecutar el command
				sqlCommand.ExecuteNonQuery();

				scopeKey = store.Id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, store);
				// Guarda las colecciones de objetos relacionados.
				if (store.StoreCategory != null)
				{
					this.SaveStoreCategoryCollection(new StoreCategoryDataAccess(), store, store.StoreCategory, store.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				store.IsNew = false;
				store.Changed = false;
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
		/// Función que elimina un StoreEntity de la base de datos.
		/// </summary>
		/// <param name="store">StoreEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="store"/> no es un <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(StoreEntity store)
		{
			Delete(store, null);
		} 

		/// <summary>
		/// Función que elimina un StoreEntity de la base de datos.
		/// </summary>
		/// <param name="store">StoreEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="store"/> no es un <c>StoreEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(StoreEntity store, Dictionary<string,IEntity> scope)
		{
			if (store == null)
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

				store = this.Load(store.Id, true);
				if (store == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DELETE FROM [Store] WHERE idStore = @idStore";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los valores de los parametros
				SqlCeParameter parameterID = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				parameterID.Value = store.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (store.StoreCategory != null)
				{
					this.DeleteStoreCategoryCollection(new StoreCategoryDataAccess(), store.StoreCategory, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(store.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = store.Id.ToString(NumberFormatInfo.InvariantInfo) + "Store";
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
			properties.Add("idStore", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("telephoneNumber", typeof( string ));
			properties.Add("internalPhoneNumber", typeof( string ));
			properties.Add("contactName", typeof( string ));
			properties.Add("ownerName", typeof( string ));
			properties.Add("email", typeof( string ));
			properties.Add("webAddress", typeof( string ));
			properties.Add("localNumber", typeof( string ));
		} 

		/// <summary>
		/// Función que carga todos los StoreEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<StoreEntity> LoadAll(bool loadRelation)
		{
			Collection<StoreEntity> storeList = new Collection<StoreEntity>();

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

				string cmdText = "SELECT idStore FROM [Store]";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				StoreEntity store;
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
					store = Load(id, loadRelation, scope);
					storeList.Add(store);
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
			return storeList;
		} 

		/// <summary>
		/// Función para cargar un StoreEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase StoreEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<StoreEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<StoreEntity> storeList;

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

				string cmdText = "SELECT idStore, name, telephoneNumber, internalPhoneNumber, contactName, ownerName, email, webAddress, localNumber, timestamp FROM [Store] WHERE " + propertyName + " " + op + " @expValue";
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
				storeList = new Collection<StoreEntity>();
				StoreEntity store;
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
					store = Load(id, loadRelation, null);
					storeList.Add(store);
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
			return storeList;
		} 

		/// <summary>
		/// Función que carga la relacion StoreCategory desde la base de datos
		/// </summary>
		/// <param name="store">Entidad padre StoreEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="store"/> no es un <c>StoreEntity</c>.
		/// </exception>
		public void LoadRelationStoreCategory(StoreEntity store, Dictionary<string,IEntity> scope)
		{
			if (store == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			StoreCategoryDataAccess storeCategoryDataAccess = new StoreCategoryDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			storeCategoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			store.StoreCategory = storeCategoryDataAccess.LoadByStoreCollection(store.Id, scope);
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
		private void SaveStoreCategoryCollection(StoreCategoryDataAccess collectionDataAccess, StoreEntity parent, Collection<StoreCategoryEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Store = parent;
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

				string command = "SELECT idStoreCategory FROM [StoreCategory] WHERE idStore = @idStore AND idStoreCategory NOT IN (" + idList + ")";

				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				SqlCeParameter sqlParameterId = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<StoreCategoryEntity> objectsToDelete = new Collection<StoreCategoryEntity>();
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
					StoreCategoryEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					StoreCategoryEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [StoreCategory] WHERE idStoreCategory = @idStoreCategory";
						SqlCeCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						SqlCeParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idStoreCategory", DbType.Int32);
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
		private bool DeleteStoreCategoryCollection(StoreCategoryDataAccess collectionDataAccess, Collection<StoreCategoryEntity> collection, Dictionary<string,IEntity> scope)
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

	} 

}

