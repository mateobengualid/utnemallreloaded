using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using System.Windows;
using PresentationLayer.ServerDesigner;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager
{
    class ServiceController : ControllerBase
    {
        private WindowDesigner windowServiceDesigner;

        /// <summary>
        /// La entidad seleccionada en el administrador
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((ServiceManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((ServiceEditor)Editor).Mode;
            }

            set
            {
                ((ServiceEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que se está agregando o modificando
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((ServiceEditor)Editor).Service;
            }

            set
            {
                ((ServiceEditor)Editor).Service = (ServiceEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">Control que contiene a este componente</param>
        /// <param name="manager">Componente que muestra una lista de entidades</param>
        /// <param name="editor">El componente que permite agregar o editar una entidad</param>
        /// <param name="firstElement">El componente que debe tener el foco cuando se muestra el editor</param>
        public ServiceController(UserControl1 control, ServiceManager manager, ServiceEditor editor,
            FrameworkElement firstElement)
            : base(control, manager, editor, firstElement, new LoadList(ServiceManager.Load),
            new SaveEntity(ServiceManager.Save),
            new RemoveEntity(ServiceManager.Delete))
        {
            ServiceManager serviceManager = (ServiceManager)manager;
            ServiceEditor addService = (ServiceEditor)editor;

            addService.OkSelected += OnOkSelected;
            addService.CancelSelected += OnCancelSelected;

            serviceManager.ItemList.NewButtonSelected += OnNewSelected;
            serviceManager.ItemList.EditButtonSelected += OnEditSelected;
            serviceManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            serviceManager.ItemList.ExtraButtonSelected += OnExtraSelected;
            serviceManager.ItemList.ExtraButton1Selected += OnExtra1Selected;
        }

        /// <summary>
        /// Elimina el elemento seleccionado
        /// </summary>
        /// <returns>La entidad eliminada</returns>
        protected override IEntity DeleteSelected()
        {
            return ((ServiceManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Recarga el contenido del administrador
        /// </summary>
        /// <param name="args">Argumento para el hilo de carga</param>
        protected override void Reload(LoaderArguments args)
        {
            ServiceManager serviceManager = ((ServiceManager)Manager);
            serviceManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                serviceManager.list.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona un botón en el menú
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        public override void OnSelected(object sender, EventArgs e)
        {
            base.OnSelected(sender, e);
            LoadStores();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón OK en el editor
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        protected override void OnOkSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;

            ((ServiceEditor)Editor).StartDate.Date = DateTime.Now;
            ((ServiceEditor)Editor).StopDate.Date = DateTime.Now.AddMonths(1);

            Save(Entity);
        }

        private void WindowServiceDesignerClosed(object sender, EventArgs e)
        {
            if (windowServiceDesigner.Result == true && windowServiceDesigner.Document.ServiceEntity != null)
            {
                windowServiceDesigner.Document.ServiceEntity.CustomerServiceData.Service = windowServiceDesigner.Document.ServiceEntity;
                Save(windowServiceDesigner.Document.ServiceEntity);
            }

            Load();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Extra en el administrador
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            try
            {
                ServiceEntity serviceEntity = (ServiceEntity)Selected;

                if (serviceEntity == null)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoServiceSelected);
                    return;
                }

                if (windowServiceDesigner != null && windowServiceDesigner.IsVisible)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ServiceDesignerOpened);
                    return;
                }

                windowServiceDesigner = new WindowDesigner(serviceEntity, Control.Session);
                windowServiceDesigner.Closed += new EventHandler(WindowServiceDesignerClosed);
                windowServiceDesigner.ShowDialog();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
                windowServiceDesigner.Close();
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón de selección en el administrador
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            base.OnNewSelected(sender, e);
            ((ServiceEditor)Editor).Session = Control.Session;
            ((ServiceEditor)Editor).StartDate.Date = DateTime.Now;
            ((ServiceEditor)Editor).StopDate.Date = DateTime.Now.AddMonths(1);
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón de Editar
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        protected override void OnEditSelected(object sender, EventArgs e)
        {
            base.OnEditSelected(sender, e);
            ((ServiceEditor)Editor).Session = Control.Session;
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Extra1
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        protected override void OnExtra1Selected(object sender, EventArgs e)
        {
            ServiceEntity serviceEntity = (ServiceEntity)Selected;

            if (serviceEntity == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoServiceSelected);
                return;
            }

            if (serviceEntity.Deployed)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.AlreadyBuilt);
                return;
            }

            if (serviceEntity.CustomerServiceData != null)
            {
                ((ServiceManager)Manager).BuildProgress.Visibility = Visibility.Visible;
                ((ServiceManager)Manager).Disable();
                CustomServiceBuilder builder = new CustomServiceBuilder(Control, serviceEntity.CustomerServiceData);
                builder.BuildFinished += new EventHandler(BuildFinished);
                builder.Build();
            }
            else
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.CantBuildEmptyService);
            }
        }

        private void BuildFinished(object sender, EventArgs e)
        {
            CustomServiceBuilder builder = (CustomServiceBuilder)sender;
            ((ServiceManager)Manager).BuildProgress.Visibility = Visibility.Hidden;
            ((ServiceManager)Manager).Enable();

            if (!builder.Succeed)
            {
                Util.ShowErrorDialog(builder.Message);
            }
            else
            {
                Util.ShowInformationDialog(Resources.ServiceBuilt, Resources.Information);
            }

            Load();
        }

        /// <summary>
        /// Carga las tiendas en el editor
        /// </summary>
        public virtual void LoadStores()
        {
            Loader.Load(Control, StoreManager.Load, Control.Session, false,
                new OnFinished(OnLoadStoresFinished));
        }

        /// <summary>
        /// Método invocado cuando se finaliza la llamada al servicio web
        /// </summary>
        /// <param name="args">El argumento del hilo de carga</param>
        protected void OnLoadStoresFinished(LoaderArguments args)
        {
            if (!args.Succeed)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadStoreListFailed);
                ((ServiceEditor)Editor).StoreList = new List<StoreEntity>();
            }
            else
            {
                List<StoreEntity> list = new List<StoreEntity>();

                foreach(StoreEntity store in args.Items)
                {
                    list.Add((StoreEntity)store);
                }
                
                ((ServiceEditor)Editor).StoreList = list;
            }
        }
    }
}
