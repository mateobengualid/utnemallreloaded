using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.ServiceModel;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.DataModel;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Interfase de servicio para establecer una asocicación entre registros de modelos de datos y categorías.
    /// </summary>
    [ServiceContract]
    public interface ICategoryAssociation
    {
        /// <summary>
        /// Registra una asociación entre registros de tablas de modelos de datos y categorías.
        /// </summary>
        /// <param name="associations">Colección de RegisterAssociation que vincula registros con categorías.</param>
        /// <param name="sessionId">El identificador de sesión.</param>
        /// <returns>True si la asiciación se realizó con éxito.</returns>
        [OperationContract]
        bool SaveAssociations(Collection<RegisterAssociationData> associations, string sessionId);
        /// <summary>
        /// Obtiene una colección de asociaciones entre registros de tablas de modelos de datos y categorías para una tienda determinada.
        /// </summary>
        /// <param name="tableName">El nombre de la tabla del modelo de datos.</param>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>Una colección de registros de asociaciones.</returns>
        [OperationContract]
        Collection<RegisterAssociationData> GetListOfAssociations(string tableName, string sessionId);
        /// <summary>
        /// Elimina una asociación entre registros de tablas de modelos de datos y categorías.
        /// </summary>
        /// <param name="associations">Listado de asociaciones.</param>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>True si la eliminación se realizó con éxito.</returns>
        [OperationContract]
        bool RemoveAssociations(Collection<RegisterAssociationData> associations, string sessionId);
    }

    /// <summary>
    /// Implementación de servicio para la asociación entre registros de modelos de datos y categorías.
    /// </summary>
    public class CategoryAssociation : ICategoryAssociation
    {
        #region Instance Methods

        /// <summary>
        /// Registra una asociación entre registros de tablas de modelos de datos y categorías.
        /// </summary>
        /// <param name="associations">Listado de asociaciones.</param>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>True si todas las asociaciones se guardaron exitosamente.</returns>
        public bool SaveAssociations(Collection<RegisterAssociationData> associations, string sessionId)
        {
            if (associations == null)
            {
                throw new ArgumentNullException("associations", "Associations must have at least one RegisterAssociation.");
            }
            if (sessionId == null)
            {
                throw new ArgumentNullException("sessionId", "SessionId must not be null.");
            }
            if (associations.Count == 0)
            {
                throw new ArgumentException("Associations list must have at least one RegisterAssociation.", "associations");
            }

            RegisterAssociationEntity RAEntity;
            RegisterAssociationCategoriesEntity RACEntity;
            RegisterAssociation RAClient = new UtnEmall.Server.BusinessLogic.RegisterAssociation();
            bool result = true;

            // Retorna la tienda del usuario actual
            StoreEntity userStore = SessionManager.Instance.StoreFromUserSession(sessionId);

            // Recorre el listado de asociaciones
            foreach (RegisterAssociationData registerAssociation in associations)
            {
                TableEntity table = ObtainTableEntity(registerAssociation.TableName, userStore, sessionId);
                RAEntity = new RegisterAssociationEntity();

                RAEntity.Table = table;
                RAEntity.IdRegister = registerAssociation.RegisterId;
                RAEntity.RegisterAssociationCategories = new Collection<RegisterAssociationCategoriesEntity>();

                foreach (int categoryId in registerAssociation.CategoriesId)
                {
                    RACEntity = new RegisterAssociationCategoriesEntity();
                    RACEntity.IdCategory = categoryId;
                    RACEntity.RegisterAssociation = RAEntity;

                    RAEntity.RegisterAssociationCategories.Add(RACEntity);
                }

                // Si el valor de retorno no es null, ha ocurrido un error.
                if (RAClient.Save(RAEntity, sessionId) != null)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtiene una lista de asociaciones entre registros de tabla de modelo de datos y categorías.
        /// </summary>
        /// <param name="tableName">El nombre de la tabla del modelo de datos.</param>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>Un listado de asociaciones.</returns>
        public Collection<RegisterAssociationData> GetListOfAssociations(string tableName, string sessionId)
        {
            if (tableName == null)
            {
                throw new ArgumentNullException("tableName", "TableName must not be null.");
            }
            if (sessionId == null)
            {
                throw new ArgumentNullException("sessionId", "SessionId must not be null.");
            }

            StoreEntity userStore = SessionManager.Instance.StoreFromUserSession(sessionId);
            TableEntity table = ObtainTableEntity(tableName, userStore, sessionId);

            Collection<RegisterAssociationEntity> listOfEntities = new BusinessLogic.RegisterAssociation().GetRegisterAssociationWhereEqual(RegisterAssociationEntity.DBIdTable, table.Id.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), true, sessionId);
            Collection<RegisterAssociationData> result = new Collection<RegisterAssociationData>();

            foreach (RegisterAssociationEntity registerAssociationEntity in listOfEntities)
            {
                RegisterAssociationData registerAssociation = new RegisterAssociationData();

                //  The thing that should not be (and it's not Cthulhu)
                //
                registerAssociationEntity.Table = new TableDataAccess().Load(registerAssociationEntity.IdTable);
                registerAssociation.TableName = registerAssociationEntity.Table.Name;
                registerAssociation.RegisterId = registerAssociationEntity.IdRegister;
                registerAssociation.CategoriesId = new List<int>();

                foreach (RegisterAssociationCategoriesEntity registerAssociationCategoriesEntity in registerAssociationEntity.RegisterAssociationCategories)
                {
                    registerAssociationCategoriesEntity.Category = new CategoryDataAccess().Load(registerAssociationCategoriesEntity.IdCategory,false);
                    registerAssociation.CategoriesId.Add(registerAssociationCategoriesEntity.Category.Id);
                }

                result.Add(registerAssociation);
            }

            return result;
        }

        /// <summary>
        /// Elimina la asociación entre registros de tablas de modelo de datos y categorías.
        /// </summary>
        /// <param name="associations">Listado de asociaciones a eliminar.</param>
        /// <param name="sessionId">Identificador de sesión.</param>
        /// <returns>True si la operación se realizó con éxito.</returns>
        public bool RemoveAssociations(Collection<RegisterAssociationData> associations, string sessionId)
        {
            if (associations == null)
            {
                throw new ArgumentNullException("associations", "Associations must have at least one RegisterAssociation.");
            }
            if (sessionId == null)
            {
                throw new ArgumentNullException("sessionId", "SessionId must not be null.");
            }
            if (associations.Count == 0)
            {
                throw new ArgumentException("Associations list must have at least one RegisterAssociation.", "associations");
            }

            bool result = true;
            RegisterAssociation RAClient = new RegisterAssociation();
            Collection<RegisterAssociationEntity> registerAssociationList = ConvertFromRegisterAssociationData(associations, sessionId);
            foreach (RegisterAssociationEntity registerAssociationEntity in registerAssociationList)
            {
                // Si el valor de retorno no es nulo, ha ocurrido un error.
                if (RAClient.Delete(registerAssociationEntity, sessionId) != null)
                {
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Obtiene una entidad de tabla, dependiendo del nombre y la tienda.
        /// </summary>
        private static TableEntity ObtainTableEntity(string tableName, StoreEntity store, string sessionId)
        {
            Table businessTableClient = new Table();
            Collection<TableEntity> tables = businessTableClient.GetTableWhereEqual(TableEntity.DBName, tableName, sessionId);
            TableEntity result = null;

            switch (tables.Count)
            {
                // Lanzar una excepción en el caso de que no exista la tabla.
                case 0: 
                    throw new UtnEmallBusinessLogicException("The table doesn't exists");

                case 1:
                    // La tabla debe tener un nombre
                    if (tables[0].Name == tableName)
                    {
                        result = tables[0];
                    }
                    else
                    {
                        // Si el nombre de la tabla es diferente, lanzar una excepción
                        throw new UtnEmallBusinessLogicException("The table doesn't exists");
                    }
                    break;

                // Obtener la tienda propietaria de la tabla
                default:
                    BusinessLogic.DataModel businessDataModelClient = new BusinessLogic.DataModel();
                    DataModelEntity dataModelEntity;
                    Collection<DataModelEntity> dataModels = businessDataModelClient.GetDataModelWhereEqual(DataModelEntity.DBIdStore, store.Id.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), true, sessionId);

                    // Recorrer el modelo de datos, hasta encontrar la tabla.
                    for (int i = 0; i < dataModels.Count && result == null; i++)
                    {
                        dataModelEntity = dataModels[i];

                        for (int j = 0; j < dataModelEntity.Tables.Count && result == null; j++)
                        {
                            TableEntity table = dataModelEntity.Tables[j];
                            
                            if (tableName == table.Name)
                            {
                                result = table;
                            }
                        }
                    }
                    break;
            }

            // Lanzar una excepción si no se pudo encontrar la tabla
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new UtnEmallBusinessLogicException("The table doesn't exists");
            }
        }

        /// <summary>
        /// Convertir los tipos de datos usados en el cliente a tipos usados en el servidor
        /// </summary>
        /// <param name="listOfRAData">Listado a convertir</param>
        /// <param name="sessionId">Identificador de sesión</param>
        /// <returns>Una colección de asociaciones</returns>
        private static Collection<RegisterAssociationEntity> ConvertFromRegisterAssociationData(Collection<RegisterAssociationData> listOfRegisterAssociationData, string sessionId)
        {
            RegisterAssociation RAClient = new RegisterAssociation();
            Collection<RegisterAssociationEntity> result = new Collection<RegisterAssociationEntity>();

            StoreEntity userStore = SessionManager.Instance.StoreFromUserSession(sessionId);

            // Convertir los objetos de entidad
            foreach (RegisterAssociationData registerAssociation in listOfRegisterAssociationData)
            {
                bool notFound = true;
                RegisterAssociationEntity registerAssociationEntity;
                TableEntity table = ObtainTableEntity(registerAssociation.TableName, userStore, sessionId);

                Collection<RegisterAssociationEntity> tableFilteredRAEntities = RAClient.GetRegisterAssociationWhereEqual(RegisterAssociationEntity.DBIdTable, table.Id.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), sessionId);
                for (int i = 0; i < tableFilteredRAEntities.Count && notFound; i++)
                {
                    registerAssociationEntity = tableFilteredRAEntities[i];

                    if (registerAssociationEntity.IdRegister == registerAssociation.RegisterId)
                    {
                        notFound = false;
                        result.Add(registerAssociationEntity);
                    }
                }

                if (notFound)
                {
                    throw new UtnEmallBusinessLogicException("At least one of the register associations does not exist.");
                }
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Representa una asociación entre registros de un modelo de datos y categorías
    /// </summary>
    [DataContract]
    public class RegisterAssociationData
    {
        // Id de registro
        int registerId;
        // Id de categorías asociada a un registro
        List<int> categoryId;
        // Nombre de tabla
        string tableName;

        /// <summary>
        /// Id del registro asociado
        /// </summary>
        [DataMember]
        public int RegisterId
        {
            get
            {
                return registerId;
            }
            set
            {
                registerId = value;
            }
        }
        /// <summary>
        /// Id de categoría asociada
        /// </summary>
        [SuppressMessage("Microsoft.Usage",
                         "CA2227:CollectionPropertiesShouldBeReadOnly",
                         Justification = "This Id will be setted from outside of the class.")]
        [DataMember]
        public List<int> CategoriesId
        {
            get
            {
                return categoryId;
            }
            set
            {
                categoryId = value;
            }
        }
        /// <summary>
        /// El nombre de tabla asociada
        /// </summary>
        [DataMember]
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
    }
}
