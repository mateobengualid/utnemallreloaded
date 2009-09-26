#region Usage Declarations

using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using UtnEmall.Client.PresentationLayer;
using System.Xml;


#endregion

namespace UtnEmall.Client.DataModel
{
    /// <summary>
    /// Encapsula la conexión con la base de datos usando un singleton
    /// </summary>
    public class DataAccessConnection
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        private static DataAccessConnection singleton;

        #endregion

        #region Instance Variables and Properties

        private string connectionString;
        private DataAccessObjectFactory dataAccessObjectFactory;

        #endregion

        #endregion

        #region Constructors

        private DataAccessConnection()
        {
            ReadConfigFile();
        }

        #endregion

        #region Static Methods

        #region Public Static Methods
        /// <summary>
        /// Devuelve un operador en formato string para usar en SQL.
        /// </summary>
        /// <param name="operatorType">El operador.</param>
        /// <returns>El operador equivalente para usar en SQL.</returns>
        public static string GetOperatorString(OperatorType operatorType)
        {
            string op = "";
            switch (operatorType)
            {
                case OperatorType.Equal:
                    op = "=";
                    break;
                case OperatorType.NotEqual:
                    op = "<>";
                    break;
                case OperatorType.Less:
                    op = "<";
                    break;
                case OperatorType.Greater:
                    op = ">";
                    break;
                case OperatorType.LessOrEqual:
                    op = "<=";
                    break;
                case OperatorType.GreaterOrEqual:
                    op = ">=";
                    break;
                case OperatorType.Like:
                    op = "like";
                    break;
                case OperatorType.NotLike:
                    op = "not like";
                    break;
                default:
                    throw new ArgumentException("Unhandled operator type.", "operatorType");
            }
            return op;
        }
        /// <summary>
        /// Devuelve un tipo DbType adecuado para usar en SQL server de acuerdo al tipo .NET proporcionado.
        /// </summary>
        /// <param name="parameterType">Tipo de datos .NET</param>
        /// <returns>DbType asociado con el tipo .NET proporcionado.</returns>
        public static DbType GetParameterDBType(Type parameterType)
        {
            switch (parameterType.FullName)
            {
                case "System.Int32":
                    return System.Data.DbType.Int32;
                case "System.String":
                    return System.Data.DbType.String;
                case "System.Boolean":
                    return System.Data.DbType.Boolean;
                case "System.Single":
                    return System.Data.DbType.Decimal;
                case "System.Double":
                    return System.Data.DbType.Decimal;
                case "System.DateTime":
                    return System.Data.DbType.DateTime;
                default:
                    throw new System.ArgumentException("Incorrect Type", "parameterType");
            }
        }

