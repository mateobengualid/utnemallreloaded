using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>CustomerServiceData</c> implementa la lógica de negocio para guardar, editar, borrar y validar un CustomerServiceDataEntity,
	/// </summary>
	public class CustomerServiceData: UtnEmall.Server.BusinessLogic.ICustomerServiceData
	{
		private CustomerServiceDataDataAccess customerservicedataDataAccess; 
		public  CustomerServiceData()
		{
			customerservicedataDataAccess = new CustomerServiceDataDataAccess();
		} 

		/// <summary>
		/// Función para guardar CustomerServiceDataEntity en la base de datos.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el CustomerServiceDataEntity se guardo con exito, el mismo CustomerServiceDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerServiceDataEntity Save(CustomerServiceDataEntity customerServiceDataEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(customerServiceDataEntity))
			{
				return customerServiceDataEntity;
			}
			try 
			{
				// Guarda un customerServiceDataEntity usando un objeto de data access
				customerservicedataDataAccess.Save(customerServiceDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un CustomerServiceDataEntity de la base de datos.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el CustomerServiceDataEntity fue eliminado con éxito, el mismo CustomerServiceDataEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerServiceDataEntity Delete(CustomerServiceDataEntity customerServiceDataEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (customerServiceDataEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un customerServiceDataEntity usando un objeto data access
				customerservicedataDataAccess.Delete(customerServiceDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un customerServiceDataEntity específico
		/// </summary>
		/// <param name="id">id del CustomerServiceDataEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerServiceDataEntity GetCustomerServiceData(int id, string session)
		{
			return GetCustomerServiceData(id, true, session);
		} 

		/// <summary>
		/// Obtiene un customerServiceDataEntity específico
		/// </summary>
		/// <param name="id">id del CustomerServiceDataEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerServiceDataEntity GetCustomerServiceData(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerservicedataDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de customerServiceDataEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CustomerServiceDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetAllCustomerServiceData(string session)
		{
			return GetAllCustomerServiceData(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de customerServiceDataEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CustomerServiceDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetAllCustomerServiceData(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerservicedataDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerServiceDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del customerServiceDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetCustomerServiceDataWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerServiceDataEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del customerServiceDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerservicedataDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerServiceDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del customerServiceDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhereEqual(string propertyName, object expValue, string session)
		{
			return GetCustomerServiceDataWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerServiceDataEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del customerServiceDataEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetCustomerServiceDataWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un CustomerServiceDataEntity antes de ser guardado.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="customerServiceDataEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(CustomerServiceDataEntity customerServiceData)
		{
			bool result = true;

			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (customerServiceData.CustomerServiceDataType < 0)
			{
				customerServiceData.Errors.Add(new Error("CustomerServiceDataType", "CustomerServiceDataType", "El tipo de dato del servicio no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

