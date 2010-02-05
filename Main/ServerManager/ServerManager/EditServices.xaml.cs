using System;
using System.Collections.Generic;
using System.Windows;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para administrar servicios de una lista de servicios disponibles.
    /// </summary>
    public partial class EditServices
    {
        #region Instance Variables and Properties

        private Dictionary<string, ServiceEntity> available;
        /// <summary>
        /// Un diccionario con el nombre del servicio como clave y el objeto de servicio como valor representa los servicios disponibles.
        /// </summary>
        public Dictionary<string, ServiceEntity> Available
        {
            get { return available; }
        }

        private CampaignEntity campaign;
        /// <summary>
        /// La campaña de la cual los servicios se están editando.
        /// </summary>
        public CampaignEntity Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        /// <summary>
        /// Una lista con los servicios seleccionados.
        /// </summary>
        public ReadOnlyCollection<ServiceEntity> Selected
        {
            get
            {
                List<ServiceEntity> selectedServices = new List<ServiceEntity>();

                foreach (string name in Selector.Selected)
                {
                    selectedServices.Add(available[name]);
                }
                return (new ReadOnlyCollection<ServiceEntity>(selectedServices));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public EditServices()
        {
            this.InitializeComponent();
            available = new Dictionary<string, ServiceEntity>();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia los servicios seleccionados.
        /// </summary>
        public void Clear()
        {
            Selector.Clear();
            available.Clear();
        }

        /// <summary>
        /// Agrega un servicio a la lista de origen.
        /// </summary>
        /// <param name="service">
        /// Un objeto de servicio.
        /// </param>
        public void AddSourceService(ServiceEntity service)
        {
            Selector.AddSource(service.Name);
            available.Add(service.Name, service);
        }

        /// <summary>
        /// Agrega el servicio a la lista de destino.
        /// </summary>
        /// <param name="service">
        /// Un objeto de servicio.
        /// </param>
        public void AddDestinationService(ServiceEntity service)
        {
            Selector.AddDestination(service.Name);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Método invocado cuando se hace click en el botón Aceptar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            System.Collections.Generic.List<ServiceCampaignEntity> toRemove = new List<ServiceCampaignEntity>();

            // Agrega los servicios quitadas a la lista.
            foreach (ServiceCampaignEntity serviceCampaign in campaign.ServiceCampaign)
            {
                bool exists = false;

                foreach (ServiceEntity service in Selected)
                {
                    if (serviceCampaign.Service.Name == service.Name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    toRemove.Add(serviceCampaign);
                }
            }

            // Quitarlas.
            foreach (ServiceCampaignEntity serviceCampaign in toRemove)
            {
                campaign.ServiceCampaign.Remove(serviceCampaign);
            }

            // Agregar los nuevos servicios.
            foreach (ServiceEntity service in Selected)
            {
                bool exists = false;

                foreach (ServiceCampaignEntity serviceCampaign in campaign.ServiceCampaign)
                {
                    if (serviceCampaign.Service.Name == service.Name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    ServiceCampaignEntity serviceCampaign = new ServiceCampaignEntity();
                    serviceCampaign.Campaign = campaign;

                    campaign.ServiceCampaign.Add(serviceCampaign);
                }
            }

            if (OkSelected != null)
            {
                OkSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Cancelar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Evento lanzado cuando se selecciona el botón Aceptar.
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// Evento lanzado cuando se selecciona el botón Cancelar.
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}