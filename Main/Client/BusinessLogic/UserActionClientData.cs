using UtnEmall.Client.DataModel;
using UtnEmall.Client.EntityModel;
using System;
using UtnEmall.Client.BusinessLogic;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.BusinessLogic
{

	/// <summary>
	/// La clase <c>UserActionClientData</c> implementa la lógica de negocio para guardar, editar, borrar y validar un UserActionClientDataEntity,
	/// </summary>
	public class UserActionClientData
	{
		private UserActionClientDataDataAccess useractionclientdataDataAccess; 
		/// <summary>
		/// <c>UserActionClientData</c> constructor
		/// </summary>
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
		public UserActionClientDataEntity Save(UserActionClientDataEntity userActionClientDataEntity)
		{
			if (userActionClientDataEntity == null)
			{
				throw new ArgumentException("The entity can't be null", "userActionClientDataEntity");
			}
			// Valida el UserActionClientDataEntity
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
				// Reenvía como una excepcion personalizada
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
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
		public UserActionClientDataEntity Delete(UserActionClientDataEntity userActionClientDataEntity)
		{
			if (userActionClientDataEntity == null)
			{
				throw new ArgumentException("The argument can't be null", "userActionClientDataEntity");
			}
			try 
			{
				// Elimina un userActionClientDataEntity usando un objeto data access
				useractionclientdataDataAccess.Delete(userActionClientDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un userActionClientDataEntity específico
		/// </summary>
		/// <param name="id">id del UserActionClientDataEntity a cargar</param>
		/// <returns>Un UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionClientDataEntity GetUserActionClientData(int id)
		{
			return GetUserActionClientData(id, true);
		} 

		/// <summary>
		/// Obtiene un userActionClientDataEntity específico
		/// </summary>
		/// <param name="id">id del UserActionClientDataEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <returns>Un UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="userActionClientDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation)
		{
			try 
			{
				return useractionclientdataDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de userActionClientDataEntity
		/// </summary>
		/// <returns>Collection de UserActionClientDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetAllUserActionClientData()
		{
			return GetAllUserActionClientData(true);
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
		public Collection<UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation)
		{
			try 
			{
				return useractionclientdataDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, OperatorType operatorType)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, true, operatorType);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			try 
			{
				return useractionclientdataDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue)
		{
			return GetUserActionClientDataWhere(propertyName, expValue, true, OperatorType.Equal);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los userActionClientDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del userActionClientDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <returns>Colección de UserActionClientDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation)
		{
			try 
			{
				return GetUserActionClientDataWhere(propertyName, expValue, loadRelation, OperatorType.Equal);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool Validate(UserActionClientDataEntity userActionClientData)
		{
			bool result = true;

			if (userActionClientData == null)
			{
				throw new ArgumentException("The argument can't be null");
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

