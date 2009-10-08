using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>RegisterAssociation</c> implement business logic to process RegisterAssociationEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class RegisterAssociation: UtnEmall.Server.BusinessLogic.IRegisterAssociation
	{
		private RegisterAssociationDataAccess registerassociationDataAccess; 
		public  RegisterAssociation()
		{
			registerassociationDataAccess = new RegisterAssociationDataAccess();
		} 

		/// <summary>
		/// Function to save a RegisterAssociationEntity to the database.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the RegisterAssociationEntity was saved successfully, the same RegisterAssociationEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public RegisterAssociationEntity Save(RegisterAssociationEntity registerAssociationEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "RegisterAssociation");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(registerAssociationEntity))
			{
				return registerAssociationEntity;
			}
			try 
			{
				// Save registerAssociationEntity using data access object
				registerassociationDataAccess.Save(registerAssociationEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a RegisterAssociationEntity from database.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the RegisterAssociationEntity was deleted successfully, the same RegisterAssociationEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public RegisterAssociationEntity Delete(RegisterAssociationEntity registerAssociationEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "RegisterAssociation");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (registerAssociationEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete registerAssociationEntity using data access object
				registerassociationDataAccess.Delete(registerAssociationEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific registerAssociationEntity
		/// </summary>
		/// <param name="id">id of the RegisterAssociationEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public RegisterAssociationEntity GetRegisterAssociation(int id, string session)
		{
			return GetRegisterAssociation(id, true, session);
		} 

		/// <summary>
		/// Get an specific registerAssociationEntity
		/// </summary>
		/// <param name="id">id of the RegisterAssociationEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public RegisterAssociationEntity GetRegisterAssociation(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "RegisterAssociation");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return registerassociationDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all registerAssociationEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all RegisterAssociationEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetAllRegisterAssociation(string session)
		{
			return GetAllRegisterAssociation(true, session);
		} 

		/// <summary>
		/// Get collection of all registerAssociationEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all RegisterAssociationEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetAllRegisterAssociation(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "RegisterAssociation");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return registerassociationDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all registerAssociationEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of registerAssociationEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetRegisterAssociationWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all registerAssociationEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of registerAssociationEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "RegisterAssociation");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return registerassociationDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all registerAssociationEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of registerAssociationEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhereEqual(string propertyName, object expValue, string session)
		{
			return GetRegisterAssociationWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all registerAssociationEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of registerAssociationEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetRegisterAssociationWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a RegisterAssociationEntity before it's saved.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the RegisterAssociationEntity was deleted successfully, the same RegisterAssociationEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(RegisterAssociationEntity registerAssociation)
		{
			bool result = true;

			if (registerAssociation == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
			if (registerAssociation.IdRegister < 0)
			{
				registerAssociation.Errors.Add(new Error("IdRegister", "IdRegister", "El id de formulario no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

