using UtnEmall.Client.DataModel;
using UtnEmall.Client.EntityModel;
using System;
using UtnEmall.Client.BusinessLogic;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.BusinessLogic
{

	/// <summary>
	/// La clase <c>Service</c> implementa la lógica de negocio para guardar, editar, borrar y validar un ServiceEntity,
	/// </summary>
	public class Service
	{
		private ServiceDataAccess serviceDataAccess; 
		/// <summary>
		/// <c>Service</c> constructor
		/// </summary>
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
		public ServiceEntity Save(ServiceEntity serviceEntity)
		{
			if (serviceEntity == null)
			{
				throw new ArgumentException("The entity can't be null", "serviceEntity");
			}
			// Valida el ServiceEntity
			if (!Validate(serviceEntity))
			{
				return serviceEntity;
			}
			try 
			{
				// Guarda un serviceEntity usando un objeto de data access
				serviceDataAccess.Save(serviceEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				// Reenvía como una excepcion personalizada
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
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
		public ServiceEntity Delete(ServiceEntity serviceEntity)
		{
			if (serviceEntity == null)
			{
				throw new ArgumentException("The argument can't be null", "serviceEntity");
			}
			try 
			{
				// Elimina un serviceEntity usando un objeto data access
				serviceDataAccess.Delete(serviceEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un serviceEntity específico
		/// </summary>
		/// <param name="id">id del ServiceEntity a cargar</param>
		/// <returns>Un ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public ServiceEntity GetService(int id)
		{
			return GetService(id, true);
		} 

		/// <summary>
		/// Obtiene un serviceEntity específico
		/// </summary>
		/// <param name="id">id del ServiceEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <returns>Un ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public ServiceEntity GetService(int id, bool loadRelation)
		{
			try 
			{
				return serviceDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de serviceEntity
		/// </summary>
		/// <returns>Collection de ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<ServiceEntity> GetAllService()
		{
			return GetAllService(true);
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
		public Collection<ServiceEntity> GetAllService(bool loadRelation)
		{
			try 
			{
				return serviceDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, OperatorType operatorType)
		{
			return GetServiceWhere(propertyName, expValue, true, operatorType);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			try 
			{
				return serviceDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue)
		{
			return GetServiceWhere(propertyName, expValue, true, OperatorType.Equal);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los serviceEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del serviceEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <returns>Colección de ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation)
		{
			try 
			{
				return GetServiceWhere(propertyName, expValue, loadRelation, OperatorType.Equal);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
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
				throw new ArgumentException("The argument can't be null");
			}
			// Check entity data
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
			// Rules::CollectionNonEmpty(ServiceCategory, "ServiceCategory can't be empty");
			// Rules::PropertyCollection(ServiceCategory, ServiceCategory)
			// {
			// Rules::PropertyNotNull(Category, "Category can't be empty");
			// };

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
					service.Errors.Add(new Error(categoryService.Name, "La categoría ", categoryService.Name + " no es una categoría de la tienda"));
				}
			}
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

