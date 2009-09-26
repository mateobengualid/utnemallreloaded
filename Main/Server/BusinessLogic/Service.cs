using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>Service</c> implementa la lógica de negocio para guardar, editar, borrar y validar un ServiceEntity,
	/// </summary>
	public class Service: UtnEmall.Server.BusinessLogic.IService
	{
		private ServiceDataAccess serviceDataAccess; 
		public  Service()
		{
			serviceDataAccess = new ServiceDataAccess();
		} 

		/// <summary>
		/// Función para guardar ServiceEntity en la base de datos.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el ServiceEntity se guardo con exito, el mismo ServiceEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public ServiceEntity Save(ServiceEntity serviceEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Service");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(serviceEntity))
			{
				return serviceEntity;
			}
			try 
			{
				// Check that the service is not deployed
				if (serviceEntity.Deployed)
				{
                    serviceEntity.Errors.Add(new Error("Service Deployed", "", "No se puede guardar el servicio porque ya ha sido desplegado."));
                    return serviceEntity;
                }
				serviceDataAccess.Save(serviceEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un ServiceEntity de la base de datos.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el ServiceEntity fue eliminado con éxito, el mismo ServiceEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public ServiceEntity Delete(ServiceEntity serviceEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Service");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (serviceEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un serviceEntity usando un objeto data access
				serviceDataAccess.Delete(serviceEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un serviceEntity específico
		/// </summary>
		/// <param name="id">id del ServiceEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public ServiceEntity GetService(int id, string session)
		{
			return GetService(id, true, session);
		} 

		/// <summary>
		/// Obtiene un serviceEntity específico
		/// </summary>
		/// <param name="id">id del ServiceEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public ServiceEntity GetService(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Service");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return serviceDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de serviceEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<ServiceEntity> GetAllService(string session)
		{
			return GetAllService(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de serviceEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<ServiceEntity> GetAllService(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Service");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return serviceDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetServiceWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Service");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return serviceDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, string session)
		{
			return GetServiceWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetServiceWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un ServiceEntity antes de ser guardado.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="serviceEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(ServiceEntity service)
		{
			bool result = true;

			if (service == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (String.IsNullOrEmpty(service.Name))
			{
				service.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			if (service.Name != null)
			{
				Collection<ServiceEntity> listOfEquals = serviceDataAccess.LoadWhere(ServiceEntity.DBName, service.Name, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != service.Id)
				{
					service.Errors.Add(new Error("Name", "Name", "Ya existe un servicio con el mismo nombre"));
					result = false;
				}
			}
			if (String.IsNullOrEmpty(service.Description))
			{
				service.Errors.Add(new Error("Description", "Description", "La descripción no puede estar vacía"));
				result = false;
			}

			if (service.Store != null)
			{
				foreach(ServiceCategoryEntity  serviceCategory in service.ServiceCategory)
				{
					CategoryEntity categoryService = serviceCategory.Category;
					bool isCategoryOfTheStore = false;
					foreach(StoreCategoryEntity  storeCategory in service.Store.StoreCategory)
					{
						CategoryEntity categoryStore = storeCategory.Category;
						if (categoryService.Id == categoryStore.Id)
						{
							isCategoryOfTheStore = true;
						}
					}

					result = isCategoryOfTheStore;
					service.Errors.Add(new Error(categoryService.Name, "Categoria de Servicio", categoryService.Name + " no es una categoría de la tienda"));
				}
			}
			// Rules::CollectionNonEmpty(ServiceCategory, "ServiceCategory can't be empty");
			// Rules::PropertyCollection(ServiceCategory, ServiceCategory)
			// {
			// Rules::PropertyNotNull(Category, "Category can't be empty");
			// };
			// Rules::PropertyNotNull(CustomerServiceData, "CustomerServiceData can't be empty");
			if (service.StartDate == null)
			{
				service.Errors.Add(new Error("StartDate", "StartDate", "La fecha de inicio no puede ser nula"));
				result = false;
			}

			if (service.StopDate < service.StartDate)
			{
				service.Errors.Add(new Error("StopDate", "StopDate", "La fecha de finalización no puede ser menor a la fecha de inicio"));
				result = false;
			}
			if (service.StopDate == null)
			{
				service.Errors.Add(new Error("StopDate", "StopDate", "La fecha de finalización no puede ser nula"));
				result = false;
			}
			return result;
		} 

	} 

}

