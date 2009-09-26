using LogicalLibrary.ServerDesignerClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System;
using UtnEmall.ServerManager.Properties;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Clase que representa un ConnectionWpf gráfico
    /// </summary>
    public class ConnectionWpf : Connection, IDrawAbleWpf
    {
        #region Constants, Variables and Properties

        private UIElement uiElementFrom;
        /// <summary>
        /// UIElement origen de la conexión.
        /// </summary>
        public UIElement UIElementFrom
        {
            get { return uiElementFrom; }
            set { uiElementFrom = value; }
        }

        private UIElement uiElementTarget;
        /// <summary>
        /// UIElement destino de la conexión.
        /// </summary>
        public UIElement UIElementTarget
        {
            get { return uiElementTarget; }
            set { uiElementTarget = value; }
        }

        private Canvas myCanvas;
        /// <summary>
        /// Canvas que representa un ConnectionWpf.
        /// Esta propiedad es seteada con DrawCanvas.
        /// </summary>
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="from">ConnectionPoint origen del ConnectionWpf</param>
        /// <param name="target">ConnectionPoint destino del ConnectionWpf</param>
        public ConnectionWpf(ConnectionPointWpf from, ConnectionPointWpf target)
            : base(from, target)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from", "from can not be null");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target", "target can not be null");
            }
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Método que dibuja una conexión en un panel específico
        /// </summary>
        /// <param name="drawArea">Panel en donde se va a dibujar la conexión</param>
        public void DrawConnection(Panel drawArea)
        {
            if (drawArea == null)
            {
                throw new ArgumentNullException("drawArea", "drawArea can not be null");
            }
            ConnectionPointWpf source = this.Source as ConnectionPointWpf;
            ConnectionPointWpf target = this.Target as ConnectionPointWpf;

            Canvas canvas = new Canvas();
            LineGeometry myLineGeometry = new LineGeometry();
            Point fromPoint, toPoint;

            fromPoint = source.UIElement.TranslatePoint(source.CentralPoint, drawArea);
            toPoint = target.UIElement.TranslatePoint(target.CentralPoint, drawArea);

            myLineGeometry.StartPoint = fromPoint;
            myLineGeometry.EndPoint = toPoint;

            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();

            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 4;
            myPath.Data = myLineGeometry;
            canvas.Children.Add(myPath);

            canvas.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(myCanvas_MouseRightButtonUp);
            GenerateContextMenu(canvas);
            this.MyCanvas = canvas;
            drawArea.Children.Insert(0, canvas);
        }

        /// <summary>
        ///  Este método no es implementado en esta clase
        /// </summary>
        public void MakeCanvas()
        {
        }

        /// <summary>
        /// Función que retorna un ConnectionWpf UIElement.
        /// </summary>
        /// <returns>UIElement para el ConnectionWpf</returns>
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
        /// Función que crea el menu para la conexión
        /// </summary>
        /// <param name="widget">El canvas de la conexión.</param>
        private void GenerateContextMenu(Canvas widget)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemDelete = new MenuItem();
            menuItemDelete.Header = Resources.Delete;
            menuItemDelete.Click += new RoutedEventHandler(menuItemDelete_Click);
            contextMenu.Items.Add(menuItemDelete);
            widget.ContextMenu = contextMenu;
        }

        /// <summary>
        /// Función llamada cuando el boton derecho es presionado sobre la conexión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void myCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseRightButtonUp != null)
            {
                MouseRightButtonUp(this, e);
            }
        }

        /// <summary>
        /// Función llamada cuando la opcion eliminar es seleccionada en el menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectionContextMenuDeleteClick != null)
            {
                ConnectionContextMenuDeleteClick(this, e);
            }
        }

        
        #endregion

        #endregion

        #region Events

        public event MouseButtonEventHandler MouseRightButtonUp;
        public event RoutedEventHandler ConnectionContextMenuDeleteClick;
        #endregion
    }
}

