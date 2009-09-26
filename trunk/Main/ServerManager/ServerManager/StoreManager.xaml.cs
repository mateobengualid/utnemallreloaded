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
    /// Esta clase define el componente visual para administrar tiendas
    /// </summary>
    public partial class StoreManager
    {
        #region Instance Variables and Properties

        /// <summary>
        /// El componente que contiene todos los elementos mostrados en pantalla.
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
        public StoreManager()
        {
            this.InitializeComponent();
            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Name, "Name"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Phone, "TelephoneNumber"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.ContactName, "ContactName"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Website, "WebAddress"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Email, "Email"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.StoreNumber, "LocalNumber"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.DoFilter = DoFilter;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia la lista de tiendas
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Carga las entidades desde un servicio web
        /// </summary>
        /// <param name="loadRelation">Indica si se deben cargar las relaciones de cada entidad</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>Lista de entidades</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.Store.GetAllStore(true, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guarda la entidad en el servidor
        /// </summary>
        /// <param name="store">La entidad a ser guardada</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>null si se realizó con éxito, una entidad con errores en el atributo Errors, si la operación falló.</returns>
        public static IEntity Save(IEntity store, string session)
        {
            return Services.Store.Save((StoreEntity)store, session);
        }

        /// <summary>
        /// Elimina la entidad en el servidor
        /// </summary>
        /// <param name="store">La entidad a ser eliminada</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>null si se realizó con éxito, una entidad con errores en el atributo Errors, si la operación falló</returns>
        public static IEntity Delete(IEntity store, string session)
        {
            return Services.Store.Delete((StoreEntity)store, session);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Filtra el objeto obj
        /// </summary>
        /// <param name="obj">
        /// El objeto al cual se aplica el filtro
        /// </param>
        /// <param name="filterText">
        /// El texto de filtrado
        /// </param>
        /// <returns>
        /// true si el filtro se realiza con éxito
        /// </returns>
        private bool DoFilter(Object obj, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            StoreEntity store = (StoreEntity)obj;

            if (store.Name.Contains(filterText)
                || store.TelephoneNumber.Contains(filterText)
                || store.ContactName.Contains(filterText)
                || store.WebAddress.Contains(filterText)
                || store.Email.Contains(filterText)
                || store.LocalNumber.Contains(filterText))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}