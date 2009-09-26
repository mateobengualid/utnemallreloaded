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
using UtnEmall.Server.BusinessLogic;
using PresentationLayer.DataModelDesigner;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para administrar servicios
    /// </summary>
    public partial class ServiceManager
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Listado de componentes que contienen los elementos mostrados en pantalla
        /// </summary>
        public ItemList ItemList
        {
            get { return list; }
        }

        /// <summary>
        /// El componente que se muestra cuando el servicio está siendo construido
        /// </summary>
        public BuildProgress BuildProgress
        {
            get { return buildProgress; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public ServiceManager()
        {
            this.InitializeComponent();

            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Name, "Name"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Description, "Description"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.StartDate, "StartDate"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.StopDate, "StopDate"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Deployed, "Deployed"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.DoFilter = DoFilter;
            list.ExtraButton.Content = UtnEmall.ServerManager.Properties.Resources.Design;
            list.ExtraButton1.Content = UtnEmall.ServerManager.Properties.Resources.Build;
            list.ExtraButton.Visibility = Visibility.Visible;
            list.ExtraButton1.Visibility = Visibility.Visible;

            list.list.SelectionChanged += new SelectionChangedEventHandler(list_SelectionChanged);
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list.Selected != null && ((ServiceEntity)list.Selected).Deployed)
            {
                list.ExtraButton1.Visibility = Visibility.Hidden;
            }
            else
            {
                list.ExtraButton1.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Deshabilita los componentes en pantalla para indicar que se está realizando algún tipo de
        /// procesamiento para no permitir al usuario interactuar con el sistema mientras se 
        /// realiza la  operación.
        /// </summary>
        public void Disable()
        {
            this.list.IsEnabled = false;
        }

        /// <summary>
        /// Habilita los componentes en pantalla
        /// </summary>
        public void Enable()
        {
            this.list.IsEnabled = true;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia la lista de servicios
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Método para la carga de entidades desde un servicio web
        /// </summary>
        /// <param name="loadRelation">Indica si se deben cargar las relaciones de entidades</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>Una lista de servicios</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.Service.GetAllService(true, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guarda la entidad en el servidor
        /// </summary>
        /// <param name="service">La entidad a guardar</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>null si se realizó con éxito la operación, la entidad con un listado de errores en el atributo Errors si la operación fallo</returns>
        public static IEntity Save(IEntity service, string session)
        {
            return Services.Service.Save((ServiceEntity)service, session);
        }

        /// <summary>
        /// Elimina la entidad desde el servidor
        /// </summary>
        /// <param name="service">La entidad a eliminar</param>
        /// <param name="session">Clave de sesión</param>
        /// <returns>null si la operación se realizó con éxito, una entidad con errores en el atributo Errors si la operación fallo</returns>
        public static IEntity Delete(IEntity service, string session)
        {
            return Services.Service.Delete((ServiceEntity)service, session);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Realiza un filtro en obj
        /// </summary>
        /// <param name="obj">El objeto al cual se le aplicará el filtro</param>
        /// <param name="filterText">Cadena de texto que filtra la lista</param>
        /// <returns>true si el filtro se realiza con éxito</returns>
        private bool DoFilter(Object obj, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            ServiceEntity service = (ServiceEntity)obj;

            if (service.Name.Contains(filterText)
                || service.Description.Contains(filterText))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}