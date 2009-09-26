using System.Windows;
using UtnEmall.Server.EntityModel;
using PresentationLayer.Widgets;
using System.Windows.Controls;
using PresentationLayer.ServerDesignerClasses;
using UtnEmall.ServerManager;
using System;
using System.IO;
using LogicalLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PresentationLayer.DataModelDesigner
{
    /// <summary>
    /// Clase que define el componente visual para gestionar un modelo de datos.
    /// </summary>
    public partial class WindowDataModelDesigner : Window
    {
        #region Instance Variables and Properties

        private bool result;
        public bool Result
        {
            get 
            { 
                return result; 
            }
        }

        private DataModelDocumentWpf dataModelDocumentWpf;
        public DataModelDocumentWpf DataModelDocumentWpf
        {
            get { return dataModelDocumentWpf; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///constructor
        /// </summary>
        /// <param name="dataModelEntity">El modelo de datos a editar</param>
        public WindowDataModelDesigner(DataModelEntity dataModelEntity)
        {
            if (dataModelEntity == null)
            {
                throw new ArgumentNullException("dataModelEntity", "dataModelEntity can not be null");
            }
            InitializeComponent();
            CreateDataModelDocument(dataModelEntity);
            this.Loaded += new RoutedEventHandler(WindowDataModelDesignerLoaded);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Esta función crea un nuevo modelo de dato wpf que representa un modelo de datos(data model) dentro del proyecto WPF.
        /// </summary>
        /// <param name="dataModelEntity">El modelo de datos</param>
        public void CreateDataModelDocument(DataModelEntity dataModelEntity)
        {
            if (dataModelEntity == null)
            {
                throw new ArgumentNullException("dataModelEntity", "dataModelEntity can not be null");
            }
            dataModelDocumentWpf = new DataModelDocumentWpf(this, dataModelEntity);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Función llamada cuando se selecciona la opcion nueva tabla.Esta función crea una nueva tabla wpf y la agrega al modelo de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewTableClicked(object sender, EventArgs e)
        {
            TableWpf newTableWPF = new TableWpf();
            dataModelDocumentWpf.AddTable(newTableWPF);
        }

        /// <summary>
        /// Función llamada cuando se selecciona la opcion uno a muchos. Esta función prepara el entorno para crear una nueva relación uno a muchos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOneToOneClicked(object sender, EventArgs e)
        {
            dataModelDocumentWpf.IsMakeConnectionAction = true;
            dataModelDocumentWpf.ConnectionWidgetFrom = dataModelDocumentWpf.ConnectionWidgetTarget = null;
            dataModelDocumentWpf.SelectedRelationType = RelationType.OneToOne;

            this.textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectTableOriginConnection;
        }

        /// <summary>
        /// Función llamada cuando se selecciona la opcion uno a muchos. Esta función prepara el entorno para crear una nueva relación uno a muchos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOneToManyClicked(object sender, EventArgs e)
        {
            dataModelDocumentWpf.IsMakeConnectionAction = true;
            dataModelDocumentWpf.ConnectionWidgetFrom = dataModelDocumentWpf.ConnectionWidgetTarget = null;
            dataModelDocumentWpf.SelectedRelationType = RelationType.OneToMany;

            this.textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectTableOriginConnection;
        }

        /// <summary>
        /// Función llamada cuando se selecciona la opcion muchos a muchos. Esta función prepara el entorno para crear una nueva relación muchos a muchos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManyToManyClicked(object sender, EventArgs e)
        {
            dataModelDocumentWpf.IsMakeConnectionAction = true;
            dataModelDocumentWpf.ConnectionWidgetFrom = dataModelDocumentWpf.ConnectionWidgetTarget = null;
            dataModelDocumentWpf.SelectedRelationType = RelationType.ManyToMany;

            this.textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectTableOriginConnection;
        }

        /// <summary>
        /// Función llamada cuando se selecciona el boton guardar. Este metodo prepara el modelo de datos, lo guarda en la base de datos y cierra el editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                Collection<Error> errors = dataModelDocumentWpf.SaveDataModel();
                if (errors.Count > 0)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.SaveInfrastructureError, errors);
                    return;
                }
                this.result = true;
                this.Close();
            }
            catch (InvalidCastException)
            {
                Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.SaveInfrastructureError, UtnEmall.ServerManager.Properties.Resources.SaveError);
            }
        }

        /// <summary>
        /// Función llamada cuando la ventana es cargada. Este método dibuja todas las tablas y relaciónes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowDataModelDesignerLoaded(object sender, RoutedEventArgs e)
        {
            dataModelDocumentWpf.RedrawConnections();
        }

        #endregion

        #endregion
    }
}
