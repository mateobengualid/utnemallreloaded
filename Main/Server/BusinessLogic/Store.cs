using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>Store</c> implement business logic to process StoreEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class Store: UtnEmall.Server.BusinessLogic.IStore
	{
		private StoreDataAccess storeDataAccess; 
		public  Store()
		{
			storeDataAccess = new StoreDataAccess();
		} 

		/// <summary>
		/// Function to save a StoreEntity to the database.
		/// </summary>
		/// <param name="storeEntity">StoreEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the StoreEntity was saved successfully, the same StoreEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public StoreEntity Save(StoreEntity storeEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Store");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(storeEntity))
			{
				return storeEntity;
			}
			try 
			{
				// Save storeEntity using data access object
				storeDataAccess.Save(storeEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a StoreEntity from database.
		/// </summary>
		/// <param name="storeEntity">StoreEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public StoreEntity Delete(StoreEntity storeEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Store");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (storeEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete storeEntity using data access object
				storeDataAccess.Delete(storeEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific storeEntity
		/// </summary>
		/// <param name="id">id of the StoreEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public StoreEntity GetStore(int id, string session)
		{
			return GetStore(id, true, session);
		} 

		/// <summary>
		/// Get an specific storeEntity
		/// </summary>
		/// <param name="id">id of the StoreEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public StoreEntity GetStore(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Store");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return storeDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all storeEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all StoreEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<StoreEntity> GetAllStore(string session)
		{
			return GetAllStore(true, session);
		} 

		/// <summary>
		/// Get collection of all storeEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all StoreEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<StoreEntity> GetAllStore(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Store");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return storeDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all storeEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of storeEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetStoreWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all storeEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of storeEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Store");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return storeDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all storeEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of storeEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, string session)
		{
			return GetStoreWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all storeEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of storeEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetStoreWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a StoreEntity before it's saved.
		/// </summary>
		/// <param name="storeEntity">StoreEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(StoreEntity store)
		{
			bool result = true;

			if (store == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
			if (String.IsNullOrEmpty(store.Name))
			{
				store.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			if (store.Name != null)
			{
				Collection<StoreEntity> listOfEquals = storeDataAccess.LoadWhere(StoreEntity.DBName, store.Name, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != store.Id)
				{
					store.Errors.Add(new Error("Name", "Name", "Ya existe una tienda con ese nombre"));
					result = false;
				}
			}
			if (String.IsNullOrEmpty(store.TelephoneNumber))
			{
				store.Errors.Add(new Error("TelephoneNumber", "TelephoneNumber", "El número de teléfono no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(store.InternalPhoneNumber))
			{
				store.Errors.Add(new Error("InternalPhoneNumber", "InternalPhoneNumber", "El número de teléfono interno no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(store.ContactName))
			{
				store.Errors.Add(new Error("ContactName", "ContactName", "El contacto no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(store.OwnerName))
			{
				store.Errors.Add(new Error("OwnerName", "OwnerName", "El campo dueño no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(store.Email))
			{
				store.Errors.Add(new Error("Email", "Email", "El campo correo electrónico no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(store.LocalNumber))
			{
				store.Errors.Add(new Error("LocalNumber", "LocalNumber", "El número de local no puede estar vacío"));
				result = false;
			}
			if (store.Name != null)
			{
				Collection<StoreEntity> listOfEquals = storeDataAccess.LoadWhere(StoreEntity.DBName, store.Name, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != store.Id)
				{
					store.Errors.Add(new Error("Name", "Name", "Ya existe un servicio con el mismo número de local"));
					result = false;
				}
			}
			if (store.StoreCategory == null)
			{
				store.Errors.Add(new Error("StoreCategory", "StoreCategory", "La categoría no puede estar vacía"));
				result = false;
			}
			if (!ValidateStoreCategory(store.StoreCategory))
			{
				result = false;
			}
			if (store.Service == null)
			{
				store.Errors.Add(new Error("Service", "Service", "El servicio no puede estar vacío"));
				result = false;
			}
			return result;
		} 

		private static bool ValidateStoreCategory(Collection<StoreCategoryEntity> StoreCategory)
		{
			bool result = true;

			for (int  i = 0; i < StoreCategory.Count; i++)
			{
				StoreCategoryEntity item = StoreCategory[i];
				if (item.Category == null)
				{
					item.Errors.Add(new Error("Category", "Category", "La categoría no puede estar vacía"));
					result = false;
				}
			}
			return result;
		} 

	} 

}

