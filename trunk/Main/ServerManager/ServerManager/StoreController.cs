using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Windows;

namespace UtnEmall.ServerManager
{
    class StoreController : ControllerBase
    {
        /// <summary>
        /// La entidad seleccionada en el administrador
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((StoreManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor (Agregar o Modificar)
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((StoreEditor)Editor).Mode;
            }

            set
            {
                ((StoreEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que está siendo agregada o modificada.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((StoreEditor)Editor).Store;
            }

            set
            {
                ((StoreEditor)Editor).Store = (StoreEntity)value;
            }
        }

        /// <summary>
        /// constructor de clase
        /// </summary>
        /// <param name="control">Una referencia al control que contiene este componente</param>
        /// <param name="manager">El componente que muestra la lista de entidades</param>
        /// <param name="editor">El componente que permite agregar o modificar entidades</param>
        /// <param name="firstElement">El componente que obtiene el foco cuando el editor se muestra en pantalla</param>
        public StoreController(UserControl1 control, StoreManager manager, StoreEditor editor,
            FrameworkElement firstElement)
            : base(control, manager, editor, firstElement, new LoadList(StoreManager.Load),
            new SaveEntity(StoreManager.Save),
            new RemoveEntity(StoreManager.Delete))
        {
            StoreManager storeManager = (StoreManager)manager;
            StoreEditor addStore = (StoreEditor)editor;

            addStore.OkSelected += OnOkSelected;
            addStore.CancelSelected += OnCancelSelected;

            storeManager.ItemList.NewButtonSelected += OnNewSelected;
            storeManager.ItemList.EditButtonSelected += OnEditSelected;
            storeManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            storeManager.ItemList.ExtraButtonSelected += OnExtraSelected;
        }

        /// <summary>
        /// Eliminar el elemento seleccionado
        /// </summary>
        /// <returns>La entidad eliminada</returns>
        protected override IEntity DeleteSelected()
        {
            return ((StoreManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Recargar el contenido del administrador
        /// </summary>
        /// <param name="args">El argumento del hilo de carga</param>
        protected override void Reload(LoaderArguments args)
        {
            StoreManager storeManager = ((StoreManager)Manager);
            storeManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                storeManager.list.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Editar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnEditSelected(object sender, EventArgs e)
        {
            base.OnEditSelected(sender, e);
            ((StoreEditor)Editor).Session = Control.Session;
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            base.OnNewSelected(sender, e);
            ((StoreEditor)Editor).Session = Control.Session;
        }
    }
}
