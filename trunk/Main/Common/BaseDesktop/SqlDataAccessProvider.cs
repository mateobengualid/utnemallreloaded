using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace UtnEmall.Server.DataModel
{
    /// <summary>
    /// Implementa el proveedor de datos para SQL Server.
    /// </summary>
    class SqlDataAccessProvider : DataAccessObjectFactory
    {        
        #region Constructors

        public SqlDataAccessProvider(string connectionString)
            : base(connectionString)
        {

        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Crea una nueva conexión para el proveedor actual.
        /// </summary>
        /// <returns>Una nueva conexión</returns>
        public override System.Data.IDbConnection GetNewConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        /// <summary>
        /// Crea un nuevo DataAdapter para el proveedor actual.
        /// </summary>
        /// <returns>Un nuevo DataAdapter.</returns>
        public override System.Data.IDataAdapter GetNewDataAdapter()
        {
            return new SqlDataAdapter();
        }

        /// <summary>
        /// Crea un nuevo comando para el proveedor actual.
        /// </summary>
        /// <param name="cmdText">El comando SQL.</param>
        /// <param name="connection">La conexión a utilizar.</param>
        /// <returns>Un nuevo comando usando la conexión indicada.</returns>
        public override System.Data.IDbCommand GetNewCommand(string cmdText, IDbConnection connection)
        {
            return new SqlCommand(cmdText, (SqlConnection)connection);
        }

        /// <summary>
        /// Crea un nuevo comando para el proveedor actual.
        /// </summary>
        /// <param name="cmdText">El comando SQL.</param>
        /// <param name="connection">Una conexión a usar.</param>
        /// <param name="transaction">Una transacción a usar.</param>
        /// <returns>Un nuevo comando.</returns>
        public override System.Data.IDbCommand GetNewCommand(string cmdText, IDbConnection connection, IDbTransaction transaction)
        {
            return new SqlCommand(cmdText, (SqlConnection)connection, (SqlTransaction)transaction);
        }

        /// <summary>
        /// Crea un nuevo SqlParameter
        /// </summary>
        /// <returns>Una nueva instancia de SqlParameter.</returns>
        public override System.Data.IDbDataParameter GetNewDataParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// Crea un nuevo SqlParameter
        /// </summary>
        /// <param name="parameterName">El nombre del parámetro.</param>
        /// <param name="dbType">El tipo del parámetro.</param>
        /// <returns>Una nueva instancia de SqlParameter</returns>
        public override System.Data.IDbDataParameter GetNewDataParameter(string parameterName, DbType dbType)
        {
            return new SqlParameter(parameterName, dbType);
        }

        #endregion
    }
}
