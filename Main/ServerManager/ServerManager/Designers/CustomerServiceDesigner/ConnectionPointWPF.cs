using LogicalLibrary.ServerDesignerClasses;
using System.Windows.Controls;
using LogicalLibrary;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Clase que representa un ConnectionPointWpf gráfico.
    /// </summary>
    public class ConnectionPointWpf : ConnectionPoint, IDrawAbleWpf
    {
        #region Constants, Variables and Properties

        private string canvasPath;

        private Canvas myCanvas;
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///Constructor.
        /// </summary>
        /// <param name="connectionType">Tipo de conector</param>
        /// <param name="parent">Padre del conector.</param>
        public ConnectionPointWpf(ConnectionPointType connectionType, Component parent)
            : base(connectionType, parent)
        {

            if (connectionType == ConnectionPointType.Input)
            {
                this.canvasPath = "CanvasConnectionPointInput.xaml";
            }
            else
            {
                this.canvasPath = "CanvasConnectionPointOutput.xaml";
            }
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods
       
        /// <summary>
        /// Obtiene el punto central del conector.
        /// </summary>
        public Point CentralPoint
        {
            get
            {
                Point point = new Point();
                if (MyCanvas == null)
                {
                    this.MakeCanvas();
                }
                Path pathComponnent = this.MyCanvas.FindName("line") as Path;

                if (this.ConnectionPointType == ConnectionPointType.Output)
                {
                    // Agrega 3 a la altura para acomodar mejor el punto de conexión
                    point = new Point(pathComponnent.Width, pathComponnent.Height + 3);
                    return point;
                }
                point = new Point(0, (pathComponnent.Width - 3) / 2);
                return point;
            }
            
        }

        /// <summary>
        /// Función que contruye el lienzo que representa el ConnectionPointWpf
        /// </summary>
        public void MakeCanvas()
        {
            Canvas canvas = new Canvas();
            canvas = Utilities.CanvasFromXaml(this.canvasPath);
            canvas.MouseLeftButtonUp += new MouseButtonEventHandler(canvas_MouseLeftButtonUp);

            this.MyCanvas = canvas;
        }

        /// <summary>
        /// Función que obtiene un UIElement para la ConnectionPoint.
        /// </summary>
        /// <returns></returns>
        public System.Windows.UIElement UIElement
        {
            get
            {
                return MyCanvas;
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Función llamada cuando el boton izquierdo del mouse es presionado sobre el punto de conexión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null) Click(this, e);
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler Click;

        #endregion
    }
}
