using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LogicalLibrary.DataModelClasses;
using PresentationLayer.ServerDesignerClasses;
using UtnEmall.Proxies;
using UtnEmall.Server.EntityModel;

namespace SilverlightVisualDesigners
{
    public partial class DataModelDesignerSilverlight : UserControl
    {
        int zindexValueForWidgets = 2;
        int ZindexValueForConnections = 1;
        private DataModel dataModel;
        private List<IConnection> tables;
        private bool isMakeConnectionAction;
        private RelationType selectedRelationType;
        private IConnection iconnectableFrom;
        private IConnection iconnectableTarget;
        private IConnection currentElement;

        private List<ConnectionSilverlight> connectionsSilverLight;

        /// <summary>
        /// Lista de las relaciones creadas entre tablas.
        /// </summary>
        public List<ConnectionSilverlight> ConnectionsSilverlight
        {
            get
            {
                if (connectionsSilverLight == null)
                {
                    connectionsSilverLight = new List<ConnectionSilverlight>();
                }
                return connectionsSilverLight;
            }
        }

        /// <summary>
        /// El objeto DataModel donde se guardaran los datos.
        /// </summary>
        public DataModel DataModel
        {
            get
            {
                return dataModel;
            }
        }

        /// <summary>
        /// Indica si esta seleccionada la accion de hacer una conexión.
        /// </summary>
        public bool IsMakeConnectionAction
        {
            get { return isMakeConnectionAction; }
            set { isMakeConnectionAction = value; }
        }

        /// <summary>
        /// Inicializacion de los atributos.
        /// </summary>
        public DataModelDesignerSilverlight()
        {
            // Requerido para inicializar variables.
            InitializeComponent();
            CompleteButtonTexts();
            this.dataModel = new DataModel();
            this.tables = new List<IConnection>();
        }

        /// <summary>
        /// Usado para competar los textos de los botones del menu.
        /// </summary>
        private void CompleteButtonTexts()
        {
            ButtonNewTable.Text = DataModelDesignerSilverlightResources.button_NewTable;
            buttonOneToOne.Text = DataModelDesignerSilverlightResources.button_OneToOne;
            buttonOneToMany.Text = DataModelDesignerSilverlightResources.button_OneToMany;
            buttonManyToMany.Text = DataModelDesignerSilverlightResources.button_ManyToMany;
            buttonSave.Text = DataModelDesignerSilverlightResources.button_Save;
            buttonCancel.Text = DataModelDesignerSilverlightResources.button_Cancel;
        }

        /// <summary>
        /// Carga el modelo de datos desde un Web Service.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadDataModel()
        {
            (App.Current as App).BeginLoadDataModel(new SilverlightVisualDesigners.DataModelLoadedCallback(DataModelLoaded));
        }

