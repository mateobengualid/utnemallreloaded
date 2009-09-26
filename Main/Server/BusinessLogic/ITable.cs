using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// La interfaz <c>ITable</c> es un contrato de servicio web para procesar TableEntity,
	/// guarda, actualiza, elimina y valida los datos del mismo.
	/// </summary>
	public interface ITable
	{
		/// <summary>
		/// Función para guardar TableEntity en la base de datos.
		/// </summary>
		/// <param name="tableEntity">TableEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el TableEntity se guardo con exito, el mismo TableEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		[System.ServiceModel.OperationContract]
		TableEntity Save(TableEntity tableEntity, string session);
		/// <summary>
		/// Función para eliminar un TableEntity de la base de datos.
		/// </summary>
		/// <param name="tableEntity">TableEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el TableEntity fue eliminado con éxito, el mismo TableEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		TableEntity Delete(TableEntity tableEntity, string session);
		/// <summary>
		/// Obtiene un tableEntity específico
		/// </summary>
		/// <param name="id">id del TableEntity a cargar</param>
		/// <returns>Un TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="tableEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		TableEntity GetTable(int id, bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de tableEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de TableEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<TableEntity> GetAllTable(bool loadRelation, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los tableEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del tableEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<TableEntity> GetTableWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
		/// <summary>
		/// Obtiene una coleccion de todos los tableEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del tableEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de TableEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		Collection<TableEntity> GetTableWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
		/// <summary>
		/// Función que valida un TableEntity antes de ser guardado.
		/// </summary>
		/// <param name="tableEntity">TableEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="tableEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>

		[System.ServiceModel.OperationContract]
		bool Validate(TableEntity table);
	} 

}

