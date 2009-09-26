using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>DataModel</c> implementa la lógica de negocio para guardar, editar, borrar y validar un DataModelEntity,
	/// </summary>
	public class DataModel: UtnEmall.Server.BusinessLogic.IDataModel
	{
		private DataModelDataAccess datamodelDataAccess; 
		public  DataModel()
		{
			datamodelDataAccess = new DataModelDataAccess();
		} 

		/// <summary>
		/// Función para guardar DataModelEntity en la base de datos.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el DataModelEntity se guardo con exito, el mismo DataModelEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public DataModelEntity Save(DataModelEntity dataModelEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(dataModelEntity))
			{
				return dataModelEntity;
			}
			try 
			{
				// Check that the service is not deployed
				if (dataModelEntity.Deployed)
				{
                    dataModelEntity.Errors.Add(new Error("DataModel Deployed", "", "No se puede guardar el modelo de datos porque ya ha sido desplegado."));
                    return dataModelEntity;
				}
				// Check that there isn't related custom services
				if (dataModelEntity.Id > 0)
				{
					CustomerServiceDataDataAccess customerServiceData = new CustomerServiceDataDataAccess();
					// Get all customer services where IdDataModel is the same as us

					int referencedServices = customerServiceData.LoadWhere(CustomerServiceDataEntity.DBIdDataModel, dataModelEntity.Id, false, OperatorType.Equal).Count;
					// If there are customer services it is an error

					if (referencedServices > 0)
					{
                        dataModelEntity.Errors.Add(new Error("DataModel Deployed", "", "No se puede guardar el modelo de datos porque tiene servicios asociados."));
                        return dataModelEntity;
                    }
				}
				// Save dataModelEntity using data access object
				datamodelDataAccess.Save(dataModelEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un DataModelEntity de la base de datos.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el DataModelEntity fue eliminado con éxito, el mismo DataModelEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public DataModelEntity Delete(DataModelEntity dataModelEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (dataModelEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Check that the service is not deployed
				if (dataModelEntity.Deployed)
				{
                    dataModelEntity.Errors.Add(new Error("DataModel Deployed", "", "No se puede borrar el modelo de datos porque ya ha sido desplegado"));
                    return dataModelEntity;
				}
				// Delete dataModelEntity using data access object
				datamodelDataAccess.Delete(dataModelEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un dataModelEntity específico
		/// </summary>
		/// <param name="id">id del DataModelEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public DataModelEntity GetDataModel(int id, string session)
		{
			return GetDataModel(id, true, session);
		} 

		/// <summary>
		/// Obtiene un dataModelEntity específico
		/// </summary>
		/// <param name="id">id del DataModelEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="dataModelEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public DataModelEntity GetDataModel(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return datamodelDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de dataModelEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de DataModelEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<DataModelEntity> GetAllDataModel(string session)
		{
			return GetAllDataModel(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de dataModelEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de DataModelEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<DataModelEntity> GetAllDataModel(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return datamodelDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los dataModelEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del dataModelEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<DataModelEntity> GetDataModelWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetDataModelWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los dataModelEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del dataModelEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<DataModelEntity> GetDataModelWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "DataModel");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return datamodelDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los dataModelEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del dataModelEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<DataModelEntity> GetDataModelWhereEqual(string propertyName, object expValue, string session)
		{
			return GetDataModelWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los dataModelEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del dataModelEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de DataModelEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<DataModelEntity> GetDataModelWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetDataModelWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un DataModelEntity antes de ser guardado.
		/// </summary>
		/// <param name="dataModelEntity">DataModelEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="dataModelEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(DataModelEntity dataModel)
		{
			bool result = true;

			if (dataModel == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (dataModel.Tables == null)
			{
				dataModel.Errors.Add(new Error("Tables", "Tables", "El nombre de la tabla no puede estar vacío"));
				result = false;
			}
			if (!ValidateTables(dataModel.Tables))
			{
				result = false;
			}
			if (dataModel.Relations == null)
			{
				dataModel.Errors.Add(new Error("Relations", "Relations", "Las relaciones no pueden estar vacías"));
				result = false;
			}
			if (!ValidateRelations(dataModel.Relations))
			{
				result = false;
			}
			return result;
		} 

		private static bool ValidateTables(Collection<TableEntity> Tables)
		{
			bool result = true;

			for (int  i = 0; i < Tables.Count; i++)
			{
				TableEntity item = Tables[i];
				if (String.IsNullOrEmpty(item.Name))
				{
					item.Errors.Add(new Error("Name", "Name", "El nombre de tabla no puede estar vacío"));
					result = false;
				}

				if (item.Fields == null)
				{
					item.Errors.Add(new Error("Fields", "Fields", "El nombre de campo no puede estar vacío"));
					result = false;
				}
				if (!ValidateFields(item.Fields))
				{
					result = false;
				}
			}
			return result;
		} 

		private static bool ValidateRelations(Collection<RelationEntity> Relations)
		{
			bool result = true;

			for (int  i = 0; i < Relations.Count; i++)
			{
				RelationEntity item = Relations[i];
				if (item.RelationType < 0)
				{
					item.Errors.Add(new Error("RelationType", "RelationType", "El tipo de relación no puede ser cero"));
					result = false;
				}
				if (item.IdTarget < 0)
				{
					item.Errors.Add(new Error("IdTarget", "IdTarget", "IdTarget no puede estar vacío"));
					result = false;
				}
				if (item.IdSource < 0)
				{
					item.Errors.Add(new Error("IdSource", "IdSource", "IdSource no puede estar vacío"));
					result = false;
				}
			}
			return result;
		} 

		private static bool ValidateFields(Collection<FieldEntity> Fields)
		{
			bool result = true;

			for (int  i = 0; i < Fields.Count; i++)
			{
				FieldEntity item = Fields[i];
				if (String.IsNullOrEmpty(item.Name))
				{
					item.Errors.Add(new Error("Name", "Name", "El nombre de campo no puede estar vacío"));
					result = false;
				}

				if (item.DataType < 0)
				{
					item.Errors.Add(new Error("DataType", "DataType", "El tipo de dato no puede estar vacío"));
					result = false;
				}
			}
			return result;
		} 

	} 

}

