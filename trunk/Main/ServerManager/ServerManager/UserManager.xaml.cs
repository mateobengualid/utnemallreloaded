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
    /// Esta clase define el componente para adminsitrar usuarios
    /// </summary>
    public partial class UserManager
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Un listado de componentes que contienen todos los elementos mostrados
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
        public UserManager()
        {
            this.InitializeComponent();

            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.FirstName, "Name"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.LastName, "Surname"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.UserName, "UserName"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Phone, "PhoneNumber"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Position, "Charge"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.DoFilter = DoFilter;
            list.ExtraButton.Content = UtnEmall.ServerManager.Properties.Resources.Groups;
            list.ExtraButton.Visibility = Visibility.Visible;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpiar los elementos de la lista
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Método para cargar entidades desde un servicio web
        /// </summary>
        /// <param name="loadRelation">Indica si se deben cargar las relaciones de cada entidad</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>Un listado de entidades</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.User.GetAllUser(true, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guardar la entidad en el servidor
        /// </summary>
        /// <param name="entity">La entidad a guardar</param>
        /// <param name="session">La clave de sesión</param>
        /// <returns>null si se guardó exitosamente</returns>
        public static IEntity Save(IEntity entity, string session)
        {
            return Services.User.Save((UserEntity)entity, session);
        }

        /// <summary>
        /// Eliminar una entidad
        /// </summary>
        /// <param name="entity">La entidad a eliminar</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>null si se eliminó correctamente</returns>
        public static IEntity Delete(IEntity entity, string session)
        {
            return Services.User.Delete((UserEntity)entity, session);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Ejecuta un filtro sobre obj
        /// </summary>
        /// <param name="obj">
        /// El objeto sobre el cual se aplicará el filtro
        /// </param>
        /// <param name="filterText">
        /// Texto para filtrar la lista
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

            UserEntity user = (UserEntity)obj;

            if (user.Name.Contains(filterText)
                || user.Surname.Contains(filterText)
                || user.UserName.Contains(filterText)
                || user.PhoneNumber.Contains(filterText)
                || user.Charge.Contains(filterText))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}