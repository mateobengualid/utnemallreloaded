using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.ServerManager
{
    class GroupController : ControllerBase
    {
        private PermissionManager permissionManager;
        /// <summary>
        /// La entindad seleccionada por el administrador.
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((GroupManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor puede ser Agregar o Editar
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((GroupEditor)Editor).Mode;
            }

            set
            {
                ((GroupEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que está siendo agregada o modificada en el editor.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((GroupEditor)Editor).Group;
            }

            set
            {
                ((GroupEditor)Editor).Group = (GroupEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">Una referencia al control que contiene este componente</param>
        /// <param name="manager">El componente que muestra un listado de entidades</param>
        /// <param name="editor">El component que permite agregar o editar una entidad</param>
        /// <param name="firstElement">El componente que debe ser seleccionado cuando se muestra el editor</param>
        public GroupController(UserControl1 control, GroupManager manager, GroupEditor editor,
            FrameworkElement firstElement, PermissionManager permissionManager)
            : base(control, manager, editor, firstElement, new LoadList(GroupManager.Load),
            new SaveEntity(GroupManager.Save),
            new RemoveEntity(GroupManager.Delete))
        {
            this.permissionManager = permissionManager;
            GroupManager groupManager = (GroupManager)manager;
            GroupEditor addGroup = (GroupEditor)editor;

            addGroup.OkSelected += OnOkSelected;
            addGroup.CancelSelected += OnCancelSelected;

            groupManager.ItemList.NewButtonSelected += OnNewSelected;
            groupManager.ItemList.EditButtonSelected += OnEditSelected;
            groupManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            groupManager.ItemList.ExtraButtonSelected += OnExtraSelected;
        }

        /// <summary>
        /// Borrar el elemento seleccionado
        /// </summary>
        /// <returns>la entidad eliminada</returns>
        protected override IEntity DeleteSelected()
        {
            return ((GroupManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Recargar el contenido del administrador
        /// </summary>
        /// <param name="args">El argumento del hilo de carga</param>
        protected override void Reload(LoaderArguments args)
        {
            GroupManager groupManager = ((GroupManager)Manager);
            groupManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                groupManager.list.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando el botón extra es seleccionado en el administrador
        /// </summary>
        /// <param name="sender">El objeto que envía la señal</param>
        /// <param name="e">Los argumentos del evento</param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            GroupEntity entity;
            if ((entity = (GroupEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoGroupSelected);
                return;
            }

            permissionManager.Group = entity;
            Control.HideLastShown();
            permissionManager.Visibility = Visibility.Visible;
            Control.LastElementShown = permissionManager;
        }
    }
}
