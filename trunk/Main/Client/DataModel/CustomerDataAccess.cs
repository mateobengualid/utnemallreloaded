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
	/// El <c>CustomerDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class CustomerDataAccess
	{
		private bool isGlobalTransaction; 
		private SqlCeConnection dbConnection; 
		private SqlCeTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,CustomerEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>CustomerDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  CustomerDataAccess()
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

			inMemoryEntities = new Dictionary<int,CustomerEntity>();
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
		/// Función para cargar un CustomerEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((CustomerEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			CustomerEntity customer = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				customer = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, customer);
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

					string cmdText = "SELECT idCustomer, name, surname, address, phoneNumber, userName, password, timestamp FROM [Customer] WHERE idCustomer = @idCustomer";
					// Crea el command

					SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					customer = new CustomerEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						customer.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							customer.Name = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							customer.Surname = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							customer.Address = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							customer.PhoneNumber = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							customer.UserName = reader.GetString(5);
						}
						if (!reader.IsDBNull(6))
						{
							customer.Password = reader.GetString(6);
						}
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, customer);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(customer.Id, customer);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						customer.Timestamp = reader.GetDateTime(7);
						customer.IsNew = false;
						customer.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationPreferences(customer, scope);
							LoadRelationDeviceProfile(customer, scope);
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
			return customer;
		} 

		/// <summary>
		/// Función para cargar un CustomerEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un CustomerEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un CustomerEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CustomerEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idCustomer", "name", "surname", "address", "phoneNumber", "userName", "password"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Customer");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Customer", fieldsName, false, fieldsType);
			}
			dbChecked = true;
		} 

		private void FillSaveParameters(CustomerEntity customer, SqlCeCommand sqlCommand)
		{
			SqlCeParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = customer.Name;
			if (String.IsNullOrEmpty(customer.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@surname", DbType.String);

			parameter.Value = customer.Surname;
			if (String.IsNullOrEmpty(customer.Surname))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@address", DbType.String);

			parameter.Value = customer.Address;
			if (String.IsNullOrEmpty(customer.Address))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@phoneNumber", DbType.String);

			parameter.Value = customer.PhoneNumber;
			if (String.IsNullOrEmpty(customer.PhoneNumber))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@userName", DbType.String);

			parameter.Value = customer.UserName;
			if (String.IsNullOrEmpty(customer.UserName))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@password", DbType.String);

			parameter.Value = customer.Password;
			if (String.IsNullOrEmpty(customer.Password))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un CustomerEntity en la base de datos.
		/// </summary>
		/// <param name="customer">CustomerEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customer"/> no es un <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(CustomerEntity customer)
		{
			Save(customer, null);
		} 

		/// <summary>
		/// Función que guarda un CustomerEntity en la base de datos.
		/// </summary>
		/// <param name="customer">CustomerEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customer"/> no es un <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = customer.Id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
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

				if (customer.IsNew || !DataAccessConnection.ExistsEntity(customer.Id, "Customer", "idCustomer", dbConnection, dbTransaction))
				{
					commandName = "INSERT INTO [Customer] (idCustomer, NAME, SURNAME, ADDRESS, PHONENUMBER, USERNAME, PASSWORD, [TIMESTAMP] ) VALUES( @idCustomer,  @name,@surname,@address,@phoneNumber,@userName,@password, GETDATE()); ";
				}
				else 
				{
					isUpdate = true;
					commandName = "UPDATE [Customer] SET name = @name, surname = @surname, address = @address, phoneNumber = @phoneNumber, userName = @userName, password = @password , timestamp=GETDATE() WHERE idCustomer = @idCustomer";
				}
				// Se crea un command
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				// Agregar los parametros del command .
				SqlCeParameter parameter;
				if (!isUpdate && customer.Id == 0)
				{
					customer.Id = DataAccessConnection.GetNextId("idCustomer", "Customer", dbConnection, dbTransaction);
				}

				parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				parameter.Value = customer.Id;
				sqlCommand.Parameters.Add(parameter);

				FillSaveParameters(customer, sqlCommand);
				// Ejecutar el command
				sqlCommand.ExecuteNonQuery();

				scopeKey = customer.Id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, customer);
				// Guarda las colecciones de objetos relacionados.
				if (customer.Preferences != null)
				{
					this.SavePreferenceCollection(new PreferenceDataAccess(), customer, customer.Preferences, customer.IsNew, scope);
				}
				if (customer.DeviceProfile != null)
				{
					this.SaveDeviceProfileCollection(new DeviceProfileDataAccess(), customer, customer.DeviceProfile, customer.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				customer.IsNew = false;
				customer.Changed = false;
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
		/// Función que elimina un CustomerEntity de la base de datos.
		/// </summary>
		/// <param name="customer">CustomerEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customer"/> no es un <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(CustomerEntity customer)
		{
			Delete(customer, null);
		} 

		/// <summary>
		/// Función que elimina un CustomerEntity de la base de datos.
		/// </summary>
		/// <param name="customer">CustomerEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customer"/> no es un <c>CustomerEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
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

				customer = this.Load(customer.Id, true);
				if (customer == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DELETE FROM [Customer] WHERE idCustomer = @idCustomer";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los valores de los parametros
				SqlCeParameter parameterID = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				parameterID.Value = customer.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (customer.Preferences != null)
				{
					this.DeletePreferenceCollection(new PreferenceDataAccess(), customer.Preferences, scope);
				}
				if (customer.DeviceProfile != null)
				{
					this.DeleteDeviceProfileCollection(new DeviceProfileDataAccess(), customer.DeviceProfile, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(customer.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = customer.Id.ToString(NumberFormatInfo.InvariantInfo) + "Customer";
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
			properties.Add("idCustomer", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("surname", typeof( string ));
			properties.Add("address", typeof( string ));
			properties.Add("phoneNumber", typeof( string ));
			properties.Add("userName", typeof( string ));
			properties.Add("password", typeof( string ));
		} 

		/// <summary>
		/// Función que carga todos los CustomerEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<CustomerEntity> LoadAll(bool loadRelation)
		{
			Collection<CustomerEntity> customerList = new Collection<CustomerEntity>();

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

				string cmdText = "SELECT idCustomer FROM [Customer]";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				CustomerEntity customer;
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
					customer = Load(id, loadRelation, scope);
					customerList.Add(customer);
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
			return customerList;
		} 

		/// <summary>
		/// Función para cargar un CustomerEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase CustomerEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<CustomerEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<CustomerEntity> customerList;

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

				string cmdText = "SELECT idCustomer, name, surname, address, phoneNumber, userName, password, timestamp FROM [Customer] WHERE " + propertyName + " " + op + " @expValue";
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
				customerList = new Collection<CustomerEntity>();
				CustomerEntity customer;
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
					customer = Load(id, loadRelation, null);
					customerList.Add(customer);
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
			return customerList;
		} 

		/// <summary>
		/// Función que carga la relacion Preferences desde la base de datos
		/// </summary>
		/// <param name="customer">Entidad padre CustomerEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customer"/> no es un <c>CustomerEntity</c>.
		/// </exception>
		public void LoadRelationPreferences(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			PreferenceDataAccess preferenceDataAccess = new PreferenceDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			preferenceDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			customer.Preferences = preferenceDataAccess.LoadByCustomerCollection(customer.Id, scope);
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
		private void SavePreferenceCollection(PreferenceDataAccess collectionDataAccess, CustomerEntity parent, Collection<PreferenceEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Customer = parent;
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

				string command = "SELECT idPreference FROM [Preference] WHERE idCustomer = @idCustomer AND idPreference NOT IN (" + idList + ")";

				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				SqlCeParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<PreferenceEntity> objectsToDelete = new Collection<PreferenceEntity>();
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
					PreferenceEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					PreferenceEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [Preference] WHERE idPreference = @idPreference";
						SqlCeCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						SqlCeParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idPreference", DbType.Int32);
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
		private bool DeletePreferenceCollection(PreferenceDataAccess collectionDataAccess, Collection<PreferenceEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Función que carga la relacion DeviceProfile desde la base de datos
		/// </summary>
		/// <param name="customer">Entidad padre CustomerEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customer"/> no es un <c>CustomerEntity</c>.
		/// </exception>
		public void LoadRelationDeviceProfile(CustomerEntity customer, Dictionary<string,IEntity> scope)
		{
			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			DeviceProfileDataAccess deviceProfileDataAccess = new DeviceProfileDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			deviceProfileDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			customer.DeviceProfile = deviceProfileDataAccess.LoadByCustomerCollection(customer.Id, scope);
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
		private void SaveDeviceProfileCollection(DeviceProfileDataAccess collectionDataAccess, CustomerEntity parent, Collection<DeviceProfileEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Customer = parent;
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

				string command = "SELECT idDeviceProfile FROM [DeviceProfile] WHERE idCustomer = @idCustomer AND idDeviceProfile NOT IN (" + idList + ")";

				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				SqlCeParameter sqlParameterId = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<DeviceProfileEntity> objectsToDelete = new Collection<DeviceProfileEntity>();
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
					DeviceProfileEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					DeviceProfileEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [DeviceProfile] WHERE idDeviceProfile = @idDeviceProfile";
						SqlCeCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						SqlCeParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idDeviceProfile", DbType.Int32);
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
		private bool DeleteDeviceProfileCollection(DeviceProfileDataAccess collectionDataAccess, Collection<DeviceProfileEntity> collection, Dictionary<string,IEntity> scope)
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

