using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using UtnEmall.Server.Base;
using System.Globalization;

namespace UtnEmall.Server.DataModel
{
    /// <summary>
    /// Encapsula un singleton para manejar la conexión a la base de datos
    /// </summary>
    public class DataAccessConnection
    {
        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        private static DataAccessConnection singleton;

        private const string dataAccessTag = "DATAACCESS";
        private const string sourceTag = "SOURCE";
        private const string catalogTag = "CATALOG";
        private const string assemblyDllTag = "ASSEMBLYDLL";
        private const string classNameTag = "CLASSNAME";

        #endregion

        #region Instance Variables and Properties

        private string connectionString;
        private DataAccessObjectFactory dataAccessObjectFactory;

        string path = "DataAccess.xml";
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        string source = String.Empty;
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        string catalog = String.Empty;
        public string Catalog
        {
            get { return catalog; }
            set { catalog = value; }
        }

        string assemblyDll = String.Empty;
        public string AssemblyDll
        {
            get { return assemblyDll; }
            set { assemblyDll = value; }
        }

        string className = String.Empty;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

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
        /// Retorna un DbType adecuado para el tipo .NET proporcionado.
        /// </summary>
        /// <param name="parameterType">Un tipo de datos .NET.</param>
        /// <returns>Un tipo DbType asociado al tipo .NET.</returns>
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
        /// Retorna la instancia singleton para manipular la conexión a la base de datos.
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
        /// Retorna el proximo Id para una tabla.
        /// </summary>
        /// <param name="nameId">Nombre del campo clave de la tabla.</param>
        /// <param name="tableName">Nombre de la tabla.</param>
        /// <param name="connection">La conexión con la base de datos.</param>
        /// <param name="transaction">La transacción a usar.</param>
        /// <returns>Un entero indicando el próximo Id a usar.</returns>
        public static int GetNextId(string nameId, string tableName, IDbConnection connection, IDbTransaction transaction)
        {
            DataAccessConnection dataAccess = DataAccessConnection.Instance;

            string sqlCommand = "NextID" + tableName;
            IDbCommand command = dataAccess.GetNewCommand(sqlCommand, connection, transaction);
            command.CommandType = CommandType.StoredProcedure;
            IDbDataParameter parameterId = dataAccess.GetNewDataParameter("@" + nameId, DbType.Int32);
            parameterId.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterId);
            command.ExecuteScalar();
            int nextId = (int)parameterId.Value;
            return nextId;

        }

        /// <summary>
        /// Retorna una cadena representando el operador especificado.
        /// </summary>
        /// <param name="operatorType">El operador como valor de enumeración.</param>
        /// <returns>El operador equivalente para usar en una instrucción SQL.</returns>
        public static string GetOperatorString(OperatorType operatorType)
        {
            string op = "";
            switch (operatorType)
            {
                case UtnEmall.Server.Base.OperatorType.Equal:
                    op = "=";
                    break;
                case UtnEmall.Server.Base.OperatorType.NotEqual:
                    op = "<>";
                    break;
                case UtnEmall.Server.Base.OperatorType.Less:
                    op = "<";
                    break;
                case UtnEmall.Server.Base.OperatorType.Greater:
                    op = ">";
                    break;
                case UtnEmall.Server.Base.OperatorType.LessOrEqual:
                    op = "<=";
                    break;
                case UtnEmall.Server.Base.OperatorType.GreaterOrEqual:
                    op = ">=";
                    break;
                case UtnEmall.Server.Base.OperatorType.Like:
                    op = "like";
                    break;
                case UtnEmall.Server.Base.OperatorType.NotLike:
                    op = "not like";
                    break;
                default:
                    throw new ArgumentException("Unhandled operator type.", "operatorType");
            }
            return op;
        }

