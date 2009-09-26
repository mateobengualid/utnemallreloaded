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
	/// El <c>CategoryDataAccess</c> es una clase
	/// que provee acceso a la base de datos para la tabla correspondiente.
	/// </summary>
	public class CategoryDataAccess
	{
		private bool isGlobalTransaction; 
		private SqlCeConnection dbConnection; 
		private SqlCeTransaction dbTransaction; 
		private DataAccessConnection dataAccess; 
		private Dictionary<int,CategoryEntity> inMemoryEntities; 
		private static Dictionary<string,Type> properties; 
		private static bool dbChecked; 
		/// <summary>
		/// Inicializa una nueva instancia de
		/// <c>CategoryDataAccess</c>.
		/// Chequea si la tabla y los procedimientos almacenados
		/// ya existen en la base de datos, si no, los crea
		/// Establece las propiedades que permite realizar consultas
		/// llamando los metodos LoadWhere.
		/// </summary>
		public  CategoryDataAccess()
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

			inMemoryEntities = new Dictionary<int,CategoryEntity>();
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
		/// Función para cargar un CategoryEntity desde la base de datos.
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga las relaciones</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CategoryEntity Load(int id, bool loadRelation, Dictionary<string,IEntity> scope)
		{
			// Crea una clave para el objeto de scope interno
			string scopeKey = id.ToString(NumberFormatInfo.InvariantInfo) + "Category";
			if (scope != null)
			{
				// Si el scope contiene el objeto, este ya fue cargado
				// retorna el objeto situado en el scope para evitar referencias circulares
				if (scope.ContainsKey(scopeKey))
				{
					return ((CategoryEntity)scope[scopeKey]);
				}
			}
			else 
			{
				// Si no existe un scope, crear uno
				scope = new Dictionary<string,IEntity>();
			}

			CategoryEntity category = null;
			// Chequear si la entidad fue ya cargada por el data access actual
			// y retornar si fue ya cargada

			if (inMemoryEntities.ContainsKey(id))
			{
				category = inMemoryEntities[id];
				// Agregar el objeto actual al scope

				scope.Add(scopeKey, category);
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

					string cmdText = "SELECT idCategory, description, name, idParentCategory, timestamp FROM [Category] WHERE idCategory = @idCategory";
					// Crea el command

					SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
					// Crear el parametro id para la consulta

					SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);
					parameter.Value = id;
					sqlCommand.Parameters.Add(parameter);
					// Usar el datareader para cargar desde la base de datos

					IDataReader reader = sqlCommand.ExecuteReader();
					category = new CategoryEntity();

					if (reader.Read())
					{
						// Cargar las filas de la entidad
						category.Id = reader.GetInt32(0);

						if (!reader.IsDBNull(1))
						{
							category.Description = reader.GetString(1);
						}
						if (!reader.IsDBNull(2))
						{
							category.Name = reader.GetString(2);
						}

						category.IdParentCategory = reader.GetInt32(3);
						// Agregar el objeto actual al scope

						scope.Add(scopeKey, category);
						// Agregar el objeto a la cahce de entidades cargadas

						inMemoryEntities.Add(category.Id, category);
						// Lee el timestamp y establece las propiedades nuevo y cambiado

						category.Timestamp = reader.GetDateTime(4);
						category.IsNew = false;
						category.Changed = false;
						// Cerrar el Reader

						reader.Close();
						// Carga los objetos relacionadoss if required

						if (loadRelation)
						{
							LoadRelationChilds(category, scope);
							LoadRelationParentCategory(category, scope);
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
			return category;
		} 

		/// <summary>
		/// Función para cargar un CategoryEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CategoryEntity Load(int id)
		{
			return Load(id, true, null);
		} 

		/// <summary>
		/// Función para cargar un CategoryEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CategoryEntity Load(int id, bool loadRelations)
		{
			return Load(id, loadRelations, null);
		} 

		/// <summary>
		/// Función para cargar un CategoryEntity desde la base de datos
		/// </summary>
		/// <param name="id">El id del registro a cargar</param>
		/// <param name="scope">Estructura interna usada para evitar la referencia circular, debe ser proveida si es llamada desde otro data access</param>
		/// <returns>La instancia de la entidad</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public CategoryEntity Load(int id, Dictionary<string,IEntity> scope)
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
			string[] fieldsName = new string[]{"idCategory", "description", "name", "idParentCategory"};
			Type[] fieldsType = new Type[]{typeof( int ), typeof( string ), typeof( string ), typeof( int )};

			bool existsTable = DataAccessConnection.DBCheckedTable("Category");

			if (!existsTable)
			{
				DataAccessConnection.CreateTable("Category", fieldsName, false, fieldsType);
			}
			dbChecked = true;
		} 

		private void FillSaveParameters(CategoryEntity category, SqlCeCommand sqlCommand)
		{
			SqlCeParameter parameter;
			parameter = dataAccess.GetNewDataParameter("@description", DbType.String);

			parameter.Value = category.Description;
			if (String.IsNullOrEmpty(category.Description))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@name", DbType.String);

			parameter.Value = category.Name;
			if (String.IsNullOrEmpty(category.Name))
			{
				parameter.Value = DBNull.Value;
			}

			sqlCommand.Parameters.Add(parameter);
			parameter = dataAccess.GetNewDataParameter("@idParentCategory", DbType.Int32);

			parameter.Value = category.IdParentCategory;
			sqlCommand.Parameters.Add(parameter);
		} 

		/// <summary>
		/// Función que guarda un CategoryEntity en la base de datos.
		/// </summary>
		/// <param name="category">CategoryEntity a guardar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="category"/> no es un <c>CategoryEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(CategoryEntity category)
		{
			Save(category, null);
		} 

		/// <summary>
		/// Función que guarda un CategoryEntity en la base de datos.
		/// </summary>
		/// <param name="category">CategoryEntity a guardar</param>
		/// <param name="scope">Estructura interna para evitar problemas con referencias circulares</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="category"/> no es un <c>CategoryEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Save(CategoryEntity category, Dictionary<string,IEntity> scope)
		{
			if (category == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crear una clave unica para identificar el objeto dentro del scope interno
			string scopeKey = category.Id.ToString(NumberFormatInfo.InvariantInfo) + "Category";
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

				if (category.IsNew || !DataAccessConnection.ExistsEntity(category.Id, "Category", "idCategory", dbConnection, dbTransaction))
				{
					commandName = "INSERT INTO [Category] (idCategory, DESCRIPTION, NAME, IDPARENTCATEGORY, [TIMESTAMP] ) VALUES( @idCategory,  @description,@name,@idParentCategory, GETDATE()); ";
				}
				else 
				{
					isUpdate = true;
					commandName = "UPDATE [Category] SET description = @description, name = @name, idParentCategory = @idParentCategory , timestamp=GETDATE() WHERE idCategory = @idCategory";
				}
				// Se crea un command
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(commandName, dbConnection, dbTransaction);
				// Agregar los parametros del command .
				SqlCeParameter parameter;
				if (!isUpdate && category.Id == 0)
				{
					category.Id = DataAccessConnection.GetNextId("idCategory", "Category", dbConnection, dbTransaction);
				}

				parameter = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);
				parameter.Value = category.Id;
				sqlCommand.Parameters.Add(parameter);

				FillSaveParameters(category, sqlCommand);
				// Ejecutar el command
				sqlCommand.ExecuteNonQuery();

				scopeKey = category.Id.ToString(NumberFormatInfo.InvariantInfo) + "Category";
				// Agregar la entidad al scope actual

				scope.Add(scopeKey, category);
				// Guarda las colecciones de objetos relacionados.
				if (category.Childs != null)
				{
					this.SaveCategoryCollection(new CategoryDataAccess(), category, category.Childs, category.IsNew, scope);
				}
				// Guardar objetos relacionados con la entidad actual
				// Actualizar
				// Cierra la conexión si fue abierta en la función
				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Actualizar los campos new y changed

				category.IsNew = false;
				category.Changed = false;
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
		/// Función que elimina un CategoryEntity de la base de datos.
		/// </summary>
		/// <param name="category">CategoryEntity a eliminar</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="category"/> no es un <c>CategoryEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(CategoryEntity category)
		{
			Delete(category, null);
		} 

		/// <summary>
		/// Función que elimina un CategoryEntity de la base de datos.
		/// </summary>
		/// <param name="category">CategoryEntity a eliminar</param>
		/// <param name="scope">Estructura interna para evitar problemas de referencia circular.</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="category"/> no es un <c>CategoryEntity</c>.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public void Delete(CategoryEntity category, Dictionary<string,IEntity> scope)
		{
			if (category == null)
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

				category = this.Load(category.Id, true);
				if (category == null)
				{
					throw new UtnEmallDataAccessException("Error al recuperar datos al intentar eliminar.");
				}
				// Crea un nuevo command para eliminar
				string cmdText = "DELETE FROM [Category] WHERE idCategory = @idCategory";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Agrega los valores de los parametros
				SqlCeParameter parameterID = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);
				parameterID.Value = category.Id;
				sqlCommand.Parameters.Add(parameterID);
				// Ejecuta el comando

				sqlCommand.ExecuteNonQuery();
				// Elimina los objetos relacionados
				if (category.Childs != null)
				{
					this.DeleteCategoryCollection(new CategoryDataAccess(), category.Childs, scope);
				}
				// Confirma la transacción si se inicio dentro de la función

				if (!isGlobalTransaction)
				{
					dbTransaction.Commit();
				}
				// Eliminamos la entidad de la lista de entidades cargadas en memoria

				inMemoryEntities.Remove(category.Id);
				// Eliminamos la entidad del scope

				if (scope != null)
				{
					string scopeKey = category.Id.ToString(NumberFormatInfo.InvariantInfo) + "Category";
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
			properties.Add("idCategory", typeof( int ));

			properties.Add("description", typeof( string ));
			properties.Add("name", typeof( string ));
			properties.Add("idParentCategory", typeof( int ));
		} 

		/// <summary>
		/// Función que carga todos los CategoryEntity desde la base de datos
		/// </summary>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista con todas las entidades</returns>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre mientras se accede a la base de datos
		/// </exception>
		public Collection<CategoryEntity> LoadAll(bool loadRelation)
		{
			Collection<CategoryEntity> categoryList = new Collection<CategoryEntity>();

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

				string cmdText = "SELECT idCategory FROM [Category]";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Crea un datareader

				IDataReader reader = sqlCommand.ExecuteReader();

				CategoryEntity category;
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
					category = Load(id, loadRelation, scope);
					categoryList.Add(category);
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
			return categoryList;
		} 

		/// <summary>
		/// Función para cargar un CategoryEntity desde la base de datos
		/// </summary>
		/// <param name="propertyName">Un string con el nombre del campo o una constante de la clase que representa ese campo</param>
		/// <param name="expValue">El valor que será insertado en la clausula where</param>
		/// <param name="loadRelation">Si es true carga la relacion</param>
		/// <returns>Una lista que contiene todas las entidades que concuerdan con la clausula where</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null or vacio.
		/// Si <paramref name="propertyName"/> no es una propiedad de la clase CategoryEntity.
		/// Si <paramref name="expValue"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallDataAccessException">
		/// Si una DbException ocurre cuando se accede a la base de datos
		/// </exception>
		public Collection<CategoryEntity> LoadWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			if (String.IsNullOrEmpty(propertyName) || expValue == null)
			{
				throw new ArgumentException("The argument can not be null or be empty", "propertyName");
			}
			if (!properties.ContainsKey(propertyName))
			{
				throw new ArgumentException("The property " + propertyName + " is not a property of this entity", "propertyName");
			}
			Collection<CategoryEntity> categoryList;

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

				string cmdText = "SELECT idCategory, description, name, idParentCategory, timestamp FROM [Category] WHERE " + propertyName + " " + op + " @expValue";
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
				categoryList = new Collection<CategoryEntity>();
				CategoryEntity category;
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
					category = Load(id, loadRelation, null);
					categoryList.Add(category);
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
			return categoryList;
		} 

		/// <summary>
		/// Función que carga la relacion Childs desde la base de datos
		/// </summary>
		/// <param name="category">Entidad padre CategoryEntity</param>
		/// <param name="scope">Estructura de datos interna para evitar los problemas de referencia circular</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="category"/> no es un <c>CategoryEntity</c>.
		/// </exception>
		public void LoadRelationChilds(CategoryEntity category, Dictionary<string,IEntity> scope)
		{
			if (category == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Crea un objeto data access para los objetos relacionados
			CategoryDataAccess categoryDataAccess = new CategoryDataAccess();
			// Establece los objetos de la conexión al data access de la relacion

			categoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
			// Carga los objetos relacionadoss

			category.Childs = categoryDataAccess.LoadByCategoryCollection(category.Id, scope);
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
		private void SaveCategoryCollection(CategoryDataAccess collectionDataAccess, CategoryEntity parent, Collection<CategoryEntity> collection, bool isNewParent, Dictionary<string,IEntity> scope)
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
				collection[i].ParentCategory = parent;
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

				string command = "SELECT idCategory FROM [Category] WHERE idParentCategory = @idParentCategory AND idCategory NOT IN (" + idList + ")";

				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(command, dbConnection, dbTransaction);

				SqlCeParameter sqlParameterId = dataAccess.GetNewDataParameter("@idParentCategory", DbType.Int32);
				sqlParameterId.Value = parent.Id;
				sqlCommand.Parameters.Add(sqlParameterId);

				IDataReader reader = sqlCommand.ExecuteReader();
				Collection<CategoryEntity> objectsToDelete = new Collection<CategoryEntity>();
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
					CategoryEntity entityToDelete = collectionDataAccess.Load(id, scope);
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
					CategoryEntity item = collection[i];
					if (!item.Changed && !item.IsNew)
					{
						// Crea el command
						string sql = "SELECT timestamp FROM [Category] WHERE idCategory = @idCategory";
						SqlCeCommand sqlCommandTimestamp = dataAccess.GetNewCommand(sql, dbConnection, dbTransaction);
						// Establece los datos a los parametros del command

						SqlCeParameter sqlParameterIdPreference = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);
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
		private bool DeleteCategoryCollection(CategoryDataAccess collectionDataAccess, Collection<CategoryEntity> collection, Dictionary<string,IEntity> scope)
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
		/// Función que carga una lista de CategoryEntity desde la base de datos por idCategory.
		/// </summary>
		/// <param name="idCategory">Foreing key</param>
		/// <param name="scope">Estructura de datos interna para evitar referencias circulares</param>
		/// <returns>List of CategoryEntity</returns>
		public Collection<CategoryEntity> LoadByCategoryCollection(int idCategory, Dictionary<string,IEntity> scope)
		{
			Collection<CategoryEntity> categoryList;
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

				string cmdText = "SELECT idCategory FROM [Category] WHERE idParentCategory = @idParentCategory";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				// Establece los parametros del command

				SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idParentCategory", DbType.Int32);
				parameter.Value = idCategory;
				sqlCommand.Parameters.Add(parameter);
				// Crea un DataReader

				IDataReader reader = sqlCommand.ExecuteReader();
				categoryList = new Collection<CategoryEntity>();
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
					categoryList.Add(Load(id, scope));
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
			return categoryList;
		} 

		/// <summary>
		/// Función para cargar una lista de CategoryEntity desde la base de datos por idCategory.
		/// </summary>
		/// <param name="idCategory">columna Foreing key</param>
		/// <returns>IList de CategoryEntity</returns>
		public Collection<CategoryEntity> LoadByCategoryCollection(int idCategory)
		{
			return LoadByCategoryCollection(idCategory, null);
		} 

		/// <summary>
		/// Función que carga la relacion ParentCategory desde la base de datos
		/// </summary>
		/// <param name="category">Padre: CategoryEntity</param>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="category"/> no es un <c>CategoryEntity</c>.
		/// </exception>
		public void LoadRelationParentCategory(CategoryEntity category, Dictionary<string,IEntity> scope)
		{
			if (category == null)
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

				string cmdText = "SELECT idParentCategory FROM [Category] WHERE idCategory = @idCategory";
				SqlCeCommand sqlCommand = dataAccess.GetNewCommand(cmdText, dbConnection, dbTransaction);
				SqlCeParameter parameter = dataAccess.GetNewDataParameter("@idCategory", DbType.Int32);
				// Establece los valores a los parametros del command

				parameter.Value = category.Id;
				sqlCommand.Parameters.Add(parameter);
				// Execute commands

				object idRelation = sqlCommand.ExecuteScalar();
				if (idRelation != null && ((int)idRelation) > 0)
				{
					// Create data access objects and set connection objects
					CategoryDataAccess categoryDataAccess = new CategoryDataAccess();
					categoryDataAccess.SetConnectionObjects(dbConnection, dbTransaction);
					// Carga los objetos relacionados

					category.ParentCategory = categoryDataAccess.Load(((int)idRelation), true, scope);
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

	} 

}

