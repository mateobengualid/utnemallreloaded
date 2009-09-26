using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>Table</c> implementa la lógica de negocio para guardar, editar, borrar y validar un TableEntity,
	/// </summary>
	public class Table: UtnEmall.Server.BusinessLogic.ITable
	{
		private TableDataAccess tableDataAccess; 
		public  Table()
		{
			tableDataAccess = new TableDataAccess();
		} 

		/// <summary>
		/// Función para guardar TableEntity en la base de datos.
		/// </summary>
		/// <param name="tableEntity">TableEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el TableEntity se guardo con exito, el mismo TableEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public TableEntity Save(TableEntity tableEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(tableEntity))
			{
				return tableEntity;
			}
			try 
			{
				// Guarda un tableEntity usando un objeto de data access
				tableDataAccess.Save(tableEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un TableEntity de la base de datos.
		/// </summary>
		/// <param name="tableEntity">TableEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el TableEntity fue eliminado con éxito, el mismo TableEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public TableEntity Delete(TableEntity tableEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (tableEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un tableEntity usando un objeto data access
				tableDataAccess.Delete(tableEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un tableEntity específico
		/// </summary>
		/// <param name="id">id del TableEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public TableEntity GetTable(int id, string session)
		{
			return GetTable(id, true, session);
		} 

		/// <summary>
		/// Obtiene un tableEntity específico
		/// </summary>
		/// <param name="id">id del TableEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public TableEntity GetTable(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return tableDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de tableEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de TableEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<TableEntity> GetAllTable(string session)
		{
			return GetAllTable(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de tableEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de TableEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<TableEntity> GetAllTable(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return tableDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los tableEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del tableEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<TableEntity> GetTableWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetTableWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los tableEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del tableEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<TableEntity> GetTableWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Table");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return tableDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los tableEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del tableEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<TableEntity> GetTableWhereEqual(string propertyName, object expValue, string session)
		{
			return GetTableWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los tableEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del tableEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<TableEntity> GetTableWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetTableWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un TableEntity antes de ser guardado.
		/// </summary>
		/// <param name="tableEntity">TableEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="tableEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(TableEntity table)
		{
			bool result = true;

			if (table == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (String.IsNullOrEmpty(table.Name))
			{
				table.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			return result;
		} 

	} 

}

