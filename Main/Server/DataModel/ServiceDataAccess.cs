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
	/// El <c>ServiceDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class ServiceDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,ServiceEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>ServiceDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  ServiceDataAccess()
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

			inMemoryEntities = new Dictionary<int,ServiceEntity>();
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
		/// Función para cargar un ServiceEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ServiceEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((ServiceEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			ServiceEntity service = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				service = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, service);
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

					string cmdText = "SELECT idService, name, description, webAccess, relativePathAssembly, pathAssemblyServer, active, global, image, website, deployed, updated, idMall, idStore, idCustomerServiceData, startDate, stopDate, timestamp FROM [Service] WHERE idService = @idService";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					service = new ServiceEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						service.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							service.Name = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							service.Description = reader.GetString(2);
						}
						if (!reader.IsDBNull(3))
						{
							service.WebAccess = reader.GetString(3);
						}
						if (!reader.IsDBNull(4))
						{
							service.RelativePathAssembly = reader.GetString(4);
						}
						if (!reader.IsDBNull(5))
						{
							service.PathAssemblyServer = reader.GetString(5);
						}

						service.Active = reader.GetBoolean(6);
						service.Global = reader.GetBoolean(7);
						if (!reader.IsDBNull(8))
						{
							service.Image = reader.GetString(8);
						}
						if (!reader.IsDBNull(9))
						{
							service.Website = reader.GetString(9);
						}

						service.Deployed = reader.GetBoolean(10);
						service.Updated = reader.GetBoolean(11);
						service.IdMall = reader.GetInt32(12);
						service.IdStore = reader.GetInt32(13);
						service.IdCustomerServiceData = reader.GetInt32(14);
						service.StartDate = reader.GetDateTime(15);
						service.StopDate = reader.GetDateTime(16);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, service);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(service.Id, service);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						service.Timestamp = reader.GetDateTime(17);
						service.IsNew = false;
						service.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationServiceCategory(service, scope);
							LoadRelationCustomerServiceData(service, scope);
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
			return service;
		} 

		/// <summary>
		/// Función para cargar un ServiceEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ServiceEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un ServiceEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ServiceEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un ServiceEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public ServiceEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idService", "name", "description", "webAccess", "relativePathAssembly", "pathAssemblyServer", "active", "global", "image", "website", "deployed", "updated", "idMall", "idStore", "idCustomerServiceData", "startDate", "stopDate"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( string ), typeof( bool ), typeof( bool ), typeof( string ), typeof( string ), typeof( bool ), typeof( bool ), typeof( int ), typeof( int ), typeof( int ), typeof( System.DateTime ), typeof( System.DateTime )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Service");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Service", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteService");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveService");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateService");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Service", "idService");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Service", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Service", "idService", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(ServiceEntity service, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = service.Name;
			if (String.IsNullOrEmpty(service.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@description", DbType.String);

			parameter.Value = service.Description;
			if (String.IsNullOrEmpty(service.Description))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@webAccess", DbType.String);

			parameter.Value = service.WebAccess;
			if (String.IsNullOrEmpty(service.WebAccess))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@relativePathAssembly", DbType.String);

			parameter.Value = service.RelativePathAssembly;
			if (String.IsNullOrEmpty(service.RelativePathAssembly))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@pathAssemblyServer", DbType.String);

			parameter.Value = service.PathAssemblyServer;
			if (String.IsNullOrEmpty(service.PathAssemblyServer))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@active", DbType.Boolean);

			parameter.Value = service.Active;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@global", DbType.Boolean);

			parameter.Value = service.Global;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@image", DbType.String);

			parameter.Value = service.Image;
			if (String.IsNullOrEmpty(service.Image))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@website", DbType.String);

			parameter.Value = service.Website;
			if (String.IsNullOrEmpty(service.Website))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@deployed", DbType.Boolean);

			parameter.Value = service.Deployed;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@updated", DbType.Boolean);

			parameter.Value = service.Updated;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);

			parameter.Value = service.IdMall;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);

			parameter.Value = service.IdStore;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = service.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@startDate", DbType.DateTime);

			parameter.Value = service.StartDate;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stopDate", DbType.DateTime);

			parameter.Value = service.StopDate;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un ServiceEntity en la base de datos.
		/// </summary>
		/// <param name="service">ServiceEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ServiceEntity service)
		{
			Save(service, null);
		} 

		/// <summary>
		/// Función que guarda un ServiceEntity en la base de datos.
		/// </summary>
		/// <param name="service">ServiceEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = service.Id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
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

				if (service.IsNew || !DataAccessConnection.ExistsEntity(service.Id, "Service", "idService", dbConnection, dbTransaction))
				{
					commandName = "SaveService";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateService";
					ServiceEntity serviceTemp3 = new ServiceEntity();
					serviceTemp3.Id = service.Id;
					LoadRelationCustomerServiceData(serviceTemp3, scope);
					if (serviceTemp3.CustomerServiceData != null && serviceTemp3.IdCustomerServiceData != service.IdCustomerServiceData)
					{
						CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
						customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						customerServiceDataDataAccess.Delete(serviceTemp3.CustomerServiceData, scope);
					}
				}
				// Se crea un command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
					parameter.Value = service.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(service, sqlCommand);
				// Ejecutar el command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					service.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = service.Id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, service);
				// Guarda las colecciones de objetos relacionados.
				if (service.ServiceCategory != null)
				{
					this.SaveServiceCategoryCollection(new ServiceCategoryDataAccess(), service, service.ServiceCategory, service.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				if (service.CustomerServiceData != null)
				{
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					customerServiceDataDataAccess.Save(service.CustomerServiceData, scope);
				}
				// Actualizar
				Update(service);
				// Cierra la conexión si fue abierta en la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				service.IsNew = false;
				service.Changed = false;
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
		/// Función que elimina un ServiceEntity de la base de datos.
		/// </summary>
		/// <param name="service">ServiceEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ServiceEntity service)
		{
			Delete(service, null);
		} 

		/// <summary>
		/// Función que elimina un ServiceEntity de la base de datos.
		/// </summary>
		/// <param name="service">ServiceEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
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

				service = this.Load(service.Id, true);
				if (service == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteService";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
				parameterID.Value = service.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (service.ServiceCategory != null)
				{
					this.DeleteServiceCategoryCollection(new ServiceCategoryDataAccess(), service.ServiceCategory, scope);
				}

				if (service.CustomerServiceData != null)
				{
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					customerServiceDataDataAccess.Delete(service.CustomerServiceData, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(service.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = service.Id.ToString(NumberFormatInfo.InvariantInfo) + "Service";
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
			properties.Add("idService", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("description", typeof( string ));
			properties.Add("webAccess", typeof( string ));
			properties.Add("relativePathAssembly", typeof( string ));
			properties.Add("pathAssemblyServer", typeof( string ));
			properties.Add("active", typeof( bool ));
			properties.Add("global", typeof( bool ));
			properties.Add("image", typeof( string ));
			properties.Add("website", typeof( string ));
			properties.Add("deployed", typeof( bool ));
			properties.Add("updated", typeof( bool ));
			properties.Add("idMall", typeof( int ));
			properties.Add("idStore", typeof( int ));
			properties.Add("idCustomerServiceData", typeof( int ));
			properties.Add("startDate", typeof( System.DateTime ));
			properties.Add("stopDate", typeof( System.DateTime ));
		} 

		/// <summary>
		/// Función que carga todos los ServiceEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<ServiceEntity> LoadAll(bool loadRelation)
		{
			Collection<ServiceEntity> serviceList = new Collection<ServiceEntity>();

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

				string cmdText = "SELECT idService FROM [Service]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				ServiceEntity service;
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
					service = Load(id, loadRelation, scope);
					serviceList.Add(service);
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
			return serviceList;
		} 

		/// <summary>
		/// Función para cargar un ServiceEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase ServiceEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<ServiceEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<ServiceEntity> serviceList;

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

				string cmdText = "SELECT idService, name, description, webAccess, relativePathAssembly, pathAssemblyServer, active, global, image, website, deployed, updated, idMall, idStore, idCustomerServiceData, startDate, stopDate, timestamp FROM [Service] WHERE " + propertyName + " " + op + " @expValue";
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
				serviceList = new Collection<ServiceEntity>();
				ServiceEntity service;
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
					service = Load(id, loadRelation, null);
					serviceList.Add(service);
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
			return serviceList;
		} 

		/// <summary>
		/// Función que carga una lista de ServiceEntity desde la base de datos por idMall.
		/// </summary>
		/// <param name="idMall">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByMallCollection(int idMall, Dictionary<string,IEntity> scope)
		{
			Collection<ServiceEntity> serviceList;
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

				string cmdText = "SELECT idService FROM [Service] WHERE idMall = @idMall";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);
				parameter.Value = idMall;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				serviceList = new Collection<ServiceEntity>();
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
					serviceList.Add(Load(id, scope));
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
			return serviceList;
		} 

		/// <summary>
		/// Función para cargar una lista de ServiceEntity desde la base de datos por idMall.
		/// </summary>
		/// <param name="idMall">columna Foreing key</param>
		/// <returns>IList de ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByMallCollection(int idMall)
		{
			return LoadByMallCollection(idMall, null);
		} 

		/// <summary>
		/// Función que carga una lista de ServiceEntity desde la base de datos por idStore.
		/// </summary>
		/// <param name="idStore">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByStoreCollection(int idStore, Dictionary<string,IEntity> scope)
		{
			Collection<ServiceEntity> serviceList;
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

				string cmdText = "SELECT idService FROM [Service] WHERE idStore = @idStore";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);
				parameter.Value = idStore;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				serviceList = new Collection<ServiceEntity>();
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
					serviceList.Add(Load(id, scope));
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
			return serviceList;
		} 

		/// <summary>
		/// Función para cargar una lista de ServiceEntity desde la base de datos por idStore.
		/// </summary>
		/// <param name="idStore">columna Foreing key</param>
		/// <returns>IList de ServiceEntity</returns>
		public Collection<ServiceEntity> LoadByStoreCollection(int idStore)
		{
			return LoadByStoreCollection(idStore, null);
		} 

		/// <summary>
		/// Función que carga la relacion ServiceCategory desde la base de datos
		/// </summary>
		/// <param name="service">Entidad padre ServiceEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		public void LoadRelationServiceCategory(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			ServiceCategoryDataAccess serviceCategoryDataAccess = new ServiceCategoryDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			serviceCategoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			service.ServiceCategory = serviceCategoryDataAccess.LoadByServiceCollection(service.Id, scope);
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
		private void SaveServiceCategoryCollection(ServiceCategoryDataAccess collectionDataAccess, ServiceEntity parent, Collection<ServiceCategoryEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Service = parent;
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

				string command = "SELECT idServiceCategory FROM [ServiceCategory] WHERE idService = @idService AND idServiceCategory NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<ServiceCategoryEntity> objectsToDelete = new Collection<ServiceCategoryEntity>();
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
					ServiceCategoryEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					ServiceCategoryEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [ServiceCategory] WHERE idServiceCategory = @idServiceCategory";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idServiceCategory", DbType.Int32);
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
		private bool DeleteServiceCategoryCollection(ServiceCategoryDataAccess collectionDataAccess, Collection<ServiceCategoryEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Función que carga la relacion CustomerServiceData desde la base de datos
		/// </summary>
		/// <param name="service">Padre: ServiceEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		public void LoadRelationCustomerServiceData(ServiceEntity service, Dictionary<string,IEntity> scope)
		{
			if (service == null)
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

				string cmdText = "SELECT idCustomerServiceData FROM [Service] WHERE idService = @idService";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = service.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					CustomerServiceDataDataAccess customerServiceDataDataAccess = new CustomerServiceDataDataAccess();
					customerServiceDataDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					service.CustomerServiceData = customerServiceDataDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que actualiza un ServiceEntity en la base de datos.
		/// </summary>
		/// <param name="service">ServiceEntity a actualizar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="service"/> no es un <c>ServiceEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		private void Update(ServiceEntity service)
		{
			if (service == null)
			{
				throw new ArgumentException("The argument can't be null", "service");
			}
			// Construir un comando para actualizar
			string commandName = "UpdateService";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Establece los parametros de actualización

			IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idService", DbType.Int32);
			parameter.Value = service.Id;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = service.Name;
			if (String.IsNullOrEmpty(service.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@description", DbType.String);

			parameter.Value = service.Description;
			if (String.IsNullOrEmpty(service.Description))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@webAccess", DbType.String);

			parameter.Value = service.WebAccess;
			if (String.IsNullOrEmpty(service.WebAccess))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@relativePathAssembly", DbType.String);

			parameter.Value = service.RelativePathAssembly;
			if (String.IsNullOrEmpty(service.RelativePathAssembly))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@pathAssemblyServer", DbType.String);

			parameter.Value = service.PathAssemblyServer;
			if (String.IsNullOrEmpty(service.PathAssemblyServer))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@active", DbType.Boolean);

			parameter.Value = service.Active;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@global", DbType.Boolean);

			parameter.Value = service.Global;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@image", DbType.String);

			parameter.Value = service.Image;
			if (String.IsNullOrEmpty(service.Image))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@website", DbType.String);

			parameter.Value = service.Website;
			if (String.IsNullOrEmpty(service.Website))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@deployed", DbType.Boolean);

			parameter.Value = service.Deployed;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@updated", DbType.Boolean);

			parameter.Value = service.Updated;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idMall", DbType.Int32);

			parameter.Value = service.IdMall;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idStore", DbType.Int32);

			parameter.Value = service.IdStore;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idCustomerServiceData", DbType.Int32);

			parameter.Value = service.IdCustomerServiceData;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@startDate", DbType.DateTime);

			parameter.Value = service.StartDate;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@stopDate", DbType.DateTime);

			parameter.Value = service.StopDate;
			sqlCommand.Parameters.Add(parameter);
			// Ejecuta la actualización

			sqlCommand.ExecuteNonQuery();
			// Actualizar los campos new y changed

			service.IsNew = false;
			service.Changed = false;
		} 

	} 

}

