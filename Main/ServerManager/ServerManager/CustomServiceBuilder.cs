using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Threading;

namespace UtnEmall.ServerManager
{
    class CustomServiceBuilder
    {
        /// <summary>
        /// El estado del proceso.
        /// </summary>
        public bool Succeed
        {
            get;
            set;
        }

        private CustomerServiceDataEntity customerServiceData;
        /// <summary>
        /// Los datos de servicio de cliente a construir.
        /// </summary>
        public CustomerServiceDataEntity CustomerServiceData
        {
            get
            {
                return customerServiceData;
            }
        }

        private bool building;

        /// <summary>
        /// El componente que será usado para invocar el evento en el hilo de la interfaz.
        /// </summary>
        public UserControl1 Control
        {
            get;
            set;
        }

        /// <summary>
        /// Un mensaje describiendo el estado del proceso.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="control">El componente que será usado para invocar el evento en el hilo de la interfaz.</param>
        /// <param name="customerServiceData">Los datos del servicio del cliente a construir.</param>
        public CustomServiceBuilder(UserControl1 control, CustomerServiceDataEntity customerServiceData)
        {
            Control = control;
            this.customerServiceData = customerServiceData;
        }

        private void DoBuild()
        {
            try
            {
                if (!Services.ServiceBuilder.BuildAndImplementCustomService(CustomerServiceData, Control.Session))
                {
                    Message = UtnEmall.ServerManager.Properties.Resources.ServiceBuildError;
                    Succeed = false;
                }
                else
                {
                    Succeed = true;
                }
            }
            catch (TargetInvocationException)
            {
                Succeed = false;
                Message = UtnEmall.ServerManager.Properties.Resources.ServiceBuildError;
            }
            catch (CommunicationException)
            {
                Succeed = false;
                Message = UtnEmall.ServerManager.Properties.Resources.ServiceBuildError;
            }

            building = false;

            if (BuildFinished != null)
            {
                Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new VoidCallback(delegate()
                {
                    BuildFinished(this, new EventArgs());
                }));
            }
        }

        public void Build()
        {
            if (!building)
            {
                building = true;
                Thread build = new Thread(new ThreadStart(DoBuild));
                build.Start();
            }
        }

        /// <summary>
        /// Evento lanzado cuando el proceso termina.
        /// </summary>
        public event EventHandler BuildFinished;
    }
}
