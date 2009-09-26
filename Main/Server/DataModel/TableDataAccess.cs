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
	/// El <c>TableDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class TableDataAccess
	{
		private bool isGlobalTransaction; 
		private IDbConnection dbConnection; 
		private IDbTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,TableEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>TableDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  TableDataAccess()
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

			inMemoryEntities = new Dictionary<int,TableEntity>();
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
		/// Función para cargar un TableEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public TableEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Table";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((TableEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			TableEntity table = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				table = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, table);
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

					string cmdText = "SELECT idTable, name, isStorage, idDataModel, idComponent, timestamp FROM [Table] WHERE idTable = @idTable";
					// Crea el command

					IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					table = new TableEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						table.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							table.Name = reader.GetString(1);
						}

						table.IsStorage = reader.GetBoolean(2);
						table.IdDataModel = reader.GetInt32(3);
						table.IdComponent = reader.GetInt32(4);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, table);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(table.Id, table);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						table.Timestamp = reader.GetDateTime(5);
						table.IsNew = false;
						table.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationFields(table, scope);
							LoadRelationComponent(table, scope);
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
			return table;
		} 

		/// <summary>
		/// Función para cargar un TableEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public TableEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un TableEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public TableEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un TableEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public TableEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idTable", "name", "isStorage", "idDataModel", "idComponent"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( bool ), typeof( int ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Table");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Table", fieldsName, true, fieldsType);
			}
			bool existsProcedureDelete = DataAccessConnection.DBCheckedStoredProcedure("DeleteTable");
			bool existsProcedureSave = DataAccessConnection.DBCheckedStoredProcedure("SaveTable");
			bool existsProcedureUpdate = DataAccessConnection.DBCheckedStoredProcedure("UpdateTable");

			if (!existsProcedureDelete)
			{
				DataAccessConnection.CreateDeleteStoredProcedure("Table", "idTable");
			}

			if (!existsProcedureSave)
			{
				DataAccessConnection.CreateSaveStoredProcedure("Table", fieldsName, fieldsType);
			}

			if (!existsProcedureUpdate)
			{
				DataAccessConnection.CreateUpdateStoredProcedure("Table", "idTable", fieldsName, fieldsType);
			}

			dbChecked = true;
		} 

		private void FillSaveParameters(TableEntity table, IDbCommand sqlCommand)
		{
			IDbDataParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = table.Name;
			if (String.IsNullOrEmpty(table.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@isStorage", DbType.Boolean);

			parameter.Value = table.IsStorage;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idDataModel", DbType.Int32);

			parameter.Value = table.IdDataModel;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);

			parameter.Value = table.IdComponent;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un TableEntity en la base de datos.
		/// </summary>
		/// <param name="table">TableEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(TableEntity table)
		{
			Save(table, null);
		} 

		/// <summary>
		/// Función que guarda un TableEntity en la base de datos.
		/// </summary>
		/// <param name="table">TableEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(TableEntity table, Dictionary<string,IEntity> scope)
		{
			if (table == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = table.Id.ToString(NumberFormatInfo.InvariantInfo) + "Table";
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

				if (table.IsNew || !DataAccessConnection.ExistsEntity(table.Id, "Table", "idTable", dbConnection, dbTransaction))
				{
					commandName = "SaveTable";
				}
				else 
				{
					isUpdate = true;
					commandName = "UpdateTable";
					TableEntity tableTemp2 = new TableEntity();
					tableTemp2.Id = table.Id;
					LoadRelationComponent(tableTemp2, scope);
					if (tableTemp2.Component != null && tableTemp2.IdComponent != table.IdComponent)
					{
						ComponentDataAccess componentDataAccess = new ComponentDataAccess();
						componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
						componentDataAccess.Delete(tableTemp2.Component, scope);
					}
				}
				// Se crea un command

				IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agregar los parametros del command .

				IDbDataParameter parameter;
				if (isUpdate)
				{
					parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
					parameter.Value = table.Id;
					sqlCommand.Parameters.Add(parameter);
				}

				FillSaveParameters(table, sqlCommand);
				// Ejecutar el command
				if (isUpdate)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else 
				{
					IDbDataParameter parameterIdOutput = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
					parameterIdOutput.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(parameterIdOutput);

					sqlCommand.ExecuteNonQuery();
					table.Id = Convert.ToInt32(parameterIdOutput.Value, NumberFormatInfo.InvariantInfo);
				}

				scopeKey = table.Id.ToString(NumberFormatInfo.InvariantInfo) + "Table";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, table);
				// Guarda las colecciones de objetos relacionados.
				if (table.Fields != null)
				{
					this.SaveFieldCollection(new FieldDataAccess(), table, table.Fields, table.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				if (table.Component != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Save(table.Component, scope);
				}
				// Actualizar
				Update(table);
				// Cierra la conexión si fue abierta en la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				table.IsNew = false;
				table.Changed = false;
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
		/// Función que elimina un TableEntity de la base de datos.
		/// </summary>
		/// <param name="table">TableEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(TableEntity table)
		{
			Delete(table, null);
		} 

		/// <summary>
		/// Función que elimina un TableEntity de la base de datos.
		/// </summary>
		/// <param name="table">TableEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(TableEntity table, Dictionary<string,IEntity> scope)
		{
			if (table == null)
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

				table = this.Load(table.Id, true);
				if (table == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DeleteTable";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				// Agrega los valores de los parametros

				IDbDataParameter parameterID = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
				parameterID.Value = table.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (table.Fields != null)
				{
					this.DeleteFieldCollection(new FieldDataAccess(), table.Fields, scope);
				}

				if (table.Component != null)
				{
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					componentDataAccess.Delete(table.Component, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(table.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = table.Id.ToString(NumberFormatInfo.InvariantInfo) + "Table";
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
			properties.Add("idTable", typeof( int ));

			properties.Add("name", typeof( string ));
			properties.Add("isStorage", typeof( bool ));
			properties.Add("idDataModel", typeof( int ));
			properties.Add("idComponent", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los TableEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<TableEntity> LoadAll(bool loadRelation)
		{
			Collection<TableEntity> tableList = new Collection<TableEntity>();

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

				string cmdText = "SELECT idTable FROM [Table]";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				TableEntity table;
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
					table = Load(id, loadRelation, scope);
					tableList.Add(table);
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
			return tableList;
		} 

		/// <summary>
		/// Función para cargar un TableEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase TableEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<TableEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<TableEntity> tableList;

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

				string cmdText = "SELECT idTable, name, isStorage, idDataModel, idComponent, timestamp FROM [Table] WHERE " + propertyName + " " + op + " @expValue";
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
				tableList = new Collection<TableEntity>();
				TableEntity table;
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
					table = Load(id, loadRelation, null);
					tableList.Add(table);
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
			return tableList;
		} 

		/// <summary>
		/// Función que carga la relacion Fields desde la base de datos
		/// </summary>
		/// <param name="table">Entidad padre TableEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		public void LoadRelationFields(TableEntity table, Dictionary<string,IEntity> scope)
		{
			if (table == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			FieldDataAccess fieldDataAccess = new FieldDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			fieldDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			table.Fields = fieldDataAccess.LoadByTableCollection(table.Id, scope);
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
		private void SaveFieldCollection(FieldDataAccess collectionDataAccess, TableEntity parent, Collection<FieldEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].Table = parent;
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

				string command = "SELECT idField FROM [Field] WHERE idTable = @idTable AND idField NOT IN (" + idList + ")";

				IDbCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				IDbDataParameter sqlParameterId = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<FieldEntity> objectsToDelete = new Collection<FieldEntity>();
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
					FieldEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					FieldEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [Field] WHERE idField = @idField";
						IDbCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						IDbDataParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idField", DbType.Int32);
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
		private bool DeleteFieldCollection(FieldDataAccess collectionDataAccess, Collection<FieldEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Función que carga una lista de TableEntity desde la base de datos por idDataModel.
		/// </summary>
		/// <param name="idDataModel">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of TableEntity</returns>
		public Collection<TableEntity> LoadByDataModelCollection(int idDataModel, Dictionary<string,IEntity> scope)
		{
			Collection<TableEntity> tableList;
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

				string cmdText = "SELECT idTable FROM [Table] WHERE idDataModel = @idDataModel";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idDataModel", DbType.Int32);
				parameter.Value = idDataModel;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				tableList = new Collection<TableEntity>();
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
					tableList.Add(Load(id, scope));
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
			return tableList;
		} 

		/// <summary>
		/// Función para cargar una lista de TableEntity desde la base de datos por idDataModel.
		/// </summary>
		/// <param name="idDataModel">columna Foreing key</param>
		/// <returns>IList de TableEntity</returns>
		public Collection<TableEntity> LoadByDataModelCollection(int idDataModel)
		{
			return LoadByDataModelCollection(idDataModel, null);
		} 

		/// <summary>
		/// Función que carga la relacion Component desde la base de datos
		/// </summary>
		/// <param name="table">Padre: TableEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		public void LoadRelationComponent(TableEntity table, Dictionary<string,IEntity> scope)
		{
			if (table == null)
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

				string cmdText = "SELECT idComponent FROM [Table] WHERE idTable = @idTable";
				IDbCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = table.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					ComponentDataAccess componentDataAccess = new ComponentDataAccess();
					componentDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					table.Component = componentDataAccess.Load(((int)idRelation), true, scope);
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
		/// Función que actualiza un TableEntity en la base de datos.
		/// </summary>
		/// <param name="table">TableEntity a actualizar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="table"/> no es un <c>TableEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		private void Update(TableEntity table)
		{
			if (table == null)
			{
				throw new ArgumentException("The argument can't be null", "table");
			}
			// Construir un comando para actualizar
			string commandName = "UpdateTable";
			IDbCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			// Establece los parametros de actualización

			IDbDataParameter parameter = dataAccess.GetNewDataParameter("@idTable", DbType.Int32);
			parameter.Value = table.Id;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = table.Name;
			if (String.IsNullOrEmpty(table.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@isStorage", DbType.Boolean);

			parameter.Value = table.IsStorage;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idDataModel", DbType.Int32);

			parameter.Value = table.IdDataModel;
			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idComponent", DbType.Int32);

			parameter.Value = table.IdComponent;
			sqlCommand.Parameters.Add(parameter);
			// Ejecuta la actualización

			sqlCommand.ExecuteNonQuery();
			// Actualizar los campos new y changed

			table.IsNew = false;
			table.Changed = false;
		} 

	} 

}

