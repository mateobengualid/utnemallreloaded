using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>UserAction</c> implement business logic to process UserActionEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class UserAction: UtnEmall.Server.BusinessLogic.IUserAction
	{
		private UserActionDataAccess useractionDataAccess; 
		public  UserAction()
		{
			useractionDataAccess = new UserActionDataAccess();
		} 

		/// <summary>
		/// Function to save a UserActionEntity to the database.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionEntity was saved successfully, the same UserActionEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Save userActionEntity using data access object
				useractionDataAccess.Save(userActionEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a UserActionEntity from database.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionEntity was deleted successfully, the same UserActionEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Delete userActionEntity using data access object
				useractionDataAccess.Delete(userActionEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific userActionEntity
		/// </summary>
		/// <param name="id">id of the UserActionEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public UserActionEntity GetUserAction(int id, string session)
		{
			return GetUserAction(id, true, session);
		} 

		/// <summary>
		/// Get an specific userActionEntity
		/// </summary>
		/// <param name="id">id of the UserActionEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userActionEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserActionEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetAllUserAction(string session)
		{
			return GetAllUserAction(true, session);
		} 

		/// <summary>
		/// Get collection of all userActionEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all UserActionEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userActionEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetUserActionWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all userActionEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all userActionEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhereEqual(string propertyName, object expValue, string session)
		{
			return GetUserActionWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all userActionEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of userActionEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of UserActionEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<UserActionEntity> GetUserActionWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetUserActionWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a UserActionEntity before it's saved.
		/// </summary>
		/// <param name="userActionEntity">UserActionEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the UserActionEntity was deleted successfully, the same UserActionEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="userActionEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(UserActionEntity userAction)
		{
			bool result = true;

			if (userAction == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
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
			// Rules::PropertyGreaterThanZero(IdTable, "IdTable can't be negative");
			// Rules::PropertyGreaterThanZero(IdRegister, "IdRegister can't be negative");
			// Rules::PropertyGreaterThanZero(IdComponent, "IdComponent can't be negative");

			if (userAction.IdService < 0)
			{
				userAction.Errors.Add(new Error("IdService", "IdService", "El id de servicio no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

