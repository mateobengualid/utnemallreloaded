using System;
using System.Collections.Generic;
using System.Data;

namespace UtnEmall.Server.DataModel
{
    /// <summary>
    /// Clase de datos abstracta para proveedores de datos
    /// </summary>
    abstract class DataAccessObjectFactory
    {
        #region Constants, Variables and Properties

        private string connectionString;
        public string ConnectionString
        {
            get { return connectionString; }
        }

        #endregion

        #region Constructors

        public DataAccessObjectFactory(string connectionString)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentException("connectionString can't be null");
            }
            this.connectionString = connectionString;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Retorna una nueva conexión para el proveedor actual.
        /// </summary>
        /// <returns>Una nueva conexión.</returns>
        public abstract IDbConnection GetNewConnection();
        
        /// <summary>
        /// Retorna un nuevo DataAdapter para el proveedor actual
        /// </summary>
        /// <returns>Un nuevo DataAdapter.</returns>
        public abstract IDataAdapter GetNewDataAdapter();

        /// <summary>
        /// Retorna un nuevo comando para el proveedor
        /// </summary>
        /// <param name="cmdText">Una cadena con el comando SQL.</param>
        /// <param name="connection">Conexión a usar.</param>
        /// <returns>Un nuevo comando para la conexión indicada.</returns>
        public abstract IDbCommand GetNewCommand(string cmdText, IDbConnection connection);

        /// <summary>
        /// Crea un nuevo comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Comando SQL.</param>
        /// <param name="connection">Conexión a usar.</param>
        /// <param name="transaction">Transacción a usar.</param>
        /// <returns>Un nuevo comando para la conexión y transacción especificada.</returns>
        public abstract IDbCommand GetNewCommand(string cmdText, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// Crea una instancia de DataParameter adecuada para el proveedor actual.
        /// </summary>
        /// <returns>Una nueva instancia de DbDataParameter.</returns>
        public abstract IDbDataParameter GetNewDataParameter();

        /// <summary>
        /// Crea una nueva instancia de DataParameter adecuada para el proveedor.
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro.</param>
        /// <param name="dbType">Tipo del parámetro.</param>
        /// <returns>Una nueva instancia de DataParameter.</returns>
        public abstract IDbDataParameter GetNewDataParameter(string parameterName, DbType dbType);

        #endregion
    }
}
