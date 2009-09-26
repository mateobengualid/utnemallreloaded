using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>UserActionClientData</c> implementa la lógica de negocio para guardar, editar, borrar y validar un UserActionClientDataEntity,
	/// </summary>
	public class UserActionClientData: UtnEmall.Server.BusinessLogic.IUserActionClientData
	{
		private UserActionClientDataDataAccess useractionclientdataDataAccess; 
		public  UserActionClientData()
		{
			useractionclientdataDataAccess = new UserActionClientDataDataAccess();
		} 

		/// <summary>
		/// Función para guardar UserActionClientDataEntity en la base de datos.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el UserActionClientDataEntity se guardo con exito, el mismo UserActionClientDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionClientDataEntity Save(UserActionClientDataEntity userActionClientDataEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "UserActionClientData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(userActionClientDataEntity))
			{
				return userActionClientDataEntity;
			}
			try 
			{
				// Guarda un userActionClientDataEntity usando un objeto de data access
				useractionclientdataDataAccess.Save(userActionClientDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un UserActionClientDataEntity de la base de datos.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el UserActionClientDataEntity fue eliminado con éxito, el mismo UserActionClientDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionClientDataEntity Delete(UserActionClientDataEntity userActionClientDataEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "UserActionClientData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (userActionClientDataEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un userActionClientDataEntity usando un objeto data access
				useractionclientdataDataAccess.Delete(userActionClientDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un userActionClientDataEntity específico
		/// </summary>
		/// <param name="id">id del UserActionClientDataEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionClientDataEntity GetUserActionClientData(int id, string session)
		{
			return GetUserActionClientData(id, true, session);
		} 

		/// <summary>
		/// Obtiene un userActionClientDataEntity específico
		/// </summary>
		/// <param name="id">id del UserActionClientDataEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "UserActionClientData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return useractionclientdataDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de userActionClientDataEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetAllUserActionClientData(string session)
		{
			return GetAllUserActionClientData(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de userActionClientDataEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "UserActionClientData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return useractionclientdataDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "UserActionClientData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return useractionclientdataDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, string session)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un UserActionClientDataEntity antes de ser guardado.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="userActionClientDataEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(UserActionClientDataEntity userActionClientData)
		{
			bool result = true;

			if (userActionClientData == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (userActionClientData.ActionType < 0)
			{
				userActionClientData.Errors.Add(new Error("ActionType", "ActionType", "El tipo de acción no puede ser negativo"));
				result = false;
			}
			if (userActionClientData.Start > System.DateTime.Now)
			{
				userActionClientData.Errors.Add(new Error("Start", "Start", "La fecha de inicio no puede ser mayor a la fecha actual"));
				result = false;
			}
			if (userActionClientData.Start == null)
			{
				userActionClientData.Errors.Add(new Error("Start", "Start", "La fecha de inicio no puede ser nula"));
				result = false;
			}

			if (userActionClientData.Stop > System.DateTime.Now)
			{
				userActionClientData.Errors.Add(new Error("Stop", "Stop", "La fecha de finalización no puede ser mayor a la fecha actual"));
				result = false;
			}

			if (userActionClientData.Stop < userActionClientData.Start)
			{
				userActionClientData.Errors.Add(new Error("Stop", "Stop", "La fecha de finalización no puede ser menor a la fecha de inicio"));
				result = false;
			}
			if (userActionClientData.Stop == null)
			{
				userActionClientData.Errors.Add(new Error("Stop", "Stop", "La fecha de finalización no puede ser nula"));
				result = false;
			}

			if (userActionClientData.IdTable < 0)
			{
				userActionClientData.Errors.Add(new Error("IdTable", "IdTable", "El id de tabla no puede ser negativo"));
				result = false;
			}
			if (userActionClientData.IdRegister < 0)
			{
				userActionClientData.Errors.Add(new Error("IdRegister", "IdRegister", "El id de registro no puede ser negativo"));
				result = false;
			}
			if (userActionClientData.IdComponent < 0)
			{
				userActionClientData.Errors.Add(new Error("IdComponent", "IdComponent", "El id de componente no puede ser negativo"));
				result = false;
			}
			if (userActionClientData.IdService < 0)
			{
				userActionClientData.Errors.Add(new Error("IdService", "IdService", "El id de servicio no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

