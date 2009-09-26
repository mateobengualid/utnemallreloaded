
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UtnEmall.ServerManager;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.EntityModel;
using LogicalLibrary;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.Widgets;
using VisualDesignerPresentationLayer;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager.Properties;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Clase que representa un Documento en wpf para un servicio este puede contener Componentes que represente el servicio y sus conexiones.
    /// </summary>
    public class ServiceDocumentWpf : ServiceDocument
    {
        #region Constants, Variables and Properties

        private WindowDesigner windowsDesigner;

        private String session;
        public String Session
        {
            get { return session; }
            set { session = value; }
        }

        private DataModelEntity dataModelEntity;
        public DataModelEntity DataModelEntity
        {
            get { return dataModelEntity; }
            set { dataModelEntity = value; }
        }

        private Canvas drawArea;
        public Canvas DrawArea
        {
            get { return drawArea; }
            set { drawArea = value; }
        }

        private ServiceEntity serviceEntity;
        public ServiceEntity ServiceEntity
        {
            get { return serviceEntity; }
            set { serviceEntity = value; }
        }

        // Variable que controla el movimiento de los componentes
        private bool isDragDropAction;
        public bool IsDragDropAction
        {
            get { return isDragDropAction; }
            set { isDragDropAction = value; }
        }

        private bool isMakeConnectionAction;
        public bool IsMakeConnectionAction
        {
            get { return isMakeConnectionAction; }
            set { isMakeConnectionAction = value; }
        }

        private Point mousePosition = new Point();
        public Point MousePosition
        {
            get { return mousePosition; }
            set { mousePosition = value; }
        }

        private UIElement currentElement;
        public UIElement CurrentElement
        {
            get { return currentElement; }
            set { currentElement = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        ///constructor
        /// </summary>
        /// <param name="window"></param>
        /// <param name="session"></param>
        public ServiceDocumentWpf(Window window, String session)
            : base()
        {
            this.session = session;
            windowsDesigner = window as WindowDesigner;
            this.DrawArea = windowsDesigner.canvasDraw;
        }

        #endregion

        #region Events

        public event EventHandler DataSourceDeleted;
        public event EventHandler FormDeleted;

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Agrega un nuevo componente a la lista de componentes del servicio
        /// </summary>
        /// <param name="widget">Componente a agregar</param>
        public override void AddWidget(Widget widget)
        {
            base.AddWidget(widget);
            this.AttachWidgetEvents(widget);
            IDrawAbleWpf drawable = widget as IDrawAbleWpf;
            this.DrawArea.Children.Add(drawable.UIElement);
            if (widget.XCoordinateRelativeToParent == 0 && widget.YCoordinateRelativeToParent == 0)
            {
                Point centralDrawAreaPoint = new Point(DrawArea.ActualWidth / 2, DrawArea.ActualHeight / 2);
                Canvas.SetLeft(drawable.UIElement, widget.XCoordinateRelativeToParent = centralDrawAreaPoint.X);
                Canvas.SetTop(drawable.UIElement, widget.YCoordinateRelativeToParent = centralDrawAreaPoint.Y);
            }
        }

        /// <summary>
        /// Función que elimina un componente del documento del servicio
        /// </summary>
        /// <param name="widget">Componente a agregar</param>
        public override void RemoveWidget(Widget widget)
        {
            base.RemoveWidget(widget);

            IDrawAbleWpf drawable = widget as IDrawAbleWpf;
            this.DrawArea.Children.Remove(drawable.UIElement);
            RedrawConnections();
        }
        
        /// <summary>
        /// Función que agrega una nueva conexión a la lista de conexiones del documento del servicio
        /// </summary>
        /// <param name="connectionWpf">La conexión a agregar</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void AddConnection(ConnectionWpf connectionWpf)
        {
            connectionWpf.ConnectionContextMenuDeleteClick += new RoutedEventHandler(myConnection_ConnectionContextMenuDeleteClick);
            base.AddConnection(connectionWpf);
        }

        /// <summary>
        /// Elimina una conexión de la lista de conexiones del documento del servicio.
        /// </summary>
        /// <param name="connectionWpf">La conexión a eliminar</param>
        public void RemoveConnection(ConnectionWpf connectionWpf)
        {
            connectionWpf.Reset(true, true);
            this.DrawArea.Children.Remove(connectionWpf.UIElement);
            this.RedrawConnections();
        }

        /// <summary>
        /// Esta función redibuja todas las conexiones
        /// </summary>
        public void RedrawConnections()
        {
            DeleteDrawConnections();
            foreach (ConnectionWpf connection in this.Connections)
            {
                connection.DrawConnection(drawArea);
            }
        }

        /// <summary>
        /// Redibuja el documento del servicio en wpf, sus componentes y conexiones entre ellos.
        /// </summary>
        /// <param name="attachWidgetEvents">Indica si se agregan los eventos</param>
        public void RedrawDocument(bool attachWidgetEvents)
        {
            this.drawArea.Children.RemoveRange(0, drawArea.Children.Count);
            foreach (Widget widget in Components)
            {
                MenuFormWpf menuFormWpf = widget as MenuFormWpf;
                if (menuFormWpf != null)
                {
                    menuFormWpf.MakeCanvas();
                }

                IDrawAbleWpf drawableWPF = widget as IDrawAbleWpf;
                if (attachWidgetEvents)
                {
                    this.AttachWidgetEvents(drawableWPF as Widget);
                }
                drawArea.Children.Add(drawableWPF.UIElement);
                Canvas.SetLeft(drawableWPF.UIElement, widget.XCoordinateRelativeToParent);
                Canvas.SetTop(drawableWPF.UIElement, widget.YCoordinateRelativeToParent);
            }
            this.DrawArea.UpdateLayout();
            RedrawConnections();
        }

        public virtual void RedrawDocument()
        {
            RedrawDocument(false);
        }

        /// <summary>
        ///Carga el modelo de datos almacenado
        /// </summary>
        /// <returns>Un modelo de datos almacenado o null si no existe un modelo de datos para esta tienda</returns>
        public LogicalLibrary.DataModelClasses.DataModel LoadDataModel()
        {

            List<DataModelEntity> dataModelEntities = new List<DataModelEntity>(Services.DataModel.GetDataModelWhereEqual("idStore", ServiceEntity.IdStore.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), true, session));
            if (dataModelEntities != null && dataModelEntities.Count > 0)
            {
                DataModelEntity = dataModelEntities[0];
                LogicalLibrary.DataModelClasses.DataModel dataModel = Utilities.ConvertEntityToDataModel(dataModelEntities[0]);
                return dataModel;
            }
            return null;
        }

        /// <summary>
        /// Función que convierte un ServiceDocument en un CustomerServiceDataEntity para poder guardarlo.
        /// </summary>
        /// <returns></returns>
        public CustomerServiceDataEntity Save()
        {
            CustomerServiceDataEntity customerServiceDataEntity = null;
            try
            {
                customerServiceDataEntity = LogicalLibrary.Utilities.ConvertServiceModelToEntity(this, this.DataModelEntity);
                return customerServiceDataEntity;
            }
            catch (InvalidCastException)
            {
                return null;
            }
        }

        /// <summary>
        /// Funcion que carga y conviernte un CustomerServiceDataEntity en un ServiceDocument para poder verlo y editarlo visualmente
        /// </summary>
        /// <returns></returns>
        public virtual bool Load()
        {
            try
            {
                if (this.ServiceEntity.CustomerServiceData != null)
                {
                    Utilities.ConvertEntityToServiceModel(this.ServiceEntity.CustomerServiceData, this);
                    return true;
                }
                throw new UtnEmallBusinessLogicException(Resources.CustomerServiceDataNull);
            }
            catch (InvalidCastException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadServiceDesignFailed);
            }
            return false;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        ///Adjunta eventos a un componente
        /// </summary>
        /// <param name="widget">Componente al que se le agregara los eventos</param>
        private void AttachWidgetEvents(Widget widget)
        {
            String widgetName = widget.GetType().Name;

            if (String.CompareOrdinal(widgetName, typeof(ShowDataFormWpf).Name) == 0)
            {
                ShowDataFormWpf myWidget = widget as ShowDataFormWpf;
                myWidget.WidgetPreviewMouseDown += new MouseButtonEventHandler(myWidget_PreviewMouseDown);
                myWidget.WidgetPreviewMouseUp += new MouseButtonEventHandler(myWidget_PreviewMouseUp);
                myWidget.WidgetClick += new MouseButtonEventHandler(myWidget_WidgetClick);
                myWidget.WidgetContextMenuEditClick += new RoutedEventHandler(myWidget_WidgetContextMenuEditClick);
                myWidget.WidgetContextMenuDeleteClick += new RoutedEventHandler(myWidget_WidgetContextMenuDeleteClick);
                myWidget.ConnectionClick += new EventHandler<ConnectionPointClickEventArgs>(myWidget_ConnectionClick);
            }
            if (String.CompareOrdinal(widgetName, typeof(EnterSingleDataFormWpf).Name) == 0)
            {
                EnterSingleDataFormWpf myWidget = widget as EnterSingleDataFormWpf;
                myWidget.WidgetPreviewMouseDown += new MouseButtonEventHandler(myWidget_PreviewMouseDown);
                myWidget.WidgetPreviewMouseUp += new MouseButtonEventHandler(myWidget_PreviewMouseUp);
                myWidget.WidgetClick += new MouseButtonEventHandler(myWidget_WidgetClick);
                myWidget.WidgetContextMenuEditClick += new RoutedEventHandler(myWidget_WidgetContextMenuEditClick);
                myWidget.WidgetContextMenuDeleteClick += new RoutedEventHandler(myWidget_WidgetContextMenuDeleteClick);
                myWidget.ConnectionClick += new EventHandler<ConnectionPointClickEventArgs>(myWidget_ConnectionClick);
            }
            if (String.CompareOrdinal(widgetName, typeof(MenuFormWpf).Name) == 0)
            {
                MenuFormWpf myWidget = widget as MenuFormWpf;
                myWidget.WidgetPreviewMouseDown += new MouseButtonEventHandler(myWidget_PreviewMouseDown);
                myWidget.WidgetPreviewMouseUp += new MouseButtonEventHandler(myWidget_PreviewMouseUp);
                myWidget.WidgetClick += new MouseButtonEventHandler(myWidget_WidgetClick);
                myWidget.WidgetContextMenuEditClick += new RoutedEventHandler(myWidget_WidgetContextMenuEditClick);
                myWidget.WidgetContextMenuDeleteClick += new RoutedEventHandler(myWidget_WidgetContextMenuDeleteClick);

                myWidget.ConnectionClick += new EventHandler<ConnectionPointClickEventArgs>(myWidget_ConnectionClick);
            }
            if (String.CompareOrdinal(widgetName, typeof(ListFormWpf).Name) == 0)
            {
                ListFormWpf myWidget = widget as ListFormWpf;
                myWidget.WidgetPreviewMouseDown += new MouseButtonEventHandler(myWidget_PreviewMouseDown);
                myWidget.WidgetPreviewMouseUp += new MouseButtonEventHandler(myWidget_PreviewMouseUp);
                myWidget.WidgetClick += new MouseButtonEventHandler(myWidget_WidgetClick);
                myWidget.WidgetContextMenuEditClick += new RoutedEventHandler(myWidget_WidgetContextMenuEditClick);
                myWidget.WidgetContextMenuDeleteClick += new RoutedEventHandler(myWidget_WidgetContextMenuDeleteClick);

                myWidget.ConnectionClick += new EventHandler<ConnectionPointClickEventArgs>(myWidget_ConnectionClick);
            }
            if (String.CompareOrdinal(widgetName, typeof(DataSourceWpf).Name) == 0)
            {
                DataSourceWpf myWidget = widget as DataSourceWpf;
                myWidget.WidgetPreviewMouseDown += new MouseButtonEventHandler(myWidget_PreviewMouseDown);
                myWidget.WidgetPreviewMouseUp += new MouseButtonEventHandler(myWidget_PreviewMouseUp);
                myWidget.WidgetClick += new MouseButtonEventHandler(myWidget_WidgetClick);
                myWidget.WidgetContextMenuDeleteClick += new RoutedEventHandler(myWidget_WidgetContextMenuDeleteClick);
                myWidget.ConnectionClick += new EventHandler<ConnectionPointClickEventArgs>(myWidget_ConnectionClick);
            }
        }


        private void myConnection_ConnectionContextMenuDeleteClick(object sender, RoutedEventArgs e)
        {
            ConnectionWpf connectionWpf = sender as ConnectionWpf;
            this.RemoveConnection(connectionWpf);
            this.RedrawDocument();
        }

        private void myWidget_WidgetContextMenuDeleteClick(object sender, RoutedEventArgs e)
        {
            Widget widget = sender as Widget;
            DataSourceWpf dataSourceWpf = widget as DataSourceWpf;
            if (dataSourceWpf != null && DataSourceDeleted != null)
            {
                DataSourceDeleted(dataSourceWpf, e);
            }

            this.RemoveWidget(widget);
            this.RedrawDocument();

            if ((widget is ListFormWpf || widget is MenuFormWpf) && FormDeleted != null)
            {
                FormDeleted(widget, e);
            }

        }

        private void myWidget_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropAction == false)
            {
                IDrawAbleWpf widget = sender as IDrawAbleWpf; // --
                isDragDropAction = true;
                currentElement = widget.UIElement;

                mousePosition.X = e.GetPosition(widget.UIElement).X;
                mousePosition.Y = e.GetPosition(widget.UIElement).Y;
            }
        }

        private void myWidget_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropAction == true)
            {
                isDragDropAction = false;
                Widget senderAsWidget = sender as Widget;
                IDrawAbleWpf senderAsIDrawAble = sender as IDrawAbleWpf;
                senderAsWidget.XCoordinateRelativeToParent = Canvas.GetLeft(senderAsIDrawAble.UIElement);
                senderAsWidget.YCoordinateRelativeToParent = Canvas.GetTop(senderAsIDrawAble.UIElement);
            }
        }

        private void myWidget_WidgetClick(object sender, MouseButtonEventArgs e)
        {
            RedrawConnections();
            ActiveWidget = sender as Widget;
        }

        /// <summary>
        /// Funcion llamada cuando se selecciona la opcion para editar un componente
        ///Esta funcion abre una ventana que permite editar el componente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myWidget_WidgetContextMenuEditClick(object sender, EventArgs e)
        {

            ShowDataFormWpf showForm = sender as ShowDataFormWpf;
            EnterSingleDataFormWpf inputForm = sender as EnterSingleDataFormWpf;
            MenuFormWpf menuForm = sender as MenuFormWpf;
            ListFormWpf listForm = sender as ListFormWpf;

            if (showForm!=null)
            {
                if (showForm.InputDataContext == null)
                {
                    Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.NoInputConnection, Resources.NoInputForm);
                    return;
                }
                WindowShowDataForm window = new WindowShowDataForm(showForm, this);
                window.Owner = windowsDesigner;
                window.ShowDialog();
                return;
            }
            if (inputForm != null)
            {
                if (inputForm.InputConnectionPoint.Connection.Count == 0)
                {
                    Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.NoInputConnection, UtnEmall.ServerManager.Properties.Resources.Error);
                    return;
                }
                WindowEnterSingleDataForm window = new WindowEnterSingleDataForm(inputForm, this);
                window.Owner = windowsDesigner;
                window.ShowDialog();
                return;
            }
            if (menuForm!=null)
            {
                WindowMenuForm window = new WindowMenuForm(menuForm, this);
                window.Owner = windowsDesigner;
                window.ShowDialog();
                return;
            }
            if (listForm!=null)
            {
                if (listForm.InputDataContext == null)
                {
                    Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.NoInputConnection, UtnEmall.ServerManager.Properties.Resources.Error);
                    return;
                }
                WindowListForm window = new WindowListForm(listForm, this);
                window.Owner = windowsDesigner;
                window.ShowDialog();
                return;
            }

        }


        /// <summary>
        /// Función llamada cuando se presiona un punto de conexión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myWidget_ConnectionClick(object sender, ConnectionPointClickEventArgs e)
        {
            if (IsMakeConnectionAction)
            {
                Component tempComponent = sender as Component;
                ConnectionPointWpf connectionPoint = e.ConnectionPoint as ConnectionPointWpf;
                // Si es una conexión
                if (ConnectionWidgetFrom == null)
                {
                    Error errorSingleOutput = VerifySingleOutputConnection(tempComponent);
                    if (errorSingleOutput != null)
                    {
                        Util.ShowErrorDialog(errorSingleOutput.Description, UtnEmall.ServerManager.Properties.Resources.ConnectionErrors);
                        return;
                    }
                    ConnectionWidgetFrom = tempComponent;
                    ConnectionPointFrom = connectionPoint;
                    windowsDesigner.textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectFormTargetConnection;
                    return;
                }
                //  
                //
                else
                {
                    ConnectionWidgetTarget = tempComponent;
                    ConnectionPointTarget = connectionPoint;

                    Error connectionError = CheckDuplicatedConnection();
                    if (connectionError != null)
                    {
                        Util.ShowErrorDialog(connectionError.Description, "Error");
                        windowsDesigner.textBlockStatusBar.Text = ".";
                        isMakeConnectionAction = false;
                        return;
                    }

                    Collection<Error> connectionErrors = CheckConnection();
                    if (connectionErrors.Count > 0)
                    {
                        Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrors, connectionErrors);
                        windowsDesigner.textBlockStatusBar.Text = ".";
                        isMakeConnectionAction = false;
                        return;
                    }

                    Collection<Error> errors = VerifyConnection();
                    if (errors.Count > 0)
                    {
                        Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrors, errors);
                        windowsDesigner.textBlockStatusBar.Text = ".";
                        windowsDesigner.textBlockStatusBar.UpdateLayout();
                        isMakeConnectionAction = false;
                        return;
                    }
                    CreateConnection();
                    RedrawConnections();
                    isMakeConnectionAction = false;
                    windowsDesigner.textBlockStatusBar.Text = ".";
                }
            }
        }

        /// <summary>
        /// Crea una nueva conexión wpf
        /// </summary>
        private void CreateConnection()
        {
            ConnectionWpf connectionWpf = new ConnectionWpf(ConnectionPointFrom as ConnectionPointWpf, ConnectionPointTarget as ConnectionPointWpf);
            connectionWpf.ConnectionContextMenuDeleteClick += new RoutedEventHandler(myConnection_ConnectionContextMenuDeleteClick);
            AddConnection(connectionWpf);
            windowsDesigner.textBlockStatusBar.Text = ".";
        }

        /// <summary>
        /// Elimina todas las conexiones
        /// </summary>
        private void DeleteDrawConnections()
        {
            foreach (Connection connection in Connections)
            {
                DrawArea.Children.Remove(((ConnectionWpf)connection).UIElement);
            }
        }

        #endregion

        #endregion
    }
}

