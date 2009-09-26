using LogicalLibrary.DataModelClasses;
using System.Windows.Controls;
using System.Windows;
using PresentationLayer.ServerDesigner;
using UtnEmall.Server.EntityModel;
using PresentationLayer.Widgets;
using System.Collections.Generic;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary;
using System.ServiceModel;
using System;
using UtnEmall.ServerManager.Properties;
using UtnEmall.ServerManager;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Shapes;

namespace PresentationLayer.DataModelDesigner
{
    /// <summary>
    /// Clase que representa el coneteder visual para MyDataModel.
    /// </summary>
	public class DataModelDocumentWpf
	{
		#region Instance Variables and Properties

        private Canvas canvasDraw;
        private Canvas canvasDrawPrincipal;        
        private WindowDataModelDesigner windowsDataModelDesigner;
                
        private DataModelEntity oldDataModelEntity;
        /// <summary>
        /// El modelo de datos original que se esta editando
        /// </summary>
        public DataModelEntity OldDataModelEntity
        {
            get{
                return oldDataModelEntity;
            } 
            set{
                oldDataModelEntity = value; 
            }
        }
        
        private DataModel myDataModel;
        /// <summary>
        /// Un MyDataModel a ser representado en el documento
        /// </summary>
        public DataModel MyDataModel
        {
            get { return myDataModel; }
            set { myDataModel = value; }
        }

        private bool isDragDropAction;
        /// <summary>
        /// Indica si fue seleccionado una acción Drag and drop.
        /// </summary>
        public bool IsDragDropAction
        {
            get { return isDragDropAction; }
            set { isDragDropAction = value; }
        }

        private bool isMakeConnectionAction;
        /// <summary>
        ///Indica si fue seleccionada la acción MakeAConnection.
        /// </summary>
        public bool IsMakeConnectionAction
        {
            get { return isMakeConnectionAction; }
            set { isMakeConnectionAction = value; }
        }

        private Point mousePosition = new Point();
        /// <summary>
        /// Indica la posicion del mouse cuando es disparadon un PreviewMouseDown and PreviewMouseUp
        /// </summary>
        public Point MousePosition
        {
            get { return mousePosition; }
            set { mousePosition = value; }
        }

        private UIElement currentElement;
        /// <summary>
        /// Indica el UIElement actual seleccionado
        /// </summary>
        public UIElement CurrentElement
        {
            get { return currentElement; }
            set { currentElement = value; }
        }

        private IDrawAbleWpf connectionWidgetFrom;
        /// <summary>
        /// Indica el Source Widget cuando una conección ha sido creada.
        /// </summary>
        public IDrawAbleWpf ConnectionWidgetFrom {
            get { return connectionWidgetFrom;}
            set{connectionWidgetFrom = value;}
        }

        private IDrawAbleWpf connectionWidgetTarget;
        /// <summary>
        /// Indica el Target Widget cuando una conexión ha sido creada.
        /// </summary>
        public IDrawAbleWpf ConnectionWidgetTarget {
            get { return connectionWidgetTarget; }
            set { connectionWidgetTarget = value; }
        }

        private RelationType selectedRelationType;
        /// <summary>
        /// Indica el tipo de relacion seleccionada para una conexión.
        /// </summary>
        public RelationType SelectedRelationType {
            get { return selectedRelationType; }
            set { selectedRelationType = value; }
        }
        
		#endregion

