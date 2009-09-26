using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Windows;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager
{
    class PermissionController : ControllerBase
    {
        /// <summary>
        /// La entidad seleccionada en el administrador
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((PermissionManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor (agregar o editar)
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((AuthorizationEditor)Editor).Mode;
            }

            set
            {
                ((AuthorizationEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que está siendo agregada o modificada por el editor
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((AuthorizationEditor)Editor).Permission;
            }

            set
            {
                ((AuthorizationEditor)Editor).Permission = (PermissionEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">El control que contiene a este componente</param>
        /// <param name="manager">El componente que muestra la lista de entidades</param>
        /// <param name="editor">El componente que permite agregar o editar una entidad</param>
        /// <param name="firstElement">El componente que se selecciona cuando se muestra el editor</param>
        public PermissionController(UserControl1 control, PermissionManager manager, AuthorizationEditor editor,
            FrameworkElement firstElement)
            : base(control, manager, editor, firstElement, null, new SaveEntity(GroupManager.Save),
            null)
        {
            PermissionManager permissionManager = (PermissionManager)manager;
            AuthorizationEditor addPermission = (AuthorizationEditor)editor;

            addPermission.OkSelected += OnOkSelected;
            addPermission.CancelSelected += OnCancelSelected;

            permissionManager.ItemList.NewButtonSelected += OnNewSelected;
            permissionManager.ItemList.EditButtonSelected += OnEditSelected;
            permissionManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            permissionManager.ItemList.ExtraButtonSelected += OnExtraSelected;
        }

        /// <summary>
        /// Elimina el elemento seleccionado
        /// </summary>
        /// <returns>La entidad eliminada</returns>
        protected override IEntity DeleteSelected()
        {
            return ((PermissionManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Recarga el contenido del administrador
        /// </summary>
        /// <param name="args">El argumento para el hilo de carga</param>
        protected override void Reload(LoaderArguments args)
        {
            PermissionManager permissionManager = ((PermissionManager)Manager);
            permissionManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                permissionManager.list.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            Mode = EditionMode.Add;
            ((AuthorizationEditor)Editor).Clear();
            Control.HideLastShown();
            Editor.Visibility = Visibility.Visible;
            Control.LastElementShown = Editor;
            FocusFirst();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Editar
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        protected override void OnEditSelected(object sender, EventArgs e)
        {
            PermissionEntity entity = (PermissionEntity)Selected;

            if (entity == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoPermissionSelected);
                return;
            }

            Mode = EditionMode.Edit;
            Entity = entity;
            Control.HideLastShown();
            Editor.Visibility = Visibility.Visible;
            Control.LastElementShown = Editor;
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Eliminar
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        protected override void OnDeleteSelected(object sender, EventArgs e)
        {
            PermissionManager permissionManager = (PermissionManager)Manager;
            PermissionEntity entity;

            if ((entity = (PermissionEntity)permissionManager.ItemList.DeleteSelected()) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoPermissionSelected);
                return;
            }

            permissionManager.Group.Permissions.Remove(entity);

            Save(permissionManager.Group);
            permissionManager.Load();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Extra
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            Control.groupManager.Visibility = Visibility.Visible;
            Control.LastElementShown = Control.groupManager;
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón OK
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        protected override void OnOkSelected(object sender, EventArgs e)
        {
            PermissionManager permissionManager = (PermissionManager)Manager;
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;

            PermissionEntity permissionEntity = (PermissionEntity)Entity;
            if (!permissionEntity.AllowDelete && !permissionEntity.AllowNew
                && !permissionEntity.AllowRead && !permissionEntity.AllowUpdate)
            {
                Util.ShowErrorDialog(Resources.EmptyPermissions);
                return;
            }

            foreach (PermissionEntity permission in permissionManager.Group.Permissions)
            {
                if (permissionEntity.BusinessClassName == permission.BusinessClassName
                    && permissionEntity.Id != permission.Id)
                {
                    Util.ShowErrorDialog(Resources.PermissionAlreadyExists);
                    return;
                }
            }

            if (((AuthorizationEditor)Editor).Mode == EditionMode.Add)
            {
                permissionManager.Group.Permissions.Add(permissionEntity);
            }

            Save(((PermissionManager)Manager).Group);
        }

        /// <summary>
        /// Carga las entidades en una lista
        /// </summary>
        public override void Load()
        {
            ((PermissionManager)Manager).Load();
        }
    }
}
