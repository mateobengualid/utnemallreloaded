using System.Windows;
using System;
using UtnEmall.Server.EntityModel;
using System.ServiceModel;
using WebApplication;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary;
using System.Reflection.Emit;
using System.Windows.Controls;
using System.Xml;
using LogicalLibrary.ServerDesignerClasses;
using UtnEmall.Proxies;
using System.Windows.Browser;
using System.Globalization;
using System.Threading;

namespace SilverlightVisualDesigners
{
    // Tipos públicos de delegados.
    public delegate void DataModelLoadedCallback(DataModel dataModel);
    public delegate void CustomerServiceLoadedCallback(ServiceDocument dataModel);

	public partial class App : Application 
	{
        // ID del servicio a cargar.
        int customServiceId;
        // Modo de diseño (Modelo de datos o servicio personalizado.)
        DesignerMode designerMode;
        // Callback de carga de modelo de datos.
        DataModelLoadedCallback dataModelLoadedCallback;
        // Modelo de datos en la base de datos.
        DataModelEntity dataModelEntity;

        public int CustomServiceId
        {
            get
            {
                return customServiceId;
            }
        }

		public App() 
		{
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ES-es");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ES-es");

			this.Startup += this.OnStartup;
			this.Exit += this.OnExit;
			InitializeComponent();
        }

		private void OnStartup(object sender, StartupEventArgs e) 
		{
			// Carga del control principal aqui.
            this.customServiceId = Convert.ToInt32(e.InitParams[SessionConstants.IdCustomerServiceDataToUse], CultureInfo.InvariantCulture);
            this.designerMode = (DesignerMode)(Convert.ToInt32(e.InitParams[SessionConstants.DesignerMode],CultureInfo.InvariantCulture));
            
            if (designerMode == DesignerMode.ServiceDesigner)
            {
                this.RootVisual = new ServiceDesignerSilverlight(this.customServiceId);
            }
            else
            {
                this.RootVisual = new DataModelDesignerSilverlight();
            }
		}

        public void BeginSaveDataModel(DataModel dataModel, SaveDataModelCallback callback)
        {
            try
            {
                DataModelEntity dataModelEntity = Utilities.ConvertDataModelToEntity(dataModel);
                dataModelEntity.IdStore = customServiceId;
                CircularReferencesManager.BrokeDataModelCircularReferences(dataModelEntity);

                SilverlightServicesClient client = Utils.SilverlightServicesClient;
                client.BeginSaveDataModel(dataModelEntity, callback);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void BeginSaveCustomerService(ServiceDocument serviceDocument, SaveCustomerServiceCallback callback)
        {
            try
            {                
                CustomerServiceDataEntity customerServiceData = Utilities.ConvertServiceModelToEntity(serviceDocument, this.dataModelEntity);                
                CircularReferencesManager.BrokeCustomerServiceDataCircularReference(customerServiceData);                

                SilverlightServicesClient client = Utils.SilverlightServicesClient;
                client.BeginSaveCustomerServiceData(customerServiceData, callback);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BeginLoadDataModel(DataModelLoadedCallback callback)
        {
            dataModelLoadedCallback = callback;
            SilverlightServicesClient client = Utils.SilverlightServicesClient;
            client.BeginGetDataModel(new GetDataModelCallback(GetDataModelCompleted));
        }        

        private void GetDataModelCompleted(GetDataModelResult result)
        {            
            dataModelEntity = result.DataModel;
            if (dataModelEntity != null)
            {
                CircularReferencesManager.FixDataModelCircularReferences(dataModelEntity);                
                DataModel dataModel = Utilities.ConvertEntityToDataModel(dataModelEntity);
                if (dataModelLoadedCallback != null)
                {
                    dataModelLoadedCallback(dataModel);
                    dataModelLoadedCallback = null;
                }
            }
        }

        public void LoadService(int idService, CustomerServiceLoadedCallback callback)
        {
            DataModel currentDataModel = null;
            // Carga del modelo de datos.
            BeginLoadDataModel( 
                delegate(DataModel dataModel){
                    currentDataModel = dataModel;
                    ServiceDocument document = document = new ServiceDocument();
                    document.DataModel = dataModel;
                    if (idService > 0)
                    {
                        // Es un servicio existente.
                        SilverlightServicesClient client = Utils.SilverlightServicesClient;
                        client.BeginGetCustomerServiceData(idService,
                            delegate(GetCustomerServiceResult result)
                            {
                                if (result.CustomerService != null)
                                {
                                    result.CustomerService.DataModel = dataModelEntity;
                                    CircularReferencesManager.FixCustomerServiceDataCircularReferences(result.CustomerService);
                                    Utilities.ConvertEntityToServiceModel(result.CustomerService, document);
                                }
                                if (callback != null)
                                {
                                    callback(document);
                                }
                            }
                        );
                    }
                    else if (callback != null)
                    {
                        callback(document);
                    }
                }
             );
        }


        private void OnExit(object sender, EventArgs e)
        {

        }
    }
}