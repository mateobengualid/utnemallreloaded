using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>UserActionClientData</c> implement business logic to process UserActionClientDataEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class UserActionClientData: UtnEmall.Server.BusinessLogic.IUserActionClientData
	{
		private UserActionClientDataDataAccess useractionclientdataDataAccess; 
		public  UserActionClientData()
		{
			useractionclientdataDataAccess = new UserActionClientDataDataAccess();
		} 

		/// <summary>
		/// Function to save a UserActionClientDataEntity to the database.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionClientDataEntity was saved successfully, the same UserActionClientDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Save userActionClientDataEntity using data access object
				useractionclientdataDataAccess.Save(userActionClientDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a UserActionClientDataEntity from database.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Delete userActionClientDataEntity using data access object
				useractionclientdataDataAccess.Delete(userActionClientDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific userActionClientDataEntity
		/// </summary>
		/// <param name="id">id of the UserActionClientDataEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public UserActionClientDataEntity GetUserActionClientData(int id, string session)
		{
			return GetUserActionClientData(id, true, session);
		} 

		/// <summary>
		/// Get an specific userActionClientDataEntity
		/// </summary>
		/// <param name="id">id of the UserActionClientDataEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userActionClientDataEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetAllUserActionClientData(string session)
		{
			return GetAllUserActionClientData(true, session);
		} 

		/// <summary>
		/// Get collection of all userActionClientDataEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userActionClientDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionClientDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all userActionClientDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionClientDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userActionClientDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionClientDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, string session)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all userActionClientDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionClientDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a UserActionClientDataEntity before it's saved.
		/// </summary>
		/// <param name="userActionClientDataEntity">UserActionClientDataEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(UserActionClientDataEntity userActionClientData)
		{
			bool result = true;

			if (userActionClientData == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
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