        /// <summary>
        /// Controla si los artefactos de necesarios para una tabla existen. 
        /// Crea la tabla, los indices y los procedimientos si es necesario.
        /// </summary>
        /// <param name="tableName">Tabla a controlar en la base de datos.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool DBCheckedTable(string tableName)
        {
            bool result = false;

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Crear el comando para controlar la existencia de la tabla
            string cmdText = "SELECT name FROM sysObjects WHERE name = @tablename AND type = 'U'";
            IDbCommand command = dataAccess.GetNewCommand(cmdText, dbConnection);
            IDbDataParameter param = dataAccess.GetNewDataParameter();
            param.ParameterName = "@tablename";
            param.DbType = DbType.AnsiString;
            param.Size = tableName.Length;
            param.Value = tableName;
            command.Parameters.Add(param);

            // Controlar la existencia de la tabla y cerrar la conexión
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
        /// Controla que el procedimiento exista en la base actual.
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool DBCheckedStoredProcedure(string procedureName)
        {
            bool result = false;

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Construir el comando para controlar la existencia del procedimiento
            string cmdText = "SELECT name FROM sysObjects WHERE name = @procedureName AND type = 'P'";
            IDbCommand command = dataAccess.GetNewCommand(cmdText, dbConnection);
            IDbDataParameter param = dataAccess.GetNewDataParameter();
            param.ParameterName = "@procedureName";
            param.DbType = DbType.AnsiString;
            param.Size = procedureName.Length;
            param.Value = procedureName;
            command.Parameters.Add(param);

            // Controlar la existencia de la tabla y cerrar la conexión
            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                result = true;
            }

            reader.Close();
            dbConnection.Close();

            return result;
        }