        /// <summary>
        /// Devuelve la instancia del controlador de base de datos
        /// </summary>
        public static DataAccessConnection Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new DataAccessConnection();
                }

                return singleton;
            }
        }

        /// <summary>
        /// Controla que los artefactos de base de datos para una tabla determinada existan, sino existen los crea.
        /// </summary>
        /// <param name="tableName">Tabla a controlar</param>
        /// <returns>True si es verdadero</returns>
        public static bool DBCheckedTable(string tableName)
        {
            bool result = false;

            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            SqlCeConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            string cmdText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tablename";
            SqlCeCommand command = dataAccess.GetNewCommand(cmdText, dbConnection, null);
            IDbDataParameter param = dataAccess.GetNewDataParameter();
            param.ParameterName = "@tablename";
            param.DbType = DbType.String;
            param.Size = tableName.Length;
            param.Value = tableName;
            command.Parameters.Add(param);

            IDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                result = true;
            }
            dataReader.Close();
            dbConnection.Close();

            return result;
        }


        /// <summary>
        /// Crea una nueva tabla sin indices para claves foraneas
        /// </summary>
        /// <param name="table">Nombre de la tabla a crear</param>
        /// <param name="fieldsName">Nombres de los campos</param>
        /// <param name="fieldsType">Tipos de los campos, en el mismo orden que los nombres de campo</param>
        /// <returns>True si tiene éxito</returns>
        public static bool CreateTable(string table, string[] fieldsName, bool isIdIdentity, Type[] fieldsType)
        {
            return CreateTable(table, fieldsName, isIdIdentity, fieldsType, null);
        }

        /// <summary>
        /// Crea una nueva tabla con indices para claves foraneas
        /// </summary>
        /// <param name="table">Nombre de la tabla a crear</param>
        /// <param name="fieldsName">Nombres de los campos</param>
        /// <param name="fieldsType">Tipos de los campos, en el mismo orden que los nombres de campo</param>
        /// <param name="indexColumns">columnas a indexar</param>
        /// <returns>True si tiene éxito</returns>
        public static bool CreateTable(string table, string[] fieldsName, bool isIdIdentity, Type[] fieldsType, string[] indexColumns)
        {
            if (fieldsName.Length == 0)
                throw new ArgumentException("");
            if (fieldsType.Length == 0)
                throw new ArgumentException("");

            // Agrega el campo de timestamp
            string[] fields = new string[fieldsName.Length + 1];
            fieldsName.CopyTo(fields, 0);
            fields[fields.Length - 1] = "timestamp";
            fieldsName = fields;

            Type[] fieldsNewTypes = new Type[fieldsType.Length + 1];
            fieldsType.CopyTo(fieldsNewTypes, 0);
            fieldsNewTypes[fieldsNewTypes.Length - 1] = Type.GetType("System.DateTime");
            fieldsType = fieldsNewTypes;

            string cmdText = "CREATE TABLE [" + table + "] (";
            cmdText += "" + fieldsName[0] + " " + GetSQLTypeName(fieldsType[0]) + " PRIMARY KEY";
            if (isIdIdentity)
            {
                cmdText += " IDENTITY";
            }
            for (int i = 1; i < fieldsName.Length; i++)
            {
                cmdText += "," + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
            }
            cmdText += ")";

            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            SqlCeConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            SqlCeCommand command = dataAccess.GetNewCommand(cmdText, dbConnection, null);
            command.ExecuteNonQuery();

            // Crea los indices
            if (indexColumns != null && indexColumns.Length > 0)
            {
                string cmdIndex;
                for (int i = 0; i < indexColumns.Length; i++)
                {
                    cmdIndex = "CREATE INDEX IDX_" + table + "_" + indexColumns[i] +
                                    " ON [" + table + "] (" + indexColumns[i] + ");";
                    command.CommandText = cmdIndex;
                    command.ExecuteNonQuery();
                }
            }

            dbConnection.Close();

            return true;
        }

        public static bool ExistsEntity(int entityId, string tableName, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            return ExistsEntity(entityId, tableName, "id" + tableName, connection, transaction);
        }

        /// <summary>
        /// Obtiene el siguiente Id para una tabla
        /// </summary>
        /// <param name="nameId">Nombre de la clave primaria</param>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="connection">conexión a usar</param>
        /// <param name="transaction">transacción a usar</param>
        /// <returns>El siguiente id</returns>
        public static int GetNextId(string nameId, string tableName, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            DataAccessConnection dataAccess = DataAccessConnection.Instance;

            string cmdText = "SELECT (MAX(" + nameId + ")+1) FROM [" + tableName + "]";
            SqlCeCommand command = dataAccess.GetNewCommand(cmdText, connection, transaction);
            
            try
		    {
                object temp = command.ExecuteScalar();
                if (temp is int)
                {
                    return (int)temp;
                }
                else
                {
                    return 1;
                }
		    }
		    catch(InvalidCastException)
		    {
			    return 1;
		    }
 
        }

        /// <summary>
        /// Controla si existe un registro en la base de datos.
        /// </summary>
        /// <param name="entityId">El id de la entidad</param>
        /// <param name="tableName">El nombre de la tabla</param>
        /// <param name="fieldName">El nombre del campo</param>
        /// <param name="connection">conexión a usar</param>
        /// <param name="transaction">transacción a usar</param>
        /// <returns>True si ya existe</returns>
        public static bool ExistsEntity(int entityId, string tableName, string fieldName, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            bool result = false;
            DataAccessConnection dataAccess = DataAccessConnection.Instance;

            string cmdText = "SELECT " + fieldName + "  FROM [" + tableName + "] WHERE " + fieldName + " = @identity";
            SqlCeCommand command = dataAccess.GetNewCommand(cmdText);
            command.Connection = connection;
            command.Transaction = transaction;
            IDbDataParameter param = dataAccess.GetNewDataParameter();
            param.ParameterName = "@identity";
            param.DbType = DbType.Int32;
            param.Value = entityId;
            command.Parameters.Add(param);

            IDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                result = true;
            }
            dataReader.Close();

            return result;
            
            
        }

        #endregion

        #region Protected and Private Static Methods

        /// <summary>
        /// Recibe un tipo .NET y retorna el tipo equivalente SQL
        /// </summary>
        /// <param name="netType">Un tipo .NET</param>
        /// <returns>El tipo equivalente en SQL</returns>
        private static string GetSQLTypeName(Type netType)
        {
            switch (netType.FullName)
            {
                case "System.Int32":
                    return "Int";
                case "System.String":
                    return "NVarChar(255)";
                case "System.Boolean":
                    return "Bit";
                case "System.DateTime":
                    return "DateTime";
                case "System.Single":
                    return "dec(18,5)";
                case "System.Double":
                    return "dec(18,5)";
                case "System.Drawing.Image":
                    return "image";
                default:
                    throw new ArgumentException("Incorrect Type");
            }
        }

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Crea una nueva conexión para el proveedor de datos actual
        /// </summary>
        /// <returns>Una nueva conexión</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public SqlCeConnection GetNewConnection()
        {
            return dataAccessObjectFactory.GetNewConnection();
        }

        /// <summary>
        /// Crea un nuevo DataAdapter 
        /// </summary>
        /// <returns>Un nuevo IDataAdapter</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IDataAdapter GetNewDataAdapter()
        {
            return dataAccessObjectFactory.GetNewDataAdapter();
        }

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <returns>Un nuevo comando</returns>
        public SqlCeCommand GetNewCommand(string cmdText)
        {
            return dataAccessObjectFactory.GetNewCommand(cmdText);
        }

        /// <summary>
        /// Crea un nuevo Comando para el proveedor actual
        /// </summary>
        /// <param name="cmdText">Cadena con el comando SQL</param>
        /// <param name="connection">Conexión a usar</param>
        /// <param name="transaction">Transacción a usar</param>
        /// <returns>Un nuevo comando</returns>
        public SqlCeCommand GetNewCommand(string cmdText, SqlCeConnection connection, SqlCeTransaction transaction)
        {
            return dataAccessObjectFactory.GetNewCommand(cmdText, connection, transaction);
        }

        /// <summary>
        /// Crea un nuevo Parameter
        /// </summary>
        /// <returns>Un nuevo SqlCeDataParameter</returns>
        public SqlCeParameter GetNewDataParameter()
        {
            return dataAccessObjectFactory.GetNewDataParameter();
        }

        /// <summary>
        /// Crea un nuevo Parameter
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro.</param>
        /// <param name="dbType">Tipo del parámetro.</param>
        /// <returns>Una nueva instancia de SqlCeParameter</returns>
        public SqlCeParameter GetNewDataParameter(string parameterName, DbType dbType)
        {
            return dataAccessObjectFactory.GetNewDataParameter(parameterName, dbType);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Carga la configuración
        /// </summary>
        private void ReadConfigFile()
        {
            string path = Path.Combine(Utilities.AppPath, "DataAccess.xml");
            dataAccessObjectFactory = null;

            if (File.Exists(path))
            {
                try
                {
                    // leer el archivo de configuración
                    StreamReader file = new StreamReader(path);

                    XmlReader reader = new XmlTextReader(file);
                    reader.ReadStartElement("dataaccess");
                    reader.ReadStartElement("source");
                    string source = Utilities.AppPath + reader.ReadString();
                    reader.ReadEndElement();

                    reader.Close();
                    
                    connectionString = "Data Source=" + source;

                    // Controlar si la base estaba creada
                    if (!File.Exists(source))
                    {
                        // Crear la base de datos
                        SqlCeEngine engine = new SqlCeEngine(connectionString);
                        engine.CreateDatabase();
                        engine.Dispose();
                    }

                    // Establecer la conexión
                    dataAccessObjectFactory = new SqlDataAccessProvider(connectionString);

                }
                catch (XmlException error)
                {
                    throw new UtnEmallDataAccessException("Error reading configuration file.", error);
                }
            }

        }

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }
}
