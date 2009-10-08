using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>ICategory</c> business contract to process CategoryEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface ICategory
	{
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
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CategoryEntity Save(CategoryEntity categoryEntity, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CategoryEntity Delete(CategoryEntity categoryEntity, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CategoryEntity GetCategory(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all categoryEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CategoryEntity> GetAllCategory(bool loadRelation, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CategoryEntity category);
	} 

}

