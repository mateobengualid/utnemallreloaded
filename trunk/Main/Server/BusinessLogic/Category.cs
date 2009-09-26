using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>Category</c> implementa la lógica de negocio para guardar, editar, borrar y validar un CategoryEntity,
	/// </summary>
	public class Category: UtnEmall.Server.BusinessLogic.ICategory
	{
		private CategoryDataAccess categoryDataAccess; 
		public  Category()
		{
			categoryDataAccess = new CategoryDataAccess();
		} 

		/// <summary>
		/// Función para guardar CategoryEntity en la base de datos.
		/// </summary>
		/// <param name="categoryEntity">CategoryEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el CategoryEntity se guardo con exito, el mismo CategoryEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CategoryEntity Save(CategoryEntity categoryEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Category");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(categoryEntity))
			{
				return categoryEntity;
			}
			try 
			{
				if (categoryEntity.IsNew)
				{
					categoryDataAccess.Save(categoryEntity);
					UtnEmall.Server.BusinessLogic.PreferenceDecorator.AddNewPreferencesDueToNewCategory(categoryEntity);
				}
				else 
				{
					categoryDataAccess.Save(categoryEntity);
				}
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un CategoryEntity de la base de datos.
		/// </summary>
		/// <param name="categoryEntity">CategoryEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el CategoryEntity fue eliminado con éxito, el mismo CategoryEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CategoryEntity Delete(CategoryEntity categoryEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Category");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (categoryEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete categoryEntity using data access object
				categoryDataAccess.Delete(categoryEntity);
				// Delete related customers' preference

				PreferenceDataAccess preferenceDataAccess = new PreferenceDataAccess();
				foreach(PreferenceEntity  preferenceEntity in preferenceDataAccess.LoadWhere(PreferenceEntity.DBIdCategory, categoryEntity.Id, false, OperatorType.Equal))
				{
					preferenceDataAccess.Delete(preferenceEntity);
				}
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
                categoryEntity.Errors.Add(new Error("Category", "Category", utnEmallDataAccessException.Message));
                return categoryEntity;
            }
		} 

		/// <summary>
		/// Obtiene un categoryEntity específico
		/// </summary>
		/// <param name="id">id del CategoryEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CategoryEntity GetCategory(int id, string session)
		{
			return GetCategory(id, true, session);
		} 

		/// <summary>
		/// Obtiene un categoryEntity específico
		/// </summary>
		/// <param name="id">id del CategoryEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CategoryEntity GetCategory(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Category");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return categoryDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de categoryEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CategoryEntity> GetAllCategory(string session)
		{
			return GetAllCategory(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de categoryEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CategoryEntity> GetAllCategory(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Category");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return categoryDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetCategoryWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Category");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return categoryDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, string session)
		{
			return GetCategoryWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetCategoryWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un CategoryEntity antes de ser guardado.
		/// </summary>
		/// <param name="categoryEntity">CategoryEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="categoryEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(CategoryEntity category)
		{
			bool result = true;

			if (category == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (String.IsNullOrEmpty(category.Description))
			{
				category.Errors.Add(new Error("Description", "Description", "La descripción no puede estar vacía"));
				result = false;
			}
			if (String.IsNullOrEmpty(category.Name))
			{
				category.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			if (category.Name != null)
			{
				Collection<CategoryEntity> listOfEquals = categoryDataAccess.LoadWhere(CategoryEntity.DBName, category.Name, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != category.Id)
				{
					category.Errors.Add(new Error("Name", "Name", "Nombre de categoría duplicado"));
					result = false;
				}
			}
			return result;
		} 

	} 

}

