using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using UtnEmall.Server.Base;
using UtnEmall.Server.EntityModel;
using PresentationLayer.DataModelDesigner;
using PresentationLayer.ServerDesigner;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager.Statistics;
using UtnEmall.ServerManager;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase contiene todos los componentes y controles que son mostrados en pantalla. Además, adminsitra la sesión de usuario.
    /// </summary>
    public partial class UserControl1
    {
        #region Instance Variables and Properties

        private UserController userController;
        private CustomerController customerController;
        private StoreController storeController;
        private DataModelController dataModelController;
        private ServiceController serviceController;
        private GroupController groupController;
        private PermissionController permissionController;
        private CategoryController categoryController;
        private StatisticController statisticController;
        private SessionController sessionController;

        private FrameworkElement lastElementShown;
        /// <summary>
        /// Una referencia al último elemento mostrado
        /// </summary>
        public FrameworkElement LastElementShown
        {
            get { return lastElementShown; }
            set { lastElementShown = value; }
        }

        private string session;
        /// <summary>
        /// La clave de sesión
        /// </summary>
        public string Session
        {
            get
            {
                return session;
            }

            set
            {
                session = value;
            }
        }

        /// <summary>
        /// True si el usuario ha iniciado sesión correctamente
        /// </summary>
        public bool IsLoggedIn
        {
            get { return !string.IsNullOrEmpty(session); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
            Justification = "This is the main window user control.")]
        public UserControl1()
        {
            this.InitializeComponent();
            welcome.FocusFirst();

            userController = new UserController(this, userManager, addUser, addUser.TxtFirstName, editGroups);
            customerController = new CustomerController(this, customerManager, addCustomer, addCustomer.TxtFirstName);
            storeController = new StoreController(this, storeManager, addStore, addStore.TxtName);
            dataModelController = new DataModelController(this, dataModelManager, addDataModel, null);
            serviceController = new ServiceController(this, serviceManager, addService, addService.TxtName);
            groupController = new GroupController(this, groupManager, addGroup, addGroup.TxtName, permissionManager);
            permissionController = new PermissionController(this, permissionManager, addPermission, addPermission.klass);
            categoryController = new CategoryController(this, categoryManager, addCategory, addCategory.TxtName);
            statisticController = new StatisticController(this, statisticsViewer, globalStatisticsAnalyzer);
            sessionController = new SessionController(this, welcome);

            menu.ManageUsersSelected += userController.OnSelected;
            menu.ManageCategoriesSelected += categoryController.OnSelected;
            menu.ManageServicesSelected += serviceController.OnSelected;
            menu.ManageStoresSelected += storeController.OnSelected;
            menu.ManageGroupsSelected += groupController.OnSelected;
            menu.ManageCustomersSelected += customerController.OnSelected;
            menu.ViewStatisticsSelected += statisticController.OnSelected;
            menu.ManageDataModelSelected += dataModelController.OnSelected;

            LastElementShown = welcome;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Cierra la sesión en el servidor
        /// </summary>
        public void LogOff()
        {
            sessionController.LogOff();
        }

        public void HideLastShown()
        {
            lastElementShown.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
