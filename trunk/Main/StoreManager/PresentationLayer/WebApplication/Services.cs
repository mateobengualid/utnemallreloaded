using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ServiceModel;
using WebApplication.ServiceReferenceStore;
using WebApplication.ServiceReferenceCategory;
using WebApplication.ServiceReferenceGroup;
using WebApplication.ServiceReferenceUser;
using WebApplication.ServiceReferenceCustomer;
using WebApplication.ServiceReferenceSessionManager;
using WebApplication.ServiceReferenceDataModel;
using WebApplication.ServiceReferenceService;
using WebApplication.ServiceReferenceStatisticsAnalyzer;
using WebApplication.ServiceReferenceCustomerServiceData;
using WebApplication.ServiceReferenceServiceBuilder;

namespace WebApplication
{
    public class Services
    {
        #region Constants, Variables and Properties

        private const string port = "8081";

        private static string server;
        public static string Server
        {
            set
            {
                server = value;
            }
        }

        private static StoreClient store;
        public static StoreClient Store
        {
            get
            {
                if (store == null)
                {
                    store = new StoreClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("Store")));
                }

                return store;
            }
        }

        private static CustomerServiceDataClient customerServiceData;
        public static CustomerServiceDataClient CustomerServiceData
        {
            get
            {
                if (customerServiceData == null)
                {
                    customerServiceData = new CustomerServiceDataClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("CustomerServiceData")));
                }

                return customerServiceData;
            }
        }

        private static CategoryClient category;
        public static CategoryClient Category
        {
            get
            {
                if (category == null)
                {
                    category = new CategoryClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("Category")));
                }

                return category;
            }
        }

        private static ServiceClient service;
        public static ServiceClient Service
        {
            get
            {
                if (service == null)
                {
                    service = new ServiceClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("Service")));
                }

                return service;
            }
        }

        private static GroupClient group;
        public static GroupClient Group
        {
            get
            {
                if (group == null)
                {
                    group = new GroupClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("Group")));
                }

                return group;
            }
        }

        private static UserClient user;
        public static UserClient User
        {
            get
            {
                if (user == null)
                {
                    user = new UserClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("User")));
                }

                return user;
            }
        }

        private static CustomerClient customer;
        public static CustomerClient Customer
        {
            get
            {
                if (customer == null)
                {
                    customer = new CustomerClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("Customer")));
                }

                return customer;
            }
        }

        private static DataModelClient dataModel;
        public static DataModelClient DataModel
        {
            get
            {
                if (dataModel == null)
                {
                    dataModel = new DataModelClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("DataModel")));
                }

                return dataModel;
            }
        }

        private static SessionManagerClient session;
        public static SessionManagerClient Session
        {
            get
            {
                if (session == null)
                {
                    session = new SessionManagerClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("SessionManager")));
                }

                return session;
            }
        }

        private static StatisticsAnalyzerClient statisticsAnalyzer;
        public static StatisticsAnalyzerClient StatisticsAnalyzer
        {
            get
            {
                if (statisticsAnalyzer == null)
                {
                    statisticsAnalyzer = new StatisticsAnalyzerClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("StatisticsAnalyzer")));
                }

                return statisticsAnalyzer;
            }
        }      

        private static ServiceBuilderClient serviceBuilder;
        public static ServiceBuilderClient ServiceBuilder 
        {
            get 
            {
                if (serviceBuilder == null)
                {
                    serviceBuilder = new ServiceBuilderClient(new BasicHttpBinding(), new EndpointAddress(GetServiceURL("ServiceBuilder")));
                }

                return serviceBuilder;
            }
        }

        #endregion

        #region Static Methods

        public static string GetServiceURL(string serviceName)
        {
            return "http:// " + server + ":" + port + "/" + serviceName;
        }

        #endregion
    }
}
