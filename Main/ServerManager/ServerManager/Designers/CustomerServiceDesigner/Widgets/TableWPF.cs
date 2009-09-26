using LogicalLibrary.DataModelClasses;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.Widgets
{
    /// <summary>
    /// Clase que define una table usada en un proyecto wpf
    /// </summary>
    public class TableWpf : Table, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        /// <summary>
        /// table canvas
        /// </summary>
        private Canvas myCanvas;
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        /// <summary>
        /// ruta del canvas xaml table 
        /// </summary>
        private string canvasPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor. Establece la ruta del canvas
        /// </summary>
        public TableWpf()
            : base("")
        {
            this.canvasPath = "CanvasTable.xaml";
            this.MakeCanvas();
        }

        /// <summary>
        /// Constructor. Establece la ruta del canvas y crea una tabla wpf para la tabla logica pasada por parametro
        /// </summary>
        /// <param name="table">Tabla logica</param>
        public TableWpf(Table table):base(table.Name)
        {
            this.Fields.Clear();
            foreach (Field f in table.Fields)
            {
                this.Fields.Add(f);
            }
            this.IsStorage = table.IsStorage;
            this.canvasPath = "CanvasTable.xaml";
            this.MakeCanvas();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Punto central de la tabla wpf
        /// </summary>
        public Point CentralPoint
        {
            get
            {
                Point centralPoint = new Point(myCanvas.ActualWidth / 2, myCanvas.Height / 2);
                return centralPoint;
            }
        }

        /// <summary>
        /// Contruye el canvas para la tabla wpf
        /// </summary>
        public void MakeCanvas()
        {
            Canvas canvas = Utilities.CanvasFromXaml(this.canvasPath);
            Canvas.SetLeft(canvas, 100);
            Canvas.SetTop(canvas, 100);
            canvas.DataContext = this;
            canvas.PreviewMouseDown += new MouseButtonEventHandler(myCanvas_PreviewMouseDown);
            canvas.PreviewMouseUp += new MouseButtonEventHandler(myCanvas_PreviewMouseUp);
            canvas.MouseUp += new MouseButtonEventHandler(myCanvas_MouseUp);
            GenerateContextMenu(canvas);

            this.myCanvas = canvas;
        }

        /// <summary>
        /// Retorna el canvas como un UIElement
        /// </summary>
        public System.Windows.UIElement UIElement
        {
            get
            {
                return myCanvas;
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Crea el menu de opciones para la tabla
        /// </summary>
        /// <param name="widget">El canvas donde se agregara el menu</param>
        private void GenerateContextMenu(Canvas widget)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemEdit = new MenuItem();
            MenuItem menuItemDelete = new MenuItem();
            
            menuItemEdit.Header = UtnEmall.ServerManager.Properties.Resources.Edit;
            menuItemDelete.Header = UtnEmall.ServerManager.Properties.Resources.Delete;
            
            menuItemEdit.Click += new RoutedEventHandler(menuItemEdit_Click);
            menuItemDelete.Click += new RoutedEventHandler(menuItemDelete_Click);
            
            contextMenu.Items.Add(menuItemEdit);
            contextMenu.Items.Add(menuItemDelete);            

            widget.ContextMenu = contextMenu;
        }

        /// <summary>
        /// Función llamada cuando el boton elimina es presionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (WidgetContextMenuDeleteClick != null)
            {
                WidgetContextMenuDeleteClick(this, e);
            }
        }

        /// <summary>
        /// Función llamada cuando el boton editar es presionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            if (WidgetContextMenuEditClick != null)
            {
                WidgetContextMenuEditClick(this, e);
            }
        }

        /// <summary>
        /// Ocurre cuando el boton del mouse pasa por encima del area del item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void myCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (WidgetClick != null)
            {
                WidgetClick(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void myCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (WidgetPreviewMouseUp != null)
            {
                WidgetPreviewMouseUp(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void myCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WidgetPreviewMouseDown != null)
            {
                WidgetPreviewMouseDown(this, e);
            }
        }

        #endregion

        #endregion

        #region Events

        public event MouseButtonEventHandler WidgetPreviewMouseDown;
        public event MouseButtonEventHandler WidgetPreviewMouseUp;
        public event MouseButtonEventHandler WidgetClick;
        public event RoutedEventHandler WidgetContextMenuEditClick;
        public event RoutedEventHandler WidgetContextMenuDeleteClick;
        

        #endregion
    }
}

