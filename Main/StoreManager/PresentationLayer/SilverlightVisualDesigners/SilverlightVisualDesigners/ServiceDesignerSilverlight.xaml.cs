using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LogicalLibrary;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;
using LogicalLibrary.Widgets;
using PresentationLayer.ServerDesignerClasses;
using SilverlightVisualDesigners.EditControls;
using SilverlightVisualDesigners.Widgets;
using UtnEmall.Proxies;
using UtnEmall.Server.EntityModel;

namespace SilverlightVisualDesigners
{
    public partial class ServiceDesignerSilverlight : UserControl
    {
        private ServiceDocument serviceDocument;
        private int numberOfForms;
        int ZindexValueForConnections = 1;
        int zindexValueForWidgets = 2;
        int idService;
        
        private bool isMakeConnectionAction;
        /// <summary>
        /// Indica si fue seleccionada la acción "MakeConnection".
        /// </summary>
        public bool IsMakeConnectionAction
        {
            get { return isMakeConnectionAction; }
            set { isMakeConnectionAction = value; }
        }

        private IConnection iconnectableFrom;
        private IConnection iconnectableTarget;
        private IConnection currentElement;

        public ServiceDesignerSilverlight(int idService)
        {
            InitializeComponent();
            CompleteButtonTexts();
            this.idService = idService;
            numberOfForms = 1;
        }

        /// <summary>
        /// Usado para completar el texto del botón en el menú.
        /// </summary>
        private void CompleteButtonTexts()
        {
            buttonList.Text = ServiceDesignerSilverlightResources.button_List;
            buttonDisplayData.Text = ServiceDesignerSilverlightResources.button_ShowData;
            buttonMenu.Text = ServiceDesignerSilverlightResources.button_Menu;
            buttonDataInput.Text = ServiceDesignerSilverlightResources.button_Input;
            buttonConnection.Text = ServiceDesignerSilverlightResources.button_Connection;
            buttonAdd.Text = ServiceDesignerSilverlightResources.button_Add;
            buttonSave.Text = ServiceDesignerSilverlightResources.button_Save;
            buttonCancel.Text = ServiceDesignerSilverlightResources.button_Cancel;
        }

        /// <summary>
        /// Enlaza eventos a componentes.
        /// </summary>
        /// <param name="iConnection">Objeto IDrawable al cual enlazar eventos.</param>
        private void AttachEvents(IConnection iConnection)
        {
            if (iConnection == null)
            {
                throw new ArgumentNullException("iConnection", "iConnection can not be null in method AttachEvents in class ServiceDesignerSilverlight.");
            }
            iConnection.MouseLeftButtonDown += new MouseButtonEventHandler(iDrawable_MouseLeftButtonDown);
            iConnection.MouseLeftButtonUp += new MouseButtonEventHandler(iDrawable_MouseLeftButtonUp);
            iConnection.Configure += new MouseMenuWidgetClickEventHandler(iConnection_Configure);
            iConnection.Deleted += new MouseMenuWidgetClickEventHandler(iConnection_Deleted);
        }

        void iConnection_Deleted(object sender, MouseDoubleClickEventArgs e)
        {
            IComponent iWidget = sender as IComponent;
            if (iWidget != null)
            {
                if (iWidget.Component is DataSource)
                {
                    DataSource dataSource = iWidget.Component as DataSource;
                    this.listDataModels.Items.Add(dataSource.RelatedTable);
                }
                if (iWidget.Component is ListForm || iWidget.Component is MenuForm)
                {
                    listBoxStartWidget.Items.Remove(iWidget.Component as Widget);
                }
                RemoveWidget(iWidget);
            }
        }

        private void RemoveWidget(IComponent iWidget)
        {
            IDraw iDrawable = iWidget as IDraw;
            if (iDrawable != null)
            {
                serviceDocument.RemoveWidget(iWidget.Component as Widget);
                canvasDraw.Children.Remove(iDrawable as UIElement);
            }
        }

