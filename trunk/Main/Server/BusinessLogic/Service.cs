using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>Service</c> implement business logic to process ServiceEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class Service: UtnEmall.Server.BusinessLogic.IService
	{
		private ServiceDataAccess serviceDataAccess; 
		public  Service()
		{
			serviceDataAccess = new ServiceDataAccess();
		} 

		/// <summary>
		/// Function to save a ServiceEntity to the database.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the ServiceEntity was saved successfully, the same ServiceEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
					serviceEntity.Errors.Add(new Error("Service Deployed", "", "The service is already deployed. Can not be saved."));
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
		/// Function to delete a ServiceEntity from database.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				// Delete serviceEntity using data access object
				serviceDataAccess.Delete(serviceEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific serviceEntity
		/// </summary>
		/// <param name="id">id of the ServiceEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public ServiceEntity GetService(int id, string session)
		{
			return GetService(id, true, session);
		} 

		/// <summary>
		/// Get an specific serviceEntity
		/// </summary>
		/// <param name="id">id of the ServiceEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all serviceEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetAllService(string session)
		{
			return GetAllService(true, session);
		} 

		/// <summary>
		/// Get collection of all serviceEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all ServiceEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all serviceEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of serviceEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetServiceWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all serviceEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of serviceEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all serviceEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of serviceEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, string session)
		{
			return GetServiceWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all serviceEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of serviceEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of ServiceEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetServiceWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a ServiceEntity before it's saved.
		/// </summary>
		/// <param name="serviceEntity">ServiceEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="serviceEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(ServiceEntity service)
		{
			bool result = true;

			if (service == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
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

