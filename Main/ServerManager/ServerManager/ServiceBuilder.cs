using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager
{
    class ServiceBuilder
    {
        /// <summary>
        /// Indica el estado de la operación
        /// </summary>
        public bool Succeed
        {
            get;
            set;
        }

        public bool InserTestData
        {
            get;
            set;
        }

        private DataModelEntity dataModel;
        /// <summary>
        /// El modelo de datos del servicio
        /// </summary>
        public DataModelEntity DataModel
        {
            get
            {
                return dataModel;
            }
        }

        private bool building;

        /// <summary>
        /// El componente que invoca al evento
        /// </summary>
        public UserControl1 Control
        {
            get;
            set;
        }

        /// <summary>
        /// Mensaje de estado de operación
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">El componente que invoca al evento</param>
        /// <param name="dataModel">El modelo de datos del servicio</param>
        public ServiceBuilder(UserControl1 control, DataModelEntity dataModel)
        {
            Control = control;
            this.dataModel = dataModel;
        }

        /// <summary>
        /// Construir el servicio
        /// </summary>
        public void Build()
        {
            if (building)
            {
                return;
            }
            Services.ServiceBuilder.BuildAndImplementInfrastructureServiceCompleted += new EventHandler<ServerManager.SAL.ServiceBuilder.BuildAndImplementInfrastructureServiceCompletedEventArgs>(ServiceBuilder_BuildAndImplementInfrastructureServiceCompleted);
            Services.ServiceBuilder.BuildAndImplementInfrastructureServiceAsync(DataModel, InserTestData, Control.Session);
            building = true;
        }

        /// <summary>
        /// Método invocado cuando se finaliza el proceso
        /// </summary>
        void ServiceBuilder_BuildAndImplementInfrastructureServiceCompleted(object sender, ServerManager.SAL.ServiceBuilder.BuildAndImplementInfrastructureServiceCompletedEventArgs e)
        {
            building = false;
            Services.ServiceBuilder.BuildAndImplementInfrastructureServiceCompleted -= new EventHandler<ServerManager.SAL.ServiceBuilder.BuildAndImplementInfrastructureServiceCompletedEventArgs>(ServiceBuilder_BuildAndImplementInfrastructureServiceCompleted);
            if (e.Cancelled)
            {
                Succeed = false;
            }
            else if (e.Error != null)
            {
                Succeed = false;
                Message = Resources.ServiceBuildError;
            }
            else
            {
                Succeed = e.Result;
            }

            if (BuildFinished != null)
            {
                Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
                {
                    BuildFinished(this, new EventArgs());
                }));
            }
        }

        /// <summary>
        /// Evento creado cuando se finaliza el proceso de construcción de servicio
        /// </summary>
        public event EventHandler BuildFinished;
    }
}
