using LogicalLibrary.DataModelClasses;
using PresentationLayer.Widgets;
using System.Windows.Controls;
using System.Windows;
using PresentationLayer.ServerDesignerClasses;
using System.Windows.Media;
using UtnEmall.ServerManager.Properties;

namespace PresentationLayer.DataModelDesigner
{
    /// <summary>
    /// Clase que representa una relacion gráfica en un DataModelDocumentWpf
    /// </summary>
    public class RelationWpf : Relation, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        private Canvas myCanvas;
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        Point fromPoint, toPoint;
        private Canvas canvasToDraw;
        private TableWpf tableSourceWPF;
        private TableWpf tableTargetWPF;

        #endregion

        #region Constructors

        /// <summary>
        ///Constructor.
        /// </summary>
        /// <param name="tableSource">TableWpf origen</param>
        /// <param name="tableTarget">TableWpf destino.</param>
        /// <param name="relationType">Tipo de relación.</param>
        public RelationWpf(TableWpf tableSource, TableWpf tableTarget, RelationType relationType)
            : base(tableSource, tableTarget, relationType)
        {
            this.tableSourceWPF = tableSource;
            this.tableTargetWPF = tableTarget;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Función que crea una representación visual para el RelationWpf.
        /// </summary>
        public void MakeCanvas()
        {
            Canvas canvas = new Canvas();
            LineGeometry myLineGeometry = new LineGeometry();
            
            Label relationTypeStart = new Label();
            Label relationTypeEnd = new Label();
            
            switch (RelationType)
            {
                case RelationType.OneToMany:
                    relationTypeStart.Content = "1";
                    relationTypeEnd.Content = "*";
                    break;
                case RelationType.ManyToMany:
                    relationTypeStart.Content = "*";
                    relationTypeEnd.Content = "*";
                    break;
                case RelationType.OneToOne:
                    relationTypeStart.Content = "1";
                    relationTypeEnd.Content = "1";
                    break;
            }

            myLineGeometry.StartPoint = this.fromPoint;
            myLineGeometry.EndPoint = this.toPoint;

            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();

            myPath.Stroke = Brushes.Crimson;
            myPath.StrokeThickness = 4;
            myPath.Data = myLineGeometry;
            canvas.Children.Add(myPath);

            double middleTop = ((this.fromPoint.Y + this.toPoint.Y) / 2);
            double middleLeft = ((this.fromPoint.X + this.toPoint.X) / 2);
            canvas.Children.Add(relationTypeStart);
            Canvas.SetTop(relationTypeStart, (middleTop + this.fromPoint.Y)/2);
            Canvas.SetLeft(relationTypeStart, (middleLeft + this.fromPoint.X)/2);

            canvas.Children.Add(relationTypeEnd);
            Canvas.SetTop(relationTypeEnd, (middleTop + this.toPoint.Y) / 2);
            Canvas.SetLeft(relationTypeEnd, (middleLeft + this.toPoint.X) / 2);

            canvas.ContextMenu = GenerateContextMenu();

            this.myCanvas = canvas;
        }

        /// <summary>
        /// Método que obtiene el UIElement de la conexión.
        /// </summary>
        /// <returns></returns>
        public System.Windows.UIElement UIElement
        {
            get
            {
                return myCanvas;
            }
        }

        /// <summary>
        ///Función que dibuja la relación
        /// </summary>
        /// <param name="drawArea">Canvas donde se dibujará la conexión</param>
        public void DrawConnection(Canvas drawArea)
        {
            this.canvasToDraw = drawArea;
            fromPoint = tableSourceWPF.UIElement.TranslatePoint(tableSourceWPF.CentralPoint, canvasToDraw);
            toPoint = tableTargetWPF.UIElement.TranslatePoint(tableTargetWPF.CentralPoint, canvasToDraw);
            MakeCanvas();
            drawArea.Children.Insert(0, myCanvas);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        ///Función que crea el menú de la conexión.
        /// </summary>
        /// <returns></returns>
        private ContextMenu GenerateContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemDelete = new MenuItem();
            menuItemDelete.Header = Resources.Delete;
            menuItemDelete.Click += new RoutedEventHandler(menuItemDelete_Click);
            contextMenu.Items.Add(menuItemDelete);

            return contextMenu;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ContextMenuDeleteClick != null)
            {
                ContextMenuDeleteClick(this, e);
            }
        }

        #endregion

        #endregion

        #region Events

        public event RoutedEventHandler ContextMenuDeleteClick;

        #endregion
    }
}

