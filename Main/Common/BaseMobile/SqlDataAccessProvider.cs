using System.Data.SqlServerCe;
using System.Data;

namespace UtnEmall.Client.DataModel
{
    /// <summary>
    /// Implementa la interfaz IDataAccessObjectFactory para usar el proveedor de SqlServer
    /// </summary>
    class SqlDataAccessProvider : DataAccessObjectFactory
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties

        #endregion

        #endregion

        #region Constructors
        public SqlDataAccessProvider(string connectionString)
            : base(connectionString)
        {

        }
        #endregion

        #region Static Methods

        #region Public Static Methods

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods
        #region IDataAccessObjectFactory Members

        /// <summary>
        /// Crea una nueva conexión para el proveedor de datos actual
        /// </summary>
        /// <returns>Una nueva conexión</returns>
        public override System.Data.SqlServerCe.SqlCeConnection GetNewConnection()
        {
            return new SqlCeConnection(this.ConnectionString);
        }

        /// <summary>
        /// Crea un nuevo DataAdapter 
        /// </summary>
        /// <returns>Un nuevo IDataAdapter</returns>
        public override System.Data.IDataAdapter GetNewDataAdapter()
        {
            return new System.Data.SqlServerCe.SqlCeDataAdapter();
        }

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <returns>Un nuevo comando</returns>
        public override System.Data.SqlServerCe.SqlCeCommand GetNewCommand(string cmdText)
        {
            return new SqlCeCommand(cmdText);
        }

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <param name="connection">Conexión a usar</param>
        /// <returns>Un nuevo comando</returns>
        public override System.Data.SqlServerCe.SqlCeCommand GetNewCommand(string cmdText, SqlCeConnection connection)
        {
            return new SqlCeCommand(cmdText, (SqlCeConnection)connection);
        }

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <param name="connection">Conexión a usar</param>
        /// <param name="transaction">Transacción a usar</param>
        /// <returns>Un nuevo comando</returns>
        public override System.Data.SqlServerCe.SqlCeCommand GetNewCommand(string cmdText, SqlCeConnection connection, SqlCeTransaction transaction)
        {

            return new SqlCeCommand(cmdText, (SqlCeConnection)connection, (SqlCeTransaction)transaction);
        }

        /// <summary>
        /// Crea un nuevo Parameter
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro.</param>
        /// <param name="dbType">Tipo del parámetro.</param>
        /// <returns>Una nueva instancia de SqlCeParameter</returns>
        public override System.Data.SqlServerCe.SqlCeParameter GetNewDataParameter()
        {
            return new SqlCeParameter();
        }

        /// <summary>
        /// Crea un nuevo Parameter
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro.</param>
        /// <param name="dbType">Tipo del parámetro.</param>
        /// <returns>Una nueva instancia de SqlCeParameter</returns>
        public override System.Data.SqlServerCe.SqlCeParameter GetNewDataParameter(string parameterName, DbType dbType)
        {
            return new SqlCeParameter(parameterName, dbType);
        }

        #endregion
        #endregion

        #region Protected and Private Instance Methods

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }
}
