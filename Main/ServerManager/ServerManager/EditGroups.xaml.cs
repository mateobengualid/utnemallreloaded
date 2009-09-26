using System;
using System.Collections.Generic;
using System.Windows;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para administrar grupos de una lista de grupos disponibles.
    /// </summary>
    public partial class EditGroups
    {
        #region Instance Variables and Properties

        private Dictionary<string, GroupEntity> available;
        /// <summary>
        /// Un diccionario con el nombre del grupo como clave y el objeto de grupo como valor representa los grupos disponibles.
        /// </summary>
        public Dictionary<string, GroupEntity> Available
        {
            get { return available; }
        }

        private UserEntity user;
        /// <summary>
        /// El usuario del cual los grupos se están editando.
        /// </summary>
        public UserEntity User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Una lista con los grupos seleccionados.
        /// </summary>
        public ReadOnlyCollection<GroupEntity> Selected
        {
            get
            {
                List<GroupEntity> selectedGroups = new List<GroupEntity>();

                foreach (string name in Selector.Selected)
                {
                    selectedGroups.Add(available[name]);
                }
                return (new ReadOnlyCollection<GroupEntity>(selectedGroups));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public EditGroups()
        {
            this.InitializeComponent();
            available = new Dictionary<string, GroupEntity>();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia los grupos seleccionados.
        /// </summary>
        public void Clear()
        {
            Selector.Clear();
            available.Clear();
        }

        /// <summary>
        /// Agrega un grupo a la lista de origen.
        /// </summary>
        /// <param name="group">
        /// Un objeto de grupo.
        /// </param>
        public void AddSourceGroup(GroupEntity group)
        {
            Selector.AddSource(group.Name);
            available.Add(group.Name, group);
        }

        /// <summary>
        /// Agrega el grupo a la lista de destino.
        /// </summary>
        /// <param name="group">
        /// Un objeto de grupo.
        /// </param>
        public void AddDestinationGroup(GroupEntity group)
        {
            Selector.AddDestination(group.Name);
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
            System.Collections.Generic.List<UserGroupEntity> toRemove = new List<UserGroupEntity>();

            // Agrega los grupos quitados a la lista.
            foreach (UserGroupEntity userGroup in user.UserGroup)
            {
                bool exists = false;

                foreach (GroupEntity group in Selected)
                {
                    if (userGroup.Group.Name == group.Name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    toRemove.Add(userGroup);
                }
            }

            // Quitarlos.
            foreach (UserGroupEntity userGroup in toRemove)
            {
                user.UserGroup.Remove(userGroup);
            }

            // Agregar los nuevos grupos.
            foreach (GroupEntity group in Selected)
            {
                bool exists = false;

                foreach (UserGroupEntity userGroup in user.UserGroup)
                {
                    if (userGroup.Group.Name == group.Name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    UserGroupEntity userGroup = new UserGroupEntity();
                    userGroup.Group = group;

                    user.UserGroup.Add(userGroup);
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