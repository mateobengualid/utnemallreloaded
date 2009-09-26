using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace UtnEmall.Client.ServiceAccessLayer
{
    public interface ICategory
    {
        /// <summary>
        /// Function to save a CategoryEntity to the database.
        /// </summary>
        /// <param name="categoryEntity">CategoryEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CategoryEntity was saved successfully, the same CategoryEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.CategoryEntity Save(UtnEmall.Client.EntityModel.CategoryEntity categoryEntity, string session);
        /// <summary>
        /// Function to delete a CategoryEntity from database.
        /// </summary>
        /// <param name="categoryEntity">CategoryEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CategoryEntity was deleted successfully, the same CategoryEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.CategoryEntity Delete(UtnEmall.Client.EntityModel.CategoryEntity categoryEntity, string session);
        /// <summary>
        /// Get an specific categoryEntity
        /// </summary>
        /// <param name="id">id of the CategoryEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A CategoryEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.CategoryEntity GetCategory(int id, bool loadRelation, string session);
        /// <summary>
        /// Get collection of all categoryEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all CategoryEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetAllCategory(bool loadRelation, string session);
        /// <summary>
        /// Get collection of all categoryEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of categoryEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CategoryEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session);
        /// <summary>
        /// Get collection of all categoryEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of categoryEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CategoryEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
        /// <summary>
        /// Function to validate a CategoryEntity before it's saved.
        /// </summary>
        /// <param name="categoryEntity">CategoryEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CategoryEntity was deleted successfully, the same CategoryEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        bool Validate(UtnEmall.Client.EntityModel.CategoryEntity category);
    }
    public interface ICustomer
    {
        /// <summary>
        /// Function to save a CustomerEntity to the database.
        /// </summary>
        /// <param name="customerEntity">CustomerEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CustomerEntity was saved successfully, the same CustomerEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.CustomerEntity Save(UtnEmall.Client.EntityModel.CustomerEntity customerEntity, string session);
        /// <summary>
        /// Function to delete a CustomerEntity from database.
        /// </summary>
        /// <param name="customerEntity">CustomerEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.CustomerEntity Delete(UtnEmall.Client.EntityModel.CustomerEntity customerEntity, string session);
        /// <summary>
        /// Get an specific customerEntity
        /// </summary>
        /// <param name="id">id of the CustomerEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A CustomerEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.CustomerEntity GetCustomer(int id, bool loadRelation, string session);
        /// <summary>
        /// Get collection of all customerEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all CustomerEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetAllCustomer(bool loadRelation, string session);
        /// <summary>
        /// Get collection of all customerEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of customerEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CustomerEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session);
        /// <summary>
        /// Get collection of all customerEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of customerEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CustomerEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
        /// <summary>
        /// Function to validate a CustomerEntity before it's saved.
        /// </summary>
        /// <param name="customerEntity">CustomerEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        bool Validate(UtnEmall.Client.EntityModel.CustomerEntity customer);
    }
    public interface IService
    {
        /// <summary>
        /// Function to save a ServiceEntity to the database.
        /// </summary>
        /// <param name="serviceEntity">ServiceEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the ServiceEntity was saved successfully, the same ServiceEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.ServiceEntity Save(UtnEmall.Client.EntityModel.ServiceEntity serviceEntity, string session);
        /// <summary>
        /// Function to delete a ServiceEntity from database.
        /// </summary>
        /// <param name="serviceEntity">ServiceEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.ServiceEntity Delete(UtnEmall.Client.EntityModel.ServiceEntity serviceEntity, string session);
        /// <summary>
        /// Get an specific serviceEntity
        /// </summary>
        /// <param name="id">id of the ServiceEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A ServiceEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.ServiceEntity GetService(int id, bool loadRelation, string session);
        /// <summary>
        /// Get collection of all serviceEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all ServiceEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetAllService(bool loadRelation, string session);
        /// <summary>
        /// Get collection of all serviceEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of serviceEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of ServiceEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session);
        /// <summary>
        /// Get collection of all serviceEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of serviceEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of ServiceEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
        /// <summary>
        /// Function to validate a ServiceEntity before it's saved.
        /// </summary>
        /// <param name="serviceEntity">ServiceEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        bool Validate(UtnEmall.Client.EntityModel.ServiceEntity service);
    }
    public interface IStore
    {
        /// <summary>
        /// Function to save a StoreEntity to the database.
        /// </summary>
        /// <param name="storeEntity">StoreEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the StoreEntity was saved successfully, the same StoreEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.StoreEntity Save(UtnEmall.Client.EntityModel.StoreEntity storeEntity, string session);
        /// <summary>
        /// Function to delete a StoreEntity from database.
        /// </summary>
        /// <param name="storeEntity">StoreEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.StoreEntity Delete(UtnEmall.Client.EntityModel.StoreEntity storeEntity, string session);
        /// <summary>
        /// Get an specific storeEntity
        /// </summary>
        /// <param name="id">id of the StoreEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A StoreEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.StoreEntity GetStore(int id, bool loadRelation, string session);
        /// <summary>
        /// Get collection of all storeEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all StoreEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetAllStore(bool loadRelation, string session);
        /// <summary>
        /// Get collection of all storeEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of storeEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of StoreEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session);
        /// <summary>
        /// Get collection of all storeEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of storeEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of StoreEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
        /// <summary>
        /// Function to validate a StoreEntity before it's saved.
        /// </summary>
        /// <param name="storeEntity">StoreEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        bool Validate(UtnEmall.Client.EntityModel.StoreEntity store);
    }
    public interface IUserActionClientData
    {
        /// <summary>
        /// Function to save a UserActionClientDataEntity to the database.
        /// </summary>
        /// <param name="userActionClientDataEntity">UserActionClientDataEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the UserActionClientDataEntity was saved successfully, the same UserActionClientDataEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.UserActionClientDataEntity Save(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity, string session);
        /// <summary>
        /// Function to delete a UserActionClientDataEntity from database.
        /// </summary>
        /// <param name="userActionClientDataEntity">UserActionClientDataEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.UserActionClientDataEntity Delete(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity, string session);
        /// <summary>
        /// Get an specific userActionClientDataEntity
        /// </summary>
        /// <param name="id">id of the UserActionClientDataEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A UserActionClientDataEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        UtnEmall.Client.EntityModel.UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation, string session);
        /// <summary>
        /// Get collection of all userActionClientDataEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all UserActionClientDataEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation, string session);
        /// <summary>
        /// Get collection of all userActionClientDataEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of userActionClientDataEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of UserActionClientDataEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session);
        /// <summary>
        /// Get collection of all userActionClientDataEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of userActionClientDataEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of UserActionClientDataEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
        /// <summary>
        /// Function to validate a UserActionClientDataEntity before it's saved.
        /// </summary>
        /// <param name="userActionClientDataEntity">UserActionClientDataEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        bool Validate(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientData);
    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Save", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategorySaveRequest
    {
        public ICategorySaveRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CategoryEntity categoryEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public ICategorySaveRequest(UtnEmall.Client.EntityModel.CategoryEntity categoryEntity, string session)
        {
            this.categoryEntity = categoryEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SaveResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategorySaveResponse
    {
        public ICategorySaveResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CategoryEntity SaveResult;
        public ICategorySaveResponse(UtnEmall.Client.EntityModel.CategoryEntity SaveResult)
        {
            this.SaveResult = SaveResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Delete", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryDeleteRequest
    {
        public ICategoryDeleteRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CategoryEntity categoryEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public ICategoryDeleteRequest(UtnEmall.Client.EntityModel.CategoryEntity categoryEntity, string session)
        {
            this.categoryEntity = categoryEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "DeleteResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryDeleteResponse
    {
        public ICategoryDeleteResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CategoryEntity DeleteResult;
        public ICategoryDeleteResponse(UtnEmall.Client.EntityModel.CategoryEntity DeleteResult)
        {
            this.DeleteResult = DeleteResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCategory", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetCategoryRequest
    {
        public ICategoryGetCategoryRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int id;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 2)]
        public string session;
        public ICategoryGetCategoryRequest(int id, bool loadRelation, string session)
        {
            this.id = id;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCategoryResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetCategoryResponse
    {
        public ICategoryGetCategoryResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CategoryEntity GetCategoryResult;
        public ICategoryGetCategoryResponse(UtnEmall.Client.EntityModel.CategoryEntity GetCategoryResult)
        {
            this.GetCategoryResult = GetCategoryResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllCategory", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetAllCategoryRequest
    {
        public ICategoryGetAllCategoryRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public ICategoryGetAllCategoryRequest(bool loadRelation, string session)
        {
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllCategoryResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetAllCategoryResponse
    {
        public ICategoryGetAllCategoryResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetAllCategoryResult;
        public ICategoryGetAllCategoryResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetAllCategoryResult)
        {
            this.GetAllCategoryResult = GetAllCategoryResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCategoryWhere", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetCategoryWhereRequest
    {
        public ICategoryGetCategoryWhereRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 3)]
        public UtnEmall.Client.DataModel.OperatorType operatorType;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 4)]
        public string session;
        public ICategoryGetCategoryWhereRequest(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.operatorType = operatorType;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCategoryWhereResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetCategoryWhereResponse
    {
        public ICategoryGetCategoryWhereResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhereResult;
        public ICategoryGetCategoryWhereResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhereResult)
        {
            this.GetCategoryWhereResult = GetCategoryWhereResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCategoryWhereEqual", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetCategoryWhereEqualRequest
    {
        public ICategoryGetCategoryWhereEqualRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 3)]
        public string session;
        public ICategoryGetCategoryWhereEqualRequest(string propertyName, object expValue, bool loadRelation, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCategoryWhereEqualResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryGetCategoryWhereEqualResponse
    {
        public ICategoryGetCategoryWhereEqualResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhereEqualResult;
        public ICategoryGetCategoryWhereEqualResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhereEqualResult)
        {
            this.GetCategoryWhereEqualResult = GetCategoryWhereEqualResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Validate", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryValidateRequest
    {
        public ICategoryValidateRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CategoryEntity category;
        public ICategoryValidateRequest(UtnEmall.Client.EntityModel.CategoryEntity category)
        {
            this.category = category;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ValidateResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICategoryValidateResponse
    {
        public ICategoryValidateResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool ValidateResult;
        public ICategoryValidateResponse(bool ValidateResult)
        {
            this.ValidateResult = ValidateResult;
        }

    }
    public class CategoryClient : Microsoft.Tools.ServiceModel.CFClientBase<UtnEmall.Client.ServiceAccessLayer.ICategory>, UtnEmall.Client.ServiceAccessLayer.ICategory
    {
        private static System.ServiceModel.EndpointAddress EndpointAddress = new System.ServiceModel.EndpointAddress("http://utnEmallserver/ICategory");
        public CategoryClient()
            : base(CreateDefaultBinding(), EndpointAddress)
        {
        }

        public CategoryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public static System.ServiceModel.Channels.Binding CreateDefaultBinding()
        {
            System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
            binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), new System.ServiceModel.Channels.HttpTransportBindingElement() });
            return binding;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategorySaveResponse Save(UtnEmall.Client.ServiceAccessLayer.ICategorySaveRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/Save";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/SaveResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategorySaveResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategorySaveRequest, UtnEmall.Client.ServiceAccessLayer.ICategorySaveResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to save a CategoryEntity to the database.
        /// </summary>
        /// <param name="categoryEntity">CategoryEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CategoryEntity was saved successfully, the same CategoryEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.CategoryEntity Save(UtnEmall.Client.EntityModel.CategoryEntity categoryEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategorySaveRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategorySaveRequest(categoryEntity, session);
            UtnEmall.Client.ServiceAccessLayer.ICategorySaveResponse response = this.Save(request);
            return response.SaveResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteResponse Delete(UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/Delete";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/DeleteResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteRequest, UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to delete a CategoryEntity from database.
        /// </summary>
        /// <param name="categoryEntity">CategoryEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CategoryEntity was deleted successfully, the same CategoryEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.CategoryEntity Delete(UtnEmall.Client.EntityModel.CategoryEntity categoryEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteRequest(categoryEntity, session);
            UtnEmall.Client.ServiceAccessLayer.ICategoryDeleteResponse response = this.Delete(request);
            return response.DeleteResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryResponse GetCategory(UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/GetCategory";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/GetCategoryResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryRequest, UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get an specific categoryEntity
        /// </summary>
        /// <param name="id">id of the CategoryEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A CategoryEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.CategoryEntity GetCategory(int id, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryRequest(id, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryResponse response = this.GetCategory(request);
            return response.GetCategoryResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryResponse GetAllCategory(UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/GetAllCategory";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/GetAllCategoryResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryRequest, UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all categoryEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all CategoryEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetAllCategory(bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryRequest(loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetAllCategoryResponse response = this.GetAllCategory(request);
            return response.GetAllCategoryResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereResponse GetCategoryWhere(UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/GetCategoryWhere";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/GetCategoryWhereResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereRequest, UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all categoryEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of categoryEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CategoryEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereRequest(propertyName, expValue, loadRelation, operatorType, session);
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereResponse response = this.GetCategoryWhere(request);
            return response.GetCategoryWhereResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualResponse GetCategoryWhereEqual(UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/GetCategoryWhereEqual";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/GetCategoryWhereEqualResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualRequest, UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all categoryEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of categoryEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CategoryEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualRequest(propertyName, expValue, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.ICategoryGetCategoryWhereEqualResponse response = this.GetCategoryWhereEqual(request);
            return response.GetCategoryWhereEqualResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICategoryValidateResponse Validate(UtnEmall.Client.ServiceAccessLayer.ICategoryValidateRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICategory/Validate";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICategory/ValidateResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICategoryValidateResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICategoryValidateRequest, UtnEmall.Client.ServiceAccessLayer.ICategoryValidateResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to validate a CategoryEntity before it's saved.
        /// </summary>
        /// <param name="categoryEntity">CategoryEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CategoryEntity was deleted successfully, the same CategoryEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="categoryEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public bool Validate(UtnEmall.Client.EntityModel.CategoryEntity category)
        {
            UtnEmall.Client.ServiceAccessLayer.ICategoryValidateRequest request = new UtnEmall.Client.ServiceAccessLayer.ICategoryValidateRequest(category);
            UtnEmall.Client.ServiceAccessLayer.ICategoryValidateResponse response = this.Validate(request);
            return response.ValidateResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Save", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerSaveRequest
    {
        public ICustomerSaveRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CustomerEntity customerEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public ICustomerSaveRequest(UtnEmall.Client.EntityModel.CustomerEntity customerEntity, string session)
        {
            this.customerEntity = customerEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SaveResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerSaveResponse
    {
        public ICustomerSaveResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CustomerEntity SaveResult;
        public ICustomerSaveResponse(UtnEmall.Client.EntityModel.CustomerEntity SaveResult)
        {
            this.SaveResult = SaveResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Delete", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerDeleteRequest
    {
        public ICustomerDeleteRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CustomerEntity customerEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public ICustomerDeleteRequest(UtnEmall.Client.EntityModel.CustomerEntity customerEntity, string session)
        {
            this.customerEntity = customerEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "DeleteResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerDeleteResponse
    {
        public ICustomerDeleteResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CustomerEntity DeleteResult;
        public ICustomerDeleteResponse(UtnEmall.Client.EntityModel.CustomerEntity DeleteResult)
        {
            this.DeleteResult = DeleteResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCustomer", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetCustomerRequest
    {
        public ICustomerGetCustomerRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int id;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 2)]
        public string session;
        public ICustomerGetCustomerRequest(int id, bool loadRelation, string session)
        {
            this.id = id;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCustomerResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetCustomerResponse
    {
        public ICustomerGetCustomerResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CustomerEntity GetCustomerResult;
        public ICustomerGetCustomerResponse(UtnEmall.Client.EntityModel.CustomerEntity GetCustomerResult)
        {
            this.GetCustomerResult = GetCustomerResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllCustomer", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetAllCustomerRequest
    {
        public ICustomerGetAllCustomerRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public ICustomerGetAllCustomerRequest(bool loadRelation, string session)
        {
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllCustomerResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetAllCustomerResponse
    {
        public ICustomerGetAllCustomerResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetAllCustomerResult;
        public ICustomerGetAllCustomerResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetAllCustomerResult)
        {
            this.GetAllCustomerResult = GetAllCustomerResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCustomerWhere", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetCustomerWhereRequest
    {
        public ICustomerGetCustomerWhereRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 3)]
        public UtnEmall.Client.DataModel.OperatorType operatorType;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 4)]
        public string session;
        public ICustomerGetCustomerWhereRequest(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.operatorType = operatorType;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCustomerWhereResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetCustomerWhereResponse
    {
        public ICustomerGetCustomerWhereResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhereResult;
        public ICustomerGetCustomerWhereResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhereResult)
        {
            this.GetCustomerWhereResult = GetCustomerWhereResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCustomerWhereEqual", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetCustomerWhereEqualRequest
    {
        public ICustomerGetCustomerWhereEqualRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 3)]
        public string session;
        public ICustomerGetCustomerWhereEqualRequest(string propertyName, object expValue, bool loadRelation, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetCustomerWhereEqualResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerGetCustomerWhereEqualResponse
    {
        public ICustomerGetCustomerWhereEqualResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhereEqualResult;
        public ICustomerGetCustomerWhereEqualResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhereEqualResult)
        {
            this.GetCustomerWhereEqualResult = GetCustomerWhereEqualResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Validate", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerValidateRequest
    {
        public ICustomerValidateRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.CustomerEntity customer;
        public ICustomerValidateRequest(UtnEmall.Client.EntityModel.CustomerEntity customer)
        {
            this.customer = customer;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ValidateResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class ICustomerValidateResponse
    {
        public ICustomerValidateResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool ValidateResult;
        public ICustomerValidateResponse(bool ValidateResult)
        {
            this.ValidateResult = ValidateResult;
        }

    }
    public class CustomerClient : Microsoft.Tools.ServiceModel.CFClientBase<UtnEmall.Client.ServiceAccessLayer.ICustomer>, UtnEmall.Client.ServiceAccessLayer.ICustomer
    {
        private static System.ServiceModel.EndpointAddress EndpointAddress = new System.ServiceModel.EndpointAddress("http://utnEmallserver/ICustomer");
        public CustomerClient()
            : base(CreateDefaultBinding(), EndpointAddress)
        {
        }

        public CustomerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public static System.ServiceModel.Channels.Binding CreateDefaultBinding()
        {
            System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
            binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), new System.ServiceModel.Channels.HttpTransportBindingElement() });
            return binding;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerSaveResponse Save(UtnEmall.Client.ServiceAccessLayer.ICustomerSaveRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/Save";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/SaveResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerSaveResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerSaveRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerSaveResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to save a CustomerEntity to the database.
        /// </summary>
        /// <param name="customerEntity">CustomerEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CustomerEntity was saved successfully, the same CustomerEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.CustomerEntity Save(UtnEmall.Client.EntityModel.CustomerEntity customerEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerSaveRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerSaveRequest(customerEntity, session);
            UtnEmall.Client.ServiceAccessLayer.ICustomerSaveResponse response = this.Save(request);
            return response.SaveResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteResponse Delete(UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/Delete";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/DeleteResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to delete a CustomerEntity from database.
        /// </summary>
        /// <param name="customerEntity">CustomerEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.CustomerEntity Delete(UtnEmall.Client.EntityModel.CustomerEntity customerEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteRequest(customerEntity, session);
            UtnEmall.Client.ServiceAccessLayer.ICustomerDeleteResponse response = this.Delete(request);
            return response.DeleteResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerResponse GetCustomer(UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/GetCustomer";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/GetCustomerResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get an specific customerEntity
        /// </summary>
        /// <param name="id">id of the CustomerEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A CustomerEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.CustomerEntity GetCustomer(int id, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerRequest(id, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerResponse response = this.GetCustomer(request);
            return response.GetCustomerResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerResponse GetAllCustomer(UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/GetAllCustomer";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/GetAllCustomerResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all customerEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all CustomerEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetAllCustomer(bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerRequest(loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetAllCustomerResponse response = this.GetAllCustomer(request);
            return response.GetAllCustomerResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereResponse GetCustomerWhere(UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/GetCustomerWhere";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/GetCustomerWhereResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all customerEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of customerEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CustomerEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereRequest(propertyName, expValue, loadRelation, operatorType, session);
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereResponse response = this.GetCustomerWhere(request);
            return response.GetCustomerWhereResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualResponse GetCustomerWhereEqual(UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/GetCustomerWhereEqual";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/GetCustomerWhereEqualResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all customerEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of customerEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of CustomerEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualRequest(propertyName, expValue, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.ICustomerGetCustomerWhereEqualResponse response = this.GetCustomerWhereEqual(request);
            return response.GetCustomerWhereEqualResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.ICustomerValidateResponse Validate(UtnEmall.Client.ServiceAccessLayer.ICustomerValidateRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/ICustomer/Validate";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/ICustomer/ValidateResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.ICustomerValidateResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.ICustomerValidateRequest, UtnEmall.Client.ServiceAccessLayer.ICustomerValidateResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to validate a CustomerEntity before it's saved.
        /// </summary>
        /// <param name="customerEntity">CustomerEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="customerEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public bool Validate(UtnEmall.Client.EntityModel.CustomerEntity customer)
        {
            UtnEmall.Client.ServiceAccessLayer.ICustomerValidateRequest request = new UtnEmall.Client.ServiceAccessLayer.ICustomerValidateRequest(customer);
            UtnEmall.Client.ServiceAccessLayer.ICustomerValidateResponse response = this.Validate(request);
            return response.ValidateResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Save", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceSaveRequest
    {
        public IServiceSaveRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.ServiceEntity serviceEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IServiceSaveRequest(UtnEmall.Client.EntityModel.ServiceEntity serviceEntity, string session)
        {
            this.serviceEntity = serviceEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SaveResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceSaveResponse
    {
        public IServiceSaveResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.ServiceEntity SaveResult;
        public IServiceSaveResponse(UtnEmall.Client.EntityModel.ServiceEntity SaveResult)
        {
            this.SaveResult = SaveResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Delete", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceDeleteRequest
    {
        public IServiceDeleteRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.ServiceEntity serviceEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IServiceDeleteRequest(UtnEmall.Client.EntityModel.ServiceEntity serviceEntity, string session)
        {
            this.serviceEntity = serviceEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "DeleteResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceDeleteResponse
    {
        public IServiceDeleteResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.ServiceEntity DeleteResult;
        public IServiceDeleteResponse(UtnEmall.Client.EntityModel.ServiceEntity DeleteResult)
        {
            this.DeleteResult = DeleteResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetService", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetServiceRequest
    {
        public IServiceGetServiceRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int id;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 2)]
        public string session;
        public IServiceGetServiceRequest(int id, bool loadRelation, string session)
        {
            this.id = id;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetServiceResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetServiceResponse
    {
        public IServiceGetServiceResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.ServiceEntity GetServiceResult;
        public IServiceGetServiceResponse(UtnEmall.Client.EntityModel.ServiceEntity GetServiceResult)
        {
            this.GetServiceResult = GetServiceResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllService", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetAllServiceRequest
    {
        public IServiceGetAllServiceRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IServiceGetAllServiceRequest(bool loadRelation, string session)
        {
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllServiceResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetAllServiceResponse
    {
        public IServiceGetAllServiceResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetAllServiceResult;
        public IServiceGetAllServiceResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetAllServiceResult)
        {
            this.GetAllServiceResult = GetAllServiceResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetServiceWhere", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetServiceWhereRequest
    {
        public IServiceGetServiceWhereRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 3)]
        public UtnEmall.Client.DataModel.OperatorType operatorType;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 4)]
        public string session;
        public IServiceGetServiceWhereRequest(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.operatorType = operatorType;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetServiceWhereResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetServiceWhereResponse
    {
        public IServiceGetServiceWhereResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhereResult;
        public IServiceGetServiceWhereResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhereResult)
        {
            this.GetServiceWhereResult = GetServiceWhereResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetServiceWhereEqual", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetServiceWhereEqualRequest
    {
        public IServiceGetServiceWhereEqualRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 3)]
        public string session;
        public IServiceGetServiceWhereEqualRequest(string propertyName, object expValue, bool loadRelation, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetServiceWhereEqualResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceGetServiceWhereEqualResponse
    {
        public IServiceGetServiceWhereEqualResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhereEqualResult;
        public IServiceGetServiceWhereEqualResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhereEqualResult)
        {
            this.GetServiceWhereEqualResult = GetServiceWhereEqualResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Validate", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceValidateRequest
    {
        public IServiceValidateRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.ServiceEntity service;
        public IServiceValidateRequest(UtnEmall.Client.EntityModel.ServiceEntity service)
        {
            this.service = service;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ValidateResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IServiceValidateResponse
    {
        public IServiceValidateResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool ValidateResult;
        public IServiceValidateResponse(bool ValidateResult)
        {
            this.ValidateResult = ValidateResult;
        }

    }
    public class ServiceClient : Microsoft.Tools.ServiceModel.CFClientBase<UtnEmall.Client.ServiceAccessLayer.IService>, UtnEmall.Client.ServiceAccessLayer.IService
    {
        private static System.ServiceModel.EndpointAddress EndpointAddress = new System.ServiceModel.EndpointAddress("http://utnEmallserver/IService");
        public ServiceClient()
            : base(CreateDefaultBinding(), EndpointAddress)
        {
        }

        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public static System.ServiceModel.Channels.Binding CreateDefaultBinding()
        {
            System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
            binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), new System.ServiceModel.Channels.HttpTransportBindingElement() });
            return binding;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceSaveResponse Save(UtnEmall.Client.ServiceAccessLayer.IServiceSaveRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/Save";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/SaveResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceSaveResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceSaveRequest, UtnEmall.Client.ServiceAccessLayer.IServiceSaveResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to save a ServiceEntity to the database.
        /// </summary>
        /// <param name="serviceEntity">ServiceEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the ServiceEntity was saved successfully, the same ServiceEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.ServiceEntity Save(UtnEmall.Client.EntityModel.ServiceEntity serviceEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceSaveRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceSaveRequest(serviceEntity, session);
            UtnEmall.Client.ServiceAccessLayer.IServiceSaveResponse response = this.Save(request);
            return response.SaveResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceDeleteResponse Delete(UtnEmall.Client.ServiceAccessLayer.IServiceDeleteRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/Delete";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/DeleteResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceDeleteResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceDeleteRequest, UtnEmall.Client.ServiceAccessLayer.IServiceDeleteResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to delete a ServiceEntity from database.
        /// </summary>
        /// <param name="serviceEntity">ServiceEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.ServiceEntity Delete(UtnEmall.Client.EntityModel.ServiceEntity serviceEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceDeleteRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceDeleteRequest(serviceEntity, session);
            UtnEmall.Client.ServiceAccessLayer.IServiceDeleteResponse response = this.Delete(request);
            return response.DeleteResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceResponse GetService(UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/GetService";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/GetServiceResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceRequest, UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get an specific serviceEntity
        /// </summary>
        /// <param name="id">id of the ServiceEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A ServiceEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.ServiceEntity GetService(int id, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceRequest(id, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceResponse response = this.GetService(request);
            return response.GetServiceResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceResponse GetAllService(UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/GetAllService";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/GetAllServiceResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceRequest, UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all serviceEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all ServiceEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetAllService(bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceRequest(loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IServiceGetAllServiceResponse response = this.GetAllService(request);
            return response.GetAllServiceResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereResponse GetServiceWhere(UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/GetServiceWhere";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/GetServiceWhereResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereRequest, UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all serviceEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of serviceEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of ServiceEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereRequest(propertyName, expValue, loadRelation, operatorType, session);
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereResponse response = this.GetServiceWhere(request);
            return response.GetServiceWhereResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualResponse GetServiceWhereEqual(UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/GetServiceWhereEqual";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/GetServiceWhereEqualResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualRequest, UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all serviceEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of serviceEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of ServiceEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualRequest(propertyName, expValue, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IServiceGetServiceWhereEqualResponse response = this.GetServiceWhereEqual(request);
            return response.GetServiceWhereEqualResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IServiceValidateResponse Validate(UtnEmall.Client.ServiceAccessLayer.IServiceValidateRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IService/Validate";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IService/ValidateResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IServiceValidateResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IServiceValidateRequest, UtnEmall.Client.ServiceAccessLayer.IServiceValidateResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to validate a ServiceEntity before it's saved.
        /// </summary>
        /// <param name="serviceEntity">ServiceEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the ServiceEntity was deleted successfully, the same ServiceEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="serviceEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public bool Validate(UtnEmall.Client.EntityModel.ServiceEntity service)
        {
            UtnEmall.Client.ServiceAccessLayer.IServiceValidateRequest request = new UtnEmall.Client.ServiceAccessLayer.IServiceValidateRequest(service);
            UtnEmall.Client.ServiceAccessLayer.IServiceValidateResponse response = this.Validate(request);
            return response.ValidateResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Save", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreSaveRequest
    {
        public IStoreSaveRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.StoreEntity storeEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IStoreSaveRequest(UtnEmall.Client.EntityModel.StoreEntity storeEntity, string session)
        {
            this.storeEntity = storeEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SaveResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreSaveResponse
    {
        public IStoreSaveResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.StoreEntity SaveResult;
        public IStoreSaveResponse(UtnEmall.Client.EntityModel.StoreEntity SaveResult)
        {
            this.SaveResult = SaveResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Delete", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreDeleteRequest
    {
        public IStoreDeleteRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.StoreEntity storeEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IStoreDeleteRequest(UtnEmall.Client.EntityModel.StoreEntity storeEntity, string session)
        {
            this.storeEntity = storeEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "DeleteResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreDeleteResponse
    {
        public IStoreDeleteResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.StoreEntity DeleteResult;
        public IStoreDeleteResponse(UtnEmall.Client.EntityModel.StoreEntity DeleteResult)
        {
            this.DeleteResult = DeleteResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetStore", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetStoreRequest
    {
        public IStoreGetStoreRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int id;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 2)]
        public string session;
        public IStoreGetStoreRequest(int id, bool loadRelation, string session)
        {
            this.id = id;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetStoreResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetStoreResponse
    {
        public IStoreGetStoreResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.StoreEntity GetStoreResult;
        public IStoreGetStoreResponse(UtnEmall.Client.EntityModel.StoreEntity GetStoreResult)
        {
            this.GetStoreResult = GetStoreResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllStore", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetAllStoreRequest
    {
        public IStoreGetAllStoreRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IStoreGetAllStoreRequest(bool loadRelation, string session)
        {
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllStoreResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetAllStoreResponse
    {
        public IStoreGetAllStoreResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetAllStoreResult;
        public IStoreGetAllStoreResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetAllStoreResult)
        {
            this.GetAllStoreResult = GetAllStoreResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetStoreWhere", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetStoreWhereRequest
    {
        public IStoreGetStoreWhereRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 3)]
        public UtnEmall.Client.DataModel.OperatorType operatorType;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 4)]
        public string session;
        public IStoreGetStoreWhereRequest(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.operatorType = operatorType;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetStoreWhereResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetStoreWhereResponse
    {
        public IStoreGetStoreWhereResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhereResult;
        public IStoreGetStoreWhereResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhereResult)
        {
            this.GetStoreWhereResult = GetStoreWhereResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetStoreWhereEqual", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetStoreWhereEqualRequest
    {
        public IStoreGetStoreWhereEqualRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 3)]
        public string session;
        public IStoreGetStoreWhereEqualRequest(string propertyName, object expValue, bool loadRelation, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetStoreWhereEqualResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreGetStoreWhereEqualResponse
    {
        public IStoreGetStoreWhereEqualResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhereEqualResult;
        public IStoreGetStoreWhereEqualResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhereEqualResult)
        {
            this.GetStoreWhereEqualResult = GetStoreWhereEqualResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Validate", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreValidateRequest
    {
        public IStoreValidateRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.StoreEntity store;
        public IStoreValidateRequest(UtnEmall.Client.EntityModel.StoreEntity store)
        {
            this.store = store;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ValidateResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IStoreValidateResponse
    {
        public IStoreValidateResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool ValidateResult;
        public IStoreValidateResponse(bool ValidateResult)
        {
            this.ValidateResult = ValidateResult;
        }

    }
    public class StoreClient : Microsoft.Tools.ServiceModel.CFClientBase<UtnEmall.Client.ServiceAccessLayer.IStore>, UtnEmall.Client.ServiceAccessLayer.IStore
    {
        private static System.ServiceModel.EndpointAddress EndpointAddress = new System.ServiceModel.EndpointAddress("http://utnEmallserver/IStore");
        public StoreClient()
            : base(CreateDefaultBinding(), EndpointAddress)
        {
        }

        public StoreClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public static System.ServiceModel.Channels.Binding CreateDefaultBinding()
        {
            System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
            binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), new System.ServiceModel.Channels.HttpTransportBindingElement() });
            return binding;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreSaveResponse Save(UtnEmall.Client.ServiceAccessLayer.IStoreSaveRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/Save";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/SaveResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreSaveResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreSaveRequest, UtnEmall.Client.ServiceAccessLayer.IStoreSaveResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to save a StoreEntity to the database.
        /// </summary>
        /// <param name="storeEntity">StoreEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the StoreEntity was saved successfully, the same StoreEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.StoreEntity Save(UtnEmall.Client.EntityModel.StoreEntity storeEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreSaveRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreSaveRequest(storeEntity, session);
            UtnEmall.Client.ServiceAccessLayer.IStoreSaveResponse response = this.Save(request);
            return response.SaveResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreDeleteResponse Delete(UtnEmall.Client.ServiceAccessLayer.IStoreDeleteRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/Delete";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/DeleteResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreDeleteResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreDeleteRequest, UtnEmall.Client.ServiceAccessLayer.IStoreDeleteResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to delete a StoreEntity from database.
        /// </summary>
        /// <param name="storeEntity">StoreEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.StoreEntity Delete(UtnEmall.Client.EntityModel.StoreEntity storeEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreDeleteRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreDeleteRequest(storeEntity, session);
            UtnEmall.Client.ServiceAccessLayer.IStoreDeleteResponse response = this.Delete(request);
            return response.DeleteResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreResponse GetStore(UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/GetStore";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/GetStoreResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreRequest, UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get an specific storeEntity
        /// </summary>
        /// <param name="id">id of the StoreEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A StoreEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.StoreEntity GetStore(int id, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreRequest(id, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreResponse response = this.GetStore(request);
            return response.GetStoreResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreResponse GetAllStore(UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/GetAllStore";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/GetAllStoreResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreRequest, UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all storeEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all StoreEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetAllStore(bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreRequest(loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IStoreGetAllStoreResponse response = this.GetAllStore(request);
            return response.GetAllStoreResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereResponse GetStoreWhere(UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/GetStoreWhere";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/GetStoreWhereResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereRequest, UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all storeEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of storeEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of StoreEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereRequest(propertyName, expValue, loadRelation, operatorType, session);
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereResponse response = this.GetStoreWhere(request);
            return response.GetStoreWhereResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualResponse GetStoreWhereEqual(UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/GetStoreWhereEqual";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/GetStoreWhereEqualResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualRequest, UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all storeEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of storeEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of StoreEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualRequest(propertyName, expValue, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IStoreGetStoreWhereEqualResponse response = this.GetStoreWhereEqual(request);
            return response.GetStoreWhereEqualResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IStoreValidateResponse Validate(UtnEmall.Client.ServiceAccessLayer.IStoreValidateRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IStore/Validate";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IStore/ValidateResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IStoreValidateResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IStoreValidateRequest, UtnEmall.Client.ServiceAccessLayer.IStoreValidateResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to validate a StoreEntity before it's saved.
        /// </summary>
        /// <param name="storeEntity">StoreEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the StoreEntity was deleted successfully, the same StoreEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="storeEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public bool Validate(UtnEmall.Client.EntityModel.StoreEntity store)
        {
            UtnEmall.Client.ServiceAccessLayer.IStoreValidateRequest request = new UtnEmall.Client.ServiceAccessLayer.IStoreValidateRequest(store);
            UtnEmall.Client.ServiceAccessLayer.IStoreValidateResponse response = this.Validate(request);
            return response.ValidateResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Save", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataSaveRequest
    {
        public IUserActionClientDataSaveRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IUserActionClientDataSaveRequest(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity, string session)
        {
            this.userActionClientDataEntity = userActionClientDataEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SaveResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataSaveResponse
    {
        public IUserActionClientDataSaveResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity SaveResult;
        public IUserActionClientDataSaveResponse(UtnEmall.Client.EntityModel.UserActionClientDataEntity SaveResult)
        {
            this.SaveResult = SaveResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Delete", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataDeleteRequest
    {
        public IUserActionClientDataDeleteRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IUserActionClientDataDeleteRequest(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity, string session)
        {
            this.userActionClientDataEntity = userActionClientDataEntity;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "DeleteResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataDeleteResponse
    {
        public IUserActionClientDataDeleteResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity DeleteResult;
        public IUserActionClientDataDeleteResponse(UtnEmall.Client.EntityModel.UserActionClientDataEntity DeleteResult)
        {
            this.DeleteResult = DeleteResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetUserActionClientData", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetUserActionClientDataRequest
    {
        public IUserActionClientDataGetUserActionClientDataRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int id;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 2)]
        public string session;
        public IUserActionClientDataGetUserActionClientDataRequest(int id, bool loadRelation, string session)
        {
            this.id = id;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetUserActionClientDataResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetUserActionClientDataResponse
    {
        public IUserActionClientDataGetUserActionClientDataResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity GetUserActionClientDataResult;
        public IUserActionClientDataGetUserActionClientDataResponse(UtnEmall.Client.EntityModel.UserActionClientDataEntity GetUserActionClientDataResult)
        {
            this.GetUserActionClientDataResult = GetUserActionClientDataResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllUserActionClientData", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetAllUserActionClientDataRequest
    {
        public IUserActionClientDataGetAllUserActionClientDataRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public string session;
        public IUserActionClientDataGetAllUserActionClientDataRequest(bool loadRelation, string session)
        {
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetAllUserActionClientDataResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetAllUserActionClientDataResponse
    {
        public IUserActionClientDataGetAllUserActionClientDataResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetAllUserActionClientDataResult;
        public IUserActionClientDataGetAllUserActionClientDataResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetAllUserActionClientDataResult)
        {
            this.GetAllUserActionClientDataResult = GetAllUserActionClientDataResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetUserActionClientDataWhere", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetUserActionClientDataWhereRequest
    {
        public IUserActionClientDataGetUserActionClientDataWhereRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 3)]
        public UtnEmall.Client.DataModel.OperatorType operatorType;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 4)]
        public string session;
        public IUserActionClientDataGetUserActionClientDataWhereRequest(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.operatorType = operatorType;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetUserActionClientDataWhereResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetUserActionClientDataWhereResponse
    {
        public IUserActionClientDataGetUserActionClientDataWhereResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhereResult;
        public IUserActionClientDataGetUserActionClientDataWhereResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhereResult)
        {
            this.GetUserActionClientDataWhereResult = GetUserActionClientDataWhereResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetUserActionClientDataWhereEqual", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetUserActionClientDataWhereEqualRequest
    {
        public IUserActionClientDataGetUserActionClientDataWhereEqualRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public string propertyName;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 1)]
        public object expValue;
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public bool loadRelation;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 3)]
        public string session;
        public IUserActionClientDataGetUserActionClientDataWhereEqualRequest(string propertyName, object expValue, bool loadRelation, string session)
        {
            this.propertyName = propertyName;
            this.expValue = expValue;
            this.loadRelation = loadRelation;
            this.session = session;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "GetUserActionClientDataWhereEqualResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataGetUserActionClientDataWhereEqualResponse
    {
        public IUserActionClientDataGetUserActionClientDataWhereEqualResponse()
        {
        }

        [System.Xml.Serialization.XmlArrayItem(Namespace = "UtnEmall.Server.EntityModel")]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhereEqualResult;
        public IUserActionClientDataGetUserActionClientDataWhereEqualResponse(System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhereEqualResult)
        {
            this.GetUserActionClientDataWhereEqualResult = GetUserActionClientDataWhereEqualResult;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Validate", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataValidateRequest
    {
        public IUserActionClientDataValidateRequest()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Namespace = "http://tempuri.org/", Order = 0)]
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientData;
        public IUserActionClientDataValidateRequest(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientData)
        {
            this.userActionClientData = userActionClientData;
        }

    }
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "ValidateResponse", Namespace = "http://tempuri.org/"), System.CodeDom.Compiler.GeneratedCodeAttribute("LayerD Proxy Generator", "1.0.0.0")]
    public class IUserActionClientDataValidateResponse
    {
        public IUserActionClientDataValidateResponse()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public bool ValidateResult;
        public IUserActionClientDataValidateResponse(bool ValidateResult)
        {
            this.ValidateResult = ValidateResult;
        }

    }
    public class UserActionClientDataClient : Microsoft.Tools.ServiceModel.CFClientBase<UtnEmall.Client.ServiceAccessLayer.IUserActionClientData>, UtnEmall.Client.ServiceAccessLayer.IUserActionClientData
    {
        private static System.ServiceModel.EndpointAddress EndpointAddress = new System.ServiceModel.EndpointAddress("http://utnEmallserver/IUserActionClientData");
        public UserActionClientDataClient()
            : base(CreateDefaultBinding(), EndpointAddress)
        {
        }

        public UserActionClientDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public static System.ServiceModel.Channels.Binding CreateDefaultBinding()
        {
            System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
            binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), new System.ServiceModel.Channels.HttpTransportBindingElement() });
            return binding;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveResponse Save(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/Save";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/SaveResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to save a UserActionClientDataEntity to the database.
        /// </summary>
        /// <param name="userActionClientDataEntity">UserActionClientDataEntity to save</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the UserActionClientDataEntity was saved successfully, the same UserActionClientDataEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity Save(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveRequest(userActionClientDataEntity, session);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataSaveResponse response = this.Save(request);
            return response.SaveResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteResponse Delete(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/Delete";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/DeleteResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to delete a UserActionClientDataEntity from database.
        /// </summary>
        /// <param name="userActionClientDataEntity">UserActionClientDataEntity to delete</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity Delete(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientDataEntity, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteRequest(userActionClientDataEntity, session);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataDeleteResponse response = this.Delete(request);
            return response.DeleteResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataResponse GetUserActionClientData(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/GetUserActionClientData";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/GetUserActionClientDataResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get an specific userActionClientDataEntity
        /// </summary>
        /// <param name="id">id of the UserActionClientDataEntity to load</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>A UserActionClientDataEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public UtnEmall.Client.EntityModel.UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataRequest(id, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataResponse response = this.GetUserActionClientData(request);
            return response.GetUserActionClientDataResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataResponse GetAllUserActionClientData(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/GetAllUserActionClientData";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/GetAllUserActionClientDataResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all userActionClientDataEntity
        /// </summary>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of all UserActionClientDataEntity</returns>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataRequest(loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetAllUserActionClientDataResponse response = this.GetAllUserActionClientData(request);
            return response.GetAllUserActionClientDataResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereResponse GetUserActionClientDataWhere(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/GetUserActionClientDataWhere";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/GetUserActionClientDataWhereResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all userActionClientDataEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of userActionClientDataEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of UserActionClientDataEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Client.DataModel.OperatorType operatorType, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereRequest(propertyName, expValue, loadRelation, operatorType, session);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereResponse response = this.GetUserActionClientDataWhere(request);
            return response.GetUserActionClientDataWhereResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualResponse GetUserActionClientDataWhereEqual(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/GetUserActionClientDataWhereEqual";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/GetUserActionClientDataWhereEqualResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Get collection of all userActionClientDataEntity that comply with certain pattern
        /// </summary>
        /// <param name="propertyName">property of userActionClientDataEntity</param>
        /// <param name="expValue">pattern</param>
        /// <param name="loadRelation">true to load the relations</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>Collection of UserActionClientDataEntity</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="propertyName"/> is null or empty.
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="expValue"/> is null or empty.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public System.Collections.ObjectModel.Collection<UtnEmall.Client.EntityModel.UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualRequest(propertyName, expValue, loadRelation, session);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataGetUserActionClientDataWhereEqualResponse response = this.GetUserActionClientDataWhereEqual(request);
            return response.GetUserActionClientDataWhereEqualResult;
        }

        private UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateResponse Validate(UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateRequest request)
        {
            Microsoft.Tools.ServiceModel.CFInvokeInfo info = new Microsoft.Tools.ServiceModel.CFInvokeInfo();
            info.Action = "http://tempuri.org/IUserActionClientData/Validate";
            info.RequestIsWrapped = true;
            info.ReplyAction = "http://tempuri.org/IUserActionClientData/ValidateResponse";
            info.ResponseIsWrapped = true;
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateResponse retVal = this.Invoke<UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateRequest, UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateResponse>(info, request);
            return retVal;
        }

        /// <summary>
        /// Function to validate a UserActionClientDataEntity before it's saved.
        /// </summary>
        /// <param name="userActionClientDataEntity">UserActionClientDataEntity to validate</param>
        /// <param name="session">User's session identifier.</param>
        /// <returns>null if the UserActionClientDataEntity was deleted successfully, the same UserActionClientDataEntity otherwise</returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="userActionClientDataEntity"/> is null.
        /// </exception>
        /// <exception cref="UtnEmallBusinessLogicException">
        /// If an UtnEmallDataAccessException occurs in DataModel.
        /// </exception>
        public bool Validate(UtnEmall.Client.EntityModel.UserActionClientDataEntity userActionClientData)
        {
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateRequest request = new UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateRequest(userActionClientData);
            UtnEmall.Client.ServiceAccessLayer.IUserActionClientDataValidateResponse response = this.Validate(request);
            return response.ValidateResult;
        }

    }
}

