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
    /// Esta clase define el componente visual para administrar modelos de datos.
    /// </summary>
    public partial class DataModelManager
    {
        #region Instance Variables and Properties

        private System.Collections.Generic.List<string> usedStores;
        /// <summary>
        /// Una lista con los nombres de las tiendas que ya tienen un modelo de datos.
        /// </summary>
        public ReadOnlyCollection<string> UsedStores
        {
            get { return new ReadOnlyCollection<string>(usedStores); }
        }

        /// <summary>
        /// El componente de lista que contiene todos los ítems mostrados.
        /// </summary>
        public ItemList ItemList
        {
            get { return list; }
        }

        /// <summary>
        /// El componente que se muestra mienstras se está construyendo.
        /// </summary>
        public BuildProgress BuildProgress
        {
            get { return buildProgress; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public DataModelManager()
        {
            this.InitializeComponent();
            
            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            usedStores = new System.Collections.Generic.List<string>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Owner, "Store.Name"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Deployed, "Deployed"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.ExtraButton.Content = UtnEmall.ServerManager.Properties.Resources.Build;
            list.ExtraButton.Visibility = Visibility.Visible;

            list.DoFilter = DoFilter;
        }

        /// <summary>
        /// Deshabilita el contenido del componente.
        /// </summary>
        public void Disable()
        {
            this.list.IsEnabled = false;
        }

        /// <summary>
        /// Habilita el contenido del componente.
        /// </summary>
        public void Enable()
        {
            this.list.IsEnabled = true;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia la lista.
        /// </summary>
        public void Clear()
        {
            list.Clear();
            usedStores.Clear();
        }

        /// <summary>
        /// Agrega un modelo de datos a la lista.
        /// </summary>
        /// <param name="dataModel">El modelo de datos a agregar.</param>
        public void Add(DataModelEntity dataModel)
        {
            if (dataModel.Store != null)
            {
                list.Add(dataModel);
                usedStores.Add(dataModel.Store.Name);
            }
            // Si la tienda es null, es el modelo de datos del shopping.
            else
            {
                StoreEntity store = new StoreEntity();
                store.Name = UtnEmall.ServerManager.Properties.Resources.Mall;
                dataModel.Store = store;
                list.Add(dataModel);
                usedStores.Add(dataModel.Store.Name);
            }
        }

        /// <summary>
        /// Carga las entidades desde el servicio web.
        /// </summary>
        /// <param name="loadRelation">Si debe cargar las relaciones de las entidades.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Una lista de entidades.</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.DataModel.GetAllDataModel(true, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guarda una entidad en el servidor.
        /// </summary>
        /// <param name="dataModel">La entidad a guardar.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Null si tiene éxito, sino, una entidad con errores.</returns>
        public static IEntity Save(IEntity dataModel, string session)
        {
            return Services.DataModel.Save((DataModelEntity)dataModel, session);
        }

        /// <summary>
        /// Elimina la entidad del servidor.
        /// </summary>
        /// <param name="dataModel">La entidad a eliminarse.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Null si tiene éxito, sino, una entidad con errores.</returns>
        public static IEntity Delete(IEntity dataModel, string session)
        {
            return Services.DataModel.Delete((DataModelEntity)dataModel, session);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Ejecuta el filtro en obj para saber si debe filtrarse.
        /// </summary>
        /// <param name="obj">
        /// El objeto sobre el cual se aplicará el filtro.
        /// </param>
        /// <param name="filterText">
        /// El texto que filtra la lista.
        /// </param>
        /// <returns>
        /// Verdadero si obj pasa el filtro, de otro modo, falso.
        /// </returns>
        private bool DoFilter(Object obj, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            DataModelEntity model = (DataModelEntity)obj;

            if (model.Store.Name.Contains(filterText))
                return true;

            return false;
        }

        #endregion

        #endregion
    }
}