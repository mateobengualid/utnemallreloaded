using System.Collections;
using System.Data;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.Server.DataModel
{
    /// <summary>
    /// Una interfaz común para todas las clases de acceso a datos.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Guarda los datos en un almacenamiento persistente.
        /// </summary>
        /// <param name="data">Una entidad a guardar.</param>
        void Save(IEntity data);

        /// <summary>
        /// Establece la conexión y una transacción a usar en la clase de acceso a datos.
        /// </summary>        
        /// <param name="connection">Una conexión a la base de datos.</param>
        /// <param name="transaction">Una transacción de base de datos.</param>
        void SetConnectionObjects(IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// Carga una entidad.
        /// </summary>
        /// <param name="id">El Id de la entidad.</param>
        /// <returns>La entidad cargada.</returns>
        IEntity Load(int id);

        /// <summary>
        /// Carga todos las entidades.
        /// </summary>
        /// <param name="loadRelation">Indica si se deben cargar los elementos relacionados.</param>
        /// <returns>Una lista de entidades.</returns>
        IList LoadAll(bool loadRelation);

        /// <summary>
        /// Elimina de forma permanente la entidad del almacenamiento.
        /// </summary>
        /// <param name="data">Una entidad a guardar.</param>
        void Delete(IEntity data);
    }
}
