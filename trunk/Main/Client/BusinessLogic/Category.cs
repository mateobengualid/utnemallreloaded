using UtnEmall.Client.DataModel;
using UtnEmall.Client.EntityModel;
using System;
using UtnEmall.Client.BusinessLogic;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.BusinessLogic
{

	/// <summary>
	/// La clase <c>Category</c> implementa la lógica de negocio para guardar, editar, borrar y validar un CategoryEntity,
	/// </summary>
	public class Category
	{
		private CategoryDataAccess categoryDataAccess; 
		/// <summary>
		/// <c>Category</c> constructor
		/// </summary>
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
		public CategoryEntity Save(CategoryEntity categoryEntity)
		{
			if (categoryEntity == null)
			{
				throw new ArgumentException("The entity can't be null", "categoryEntity");
			}
			// Valida el CategoryEntity
			if (!Validate(categoryEntity))
			{
				return categoryEntity;
			}
			try 
			{
				// Guarda un categoryEntity usando un objeto de data access
				categoryDataAccess.Save(categoryEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				// Reenvía como una excepcion personalizada
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
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
		public CategoryEntity Delete(CategoryEntity categoryEntity)
		{
			if (categoryEntity == null)
			{
				throw new ArgumentException("The argument can't be null", "categoryEntity");
			}
			try 
			{
				// Elimina un categoryEntity usando un objeto data access
				categoryDataAccess.Delete(categoryEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un categoryEntity específico
		/// </summary>
		/// <param name="id">id del CategoryEntity a cargar</param>
		/// <returns>Un CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CategoryEntity GetCategory(int id)
		{
			return GetCategory(id, true);
		} 

		/// <summary>
		/// Obtiene un categoryEntity específico
		/// </summary>
		/// <param name="id">id del CategoryEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <returns>Un CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CategoryEntity GetCategory(int id, bool loadRelation)
		{
			try 
			{
				return categoryDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de categoryEntity
		/// </summary>
		/// <returns>Collection de CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CategoryEntity> GetAllCategory()
		{
			return GetAllCategory(true);
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
		public Collection<CategoryEntity> GetAllCategory(bool loadRelation)
		{
			try 
			{
				return categoryDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, OperatorType operatorType)
		{
			return GetCategoryWhere(propertyName, expValue, true, operatorType);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			try 
			{
				return categoryDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue)
		{
			return GetCategoryWhere(propertyName, expValue, true, OperatorType.Equal);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los categoryEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del categoryEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <returns>Colección de CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation)
		{
			try 
			{
				return GetCategoryWhere(propertyName, expValue, loadRelation, OperatorType.Equal);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
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
				throw new ArgumentException("The argument can't be null");
			}
			// Check entity data
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

