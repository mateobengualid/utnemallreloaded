using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>Category</c> implement business logic to process CategoryEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class Category: UtnEmall.Server.BusinessLogic.ICategory
	{
		private CategoryDataAccess categoryDataAccess; 
		public  Category()
		{
			categoryDataAccess = new CategoryDataAccess();
		} 

		/// <summary>
		/// Function to save a CategoryEntity to the database.
		/// </summary>
		/// <param name="categoryEntity">CategoryEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CategoryEntity was saved successfully, the same CategoryEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Function to delete a CategoryEntity from database.
		/// </summary>
		/// <param name="categoryEntity">CategoryEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CategoryEntity was deleted successfully, the same CategoryEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific categoryEntity
		/// </summary>
		/// <param name="id">id of the CategoryEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CategoryEntity GetCategory(int id, string session)
		{
			return GetCategory(id, true, session);
		} 

		/// <summary>
		/// Get an specific categoryEntity
		/// </summary>
		/// <param name="id">id of the CategoryEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all categoryEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetAllCategory(string session)
		{
			return GetAllCategory(true, session);
		} 

		/// <summary>
		/// Get collection of all categoryEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all categoryEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of categoryEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetCategoryWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all categoryEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of categoryEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
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
		/// Get collection of all categoryEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of categoryEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, string session)
		{
			return GetCategoryWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all categoryEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of categoryEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CategoryEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetCategoryWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a CategoryEntity before it's saved.
		/// </summary>
		/// <param name="categoryEntity">CategoryEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CategoryEntity was deleted successfully, the same CategoryEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="categoryEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(CategoryEntity category)
		{
			bool result = true;

			if (category == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
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