        void iConnection_Configure(object sender, MouseDoubleClickEventArgs e)
        {

            IComponent iWidget = sender as IComponent;
            if (iWidget != null)
            {
                if (iWidget.Component.InputDataContext == null && e.WidgetType != WidgetType.MenuForm)
                {
                    Dialog.ShowInformationDialog(SilverlightVisualDesigners.Properties.Resources.InvalidConnectionError, SilverlightVisualDesigners.Properties.Resources.InvalidConnectionMessage, this.LayoutRoot);
                    return;
                }

                IWindow iwindows = null;
                switch (e.WidgetType)
                {
                    case WidgetType.ListForm:
                        iwindows = new EditListFormControl(sender as ListFormSilverlight, serviceDocument.DataModel);
                        break;
                    case WidgetType.MenuForm:
                        iwindows = new EditMenuFormControl(sender as MenuFormSilverlight, serviceDocument.DataModel);
                        break;
                    case WidgetType.ShowDataForm:
                        iwindows = new EditShowDataFormControl(sender as ShowDataFormSilverlight, serviceDocument.DataModel);
                        break;
                    case WidgetType.EnterSingleDataForm:
                        iwindows = new EditEnterSingleDataFormControl(sender as EnterSingleDataFormSilverlight);
                        break;
                    case WidgetType.Table:
                        iwindows = new EditTableControl(sender as TableSilverlight, this);
                        break;
                    default:
                        break;
                }

                // Deshabilitar el Canvas para no capturar más eventos.
                Utils.DisablePanel(this.LayoutRoot);

                UserControl userControl = iwindows as UserControl;
                this.LayoutRoot.Children.Add(userControl);
                Grid.SetColumn(userControl, 2);
                Grid.SetRow(userControl, 1);
                iwindows.Closed += new EventHandler(editControl_Closed);
            }
        }

        void iDrawable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentElement = sender as IConnection;
        }