        void DataModelLoaded(DataModel dataModel)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                this.SetDataModel(dataModel);
            });
        }

        /// <summary>
        /// Construye el objeto DataModelSilverlight desde un objeto DataModel cargado.
        /// </summary>
        /// <param name="dataModel">objeto DataModel usado para construir</param>
        public void SetDataModel(DataModel dataModel)
        {
            this.dataModel = dataModel;
            this.tables = new List<IConnection>();

            // Construye y dibuja las tablas.
            foreach (Table table in dataModel.Tables)
            {
                TableSilverlight tableSilverlight = new TableSilverlight(table);
                AtachEvents(tableSilverlight);

                tables.Add(tableSilverlight);

                Canvas.SetLeft(tableSilverlight, tableSilverlight.XCoordinateRelativeToParent);
                Canvas.SetTop(tableSilverlight, tableSilverlight.YCoordinateRelativeToParent);
                this.canvasDraw.Add(tableSilverlight);
                Canvas.SetZIndex(tableSilverlight, zindexValueForWidgets);
                tableSilverlight.UpdateLayout();
            }

            // Construye y dibuja las relaciones.
            foreach (Relation relation in dataModel.Relations)
            {
                int SourcePosition = dataModel.Tables.IndexOf(relation.Source);
                int TargetPosition = dataModel.Tables.IndexOf(relation.Target);

                ConnectionSilverlight connection = new ConnectionSilverlight(tables[SourcePosition], tables[TargetPosition], relation.RelationType);
                connection.Relation = relation;

                connection.Change += new EventHandler(connection_Change);
                connection.Reset += new EventHandler(connection_Reseted);
                connection.Loaded += new EventHandler(connection_Loaded);

                this.canvasDraw.Children.Add(connection.MyMenuWidget);
                Canvas.SetZIndex(connection.MyMenuWidget, ZindexValueForConnections + 1);

                canvasDraw.Children.Add(connection.Path);
                Canvas.SetZIndex(connection.Path, ZindexValueForConnections);

                ConnectionsSilverlight.Add(connection);
            }
        }

        void connection_Loaded(object sender, EventArgs e)
        {
            UpdateConnection(sender as ConnectionSilverlight);
        }


        /// <summary>
        /// Añade un Wodget al objeto dataModelSilverlight.
        /// </summary>
        /// <param name="tableSilverlight">TableSilverligth a ser añadido.</param>
        private void AddWidget(TableSilverlight tableSilverlight)
        {
            AtachEvents(tableSilverlight);
            //  
            //
            tables.Add(tableSilverlight);
            //  
            //
            this.canvasDraw.Add(tableSilverlight);
            Canvas.SetZIndex(tableSilverlight, zindexValueForWidgets);
            dataModel.AddTable(tableSilverlight.Table);
        }

        /// <summary>
        /// Elimina un IComponent de un objeto dataModelSilverlight.
        /// </summary>
        /// <param name="iWidget">Componente a ser eliminado.</param>
        private void RemoveWidget(IComponent iWidget)
        {
            IDraw iDrawable = iWidget as IDraw;
            if (iDrawable != null)
            {
                dataModel.RemoveTable(iWidget.Component as Table);
                //  
                //
                tables.Remove(iWidget as IConnection);
                //  
                //

                canvasDraw.Children.Remove(iDrawable as UIElement);
            }
        }

        /// <summary>
        /// Dibuja la conexión en el canvas.
        /// </summary>
        /// <param name="connection">Conexión a ser dibujada.</param>
        private void AddRelation(ConnectionSilverlight connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection", "connectionSilverlight can not be null.");
            }

            this.canvasDraw.Children.Add(connection.MyMenuWidget);
            Canvas.SetZIndex(connection.MyMenuWidget, ZindexValueForConnections + 1);

            UpdateConnection(connection);

            iconnectableFrom = null;
            dataModel.AddRelation(connection.Relation);

            canvasDraw.Children.Add(connection.Path);
            Canvas.SetZIndex(connection.Path, ZindexValueForConnections);
        }

        /// <summary>
        /// Suscribe los eventos del componente necesarios para manejarlo en el diseñador visual.
        /// </summary>
        /// <param name="iConnection">Objeto IDrawable el cual se desea suscribirlo a los eventos</param>
        private void AtachEvents(IConnection iConnection)
        {
            if (iConnection == null)
            {
                throw new ArgumentNullException("iConnection", "iConnection can not be null.");
            }
            iConnection.MouseLeftButtonDown += new MouseButtonEventHandler(userControl_MouseLeftButtonDown);
            iConnection.MouseLeftButtonUp += new MouseButtonEventHandler(userControl_MouseLeftButtonUp);
            iConnection.Configure += new MouseMenuWidgetClickEventHandler(iConnection_Configure);
            iConnection.Deleted += new MouseMenuWidgetClickEventHandler(iConnection_Deleted);
        }

        void iConnection_Deleted(object sender, MouseDoubleClickEventArgs e)
        {
            IComponent iWidget = sender as IComponent;
            if (iWidget != null)
            {
                RemoveWidget(iWidget);
            }
        }

        void iConnection_Configure(object sender, MouseDoubleClickEventArgs e)
        {
            IComponent iWidget = sender as IComponent;
            if (iWidget != null)
            {
                EditTableControl iwindows = new EditTableControl(sender as TableSilverlight, this);
                iwindows.TableNameChanged += new EventHandler(DataModelDesignerSilverlight_TableNameChanged);

                // Deshabilita el canvas para no capturar mas eventos.
                Utils.DisablePanel(this.LayoutRoot);

                this.LayoutRoot.Children.Add(iwindows as UserControl);
                iwindows.Closed += new EventHandler(editControl_Closed);
            }
        }

        void DataModelDesignerSilverlight_TableNameChanged(object sender, EventArgs e)
        {
            EditTableControl editTableControl = (EditTableControl)sender;
            string tableName = editTableControl.textBoxName.Text;

            foreach (TableSilverlight tableSilverlight in tables)
            {
                if (editTableControl.TableSilverlight != tableSilverlight && string.CompareOrdinal(tableSilverlight.Table.Name.ToUpper(CultureInfo.InvariantCulture), tableName.ToUpper(CultureInfo.InvariantCulture)) == 0)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.TableNameDuplicateError, SilverlightVisualDesigners.Properties.Resources.TableNameDuplicateMessage, editTableControl.LayoutRoot);
                    editTableControl.textBoxName.Text = String.Empty;
                }
            }
        }

        void editControl_Closed(object sender, EventArgs e)
        {
            EditTableControl editTableControl = sender as EditTableControl;
            this.LayoutRoot.Children.Remove(editTableControl);
            this.LayoutRoot.Children.Remove(Utils.CoverRectangle);
        }

        void userControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMakeConnectionAction)
            {
                ProcessConnection(sender as IConnection);
            }
        }

        /// <summary>
        /// Usado para concer el origen y destino en un objeto ConnectionSilverlight cuando el usuario dispara un evento MousseUp en un objeto IConnection.
        /// </summary>
        /// <param name="iConnection">Objeto IConnection a ser "conectado".</param>
        private void ProcessConnection(IConnection iConnectable)
        {
            if (iConnectable == null)
            {
                throw new ArgumentNullException("iConnectable", "iConnectable can not be null.");
            }

            TableSilverlight table = iConnectable as TableSilverlight;
            Error error = DataModel.CheckConnectionWithStorage(table.Table);
            if (error != null)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidConnectionError, error.Description, this.LayoutRoot);
                return;
            }

            if (iconnectableFrom == null)
            {
                iconnectableFrom = iConnectable;
            }
            else if (iConnectable != iconnectableFrom)
            {
                iconnectableTarget = iConnectable;
                CreateConnection(iconnectableFrom, iconnectableTarget);
                isMakeConnectionAction = false;
            }
        }

        /// <summary>
        /// Crea el objeto SilverlightConnection.
        /// </summary>
        /// <param name="from">Objeto IConnection origen de la conexión.</param>
        /// <param name="targetTable">Objeto IConnection Destino de la conexión.</param>
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

            TableSilverlight fromTableSilverlight = (from as TableSilverlight);
            TableSilverlight targetTableSilverlight = (target as TableSilverlight);

            if (fromTableSilverlight == null)
            {
                throw new NullReferenceException("fromTableSilverlight can not be null in method CreateConnection in class DataModelDesignerSilverlight.");
            }
            if (targetTableSilverlight == null)
            {
                throw new NullReferenceException("targetTableSilverlight can not be null in method CreateConnection in class DataModelDesignerSilverlight.");
            }

            Error error = DataModel.CheckDuplicatedConnection(fromTableSilverlight.Table, targetTableSilverlight.Table);
            if (error != null)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidConnectionError, error.Description, this.LayoutRoot);
                return;
            }

            ConnectionSilverlight connection = new ConnectionSilverlight(from, target, selectedRelationType);
            connection.Change += new EventHandler(connection_Change);
            connection.Reset += new EventHandler(connection_Reseted);

            AddRelation(connection);
            ConnectionsSilverlight.Add(connection);
        }

        void connection_Change(object sender, EventArgs e)
        {
            UpdateConnection(sender as ConnectionSilverlight);
        }

        void connection_Reseted(object sender, EventArgs e)
        {
            ConnectionSilverlight connectionSilverlight = sender as ConnectionSilverlight;
            connectionSilverlight.Relation.Reset(null);
            // La elimino de la conexión.
            dataModel.RemoveRelation(connectionSilverlight.Relation);
            canvasDraw.Children.Remove(connectionSilverlight.Path);
            canvasDraw.Children.Remove(connectionSilverlight.MyMenuWidget);

            canvasDraw.Children.Remove(connectionSilverlight.FromTableRelationType);
            canvasDraw.Children.Remove(connectionSilverlight.ToTableRelationType);

            // La elimino visualmente.
            this.ConnectionsSilverlight.Remove(connectionSilverlight);
        }

        /// <summary>
        /// Actualizo el punto de inicio y el punto final de la conexión en relacion con el canvas.
        /// </summary>
        /// <param name="connection">Conexión a ser actualizada.</param>
        private void UpdateConnection(ConnectionSilverlight connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection", "connection can not be null.");
            }
            GeneralTransform fromGeneralTransform = (connection.WidgetSource as UIElement).TransformToVisual(this.canvasDraw);
            GeneralTransform targetGeneralTransform = (connection.WidgetTarget as UIElement).TransformToVisual(this.canvasDraw);

            connection.StartPoint = fromGeneralTransform.Transform(connection.WidgetSource.VisualInputPoint);
            connection.Endpoint = targetGeneralTransform.Transform(connection.WidgetTarget.VisualOutputPoint);

            Canvas.SetLeft(connection.MyMenuWidget, ((connection.Endpoint.X - connection.StartPoint.X) / 2) + connection.StartPoint.X - (connection.MyMenuWidget.ActualWidth / 2));
            Canvas.SetTop(connection.MyMenuWidget, ((connection.Endpoint.Y - connection.StartPoint.Y) / 2) + connection.StartPoint.Y - (connection.MyMenuWidget.ActualHeight / 2));

            double middleTop = ((connection.StartPoint.Y + connection.Endpoint.Y) / 2);
            double middleLeft = ((connection.StartPoint.X + connection.Endpoint.X) / 2);
            this.canvasDraw.Children.Remove(connection.FromTableRelationType);
            this.canvasDraw.Children.Remove(connection.ToTableRelationType);

            this.canvasDraw.Children.Add(connection.FromTableRelationType);
            Canvas.SetTop(connection.FromTableRelationType, (middleTop + connection.StartPoint.Y) / 2);
            Canvas.SetLeft(connection.FromTableRelationType, (middleLeft + connection.StartPoint.X) / 2);

            this.canvasDraw.Children.Add(connection.ToTableRelationType);
            Canvas.SetTop(connection.ToTableRelationType, (middleTop + connection.Endpoint.Y) / 2);
            Canvas.SetLeft(connection.ToTableRelationType, (middleLeft + connection.Endpoint.X) / 2);
        }

        /// <summary>
        /// Verifica si la tabla tiene relaciones.
        /// </summary>
        /// <param name="table">Tabla a verificar.</param>
        public bool HasRelations(Table table)
        {

            foreach (ConnectionSilverlight con in this.ConnectionsSilverlight)
            {
                if (con.Relation.Source == table || con.Relation.Target == table)
                {
                    return true;
                }
            }
            return false;
        }

        void userControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentElement = sender as IConnection;
        }

        void canvasDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragAndDropCanvas.IsDragDropAction)
            {
                currentElement.OnDrag();
                currentElement.XCoordinateRelativeToParent = Canvas.GetLeft(currentElement as UIElement);
                currentElement.YCoordinateRelativeToParent = Canvas.GetTop(currentElement as UIElement);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void ButtonNewTable_Clicked(object sender, EventArgs e)
        {
            try
            {
                TableSilverlight tableSilverlight = new TableSilverlight("");
                AddWidget(tableSilverlight);
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonOneToOne_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = true;
                selectedRelationType = RelationType.OneToOne;
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonOneToMany_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = true;
                selectedRelationType = RelationType.OneToMany;
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonManyToMany_Clicked(object sender, EventArgs e)
        {
            try
            {
                isMakeConnectionAction = true;
                selectedRelationType = RelationType.ManyToMany;
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
            catch (Exception error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.LayoutRoot);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exception")]
        private void buttonSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                Collection<Error> errors = new Collection<Error>();
                dataModel.Validate(errors);
                if (errors.Count > 0)
                {
                    string message = String.Empty;
                    foreach (Error error in errors)
                    {
                        message = message + error.Name + ":" + error.Property + ":" + error.Description + "\n";
                    }
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidCustomService, message, this.LayoutRoot);
                }
                else
                {
                    (App.Current as App).BeginSaveDataModel(this.dataModel, new SaveDataModelCallback(EndSaveDataModel));
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

        private void EndSaveDataModel(SaveDataModelResult result)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (result.Success)
                {
                    Dialog.ShowOkDialog(SilverlightVisualDesigners.Properties.Resources.Information, SilverlightVisualDesigners.Properties.Resources.DataModelSuccessfullySaved, this.LayoutRoot, PagesConstants.StoreProfilePage);
                }
                else
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Information, SilverlightVisualDesigners.Properties.Resources.ErrorWhileSavingDataModel, this.LayoutRoot);
                }
            }
            );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is Called from others private method in the same Class")]
        private static void ReturnToStoreProfile()
        {
            HtmlPage.Window.Navigate(new Uri(PagesConstants.StoreProfilePage, UriKind.Relative));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataModel();
        }

        private void buttonCancel_Clicked(object sender, EventArgs e)
        {
            ReturnToStoreProfile();
        }
    }
}
