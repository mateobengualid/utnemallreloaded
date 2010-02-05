using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;

namespace UtnEmall.ServerManager
{
    class CampaignController : ControllerBase
    {
		private EditServices editServices;

        /// <summary>
        /// La entidad seleccionada por el administrador.
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((CampaignManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor puede ser Agregar o Editar
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((CampaignEditor)Editor).Mode;
            }

            set
            {
                ((CampaignEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que está siendo agregada o modificada en el editor.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((CampaignEditor)Editor).Campaign;
            }

            set
            {
                ((CampaignEditor)Editor).Campaign = (CampaignEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">Una referencia al control que contiene a este componente</param>
        /// <param name="manager">El componente que muestra un listado de entidades</param>
        /// <param name="editor">El componente que permite agregar o editar una entidad</param>
        /// <param name="firstElement">El componente que debe ser seleccionado cuando se muestra el editor</param>
        public CampaignController(UserControl1 control, CampaignManager manager, CampaignEditor editor,
            FrameworkElement firstElement, EditServices editServices)
            : base(control, manager, editor, firstElement, new LoadList(CampaignManager.Load),
            new SaveEntity(CampaignManager.Save),
            new RemoveEntity(CampaignManager.Delete))
        {
            CampaignManager campaignManager = (CampaignManager)manager;
            CampaignEditor addCampaign = (CampaignEditor)editor;

            this.editServices = editServices;

            addCampaign.OkSelected += OnOkSelected;
            addCampaign.CancelSelected += OnCancelSelected;

            campaignManager.ItemList.NewButtonSelected += OnNewSelected;
            campaignManager.ItemList.EditButtonSelected += OnEditSelected;
            campaignManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            campaignManager.ItemList.ExtraButtonSelected += OnExtraSelected;

            editServices.OkSelected += new EventHandler(OnEditServicesOkSelected);
            editServices.CancelSelected += new EventHandler(OnEditServicesCancelSelected);
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón OK
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void OnEditServicesOkSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;

            Save(editServices.Campaign);
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Cancelar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void OnEditServicesCancelSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;
        }

        /// <summary>
        /// Borrar el elemento seleccionado
        /// </summary>
        /// <returns>la entidad eliminada</returns>
        protected override IEntity DeleteSelected()
        {
            return ((CampaignManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Recargar el contenido del administrador
        /// </summary>
        /// <param name="args">El argumento del hilo de carga</param>
        protected override void Reload(LoaderArguments args)
        {
            CampaignManager campaignManager = ((CampaignManager)Manager);
            campaignManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                campaignManager.list.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            Mode = EditionMode.Add;
            Control.HideLastShown();
            Editor.Visibility = Visibility.Visible;
            Control.LastElementShown = Editor;
            FocusFirst();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Editar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnEditSelected(object sender, EventArgs e)
        {
            CampaignEditor addCampaign = (CampaignEditor)Editor;
            CampaignEntity campaign;
            Mode = EditionMode.Edit;

            if ((campaign = (CampaignEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoCampaignSelected);
                return;
            }

            addCampaign.Campaign = campaign;

            if (campaign == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoCampaignSelected);
                return;
            }

            Control.HideLastShown();
            addCampaign.Visibility = Visibility.Visible;
            Control.LastElementShown = addCampaign;
        }

        /// <summary>
        /// Método invocado cuando el botón extra es seleccionado en el administrador
        /// </summary>
        /// <param name="sender">El objeto que envía la señal</param>
        /// <param name="e">Los argumentos del evento</param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            CampaignEntity entity;

            if ((entity = (CampaignEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoCampaignSelected);
                return;
            }

            editServices.Clear();

            try
            {
                foreach (ServiceEntity service in Services.Service.GetAllService(false, Control.Session))
                {
                    editServices.AddSourceService(service);
                }
            }
            catch (TargetInvocationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorServicesNotLoaded);
                return;
            }
            catch (CommunicationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorServicesNotLoaded);
                return;
            }

            foreach (ServiceCampaignEntity serviceCampaign in entity.ServiceCampaign)
            {
                editServices.AddDestinationService(serviceCampaign.Service);
            }

            editServices.Campaign = entity;
            Control.HideLastShown();
            editServices.Visibility = Visibility.Visible;
            Control.LastElementShown = editServices;
        }
    }
}
