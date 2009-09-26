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
	/// El <c>DeviceProfileDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class DeviceProfileDataAccess
	{
		private bool isGlobalTransaction; 
		private SqlCeConnection dbConnection; 
		private SqlCeTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,DeviceProfileEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>DeviceProfileDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  DeviceProfileDataAccess()
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

			inMemoryEntities = new Dictionary<int,DeviceProfileEntity>();
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
		/// Función para cargar un DeviceProfileEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public DeviceProfileEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "DeviceProfile";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((DeviceProfileEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			DeviceProfileEntity deviceProfile = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				deviceProfile = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, deviceProfile);
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

					string cmdText = "SELECT idDeviceProfile, deviceType, deviceModel, macAddress, windowsMobileVersion, idCustomer, timestamp FROM [DeviceProfile] WHERE idDeviceProfile = @idDeviceProfile";
					// Crea el command

					SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idDeviceProfile", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					deviceProfile = new DeviceProfileEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						deviceProfile.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							deviceProfile.DeviceType = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							deviceProfile.DeviceModel = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							deviceProfile.MacAddress = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							deviceProfile.WindowsMobileVersion = reader.GetString(4);
						}

						deviceProfile.IdCustomer = reader.GetInt32(5);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, deviceProfile);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(deviceProfile.Id, deviceProfile);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						deviceProfile.Timestamp = reader.GetDateTime(6);
						deviceProfile.IsNew = false;
						deviceProfile.Changed = false;
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
			return deviceProfile;
		} 

		/// <summary>
		/// Función para cargar un DeviceProfileEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public DeviceProfileEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un DeviceProfileEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public DeviceProfileEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un DeviceProfileEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public DeviceProfileEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idDeviceProfile", "deviceType", "deviceModel", "macAddress", "windowsMobileVersion", "idCustomer"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("DeviceProfile");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("DeviceProfile", fieldsName, false, fieldsType);
			}
			dbChecked = true;
		} 

		private void FillSaveParameters(DeviceProfileEntity deviceProfile, SqlCeCommand sqlCommand)
		{
			SqlCeParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@deviceType", DbType.String);

			parameter.Value = deviceProfile.DeviceType;
			if (String.IsNullOrEmpty(deviceProfile.DeviceType))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@deviceModel", DbType.String);

			parameter.Value = deviceProfile.DeviceModel;
			if (String.IsNullOrEmpty(deviceProfile.DeviceModel))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@macAddress", DbType.String);

			parameter.Value = deviceProfile.MacAddress;
			if (String.IsNullOrEmpty(deviceProfile.MacAddress))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@windowsMobileVersion", DbType.String);

			parameter.Value = deviceProfile.WindowsMobileVersion;
			if (String.IsNullOrEmpty(deviceProfile.WindowsMobileVersion))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);

			parameter.Value = deviceProfile.IdCustomer;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un DeviceProfileEntity en la base de datos.
		/// </summary>
		/// <param name="deviceProfile">DeviceProfileEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="deviceProfile"/> no es un <c>DeviceProfileEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(DeviceProfileEntity deviceProfile)
		{
			Save(deviceProfile, null);
		} 

		/// <summary>
		/// Función que guarda un DeviceProfileEntity en la base de datos.
		/// </summary>
		/// <param name="deviceProfile">DeviceProfileEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="deviceProfile"/> no es un <c>DeviceProfileEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(DeviceProfileEntity deviceProfile, Dictionary<string,IEntity> scope)
		{
			if (deviceProfile == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = deviceProfile.Id.ToString(NumberFormatInfo.InvariantInfo) + "DeviceProfile";
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

				if (deviceProfile.IsNew || !DataAccessConnection.ExistsEntity(deviceProfile.Id, "DeviceProfile", "idDeviceProfile", dbConnection, dbTransaction))
				{
					commandName = "INSERT INTO [DeviceProfile] (idDeviceProfile, DEVICETYPE, DEVICEMODEL, MACADDRESS, WINDOWSMOBILEVERSION, IDCUSTOMER, [TIMESTAMP] ) VALUES( @idDeviceProfile,  @deviceType,@deviceModel,@macAddress,@windowsMobileVersion,@idCustomer, GETDATE()); ";
				}
				else 
				{
					isUpdate = true;
					commandName = "UPDATE [DeviceProfile] SET deviceType = @deviceType, deviceModel = @deviceModel, macAddress = @macAddress, windowsMobileVersion = @windowsMobileVersion, idCustomer = @idCustomer , timestamp=GETDATE() WHERE idDeviceProfile = @idDeviceProfile";
				}
				// Se crea un command
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				// Agregar los parametros del command .
				SqlCeParameter parameter;
				if (!isUpdate && deviceProfile.Id == 0)
				{
					deviceProfile.Id = DataAccessConnection.GetNextId("idDeviceProfile", "DeviceProfile", dbConnection, dbTransaction);
				}

				parameter = dataAccess.GetNewDataParameter("@idDeviceProfile", DbType.Int32);
				parameter.Value = deviceProfile.Id;
				sqlCommand.Parameters.Add(parameter);

				FillSaveParameters(deviceProfile, sqlCommand);
				// Ejecutar el command
				sqlCommand.ExecuteNonQuery();

				scopeKey = deviceProfile.Id.ToString(NumberFormatInfo.InvariantInfo) + "DeviceProfile";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, deviceProfile);
				// Guarda las colecciones de objetos relacionados.
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				deviceProfile.IsNew = false;
				deviceProfile.Changed = false;
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
		/// Función que elimina un DeviceProfileEntity de la base de datos.
		/// </summary>
		/// <param name="deviceProfile">DeviceProfileEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="deviceProfile"/> no es un <c>DeviceProfileEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(DeviceProfileEntity deviceProfile)
		{
			Delete(deviceProfile, null);
		} 

		/// <summary>
		/// Función que elimina un DeviceProfileEntity de la base de datos.
		/// </summary>
		/// <param name="deviceProfile">DeviceProfileEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="deviceProfile"/> no es un <c>DeviceProfileEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(DeviceProfileEntity deviceProfile, Dictionary<string,IEntity> scope)
		{
			if (deviceProfile == null)
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

				deviceProfile = this.Load(deviceProfile.Id, true);
				if (deviceProfile == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DELETE FROM [DeviceProfile] WHERE idDeviceProfile = @idDeviceProfile";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los valores de los parametros
				SqlCeParameter parameterID = dataAccess.GetNewDataParameter("@idDeviceProfile", DbType.Int32);
				parameterID.Value = deviceProfile.Id;
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

				inMemoryEntities.Remove(deviceProfile.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = deviceProfile.Id.ToString(NumberFormatInfo.InvariantInfo) + "DeviceProfile";
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
			properties.Add("idDeviceProfile", typeof( int ));

			properties.Add("deviceType", typeof( string ));
			properties.Add("deviceModel", typeof( string ));
			properties.Add("macAddress", typeof( string ));
			properties.Add("windowsMobileVersion", typeof( string ));
			properties.Add("idCustomer", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los DeviceProfileEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<DeviceProfileEntity> LoadAll(bool loadRelation)
		{
			Collection<DeviceProfileEntity> deviceProfileList = new Collection<DeviceProfileEntity>();

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

				string cmdText = "SELECT idDeviceProfile FROM [DeviceProfile]";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				DeviceProfileEntity deviceProfile;
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
					deviceProfile = Load(id, loadRelation, scope);
					deviceProfileList.Add(deviceProfile);
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
			return deviceProfileList;
		} 

		/// <summary>
		/// Función para cargar un DeviceProfileEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase DeviceProfileEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<DeviceProfileEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<DeviceProfileEntity> deviceProfileList;

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

				string cmdText = "SELECT idDeviceProfile, deviceType, deviceModel, macAddress, windowsMobileVersion, idCustomer, timestamp FROM [DeviceProfile] WHERE " + propertyName + " " + op + " @expValue";
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
				deviceProfileList = new Collection<DeviceProfileEntity>();
				DeviceProfileEntity deviceProfile;
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
					deviceProfile = Load(id, loadRelation, null);
					deviceProfileList.Add(deviceProfile);
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
			return deviceProfileList;
		} 

		/// <summary>
		/// Función que carga una lista de DeviceProfileEntity desde la base de datos por idCustomer.
		/// </summary>
		/// <param name="idCustomer">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of DeviceProfileEntity</returns>
		public Collection<DeviceProfileEntity> LoadByCustomerCollection(int idCustomer, Dictionary<string,IEntity> scope)
		{
			Collection<DeviceProfileEntity> deviceProfileList;
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

				string cmdText = "SELECT idDeviceProfile FROM [DeviceProfile] WHERE idCustomer = @idCustomer";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idCustomer", DbType.Int32);
				parameter.Value = idCustomer;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				deviceProfileList = new Collection<DeviceProfileEntity>();
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
					deviceProfileList.Add(Load(id, scope));
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
			return deviceProfileList;
		} 

		/// <summary>
		/// Función para cargar una lista de DeviceProfileEntity desde la base de datos por idCustomer.
		/// </summary>
		/// <param name="idCustomer">columna Foreing key</param>
		/// <returns>IList de DeviceProfileEntity</returns>
		public Collection<DeviceProfileEntity> LoadByCustomerCollection(int idCustomer)
		{
			return LoadByCustomerCollection(idCustomer, null);
		} 

	} 

}

