using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>UserAction</c> implementa la lógica de negocio para guardar, editar, borrar y validar un UserActionEntity,
	/// </summary>
	public class UserAction: UtnEmall.Server.BusinessLogic.IUserAction
	{
		private UserActionDataAccess useractionDataAccess; 
		public  UserAction()
		{
			useractionDataAccess = new UserActionDataAccess();
		} 

		/// <summary>
		/// Función para guardar UserActionEntity en la base de datos.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el UserActionEntity se guardo con exito, el mismo UserActionEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionEntity Save(UserActionEntity userActionEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "UserAction");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(userActionEntity))
			{
				return userActionEntity;
			}
			try 
			{
				// Guarda un userActionEntity usando un objeto de data access
				useractionDataAccess.Save(userActionEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un UserActionEntity de la base de datos.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el UserActionEntity fue eliminado con éxito, el mismo UserActionEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionEntity Delete(UserActionEntity userActionEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "UserAction");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (userActionEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un userActionEntity usando un objeto data access
				useractionDataAccess.Delete(userActionEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un userActionEntity específico
		/// </summary>
		/// <param name="id">id del UserActionEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionEntity GetUserAction(int id, string session)
		{
			return GetUserAction(id, true, session);
		} 

		/// <summary>
		/// Obtiene un userActionEntity específico
		/// </summary>
		/// <param name="id">id del UserActionEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionEntity GetUserAction(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "UserAction");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return useractionDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de userActionEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserActionEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserActionEntity> GetAllUserAction(string session)
		{
			return GetAllUserAction(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de userActionEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserActionEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserActionEntity> GetAllUserAction(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "UserAction");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return useractionDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetUserActionWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "UserAction");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return useractionDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhereEqual(string propertyName, object expValue, string session)
		{
			return GetUserActionWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetUserActionWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un UserActionEntity antes de ser guardado.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="userActionEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(UserActionEntity userAction)
		{
			bool result = true;

			if (userAction == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (userAction.ActionType < 0)
			{
				userAction.Errors.Add(new Error("ActionType", "ActionType", "El tipo de acción no puede ser negativo"));
				result = false;
			}
			if (userAction.Start > System.DateTime.Now)
			{
				userAction.Errors.Add(new Error("Start", "Start", "La fecha de inicio no puede ser mayor a la fecha actual"));
				result = false;
			}
			if (userAction.Start == null)
			{
				userAction.Errors.Add(new Error("Start", "Start", "El inicio no puede ser nulo"));
				result = false;
			}

			if (userAction.Stop > System.DateTime.Now)
			{
				userAction.Errors.Add(new Error("Stop", "Stop", "La fecha de finalización no puede ser mayor a la fecha actual"));
				result = false;
			}

			if (userAction.Stop < userAction.Start)
			{
				userAction.Errors.Add(new Error("Stop", "Stop", "La fecha de finalización no puede ser menor a la fecha de inicio"));
				result = false;
			}
			if (userAction.Stop == null)
			{
				userAction.Errors.Add(new Error("Stop", "Stop", "La fecha de finalización no puede ser nula"));
				result = false;
			}

			if (userAction.IdService < 0)
			{
				userAction.Errors.Add(new Error("IdService", "IdService", "El id de servicio no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

