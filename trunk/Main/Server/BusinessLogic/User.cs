using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>User</c> implementa la lógica de negocio para guardar, editar, borrar y validar un UserEntity,
	/// </summary>
	public class User: UtnEmall.Server.BusinessLogic.IUser
	{
		private UserDataAccess userDataAccess; 
		public  User()
		{
			userDataAccess = new UserDataAccess();
		} 

		/// <summary>
		/// Función para guardar UserEntity en la base de datos.
		/// </summary>
		/// <param name="userEntity">UserEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el UserEntity se guardo con exito, el mismo UserEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserEntity Save(UserEntity userEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "User");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(userEntity))
			{
				return userEntity;
			}
			try 
			{
				// Guarda un userEntity usando un objeto de data access
				userDataAccess.Save(userEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un UserEntity de la base de datos.
		/// </summary>
		/// <param name="userEntity">UserEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el UserEntity fue eliminado con éxito, el mismo UserEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserEntity Delete(UserEntity userEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "User");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (userEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un userEntity usando un objeto data access
				userDataAccess.Delete(userEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un userEntity específico
		/// </summary>
		/// <param name="id">id del UserEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserEntity GetUser(int id, string session)
		{
			return GetUser(id, true, session);
		} 

		/// <summary>
		/// Obtiene un userEntity específico
		/// </summary>
		/// <param name="id">id del UserEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserEntity GetUser(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "User");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return userDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de userEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserEntity> GetAllUser(string session)
		{
			return GetAllUser(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de userEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de UserEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserEntity> GetAllUser(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "User");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return userDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetUserWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "User");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return userDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhereEqual(string propertyName, object expValue, string session)
		{
			return GetUserWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetUserWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un UserEntity antes de ser guardado.
		/// </summary>
		/// <param name="userEntity">UserEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="userEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(UserEntity user)
		{
			bool result = true;

			if (user == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (String.IsNullOrEmpty(user.UserName))
			{
				user.Errors.Add(new Error("UserName", "UserName", "Nombre de usuario no puede estar vacío"));
				result = false;
			}
			if (user.UserName != null)
			{
				Collection<UserEntity> listOfEquals = userDataAccess.LoadWhere(UserEntity.DBUserName, user.UserName, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != user.Id)
				{
					user.Errors.Add(new Error("UserName", "UserName", "Nombre de usuario duplicado"));
					result = false;
				}
			}
			if (String.IsNullOrEmpty(user.Password))
			{
				user.Errors.Add(new Error("Password", "Password", "La contraseña no puede estar vacía"));
				result = false;
			}
			if (String.IsNullOrEmpty(user.Name))
			{
				user.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(user.Surname))
			{
				user.Errors.Add(new Error("Surname", "Surname", "El apellido no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(user.PhoneNumber))
			{
				user.Errors.Add(new Error("PhoneNumber", "PhoneNumber", "El número de teléfono no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(user.Charge))
			{
				user.Errors.Add(new Error("Charge", "Charge", "El cargo no puede estar vacío"));
				result = false;
			}

			if (user.UserGroup == null)
			{
				user.Errors.Add(new Error("UserGroup", "UserGroup", "El grupo de usuario no puede estar vacío"));
				result = false;
			}
			if (!ValidateUserGroup(user.UserGroup))
			{
				result = false;
			}
			return result;
		} 

		private static bool ValidateUserGroup(Collection<UserGroupEntity> UserGroup)
		{
			bool result = true;

			for (int  i = 0; i < UserGroup.Count; i++)
			{
				UserGroupEntity item = UserGroup[i];
				if (item.Group == null)
				{
					item.Errors.Add(new Error("Group", "Group", "El grupo no puede estar vacío"));
					result = false;
				}
			}
			return result;
		} 

	} 

}

