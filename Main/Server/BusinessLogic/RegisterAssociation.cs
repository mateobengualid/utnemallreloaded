using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>RegisterAssociation</c> implementa la lógica de negocio para guardar, editar, borrar y validar un RegisterAssociationEntity,
	/// </summary>
	public class RegisterAssociation: UtnEmall.Server.BusinessLogic.IRegisterAssociation
	{
		private RegisterAssociationDataAccess registerassociationDataAccess; 
		public  RegisterAssociation()
		{
			registerassociationDataAccess = new RegisterAssociationDataAccess();
		} 

		/// <summary>
		/// Función para guardar RegisterAssociationEntity en la base de datos.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el RegisterAssociationEntity se guardo con exito, el mismo RegisterAssociationEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
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
				// Guarda un registerAssociationEntity usando un objeto de data access
				registerassociationDataAccess.Save(registerAssociationEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un RegisterAssociationEntity de la base de datos.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el RegisterAssociationEntity fue eliminado con éxito, el mismo RegisterAssociationEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
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
				// Elimina un registerAssociationEntity usando un objeto data access
				registerassociationDataAccess.Delete(registerAssociationEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un registerAssociationEntity específico
		/// </summary>
		/// <param name="id">id del RegisterAssociationEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public RegisterAssociationEntity GetRegisterAssociation(int id, string session)
		{
			return GetRegisterAssociation(id, true, session);
		} 

		/// <summary>
		/// Obtiene un registerAssociationEntity específico
		/// </summary>
		/// <param name="id">id del RegisterAssociationEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="registerAssociationEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
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
		/// Obtiene una colección de registerAssociationEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de RegisterAssociationEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetAllRegisterAssociation(string session)
		{
			return GetAllRegisterAssociation(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de registerAssociationEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de RegisterAssociationEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
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
		/// Obtiene una coleccion de todos los registerAssociationEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del registerAssociationEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetRegisterAssociationWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los registerAssociationEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del registerAssociationEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
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
		/// Obtiene una coleccion de todos los registerAssociationEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del registerAssociationEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhereEqual(string propertyName, object expValue, string session)
		{
			return GetRegisterAssociationWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los registerAssociationEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del registerAssociationEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de RegisterAssociationEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<RegisterAssociationEntity> GetRegisterAssociationWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetRegisterAssociationWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un RegisterAssociationEntity antes de ser guardado.
		/// </summary>
		/// <param name="registerAssociationEntity">RegisterAssociationEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="registerAssociationEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(RegisterAssociationEntity registerAssociation)
		{
			bool result = true;

			if (registerAssociation == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (registerAssociation.IdRegister < 0)
			{
				registerAssociation.Errors.Add(new Error("IdRegister", "IdRegister", "El id de formulario no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

