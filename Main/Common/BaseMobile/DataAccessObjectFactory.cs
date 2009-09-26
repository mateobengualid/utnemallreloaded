#region Usage Declarations

using System;
using System.Data;
using System.Data.SqlServerCe;

#endregion

namespace UtnEmall.Client.DataModel
{
    /// <summary>
    /// Clase abstracta para proveedores de datos
    /// </summary>
    abstract class DataAccessObjectFactory
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
        }
        #endregion

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

        #region Static Methods

        #region Public Static Methods

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Crea una nueva conexión para el proveedor de datos actual
        /// </summary>
        /// <returns>Una nueva conexión</returns>
        public abstract SqlCeConnection GetNewConnection();

        /// <summary>
        /// Crea un nuevo DataAdapter 
        /// </summary>
        /// <returns>Un nuevo IDataAdapter</returns>
        public abstract IDataAdapter GetNewDataAdapter();
        
        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <returns>Un nuevo comando</returns>
        public abstract SqlCeCommand GetNewCommand(string cmdText);

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <param name="connection">Conexión a usar</param>
        /// <returns>Un nuevo comando</returns>
        public abstract SqlCeCommand GetNewCommand(string cmdText, SqlCeConnection connection);

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <param name="connection">Conexión a usar</param>
        /// <param name="transaction">Transacción a usar</param>
        /// <returns>Un nuevo comando</returns>
        public abstract SqlCeCommand GetNewCommand(string cmdText, SqlCeConnection connection, SqlCeTransaction transaction);

        /// <summary>
        /// Crea un nuevo Parameter
        /// </summary>
        /// <returns>Una nueva instancia de SqlCeParameter</returns>
        public abstract SqlCeParameter GetNewDataParameter();

        /// <summary>
        /// Crea un nuevo Parameter
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro.</param>
        /// <param name="dbType">Tipo del parámetro.</param>
        /// <returns>Una nueva instancia de SqlCeParameter</returns>
        public abstract SqlCeParameter GetNewDataParameter(string parameterName, DbType dbType);

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
