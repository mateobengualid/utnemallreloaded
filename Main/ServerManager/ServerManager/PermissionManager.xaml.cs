using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using UtnEmall.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para administrar permisos
    /// </summary>
    public partial class PermissionManager
    {
        #region Instance Variables and Properties

        private GroupEntity group;
        /// <summary>
        /// El grupo que está siendo creado o modificado
        /// </summary>
        public GroupEntity Group
        {
            get { return group; }
            set
            {
                group = value;
                Load();
            }
        }

        /// <summary>
        /// La lista de componentes que contienen todos los elementos
        /// </summary>
        public ItemList ItemList
        {
            get { return list; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public PermissionManager()
        {
            this.InitializeComponent();

            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Class, "BusinessClassName"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Read, "AllowRead"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Create, "AllowNew"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Delete, "AllowDelete"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Update, "AllowUpdate"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.DoFilter = DoFilter;
            list.ExtraButton.Content = UtnEmall.ServerManager.Properties.Resources.Back;
            list.ExtraButton.Visibility = Visibility.Visible;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia los grupos
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Carga los permisos del grupo
        /// </summary>
        /// <returns>
        /// true si la carga se realiza exitosamente
        /// </returns>
        public bool Load()
        {
            Clear();

            foreach (PermissionEntity permission in group.Permissions)
            {
                list.Add(permission);
            }
            return true;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Realiza el filtro sobre obj
        /// </summary>
        /// <param name="obj">
        /// El objeto sobre el que se aplica el filtro
        /// </param>
        /// <param name="filterText">
        /// El texto que filtra la lista
        /// </param>
        /// <returns>
        /// true si el filtro se realiza exitosamente
        /// </returns>
        private bool DoFilter(Object obj, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            PermissionEntity permission = (PermissionEntity)obj;

            if (permission.BusinessClassName.Contains(filterText))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}