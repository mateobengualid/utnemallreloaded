using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Windows;
using System.Reflection;
using System.ServiceModel;

namespace UtnEmall.ServerManager
{
    public class UserController : ControllerBase
    {
        private EditGroups editGroups;

        /// <summary>
        /// La entidad seleccionada
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((UserManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor.
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((UserEditor)Editor).Mode;
            }

            set
            {
                ((UserEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que está siendo agregada o modificada.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((UserEditor)Editor).User;
            }

            set
            {
                ((UserEditor)Editor).User = (UserEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">Una referencia al control que contiene a este componente</param>
        /// <param name="manager">El componente que muestra un listado de entidades</param>
        /// <param name="editor">El componente que permite agregar o modificar una entidad</param>
        /// <param name="firstElement">El componente que tener el foco cuando se muestra el editor</param>
        public UserController(UserControl1 control, UserManager manager, UserEditor editor,
            FrameworkElement firstElement, EditGroups editGroups)
            : base(control, manager, editor, firstElement, new LoadList(UserManager.Load),
            new SaveEntity(UserManager.Save),
            new RemoveEntity(UserManager.Delete))
        {
            UserManager userManager = (UserManager)manager;
            UserEditor addUser = (UserEditor)editor;

            this.editGroups = editGroups;

            addUser.OkSelected += OnOkSelected;
            addUser.CancelSelected += OnCancelSelected;

            userManager.ItemList.NewButtonSelected += OnNewSelected;
            userManager.ItemList.EditButtonSelected += OnEditSelected;
            userManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            userManager.ItemList.ExtraButtonSelected += OnExtraSelected;

            editGroups.OkSelected += new EventHandler(OnEditGroupsOkSelected);
            editGroups.CancelSelected += new EventHandler(OnEditGroupsCancelSelected);
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón OK
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void OnEditGroupsOkSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;

            Save(editGroups.User);
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Cancelar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        void OnEditGroupsCancelSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;
        }

        /// <summary>
        /// Eliminar el elemento seleccionado
        /// </summary>
        /// <returns>La entidad eliminada</returns>
        protected override IEntity DeleteSelected()
        {
            return ((UserManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Recarga el contenido del adminsitrador
        /// </summary>
        /// <param name="args">Los argumentos para el hilo de carga</param>
        protected override void Reload(LoaderArguments args)
        {
            UserManager userManager = ((UserManager)Manager);
            userManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                userManager.list.Add(entity);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            if (!((UserEditor)Editor).LoadStores(Control.Session))
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
        /// Método invocado cuando se presiona el botón Editar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnEditSelected(object sender, EventArgs e)
        {
            UserEditor addUser = (UserEditor)Editor;
            UserEntity user;
            Mode = EditionMode.Edit;

            if ((user = (UserEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoUserSelected);
                return;
            }

            addUser.User = user;

            if (!addUser.LoadStores(Control.Session))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadStoreListFailed);
                return;
            }

            if (user == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoUserSelected);
                return;
            }

            Control.HideLastShown();
            addUser.Visibility = Visibility.Visible;
            Control.LastElementShown = addUser;
        }

        /// <summary>
        /// Método invocado cuando se presiona el bo´ton Extra
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            UserEntity entity;

            if ((entity = (UserEntity)Selected) == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoUserSelected);
                return;
            }

            editGroups.Clear();

            try
            {
                foreach (GroupEntity group in Services.Group.GetAllGroup(false, Control.Session))
                {
                    editGroups.AddSourceGroup(group);
                }
            }
            catch (TargetInvocationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorGroupsNotLoaded);
                return;
            }
            catch (CommunicationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorGroupsNotLoaded);
                return;
            }

            foreach (UserGroupEntity userGroup in entity.UserGroup)
            {
                editGroups.AddDestinationGroup(userGroup.Group);
            }

            editGroups.User = entity;
            Control.HideLastShown();
            editGroups.Visibility = Visibility.Visible;
            Control.LastElementShown = editGroups;
        }
    }
}
