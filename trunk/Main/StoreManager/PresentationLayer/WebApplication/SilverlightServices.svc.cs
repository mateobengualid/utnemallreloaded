using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using UtnEmall.Server.EntityModel;
using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Web.Services;
using System.ServiceModel.Web;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.Base;
using System.IO;
using System.Xml;
using System.Text;

namespace WebApplication
{
    [ServiceContract(Namespace = "http://tempuri.org", SessionMode = SessionMode.Allowed)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SilverlightServices
    {
        HttpSessionState Session;

        string UserSession
        {
            get
            {
                return (string)Session[SessionConstants.UserSession];
            }
        }
        UserEntity User
        {
            get
            {
                return (UserEntity)Session[SessionConstants.User];
            }
        }

        SilverlightServices()
        {
            this.Session = HttpContext.Current.Session;
        }

        // Add more operations here and mark them with [OperationContract]
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Xml,
        ResponseFormat = WebMessageFormat.Xml,
        BodyStyle = WebMessageBodyStyle.Bare)]
        public bool SaveDataModel(DataModelEntity dataModelEntity)
        {
            try
            {
                CircularReferencesManager.FixDataModelCircularReferences(dataModelEntity);
                DataModelEntity lastDataModel = InternalLoadDataModel();
                if (lastDataModel == null)
                {
                    lastDataModel = dataModelEntity;
                    lastDataModel.IdStore = this.User.IdStore;
                }
                else
                {
                    lastDataModel.Tables = dataModelEntity.Tables;
                    lastDataModel.Relations = dataModelEntity.Relations;
                }
                DataModelEntity result = ServicesClients.DataModel.Save(lastDataModel, this.UserSession);
                return result == null;
            }
            catch (UtnEmallPermissionException permissionException)
            {
                throw new FaultException(permissionException.Message);
            }
            catch (UtnEmallBusinessLogicException businessLogicException)
            {
                throw new FaultException(businessLogicException.Message);
            }
        }

        private DataModelEntity InternalLoadDataModel()
        {
            UserEntity userEntity = this.User;
            DataModelEntity[] dataModels = ServicesClients.DataModel.GetDataModelWhere(DataModelEntity.DBIdStore, userEntity.IdStore, true, OperatorType.Equal, this.UserSession);
            if (dataModels.Length > 0)
            {
                return dataModels[0];
            }
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Xml,
        ResponseFormat = WebMessageFormat.Xml,
        BodyStyle = WebMessageBodyStyle.Bare)]
        public DataModelEntity GetDataModel()
        {
            DataModelEntity dataModel = InternalLoadDataModel();
            if (dataModel != null)
            {
                CircularReferencesManager.BrokeDataModelCircularReferences(dataModel);
                return dataModel;
            }
            return null;
        }

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Xml,
        ResponseFormat = WebMessageFormat.Xml,
        BodyStyle = WebMessageBodyStyle.Bare)]
        [UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
        public CustomerServiceDataEntity GetCustomerServiceData(int customerServiceId)
        {
            CustomerServiceDataEntity customerService = ServicesClients.CustomerServiceData.GetCustomerServiceData(
                customerServiceId, true, this.UserSession);

            if (customerService != null)
            {
                CircularReferencesManager.BrokeCustomerServiceDataCircularReference(customerService);
                return customerService;
            }
            return null;
        }

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Xml,
        ResponseFormat = WebMessageFormat.Xml,
        BodyStyle = WebMessageBodyStyle.Bare)]
        public bool SaveCustomerServiceData(CustomerServiceDataEntity customerServiceDataEntity)
        {
            ServiceEntity currentService = null;

            // Carga el servicio.
            currentService = ServicesClients.Service.GetService((int)Session[SessionConstants.IdCurrentService], true, this.UserSession);
            if (currentService != null)
            {
                // Corrige referencias circulares.
                customerServiceDataEntity.DataModel = this.InternalLoadDataModel();
                CircularReferencesManager.FixCustomerServiceDataCircularReferences(customerServiceDataEntity);
                // Asigna los datos del servicio personalizado al servicio.
                currentService.CustomerServiceData = customerServiceDataEntity;
                customerServiceDataEntity.Service = currentService;

                // Guarda el servicio.
                ServiceEntity result = ServicesClients.Service.Save(currentService, this.UserSession);
                if (result == null)
                {
                    return true;
                }
            }
            // ...sino retorna falso.
            return false;
        }

    }
}