		#region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window">Windows Container para el DataModelDocument</param>
        /// <param name="dataModelEntity">DataModelEntity a dibujar</param>
        /// <param name="session">Identificador de sesión</param>
        public DataModelDocumentWpf(WindowDataModelDesigner window, DataModelEntity dataModelEntity)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window","window can not be null");
            }
            if (dataModelEntity == null)
            {
                throw new ArgumentNullException("dataModelEntity","dataModelEntity can not be null");
            }
            // this.session = session;
            this.oldDataModelEntity = dataModelEntity;
            windowsDataModelDesigner = window;
            this.canvasDraw = windowsDataModelDesigner.canvasDraw;
            this.canvasDrawPrincipal = windowsDataModelDesigner.canvasDrawPrincipal;
            this.canvasDrawPrincipal.MouseMove += new System.Windows.Input.MouseEventHandler(canvasDraw_MouseMove);
            LoadDataModel(dataModelEntity);
        }

		#endregion

		#region Instance Methods

		#region Public Instance Methods

        /// <summary>
        /// Función que permite agregar una tabla al DataModelDocumentWpf.
        /// </summary>
        /// <param name="newTable">Tabla a ser agregada.</param>
        public void AddTable(TableWpf newTable)
        {
            if (newTable == null)
            {
                throw new ArgumentNullException("newTable","newTable can not be null");
            }
            MyDataModel.AddTable(newTable);
            AttachWidgetEvent(newTable);
            canvasDraw.Children.Add(newTable.UIElement);
        }
        
        /// <summary>
        /// Función que permite eliminar una tabla desde el DataModelDocumentWpf
        /// </summary>
        /// <param name="table">Tabla a ser eliminada.</param>
        public void RemoveTable(TableWpf table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            RemoveRelatedRelation(table);
            MyDataModel.RemoveTable(table);
            this.canvasDraw.Children.Remove(table.UIElement);
            RedrawConnections();
        }

        /// <summary>
        /// Funcion que elimina todas las relaciones con una tabla
        /// </summary>
        /// <param name="table">La tabla con las relaciones a eliminar</param>
        public void RemoveRelationWithTable(Table table)
        {
            List<Relation> relationToDelete = new List<Relation>();
            foreach (Relation rel in this.myDataModel.Relations)
            {
                if (rel.Source == table || rel.Target == table)
                {
                    relationToDelete.Add(rel);
                }
            }

            foreach (Relation rel in relationToDelete)
            {
                this.RemoveRelation(rel as RelationWpf);
            }
        }

        /// <summary>
        /// Función que permite agregar una relación al DataModelDocumentWpf.
        /// </summary>
        /// <param name="newRelation">Relación a ser agregada.</param>
        public void AddRelation(RelationWpf newRelation)
        {
            if (newRelation == null)
            {
                throw new ArgumentNullException("newRelation", "newRelation can not be null");
            }
            newRelation.ContextMenuDeleteClick += new RoutedEventHandler(newRelation_ContextMenuDeleteClick);
            MyDataModel.AddRelation(newRelation);
        }

        /// <summary>
        /// Función que permite elimiar una relación al DataModelDocumentWpf.
        /// </summary>
        /// <param name="relation">Relación a ser eliminada.</param>
        public void RemoveRelation(RelationWpf relation)
        {
            if (relation == null)
            {
                throw new ArgumentNullException("relation", "relation can not be null");
            }
            MyDataModel.RemoveRelation(relation);
            this.canvasDraw.Children.Remove(relation.UIElement);
        }
        
        /// <summary>
        /// Función que agrega los eventos a la tabla.
        /// </summary>
        /// <param name="table">La tabla a la que se le agregara los eventos</param>
        public void AttachWidgetEvent(TableWpf table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            table.WidgetPreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(table_WidgetPreviewMouseDown);
            table.WidgetPreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(table_WidgetPreviewMouseUp);
            table.WidgetClick += new System.Windows.Input.MouseButtonEventHandler(table_WidgetClick);
            table.WidgetContextMenuEditClick += new RoutedEventHandler(table_WidgetContextMenuEditClick);
            table.WidgetContextMenuDeleteClick += new RoutedEventHandler(table_WidgetContextMenuDeleteClick);
        }

        /// <summary>
        /// Función que permite redibujar relaciones en el DocumentDataModel.
        /// </summary>
        public void RedrawConnections()
        {
            this.DeleteDrawConnections();
            foreach (RelationWpf relation in MyDataModel.Relations)
            {
                relation.DrawConnection(canvasDraw);
                relation.ContextMenuDeleteClick += new RoutedEventHandler(newRelation_ContextMenuDeleteClick);
            }
        }

        /// <summary>
        /// Función que prepara el DataModelEntity que va a ser almacenado.
        /// </summary>
        public Collection<Error> SaveDataModel()
        {
            Collection<Error> errors = new Collection<Error>();
            myDataModel.Validate(errors);
            if (errors.Count == 0)
            {
                // Reemplaza el modelo de dato viejo con el actual
                // Generará nuevos registros para tablas y columnas
                DataModelEntity dataModelEntity = LogicalLibrary.Utilities.ConvertDataModelToEntity(this.myDataModel);
                dataModelEntity.Id = oldDataModelEntity.Id;
                dataModelEntity.IdStore = oldDataModelEntity.IdStore;
                dataModelEntity.IsNew = false;
                this.oldDataModelEntity = dataModelEntity;
            }
            return errors;
        }
        
        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Función que elimina las relaciones con una tabla en particular
        /// Elimina todas las relaciones donde la tabla es origen o destino.
        /// </summary>
        /// <param name="table">La tabla a la que se le eliminará sus relaciones.</param>
        private void RemoveRelatedRelation(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            List<RelationWpf> relationToDelete = new List<RelationWpf>();
            foreach (RelationWpf relation in MyDataModel.Relations)
            {
                if (relation.Source.Equals(table) || relation.Target.Equals(table))
                {
                    relationToDelete.Add(relation);
                }
            }
            foreach (RelationWpf relation in relationToDelete)
            {
                this.RemoveRelation(relation);
            }
        }
        
        /// <summary>
        /// Función que carga un DataModelEntity al DataModelDocumentWpf
        /// </summary>
        /// <param name="dataModelEntity"></param>
        private void LoadDataModel(DataModelEntity dataModelEntity)
        {
            if (dataModelEntity == null)
            {
                throw new ArgumentNullException("dataModelEntity", "dataModelEntity can not be null");
            }

            DataModel dataModelLoaded = Utilities.ConvertEntityToDataModel(dataModelEntity);
            if (dataModelLoaded != null)
            {
                myDataModel = dataModelLoaded;
                ReDrawDataModel();
                return;
            }
            
            this.MyDataModel = new DataModel();   
            
        }

        /// <summary>
        /// Función que crea una conexión
        /// </summary>
        /// <param name="from">IDrawAbleWpf origen de la conexón.</param>
        /// <param name="target">IDrawAbleWpf destino de la conexión.</param>
        /// <param name="relationType">Tipo de relación que se creará.</param>
        private void CreateConnection(IDrawAbleWpf from, IDrawAbleWpf target, RelationType relationType)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from", "from can not be null");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target", "target can not be null");
            }
            Widget widgetTarget = target as Widget;
            Widget widgetFrom = from as Widget;

            Error error = myDataModel.CheckDuplicatedConnection(widgetFrom as Table, widgetTarget as Table);
            if (error != null)
            {
                Util.ShowErrorDialog(error.Description);
                windowsDataModelDesigner.textBlockStatusBar.Text = String.Empty;
                return;
            }

            RelationWpf newRelation = new RelationWpf(widgetFrom as TableWpf, widgetTarget as TableWpf, relationType);

            this.AddRelation(newRelation);
            RedrawConnections();
        }

        /// <summary>
        /// Función que elimina todos los UIElement que representa una conección desde el DataModelDocumentWpf
        /// </summary>
        private void DeleteDrawConnections()
        {
            foreach (RelationWpf relation in MyDataModel.Relations)
            {
                this.canvasDraw.Children.Remove(relation.UIElement);
            }
        }

        /// <summary>
        /// Función que valida la conección y llama la función de finalización.
        /// </summary>
        private void FinalizeConnection()
        {
            if (connectionWidgetFrom == connectionWidgetTarget)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.SameNodesSelected);
            }
            else
            {
                CreateConnection(connectionWidgetFrom, connectionWidgetTarget, selectedRelationType);
                isMakeConnectionAction = false;
                windowsDataModelDesigner.textBlockStatusBar.Text = String.Empty;

            }
        }

        /// <summary>
        /// Función que refrezca la visualización del documento.
        /// </summary>
        private void ReDrawDataModel()
        {
            this.canvasDraw.Children.RemoveRange(0, canvasDraw.Children.Count);
            foreach (TableWpf tableWpf in this.MyDataModel.Tables)
            {
                this.AttachWidgetEvent(tableWpf);
                IDrawAbleWpf tableDrawable = tableWpf as IDrawAbleWpf;
                TextBlock textBlockTableName = (tableWpf.UIElement as Canvas).FindName("textBlockTableName") as TextBlock;
                textBlockTableName.Text = tableWpf.Name;
                canvasDraw.Children.Add(tableDrawable.UIElement);
                Canvas.SetLeft(tableDrawable.UIElement, tableWpf.XCoordinateRelativeToParent);
                Canvas.SetTop(tableDrawable.UIElement, tableWpf.YCoordinateRelativeToParent);
            }
            RedrawConnections();
        }

        /// <summary>
        /// Función que verifica si ya existe una tabla con un determinado nombre
        /// </summary>
        /// <param name="tableName">Nombre que se desea verificar su existencia</param>
        /// <returns></returns>
        public bool ExistsTableName(Table actualTable, String tableName)
        {
            foreach (Table table in MyDataModel.Tables)
            {
                if (table != actualTable && (String.CompareOrdinal(tableName.ToLower(CultureInfo.CurrentCulture), table.Name.ToLower(CultureInfo.CurrentCulture)) == 0))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///Función llamada cuando se selecciono la opción eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void table_WidgetContextMenuDeleteClick(object sender, RoutedEventArgs e)
        {
            TableWpf tableWPF = sender as TableWpf;
            this.RemoveTable(tableWPF);
        }

        /// <summary>
        ///Función llamada cuando se selecciono la opción editar. Se abre una ventana para editar las columnas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void table_WidgetContextMenuEditClick(object sender, RoutedEventArgs e)
        {
            TableWpf tableWPF = sender as TableWpf;
            WindowTable window = new WindowTable(tableWPF,this);
            window.ShowDialog();
        }

        /// <summary>
        /// Función llamada cuando se presiono una tabla. Si es una acción 'make connection' se establece a esta tabla como parte de la conexión.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void table_WidgetClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsMakeConnectionAction)
            {
                TableWpf tableWPF = sender as TableWpf;
                // Un origen de datos no puede ser parte de una relación.
                Error error = DataModel.CheckConnectionWithStorage(tableWPF as Table);
                if (error != null)
                {
                    Util.ShowErrorDialog(error.Description);
                    return;
                }

                IDrawAbleWpf tempWidget = sender as IDrawAbleWpf;
                if (connectionWidgetFrom == null)
                {
                    connectionWidgetFrom = tempWidget;
                    windowsDataModelDesigner.textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectTableTargetConnection;
                    return;
                }
                else
                {
                    connectionWidgetTarget = tempWidget;
                    FinalizeConnection();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void table_WidgetPreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDragDropAction)
            {
                isDragDropAction = false;
                Widget senderAsWidget = sender as Widget;
                IDrawAbleWpf senderAsIDrawAble = sender as IDrawAbleWpf;
                senderAsWidget.XCoordinateRelativeToParent = Canvas.GetLeft(senderAsIDrawAble.UIElement);
                senderAsWidget.YCoordinateRelativeToParent = Canvas.GetTop(senderAsIDrawAble.UIElement);
            }
        }
         /// <summary>
         /// Constructor
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        void table_WidgetPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isDragDropAction)
            {
                isDragDropAction = true;
                IDrawAbleWpf widget = sender as IDrawAbleWpf;
                currentElement = widget.UIElement;

                mousePosition.X = e.GetPosition(widget.UIElement).X;
                mousePosition.Y = e.GetPosition(widget.UIElement).Y;
            }
        }

        /// <summary>
        /// Función llamada cuando el usuario mueve el mouse. Si es una accion de drag and drop se mueve la tabla seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void canvasDraw_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsDragDropAction)
            {
                Point newPosition = new Point();
                newPosition.X = e.GetPosition(canvasDraw).X - MousePosition.X;
                newPosition.Y = e.GetPosition(canvasDraw).Y - MousePosition.Y;
                Canvas.SetLeft(CurrentElement, newPosition.X);
                Canvas.SetTop(CurrentElement, newPosition.Y);
                RedrawConnections();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newRelation_ContextMenuDeleteClick(object sender, RoutedEventArgs e)
        {
            this.RemoveRelation(sender as RelationWpf);
        }

		#endregion
        
		#endregion
	}
}