        void iDrawable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DragAndDropCanvas.IsResizeAction = false;
            this.serviceDocument.ActiveWidget = (sender as IComponent).Component as Widget;
            if (isMakeConnectionAction)
            {
                IConnection iConnection = sender as IConnection;
                ProcessConnection(iConnection);
            }
        }

        void editControl_Closed(object sender, EventArgs e)
        {
            UserControl editControl = sender as UserControl;
            if (editControl is EditListFormControl || editControl is EditMenuFormControl)
            {
                listBoxStartWidget.Items.Clear();
                LoadListBoxStartWidget();
            }
            this.LayoutRoot.Children.Remove(editControl);
            this.LayoutRoot.Children.Remove(Utils.CoverRectangle);
        }

        /// <summary>
        /// Usado para saver origen y destino en una conexión cuando el usuario lanza un evento MousseUp en un objeto IConnection.
        /// </summary>
        /// <param name="iConnection">Objeto IConnection a ser conectado.</param>
        private void ProcessConnection(IConnection iConnectable)
        {
            if (iConnectable == null)
            {
                throw new ArgumentNullException("iConnectable", "iConnectable can not be null.");
            }

            if (iconnectableFrom == null)
            {
                // Verificar que no se hayan cometido errores.
                Error errorSingleOutput = ServiceDocument.VerifySingleOutputConnection(iConnectable.Component);
                if (errorSingleOutput != null)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, errorSingleOutput.Description, this.LayoutRoot);
                    return;
                }

                MenuFormSilverlight iConnectableSL = iConnectable as MenuFormSilverlight;
                if (iConnectableSL != null)
                {
                    if (iConnectableSL.ClickedMenuItem == null)
                    {
                        Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidConnectionError, SilverlightVisualDesigners.Properties.Resources.InvalidConnection_MenuItemMustBeSourceInMenu, this.LayoutRoot);
                        return;
                    }
                    iconnectableFrom = iConnectableSL.ClickedMenuItem;
                }
                else
                {
                    iconnectableFrom = iConnectable;
                }

                // Recordar que un objeto MenuItem tiene a su widget como un Component.
                serviceDocument.ConnectionWidgetFrom = iconnectableFrom.Component;
            }
            else if (iConnectable != iconnectableFrom)
            {
                iconnectableTarget = iConnectable;
                serviceDocument.ConnectionWidgetTarget = iconnectableTarget.Component;

                Error connectionError = serviceDocument.CheckDuplicatedConnection();
                if (connectionError != null)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, connectionError.Description, this.LayoutRoot);
                    isMakeConnectionAction = false;
                    return;
                }

                Collection<Error> connectionErrors = serviceDocument.CheckConnection();

                if (connectionErrors.Count > 0)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, connectionErrors[0].Description, this.LayoutRoot);
                    isMakeConnectionAction = false;
                    return;
                }

                Collection<Error> errors = serviceDocument.VerifyConnection();
                if (errors.Count > 0)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, errors[0].Description, this.LayoutRoot);
                    isMakeConnectionAction = false;
                    return;
                }

                // Verificar si se puede asignar el contexto.
                CreateConnection(iconnectableFrom, iconnectableTarget);
                isMakeConnectionAction = false;
            }
        }

        /// <summary>
        /// Crear el objeto SilverlightConnectionObject.
        /// </summary>
        /// <param name="from">Objeto IConnection origen de ConnectionSilverlight.</param>
        /// <param name="targetTable">Objeto IConnection destino de ConnectionSilverlight.</param>
        private void CreateConnection(IConnection from, IConnection target)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from", "from can not be null.");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target", "target can not be null.");
            }

            ConnectionSilverlight connection = new ConnectionSilverlight(from, target);
            serviceDocument.AddConnection(connection.Connection);
            AddRelation(connection);
        }

        void connection_Change(object sender, EventArgs e)
        {
            UpdateConnection(sender as ConnectionSilverlight);
        }

        /// <summary>
        /// Dibuja el objeto ConnectionSilverlight en un CanvasDraw, es decir:
        /// Establece "top" y "left" de un UserControl que representa una conexión
        /// y la agrega a la colección hija del objeto CanvasDraw.
        /// </summary>
        /// <param name="connection">Conexión a dibujar.</param>
        private void AddRelation(ConnectionSilverlight connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection", "connection can not be null in method AddRelation in class ServiceDesignerSilverlight.");
            }
            this.canvasDraw.Children.Add(connection.MyMenuWidget);
            Canvas.SetZIndex(connection.MyMenuWidget, ZindexValueForConnections + 1);
            connection.Change += new EventHandler(connection_Change);
            connection.Reset += new EventHandler(connection_Reset);
            connection.Delete += new EventHandler(connection_Delete);
            UpdateConnection(connection);

            iconnectableFrom = null;

            canvasDraw.Children.Add(connection.Path);
            Canvas.SetZIndex(connection.Path, ZindexValueForConnections);
        }

        void connection_Delete(object sender, EventArgs e)
        {
            ConnectionSilverlight connectionSilverlight = sender as ConnectionSilverlight;
            serviceDocument.RemoveConnection(connectionSilverlight.Connection);
            canvasDraw.Children.Remove(connectionSilverlight.Path);
            canvasDraw.Children.Remove(connectionSilverlight.MyMenuWidget);
        }

        void connection_Reset(object sender, EventArgs e)
        {
            ConnectionSilverlight connectionSilverlight = sender as ConnectionSilverlight;
            connectionSilverlight.Connection.Reset(true, true);
        }

        /// <summary>
        /// Actualiza el punto de origen y el punto de destino de un objeto
        /// ConnectionSilverlight en relación a su CanvasDraw.
        /// </summary>
        /// <param name="connection">Conexión a actualizar.</param>
        private void UpdateConnection(ConnectionSilverlight connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection", "connection can not be null.");
            }
            GeneralTransform fromGeneralTransform = (connection.WidgetSource as UIElement).TransformToVisual(this.canvasDraw);
            GeneralTransform targetGeneralTransform = (connection.WidgetTarget as UIElement).TransformToVisual(this.canvasDraw);

            connection.StartPoint = fromGeneralTransform.Transform(connection.WidgetSource.VisualOutputPoint);
            connection.Endpoint = targetGeneralTransform.Transform(connection.WidgetTarget.VisualInputPoint);

            Canvas.SetLeft(connection.MyMenuWidget, ((connection.Endpoint.X - connection.StartPoint.X) / 2) + connection.StartPoint.X - (connection.MyMenuWidget.ActualWidth / 2));
            Canvas.SetTop(connection.MyMenuWidget, ((connection.Endpoint.Y - connection.StartPoint.Y) / 2) + connection.StartPoint.Y - (connection.MyMenuWidget.ActualHeight / 2));
        }

        private void LoadListDataSources()
        {
            if (serviceDocument.DataModel.Tables != null)
            {
                foreach (Table table in serviceDocument.DataModel.Tables)
                {
                    listDataModels.Items.Add(table);
                    if (serviceDocument != null)
                    {
                        foreach (Component component in serviceDocument.Components)
                        {
                            DataSource dataSource = component as DataSource;
                            if (dataSource != null)
                            {
                                listDataModels.Items.Remove(dataSource.RelatedTable);
                            }
                        }
                    }
                }
            }
        }

        private void LoadListBoxStartWidget()
        {
            if (serviceDocument.Components != null)
            {
                foreach (Widget widget in serviceDocument.Components)
                {
                    if (widget is ListForm || widget is MenuForm)
                    {
                        listBoxStartWidget.Items.Add(widget);
                        if (serviceDocument.StartWidget == widget)
                        {
                            listBoxStartWidget.SelectedItem = widget;
                        }
                    }
                }
                if (listBoxStartWidget.Items.Count > 0)
                {
                    listBoxStartWidget.SelectedIndex = 0;
                }
            }
        }

        private void EndSaveCustomerService(SaveCustomerServiceResult result)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (result.Success)
                {
                    Dialog.ShowOkDialog(SilverlightVisualDesigners.Properties.Resources.Information, SilverlightVisualDesigners.Properties.Resources.CustomerServiceSuccessfullySaved, this.LayoutRoot, PagesConstants.ListServicesPage);
                }
                else
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.ErrorWhileSavingCustomerService, this.LayoutRoot);
                }
            }
            );
        }

        private void Builder(IConnection iConnection)
        {
            if (iConnection == null)
            {
                throw new ArgumentNullException("iConnection", "iConnection can not be null.");
            }
            AttachEvents(iConnection);

            UIElement iConnectionUpCast = iConnection as UIElement;
            canvasDraw.Add(iConnectionUpCast);
            Canvas.SetLeft(iConnectionUpCast, iConnection.XCoordinateRelativeToParent);
            Canvas.SetTop(iConnectionUpCast, iConnection.YCoordinateRelativeToParent);
            Canvas.SetZIndex(iConnectionUpCast, zindexValueForWidgets);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisableAllOptions();

            (App.Current as App).LoadService(idService, new CustomerServiceLoadedCallback(CustomerServiceLoaded));
        }

        void CustomerServiceLoaded(ServiceDocument serviceDocument)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                this.serviceDocument = serviceDocument;
                LoadServiceData(serviceDocument);
                UpdateWindow();
                EnableAllOptions();
            });
        }

        void UpdateWindow()
        {
            LoadListDataSources();
            LoadListBoxStartWidget();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void DisableAllOptions()
        {
            buttonList.Enabled = false;
            buttonDisplayData.Enabled = false;
            buttonMenu.Enabled = false;
            buttonDataInput.Enabled = false;
            buttonConnection.Enabled = false;
            buttonAdd.Enabled = false;
            buttonSave.Enabled = false;
            buttonCancel.Enabled = true;
        }

        private void EnableAllOptions()
        {
            buttonList.Enabled = true;
            buttonDisplayData.Enabled = true;
            buttonMenu.Enabled = true;
            buttonDataInput.Enabled = true;
            buttonConnection.Enabled = true;
            buttonAdd.Enabled = true;
            buttonSave.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void canvasDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragAndDropCanvas.IsDragDropAction)
            {
                currentElement.OnDrag();
                currentElement.XCoordinateRelativeToParent = Canvas.GetLeft(currentElement as UIElement);
                currentElement.YCoordinateRelativeToParent = Canvas.GetTop(currentElement as UIElement);
            }
        }

        private void buttonList_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = false;
                ListFormSilverlight listFormSilverlight = new ListFormSilverlight();
                listFormSilverlight.ChangeTitle(SilverlightVisualDesigners.Properties.Resources.ListFormName + " " + numberOfForms);
                numberOfForms++;
                Builder(listFormSilverlight);
                serviceDocument.AddWidget(listFormSilverlight.ListForm);
                listBoxStartWidget.Items.Add(listFormSilverlight.ListForm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void buttonDisplayData_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = false;
                ShowDataFormSilverlight showDataFormSilverlight = new ShowDataFormSilverlight();
                showDataFormSilverlight.ChangeTitle(SilverlightVisualDesigners.Properties.Resources.ShowDataFormName + " " + numberOfForms);
                numberOfForms++;
                Builder(showDataFormSilverlight);
                serviceDocument.AddWidget(showDataFormSilverlight.ShowDataForm);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = false;
                MenuFormSilverlight menuFormSilverlight = new MenuFormSilverlight();
                menuFormSilverlight.ChangeTitle(SilverlightVisualDesigners.Properties.Resources.MenuFormName + " " + numberOfForms);
                numberOfForms++;
                Builder(menuFormSilverlight);
                serviceDocument.AddWidget(menuFormSilverlight.MenuForm);
                listBoxStartWidget.Items.Add(menuFormSilverlight.MenuForm);
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.SaveError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonDataInput_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = false;
                EnterSingleDataFormSilverlight singleDataSilverlight = new EnterSingleDataFormSilverlight();
                singleDataSilverlight.ChangeTitle(SilverlightVisualDesigners.Properties.Resources.EnterSingleDataFormName + " " + numberOfForms);
                numberOfForms++;
                Builder(singleDataSilverlight);
                serviceDocument.AddWidget(singleDataSilverlight.EnterSingleDataForm);
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.SaveError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonConnection_Clicked(object sender, EventArgs e)
        {
            try
            {
                iconnectableFrom = null;
                iconnectableTarget = null;

                isMakeConnectionAction = true;
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.SaveError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                Table tableSelected = listDataModels.SelectedItem as Table;
                if (listDataModels.SelectedIndex != -1 && tableSelected != null)
                {
                    isMakeConnectionAction = false;
                    DataSourceSilverlight dataSourceSilverlight = new DataSourceSilverlight(tableSelected);
                    Builder(dataSourceSilverlight);
                    serviceDocument.AddWidget(dataSourceSilverlight.DataSource);
                    listDataModels.Items.Remove(tableSelected);
                }
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.SaveError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                serviceDocument.StartWidget = listBoxStartWidget.SelectedItem as Widget;
                if (serviceDocument.StartWidget == null)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidCustomService, SilverlightVisualDesigners.Properties.Resources.NoStartForm, this.LayoutRoot);
                    return;
                }

                Collection<Error> errors = serviceDocument.CheckDesignerLogic();
                if (errors.Count > 0)
                {
                    string message = String.Empty;
                    foreach (Error error in errors)
                    {
                        message = message + error.Name + ":" + error.Description + "\n";
                    }
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidCustomService, message, this.LayoutRoot);
                    return;
                }

                if (!serviceDocument.CheckValidPathForms())
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.InvalidServicePath, this.LayoutRoot);
                    return;
                }

                // Llamar al Servicio Web.
                (App.Current as App).BeginSaveCustomerService(this.serviceDocument, new SaveCustomerServiceCallback(EndSaveCustomerService));
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.SaveError, error.Message, this.LayoutRoot);
            }
        }

        private void buttonCancel_Clicked(object sender, EventArgs e)
        {
            ReturnToListServices();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private static void ReturnToListServices()
        {
            // Volver a la página de origen.
            HtmlPage.Window.Navigate(new Uri(PagesConstants.Homepage, UriKind.Relative));
        }

        private void LoadServiceData(ServiceDocument serviceDocument)
        {
            Dictionary<Component, IConnection> widgetEquivalences = new Dictionary<Component, IConnection>();

            foreach (Component component in serviceDocument.Components)
            {
                Type type = component.GetType();
                switch (type.Name)
                {
                    case "DataSource":
                        DataSource dataSource = component as DataSource;
                        DataSourceSilverlight dataSourceSilverlight = new DataSourceSilverlight(dataSource);
                        Builder(dataSourceSilverlight);
                        widgetEquivalences.Add(dataSource, dataSourceSilverlight);
                        break;
                    case "ListForm":
                        ListForm listForm = component as ListForm;
                        ListFormSilverlight listFormSilverlight = new ListFormSilverlight(listForm);
                        Builder(listFormSilverlight);
                        widgetEquivalences.Add(listForm, listFormSilverlight);
                        break;
                    case "ShowDataForm":
                        ShowDataForm showDataForm = component as ShowDataForm;
                        ShowDataFormSilverlight showDataFormSilverlight = new ShowDataFormSilverlight(showDataForm);
                        Builder(showDataFormSilverlight);
                        widgetEquivalences.Add(showDataForm, showDataFormSilverlight);
                        break;
                    case "EnterSingleDataForm":
                        EnterSingleDataForm enterSingleDataForm = component as EnterSingleDataForm;
                        EnterSingleDataFormSilverlight singleDataSilverlight = new EnterSingleDataFormSilverlight(enterSingleDataForm);
                        Builder(singleDataSilverlight);
                        widgetEquivalences.Add(enterSingleDataForm, singleDataSilverlight);
                        break;
                    case "MenuForm":
                        MenuForm menuForm = component as MenuForm;
                        MenuFormSilverlight menuSilverlight = new MenuFormSilverlight(menuForm);
                        Builder(menuSilverlight);
                        widgetEquivalences.Add(menuForm, menuSilverlight);
                        foreach (MenuItemSilverlight menuItemSilverlight in menuSilverlight.MenuItemsSilverlight)
                        {
                            widgetEquivalences.Add(menuItemSilverlight.FormMenuItem, menuItemSilverlight);
                        }
                        break;
                    default:
                        throw new ArgumentException(SilverlightVisualDesigners.Properties.Resources.InvalidTypeOfWidget);
                }
            }

            this.canvasDraw.UpdateLayout();

            foreach (Connection connection in serviceDocument.Connections)
            {
                IConnection from = widgetEquivalences[connection.Source.Parent];
                IConnection target = widgetEquivalences[connection.Target.Parent];

                ConnectionSilverlight connectionSilverlight = new ConnectionSilverlight(from, target);
                connectionSilverlight.Connection = connection;
                AddRelation(connectionSilverlight);
            }
        }
    }
}