        /// <summary>
        /// Crea un procedimiento de eliminación para una tabla determinada
        /// </summary>
        /// <param name="tableName">Nombre de la tabla.</param>
        /// <param name="idColumnName">Nombre de la columna clave.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool CreateDeleteStoredProcedure(string tableName, string idColumnName)
        {
            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Crear el comando para crear el procedimiento
            string procedure = "CREATE PROCEDURE Delete" + tableName + "( @" + idColumnName + " int) AS DELETE FROM [" + tableName + "] WHERE " + idColumnName + " = @" + idColumnName;
            IDbCommand command = dataAccess.GetNewCommand(procedure, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Crea el procedimiento "CreateNextId" para la tabla especificada.
        /// </summary>
        /// <param name="tableName">Nombre de la tabla.</param>
        /// <param name="idColumnName">Nombre de la columna clave.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool CreateNextIdStoredProcedure(string tableName, string idColumnName)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentException("Must provide a name for the table.", "tableName");

            // Crear el comando para crear el procedimiento
            string cmdText = "CREATE PROCEDURE NextID" + tableName + "( @" + idColumnName + " int OUT)" +
                                "AS IF (SELECT COUNT(*) FROM [" + tableName + "]) = 0 " +
                                "BEGIN " +
                                "    SET @" + idColumnName + " = 1 " +
                                "END " +
                                "ELSE " +
                                "BEGIN " +
                                "    SET @" + idColumnName + " = (SELECT (MAX(" + idColumnName + ")+1) FROM [" + tableName + "]) " +
                                "END";

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Ejecutar el procedimiento y cerrar la conexión
            IDbCommand command = dataAccess.GetNewCommand(cmdText, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Crea un procedimiento para guardar un registro.
        /// No usa clave autonumerica.
        /// </summary>
        /// <param name="tableName">Nombre de la tabla.</param>
        /// <param name="fieldsName">Nombre de los campos.</param>
        /// <param name="fieldsType">Tipos .NET de los campos, en el mismo orden que los nombres.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool CreateSaveStoredProcedureNonAutonumeric(string tableName, string[] fieldsName, Type[] fieldsType)
        {
            if (fieldsName.Length == 0)
            {
                throw new ArgumentException("Must provide the table's field names.", "fieldsName");
            }
            if (fieldsType.Length == 0)
            {
                throw new ArgumentException("Must provide the table's field types.", "fieldsType");
            }

            // Crea el comando SQL para insertar un registro en la tabla
            string cmdText = "INSERT INTO [" + tableName + "] VALUES(";
            for (int i = 0; i < fieldsName.Length; i++)
            {
                if (i == 0)
                {
                    cmdText += "@" + fieldsName[i];
                }
                else
                {
                    cmdText += ",@" + fieldsName[i];
                }
            }
            cmdText += ", GETDATE())";

            // Crear el comando para crear el procedimiento de inserción
            string procedure = "CREATE PROCEDURE Save" + tableName + "(";
            for (int i = 0; i < fieldsName.Length; i++)
            {
                if (i == 0)
                {
                    procedure += "@" + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
                }
                else
                {
                    procedure += ",@" + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
                }
            }
            procedure += ") AS " + cmdText;

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Ejecutar el procedimiento y cerrar la conexión
            IDbCommand command = dataAccess.GetNewCommand(procedure, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Crea un procedimiento de grabación para la tabla indicada usando clave autonumerica.
        /// </summary>
        /// <param name="tableName">Nombre de la tabla.</param>
        /// <param name="fieldsName">Nombres de los campos de la tabla.</param>
        /// <param name="fieldsType">Tipos .NET de los campos en el mismo orden que los nombres.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool CreateSaveStoredProcedure(string tableName, string[] fieldsName, Type[] fieldsType)
        {
            if (fieldsName.Length == 0)
            {
                throw new ArgumentException("Must provide the table's field names.", "fieldsName");
            }
            if (fieldsType.Length == 0)
            {
                throw new ArgumentException("Must provide the table's field types.", "fieldsType");
            }

            // Crea el comando SQL para insertar un registro en la tabla
            string cmdText = "INSERT INTO [" + tableName + "] VALUES(";
            for (int i = 1; i < fieldsName.Length; i++)
            {
                if (i == 1)
                {
                    cmdText += "@" + fieldsName[i];
                }
                else
                {
                    cmdText += ",@" + fieldsName[i];
                }
            }
            cmdText += ", GETDATE())";

            // Crear el comando para crear el procedimiento de inserción
            string procedure = "CREATE PROCEDURE Save" + tableName + "(";
            for (int i = 1; i < fieldsName.Length; i++)
            {
                if (i == 1)
                {
                    procedure += "@" + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
                }
                else
                {
                    procedure += ",@" + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
                }
            }
            procedure += ") AS " + cmdText;
            procedure += " RETURN SCOPE_IDENTITY()";

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Ejecutar el procedimiento y cerrar la conexión
            IDbCommand command = dataAccess.GetNewCommand(procedure, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Crea un procedimiento de actualización para la tabla indicada.
        /// </summary>
        /// <param name="tableName">Nombre de la tabla.</param>
        /// <param name="idColumnName">The name of the id column</param>
        /// <param name="fieldsName">Nombres de los campos de la tabla.</param>
        /// <param name="fieldsType">Tipos .NET de los campos en el mismo orden que los nombres.</param>
        /// <returns>True if successful</returns>
        public static bool CreateUpdateStoredProcedure(string tableName, string idColumnName, string[] fieldsName, Type[] fieldsType)
        {
            if (fieldsName.Length == 0)
            {
                throw new ArgumentException("Must provide the table's field names.", "fieldsName");
            }
            if (fieldsType.Length == 0)
            {
                throw new ArgumentException("Must provide the table's field types.", "fieldsType");
            }

            // Crea el comando para actualizar un registro
            string cmdText = "UPDATE [" + tableName + "] SET ";
            for (int i = 1; i < fieldsName.Length; i++)
            {
                if (i == 1)
                {
                    cmdText += fieldsName[i] + " = @" + fieldsName[i];
                }
                else
                {
                    cmdText += ", [" + fieldsName[i] + "] = @" + fieldsName[i];
                }
            }
            cmdText += " , timestamp=GETDATE() WHERE [" + idColumnName + "] = @" + idColumnName;

            // Crear el comando para crear el procedimiento de actualización
            string procedure = "CREATE PROCEDURE Update" + tableName + "(";
            for (int i = 0; i < fieldsName.Length; i++)
            {
                if (i == 0)
                {
                    procedure += "@" + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
                }
                else
                {
                    procedure += ",@" + fieldsName[i] + " " + GetSQLTypeName(fieldsType[i]);
                }

            }
            procedure += ")AS " + cmdText;

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Ejecutar el procedimiento y cerrar la conexión
            IDbCommand command = dataAccess.GetNewCommand(procedure, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Crea una nueva tabla sin indices de clave foranea
        /// </summary>
        /// <param name="table">El nombre de la tabla a crear.</param>
        /// <param name="fieldsName">Los nombres de los campos.</param>
        /// <param name="fieldsType">Los tipos de los campos.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool CreateTable(string table, string[] fieldsName, bool isIdIdentity, Type[] fieldsType)
        {
            return CreateTable(table, fieldsName, isIdIdentity, fieldsType, null);
        }

        /// <summary>
        /// Crea una nueva tabla con indices de clave foranea
        /// </summary>
        /// <param name="table">El nombre de la tabla a crear.</param>
        /// <param name="fieldsName">Los nombres de los campos.</param>
        /// <param name="fieldsType">Los tipos de los campos.</param>
        /// <param name="indexColumns">Los nombres de los campos a indizar.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool CreateTable(string table, string[] fieldsName, bool isIdIdentity, Type[] fieldsType, string[] indexColumns)
        {
            if (String.IsNullOrEmpty(table))
            {
                throw new ArgumentException("Must provide a name for the table.", "table");
            }
            if (fieldsName.Length == 0)
            {
                throw new ArgumentException("Must provide at least one field name.", "table");
            }
            if (fieldsType.Length == 0)
            {
                throw new ArgumentException("Must provide at least one field type.", "fieldsType");
            }
            if (fieldsName.Length != fieldsType.Length)
            {
                throw new ArgumentException("Quantity oif fields names and types must be equal.", "fieldsType");
            }

            // Agrega el campo timestamp
            string[] fields = new string[fieldsName.Length + 1];
            fieldsName.CopyTo(fields, 0);
            fields[fields.Length - 1] = "timestamp";
            fieldsName = fields;

            // Agrega el tipo del campo timestamp
            Type[] fieldsNewTypes = new Type[fieldsType.Length + 1];
            fieldsType.CopyTo(fieldsNewTypes, 0);
            fieldsNewTypes[fieldsNewTypes.Length - 1] = Type.GetType("System.DateTime");
            fieldsType = fieldsNewTypes;

            // Crear el comando SQL para la creación de la tabla
            string cmdText = "CREATE TABLE [" + table + "] (";
            cmdText += "" + fieldsName[0] + " " + GetSQLTypeName(fieldsType[0]) + " PRIMARY KEY";
            if (isIdIdentity)
            {
                cmdText += " IDENTITY";
            }
            for (int i = 1; i < fieldsName.Length; i++)
            {
                cmdText += ",[" + fieldsName[i] + "] " + GetSQLTypeName(fieldsType[i]);
            }
            cmdText += ")";

            // Conectarse a la base de datos
            DataAccessConnection dataAccess = DataAccessConnection.Instance;
            IDbConnection dbConnection = dataAccess.GetNewConnection();
            dbConnection.Open();

            // Ejecutar el comando
            IDbCommand command = dataAccess.GetNewCommand(cmdText, dbConnection);
            command.ExecuteNonQuery();

            // Crear los índices
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

            // Cerrar la conexión
            dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Controla la existencia de una entidad en base a su Id y nombre de Tabla.
        /// </summary>
        /// <param name="entityId">El valor del campo clave.</param>
        /// <param name="tableName">El nombre de la tabla.</param>
        /// <param name="connection">La conexión a la base de datos.</param>
        /// <param name="transaction">La transacción, si existe.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool ExistsEntity(int entityId, string tableName, IDbConnection connection, IDbTransaction transaction)
        {
            return ExistsEntity(entityId, tableName, "id" + tableName, connection, transaction);
        }

        /// <summary>
        /// Controla la existencia de una entidad en base a su Id y nombre de Tabla.
        /// </summary>
        /// <param name="entityId">El valor del campo clave.</param>
        /// <param name="tableName">El nombre de la tabla.</param>
        /// <param name="fieldName">El nombre del campo.</param>
        /// <param name="connection">La conexión a la base de datos.</param>
        /// <param name="transaction">La transacción, si existe.</param>
        /// <returns>True si tiene éxito.</returns>
        public static bool ExistsEntity(int entityId, string tableName, string fieldName, IDbConnection connection, IDbTransaction transaction)
        {
            bool result = false;

            DataAccessConnection dataAccess = DataAccessConnection.Instance;


            string cmdText = "SELECT " + fieldName + "  FROM [" + tableName + "] WHERE " + fieldName + " = @identity";
            IDbCommand command = dataAccess.GetNewCommand(cmdText, connection);
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
        /// Traduce un tipo .NET a su equivalente SQL.
        /// </summary>
        /// <param name="netType">Un tipo .NET.</param>
        /// <returns>Una cadena que representa el tipo SQL.</returns>
        private static string GetSQLTypeName(Type netType)
        {
            if (netType == null)
            {
                throw new ArgumentException("Must provide an instance of Type.", "netType");
            }

            switch (netType.FullName)
            {
                case "System.Int32":
                    return "Int";
                case "System.String":
                    return "VarChar(255)";
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
                    throw new ArgumentException("Incorrect Type", "netType");
            }
        }

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Crea una nueva conección para el proveedor actual.
        /// </summary>
        /// <returns>Una nueva conexión.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate")]
        public IDbConnection GetNewConnection()
        {
            return dataAccessObjectFactory.GetNewConnection();
        }

        /// <summary>
        /// Crea un nuevo DataAdapter para el proveedor actual.
        /// </summary>
        /// <returns>Un nuevo DataAdapter.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IDataAdapter GetNewDataAdapter()
        {
            return dataAccessObjectFactory.GetNewDataAdapter();
        }

        /// <summary>
        /// Crea un nuevo comando para el proveedor actual.
        /// </summary>
        /// <param name="cmdText">El comando SQL.</param>
        /// <param name="connection">La conexión a utilizar.</param>
        /// <returns>Un nuevo comando usando la conexión indicada.</returns>
        public IDbCommand GetNewCommand(string cmdText, IDbConnection connection)
        {
            return dataAccessObjectFactory.GetNewCommand(cmdText, connection);
        }

        /// <summary>
        /// Crea un nuevo comando para el proveedor actual.
        /// </summary>
        /// <param name="cmdText">El comando SQL.</param>
        /// <param name="connection">La conexión a usar para el comando.</param>
        /// <param name="transaction">La transacción a usar para el comando.</param>
        /// <returns>Un nuevo comando.</returns>
        public IDbCommand GetNewCommand(string cmdText, IDbConnection connection, IDbTransaction transaction)
        {
            return dataAccessObjectFactory.GetNewCommand(cmdText, connection, transaction);
        }

        /// <summary>
        /// Crea un nuevo DataParameter para el proveedor actual.
        /// </summary>
        /// <returns>Una nueva instancia de DbDataParameter.</returns>
        public IDbDataParameter GetNewDataParameter()
        {
            return dataAccessObjectFactory.GetNewDataParameter();
        }

        /// <summary>
        /// Crea un nuevo DataParameter para el proveedor actual.
        /// </summary>
        /// <param name="parameterName">Un nombre para el parámetro.</param>
        /// <param name="dbType">El tipo del parámetro.</param>
        /// <returns>Una nueva instancia de DbDataParameter.</returns>
        public IDbDataParameter GetNewDataParameter(string parameterName, DbType dbType)
        {
            return dataAccessObjectFactory.GetNewDataParameter(parameterName, dbType);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Carga la configuración de base de datos desde el archivo de configuración.
        /// </summary>
        private void ReadConfigFile()
        {
            dataAccessObjectFactory = null;

            if (File.Exists(Path))
            {
                XmlReader reader = null;

                try
                {
                    // Abrir y leer el archivo XML
                    reader = new XmlTextReader(Path);

                    while (reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) && !reader.IsEmptyElement)
                        {
                            switch (reader.LocalName.ToUpperInvariant())
                            {
                                case sourceTag:
                                    Source = reader.ReadString();
                                    break;
                                case catalogTag:
                                    Catalog = reader.ReadString();
                                    break;
                                case assemblyDllTag:
                                    AssemblyDll = reader.ReadString();
                                    break;
                                case classNameTag:
                                    ClassName = reader.ReadString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                catch (XmlException error)
                {
                    throw new UtnEmallDataAccessException("Error reading configuration file.", error);
                }
                finally
                {
                    // Cerrar la conexión con el archivo
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

                if (!String.IsNullOrEmpty(AssemblyDll))
                {
                    // Cargar el assembly indicado
                    Assembly dll = Assembly.Load(AssemblyDll);
                    dataAccessObjectFactory = (DataAccessObjectFactory)dll.CreateInstance(ClassName);
                }                
            }

            if (String.IsNullOrEmpty(Source))
            {
                throw new UtnEmallDataAccessException("Must provide the SQL Server name instance.");
            }
            else
            {
                connectionString = SetConnectionString();
            }

            // Usar el proveedor por defecto
            if (dataAccessObjectFactory == null)
            {
                dataAccessObjectFactory = new SqlDataAccessProvider(connectionString);
            }
        }
        
        /// <summary>
        /// Crea la cadena de conexión dependiendo de si se debe adjuntar una base de datos o no.
        /// </summary>
        /// <returns>Cadena de conexión</returns>
        private string SetConnectionString()
        {
            if (Catalog.EndsWith(".mdf", StringComparison.OrdinalIgnoreCase) && File.Exists(Catalog))
            {
                return "Data Source=" + Source + "; AttachDBFilename=" + System.IO.Path.GetFullPath(Catalog) + "; Database=" + System.IO.Path.GetFileNameWithoutExtension(Catalog) + "; Integrated Security=True; Min Pool Size=5;Max Pool Size=100;";
            }
            else
            {
                return "Data Source=" + Source + "; Database=" + Catalog + "; Integrated Security=SSPI; Min Pool Size=5;Max Pool Size=100;";
            }           
        }

        #endregion

        #endregion
    }
}
