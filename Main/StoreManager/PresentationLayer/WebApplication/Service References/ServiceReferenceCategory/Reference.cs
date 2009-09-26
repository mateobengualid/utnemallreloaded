﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.ServiceReferenceCategory {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReferenceCategory.ICategory")]
    public interface ICategory {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/Save", ReplyAction="http://tempuri.org/ICategory/SaveResponse")]
        UtnEmall.Server.EntityModel.CategoryEntity Save(UtnEmall.Server.EntityModel.CategoryEntity categoryEntity, string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/Delete", ReplyAction="http://tempuri.org/ICategory/DeleteResponse")]
        UtnEmall.Server.EntityModel.CategoryEntity Delete(UtnEmall.Server.EntityModel.CategoryEntity categoryEntity, string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/GetCategory", ReplyAction="http://tempuri.org/ICategory/GetCategoryResponse")]
        UtnEmall.Server.EntityModel.CategoryEntity GetCategory(int id, bool loadRelation, string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/GetAllCategory", ReplyAction="http://tempuri.org/ICategory/GetAllCategoryResponse")]
        System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity> GetAllCategory(bool loadRelation, string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/GetCategoryWhere", ReplyAction="http://tempuri.org/ICategory/GetCategoryWhereResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.Base.OperatorType))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.CategoryEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.Error>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.Error))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.PreferenceEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.PreferenceEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.CustomerEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.MallEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ServiceEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ServiceEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.StoreEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.StoreCategoryEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.StoreCategoryEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.DataModelEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.DataModelEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.TableEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.TableEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.FieldEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.FieldEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ComponentEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.CustomerServiceDataEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ComponentEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ConnectionWidgetEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ConnectionWidgetEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ConnectionPointEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.RelationEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.RelationEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.UserEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.UserEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.UserGroupEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.UserGroupEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.GroupEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.PermissionEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.PermissionEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ServiceCategoryEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ServiceCategoryEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.StoreEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.CustomerEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.DeviceProfileEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.DeviceProfileEntity))]
        System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Server.Base.OperatorType operatorType, string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/GetCategoryWhereEqual", ReplyAction="http://tempuri.org/ICategory/GetCategoryWhereEqualResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.Base.OperatorType))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.CategoryEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.Error>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.Error))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.PreferenceEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.PreferenceEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.CustomerEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.MallEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ServiceEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ServiceEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.StoreEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.StoreCategoryEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.StoreCategoryEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.DataModelEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.DataModelEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.TableEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.TableEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.FieldEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.FieldEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ComponentEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.CustomerServiceDataEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ComponentEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ConnectionWidgetEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ConnectionWidgetEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ConnectionPointEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.RelationEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.RelationEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.UserEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.UserEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.UserGroupEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.UserGroupEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.GroupEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.PermissionEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.PermissionEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.ServiceCategoryEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.ServiceCategoryEntity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.StoreEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.CustomerEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<UtnEmall.Server.EntityModel.DeviceProfileEntity>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(UtnEmall.Server.EntityModel.DeviceProfileEntity))]
        System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategory/Validate", ReplyAction="http://tempuri.org/ICategory/ValidateResponse")]
        bool Validate(UtnEmall.Server.EntityModel.CategoryEntity category);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ICategoryChannel : WebApplication.ServiceReferenceCategory.ICategory, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class CategoryClient : System.ServiceModel.ClientBase<WebApplication.ServiceReferenceCategory.ICategory>, WebApplication.ServiceReferenceCategory.ICategory {
        
        public CategoryClient() {
        }
        
        public CategoryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CategoryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CategoryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CategoryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public UtnEmall.Server.EntityModel.CategoryEntity Save(UtnEmall.Server.EntityModel.CategoryEntity categoryEntity, string session) {
            return base.Channel.Save(categoryEntity, session);
        }
        
        public UtnEmall.Server.EntityModel.CategoryEntity Delete(UtnEmall.Server.EntityModel.CategoryEntity categoryEntity, string session) {
            return base.Channel.Delete(categoryEntity, session);
        }
        
        public UtnEmall.Server.EntityModel.CategoryEntity GetCategory(int id, bool loadRelation, string session) {
            return base.Channel.GetCategory(id, loadRelation, session);
        }
        
        public System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity> GetAllCategory(bool loadRelation, string session) {
            return base.Channel.GetAllCategory(loadRelation, session);
        }
        
        public System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, UtnEmall.Server.Base.OperatorType operatorType, string session) {
            return base.Channel.GetCategoryWhere(propertyName, expValue, loadRelation, operatorType, session);
        }
        
        public System.Collections.Generic.List<UtnEmall.Server.EntityModel.CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session) {
            return base.Channel.GetCategoryWhereEqual(propertyName, expValue, loadRelation, session);
        }
        
        public bool Validate(UtnEmall.Server.EntityModel.CategoryEntity category) {
            return base.Channel.Validate(category);
        }
    }
}
