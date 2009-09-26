using System.Collections;
using System.Data.SqlServerCe;
using UtnEmall.Client.EntityModel;

namespace UtnEmall.Client.DataModel
{
    /// <summary>
    /// Interfaz común para las clases de capa de acceso a datos
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Guarda una entidad
        /// </summary>
        void Save(IEntity data);

        /// <summary>
        /// Establece la conexión y transacción a usar
        /// </summary>
        void SetConnectionObjects(SqlCeConnection connection, SqlCeTransaction transaction);

        /// <summary>
        /// Carga una entidad
        /// </summary>
        IEntity Load(int id);

        /// <summary>
        /// Carga todas las entidades
        /// </summary>
        IList LoadAll(bool loadRelation);

        /// <summary>
        /// Borra de forma permanente una entidad
        /// </summary>
        void Delete(IEntity data);
    }
}
