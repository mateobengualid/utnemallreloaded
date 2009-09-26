using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>ICategory</c> es un contrato de servicio web para procesar CategoryEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface ICategory
	{
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
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CategoryEntity Save(CategoryEntity categoryEntity, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CategoryEntity Delete(CategoryEntity categoryEntity, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CategoryEntity GetCategory(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de categoryEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CategoryEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CategoryEntity> GetAllCategory(bool loadRelation, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CategoryEntity category);
	} 

}

