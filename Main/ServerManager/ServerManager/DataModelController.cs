using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using PresentationLayer.DataModelDesigner;
using System.Windows;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager
{
    class DataModelController : ControllerBase
    {
        #region Constants, Variables and Properties

        private WindowDataModelDesigner designer;
        private DataModelManager dataModelManager;

        /// <summary>
        /// La entidad seleccionada en el administrador.
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((DataModelManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor, puede ser "Add" o "Edit".
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return EditionMode.Add;
            }

            set
            {

            }
        }

        /// <summary>
        /// La entidad que se está agregando o modificando en el editor.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((DataModelEditor)Editor).DataModel;
            }

            set
            {
                ((DataModelEditor)Editor).DataModel = (DataModelEntity)value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="control">Referencia al control que contiene este componente.</param>
        /// <param name="manager">Componente que muestra la lista de entidades.</param>
        /// <param name="editor">Componente que permite agregar o editar una entidad.</param>
        /// <param name="firstElement">Componente que debe enfocarse cuando el editor se muestra.</param>
        public DataModelController(UserControl1 control, DataModelManager manager, DataModelEditor editor,
            FrameworkElement firstElement)
            : base(control, manager, editor, firstElement, new LoadList(DataModelManager.Load),
            new SaveEntity(DataModelManager.Save),
            new RemoveEntity(DataModelManager.Delete))
        {
            dataModelManager = (DataModelManager)manager;
            DataModelEditor addDataModel = (DataModelEditor)editor;

            addDataModel.OkSelected += OnOkSelected;
            addDataModel.CancelSelected += OnCancelSelected;

            dataModelManager.ItemList.NewButtonSelected += OnNewSelected;
            dataModelManager.ItemList.EditButtonSelected += OnEditSelected;
            dataModelManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            dataModelManager.ItemList.ExtraButtonSelected += OnExtraSelected;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Elimina el elemento seleccionado.
        /// </summary>
        /// <returns>La entidad eliminada.</returns>
        protected override IEntity DeleteSelected()
        {
            return ((DataModelManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Nuevo.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            ReadOnlyCollection<string> usedStores = new ReadOnlyCollection<string>(((DataModelManager)Manager).UsedStores);

            if (!((DataModelEditor)Editor).LoadStores(Control.Session, usedStores))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadStoreListFailed);
                return;
            }

            Mode = EditionMode.Add;
            Control.HideLastShown();
            Editor.Visibility = Visibility.Visible;
            Control.LastElementShown = Editor;
            FocusFirst();
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Editar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnEditSelected(object sender, EventArgs e)
        {
            DataModelEntity dataModel = (DataModelEntity)Selected;

            if (dataModel == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoDataModelSelected);
                return;
            }

            if (designer != null && designer.IsVisible)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.DesignerOpened);
                return;
            }

            designer = new WindowDataModelDesigner(dataModel);
            designer.Closed += new EventHandler(OnDesignerClosed);
            designer.ShowDialog();
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Eliminar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnDeleteSelected(object sender, EventArgs e)
        {
            DataModelEntity entity;

            if ((entity = (DataModelEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoDataModelSelected);
                return;
            }

            // Si la tienda es nueva, significa que esa tienda es la tienda ficticia
            // del shopping.
            if (entity.Store.IsNew)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.CantDeleteMallDataModel);
                return;
            }

            if (Util.ShowConfirmDialog(UtnEmall.ServerManager.Properties.Resources.ConfirmDeleteDataModel, UtnEmall.ServerManager.Properties.Resources.Confirm))
            {
                DeleteSelected();
                Delete(entity);
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario.
        /// </summary>
        /// <param name="args">Los argumentos del hilo que carga.</param>
        protected override void Reload(LoaderArguments args)
        {
            DataModelManager modelManager = ((DataModelManager)Manager);
            modelManager.Clear();

            foreach (DataModelEntity entity in args.Items)
            {
                modelManager.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón extra.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            DataModelEntity entity;

            // Asegurar que hay un modelo de datos seleccionado.
            if ((entity = (DataModelEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoDataModelSelected);
                return;
            }

            // Preguntar por inserción de datos de prueba.
            bool insertTestData = Util.ShowConfirmDialog(
                Resources.QueryInsertTestDataOnInfrastructureService, Resources.Confirm);

            // Crear clase de construcción de servicio.
            ServiceBuilder builder = new ServiceBuilder(Control, entity);
            builder.BuildFinished += new EventHandler(ServiceBuilderBuildFinished);
            builder.InserTestData = insertTestData;

            dataModelManager.BuildProgress.Visibility = Visibility.Visible;
            dataModelManager.Disable();
            // Ejecutar servicios de construcción asíncrona de modelo de datos.
            builder.Build();
        }

        private void ServiceBuilderBuildFinished(object sender, EventArgs e)
        {
            dataModelManager.BuildProgress.Visibility = Visibility.Hidden;
            ServiceBuilder builder = (ServiceBuilder)sender;

            if (builder.Succeed)
            {
                Util.ShowInformationDialog(Resources.InfrastructureServiceBuilt, Resources.Information);
            }
            else
            {
                Util.ShowErrorDialog(builder.Message);
            }

            Load();
            dataModelManager.Enable();
        }

        private void OnDesignerClosed(object sender, EventArgs e)
        {
            if (designer.Result == true && designer.DataModelDocumentWpf.OldDataModelEntity != null)
            {
                Save(designer.DataModelDocumentWpf.OldDataModelEntity);
            }
            else
            {
                Load();
            }
        }

        #endregion
    }
}

