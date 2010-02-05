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
    /// Esta clase define el componente visual para administrar campañas
    /// </summary>
    public partial class CampaignManager
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Un listado de componentes que contienen todos los elementos a mostrar
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
        public CampaignManager()
        {
            this.InitializeComponent();
            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Name, "Name"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Description, "Description"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.DoFilter = DoFilter;
            list.ExtraButton.Visibility = Visibility.Visible;
            list.ExtraButton.Content = UtnEmall.ServerManager.Properties.Resources.Services;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Elimina los grupos
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Método para cargar las entidades desde un servicio web
        /// </summary>
        /// <param name="loadRelation">indica si se debe cargar las relaciones de cada entidad</param>
        /// <param name="session">la clave de sesión</param>
        /// <returns>un listado de entidades</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.Campaign.GetAllCampaign(true, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guarda la entidad en el servidor
        /// </summary>
        /// <param name="campaign">La entidad a ser guardada</param>
        /// <param name="session">La clave de sesión</param>
        /// <returns>null si se guardó exitósamente, una entidad con errores en el atributo Errors si falló</returns>
        public static IEntity Save(IEntity campaign, string session)
        {
            return Services.Campaign.Save((CampaignEntity)campaign, session);
        }

        /// <summary>
        /// Elimina una entidad del servidor
        /// </summary>
        /// <param name="group">La entidad a ser eliminada</param>
        /// <param name="session">La clave de sesión</param>
        /// <returns>null si se eliminó con éxtio, una entidad con errores en el atributo Errors si falló</returns>
        public static IEntity Delete(IEntity campaign, string session)
        {
            return Services.Campaign.Delete((CampaignEntity)campaign, session);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// corre el filtro en obj
        /// </summary>
        /// <param name="obj">
        /// el objeto al cual se le aplicará el filtro
        /// </param>
        /// <param name="filterText">
        /// el texto que filtra la lista
        /// </param>
        /// <returns>
        /// true si oj pasa el filtro
        /// </returns>
        private bool DoFilter(Object obj, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            CampaignEntity campaign = (CampaignEntity)obj;

            if (campaign.Name.Contains(filterText)
                || campaign.Description.Contains(filterText))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}