using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para mostrar todos los elementos en un menú
    /// </summary>
    public partial class Menu
    {
        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public Menu()
        {
            this.InitializeComponent();
            Users.Text = UtnEmall.ServerManager.Properties.Resources.Users;
            Users.Image = "../imgs/users.png";

            Customers.Text = UtnEmall.ServerManager.Properties.Resources.Customers;
            Customers.Image = "../imgs/customers.png";

            Stores.Text = UtnEmall.ServerManager.Properties.Resources.Stores;
            Stores.Image = "../imgs/stores.png";

            Statistics.Text = UtnEmall.ServerManager.Properties.Resources.Statistics;
            Statistics.Image = "../imgs/statistics.png";

            Services.Text = UtnEmall.ServerManager.Properties.Resources.Services;
            Services.Image = "../imgs/services.png";

            //Campaigns.Text = UtnEmall.ServerManager.Properties.Resources.Campaigns;
            //Campaigns.Image = "../imgs/campaigns.png";

            Categories.Text = UtnEmall.ServerManager.Properties.Resources.Categories;
            Categories.Image = "../imgs/categories.png";

            Groups.Text = UtnEmall.ServerManager.Properties.Resources.Groups;
            Groups.Image = "../imgs/groups.png";

            DataModel.Text = UtnEmall.ServerManager.Properties.Resources.DataModel;
            DataModel.Image = "../imgs/datamodel.png";
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método invocado cuando se selecciona Usuarios en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageUsersClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageUsersSelected != null)
            {
                ManageUsersSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Clientes en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageCustomersClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageCustomersSelected != null)
            {
                ManageCustomersSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Categorías en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageCategoriesClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageCategoriesSelected != null)
            {
                ManageCategoriesSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Servicios en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageServicesClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageServicesSelected != null)
            {
                ManageServicesSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Campañas en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageCampaignsClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageCampaignsSelected != null)
            {
                ManageCampaignsSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Tiendas en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageStoresClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageStoresSelected != null)
            {
                ManageStoresSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Grupos en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageGroupsClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageGroupsSelected != null)
            {
                ManageGroupsSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Modelo de Datos en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnManageDataModelClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ManageDataModelSelected != null)
            {
                ManageDataModelSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona Ver Estadísticas en el menú
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnViewStatisticsClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ViewStatisticsSelected != null)
            {
                ViewStatisticsSelected(sender, e);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento creado cuando se presiona el botón de Usuarios
        /// </summary>
        public event EventHandler ManageUsersSelected;

        /// <summary>
        /// Evento creado cuando se presiona el botón Clientes
        /// </summary>
        public event EventHandler ManageCustomersSelected;

        /// <summary>
        /// Evento creado cuando se selecciona el botón Categorias
        /// </summary>
        public event EventHandler ManageCategoriesSelected;

        /// <summary>
        /// Evento creado cuando se selecciona el botón Servicios
        /// </summary>
        public event EventHandler ManageServicesSelected;

        /// <summary>
        /// Evento creado cuando se selecciona el botón Campañas
        /// </summary>
        public event EventHandler ManageCampaignsSelected;

        /// <summary>
        /// Evento creado cuando se presiona el botón Tiendas
        /// </summary>
        public event EventHandler ManageStoresSelected;

        /// <summary>
        /// Evento creado cuando se presiona el botón Grupos
        /// </summary>
        public event EventHandler ManageGroupsSelected;

        /// <summary>
        /// Evento creado cuando se presiona el botón Modelo de Datos
        /// </summary>
        public event EventHandler ManageDataModelSelected;

        /// <summary>
        /// Evento creado cuando se presiona el botón EStadísticas
        /// </summary>
        public event EventHandler ViewStatisticsSelected;

        #endregion
    }
}