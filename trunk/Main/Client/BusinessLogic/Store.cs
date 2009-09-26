using UtnEmall.Client.DataModel;
using UtnEmall.Client.EntityModel;
using System;
using UtnEmall.Client.BusinessLogic;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.BusinessLogic
{

	/// <summary>
	/// La clase <c>Store</c> implementa la lógica de negocio para guardar, editar, borrar y validar un StoreEntity,
	/// </summary>
	public class Store
	{
		private StoreDataAccess storeDataAccess; 
		/// <summary>
		/// <c>Store</c> constructor
		/// </summary>
		public  Store()
		{
			storeDataAccess = new StoreDataAccess();
		} 

		/// <summary>
		/// Función para guardar StoreEntity en la base de datos.
		/// </summary>
		/// <param name="storeEntity">StoreEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el StoreEntity se guardo con exito, el mismo StoreEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public StoreEntity Save(StoreEntity storeEntity)
		{
			if (storeEntity == null)
			{
				throw new ArgumentException("The entity can't be null", "storeEntity");
			}
			// Valida el StoreEntity
			if (!Validate(storeEntity))
			{
				return storeEntity;
			}
			try 
			{
				// Guarda un storeEntity usando un objeto de data access
				storeDataAccess.Save(storeEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				// Reenvía como una excepcion personalizada
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un StoreEntity de la base de datos.
		/// </summary>
		/// <param name="storeEntity">StoreEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el StoreEntity fue eliminado con éxito, el mismo StoreEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public StoreEntity Delete(StoreEntity storeEntity)
		{
			if (storeEntity == null)
			{
				throw new ArgumentException("The argument can't be null", "storeEntity");
			}
			try 
			{
				// Elimina un storeEntity usando un objeto data access
				storeDataAccess.Delete(storeEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un storeEntity específico
		/// </summary>
		/// <param name="id">id del StoreEntity a cargar</param>
		/// <returns>Un StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public StoreEntity GetStore(int id)
		{
			return GetStore(id, true);
		} 

		/// <summary>
		/// Obtiene un storeEntity específico
		/// </summary>
		/// <param name="id">id del StoreEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <returns>Un StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="storeEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public StoreEntity GetStore(int id, bool loadRelation)
		{
			try 
			{
				return storeDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de storeEntity
		/// </summary>
		/// <returns>Collection de StoreEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<StoreEntity> GetAllStore()
		{
			return GetAllStore(true);
		} 

		/// <summary>
		/// Obtiene una colección de storeEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de StoreEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<StoreEntity> GetAllStore(bool loadRelation)
		{
			try 
			{
				return storeDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los storeEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del storeEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, OperatorType operatorType)
		{
			return GetStoreWhere(propertyName, expValue, true, operatorType);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los storeEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del storeEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación</param>
		/// <returns>Colección de StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			try 
			{
				return storeDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los storeEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del storeEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue)
		{
			return GetStoreWhere(propertyName, expValue, true, OperatorType.Equal);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los storeEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del storeEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <returns>Colección de StoreEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation)
		{
			try 
			{
				return GetStoreWhere(propertyName, expValue, loadRelation, OperatorType.Equal);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función que valida un StoreEntity antes de ser guardado.
		/// </summary>
		/// <param name="storeEntity">StoreEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="storeEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(StoreEntity store)
		{
			bool result = true;

			if (store == null)
			{
				throw new ArgumentException("The argument can't be null");
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
				store.Errors.Add(new Error("OwnerName", "OwnerName", "El dueño no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(store.Email))
			{
				store.Errors.Add(new Error("Email", "Email", "La dirección de correo electrónico no puede estar vacía"));
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
				store.Errors.Add(new Error("StoreCategory", "StoreCategory", "La categoría de la tienda no puede estar vacía"));
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

