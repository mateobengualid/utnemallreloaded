using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>User</c> implement business logic to process UserEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class User: UtnEmall.Server.BusinessLogic.IUser
	{
		private UserDataAccess userDataAccess; 
		public  User()
		{
			userDataAccess = new UserDataAccess();
		} 

		/// <summary>
		/// Function to save a UserEntity to the database.
		/// </summary>
		/// <param name="userEntity">UserEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserEntity was saved successfully, the same UserEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Save userEntity using data access object
				userDataAccess.Save(userEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a UserEntity from database.
		/// </summary>
		/// <param name="userEntity">UserEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserEntity was deleted successfully, the same UserEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Delete userEntity using data access object
				userDataAccess.Delete(userEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific userEntity
		/// </summary>
		/// <param name="id">id of the UserEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public UserEntity GetUser(int id, string session)
		{
			return GetUser(id, true, session);
		} 

		/// <summary>
		/// Get an specific userEntity
		/// </summary>
		/// <param name="id">id of the UserEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserEntity> GetAllUser(string session)
		{
			return GetAllUser(true, session);
		} 

		/// <summary>
		/// Get collection of all userEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetUserWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all userEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhereEqual(string propertyName, object expValue, string session)
		{
			return GetUserWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all userEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserEntity> GetUserWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetUserWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a UserEntity before it's saved.
		/// </summary>
		/// <param name="userEntity">UserEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserEntity was deleted successfully, the same UserEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(UserEntity user)
		{
			bool result = true;

			if (user == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
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

