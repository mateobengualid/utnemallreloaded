using LogicalLibrary.ServerDesignerClasses;
using System.Windows.Controls;
using PresentationLayer.ServerDesignerClasses;
using System;
using VisualDesignerPresentationLayer;
using System.Windows.Media;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Clase que representa un FormMenuItemWpf gráfico.
    /// </summary>
    public class FormMenuItemWpf : FormMenuItem, IDrawAbleWpf
    {
        #region Constants, Variables and Properties

        private Canvas myCanvas;
        /// <summary>
        /// Propiedad que establece u obtiene un canvas para representar un FormMenuItemWpf.
        /// </summary>
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        private bool connectionClickAttached;
        /// <summary>
        /// Propiedad que indica si ya se agrego el evento de click en la conexión.
        /// </summary>
        public bool ConnectionClickAttached
        {
            get { return connectionClickAttached; }
            set { connectionClickAttached = value; }
        }

        private bool connectionPointClickAttached;
        /// <summary>
        /// Propiedad que indica si ya se agrego el evento de click en la conexión.
        /// </summary>
        public bool ConnectionPointClickAttached
        {
            get { return connectionPointClickAttached; }
            set { connectionPointClickAttached= value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///Constructor.
        /// </summary>
        /// <param name="text">Texto a mostrarse en el FormMenuItemWpf.</param>
        /// <param name="name">Fuente del FormMenuItemWpf.</param>
        public FormMenuItemWpf(string text, string help, FontName name)
            : base(text, help, name)
        {
            OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Función que contruye un canvas para el FormMenuItemWpf.
        /// </summary>
        public void MakeCanvas()
        {
            ConnectionPointWpf outputConnectionPoint = this.OutputConnectionPoint as ConnectionPointWpf;
            outputConnectionPoint.MakeCanvas();

            // Agrega los eventos a los puntos de conexión.
            if (!ConnectionPointClickAttached)
            {
                ConnectionPointClickAttached = true;
                outputConnectionPoint.Click += new EventHandler(outputConnectionPoint_Click);
            }

            // Crea el grupo de canvas
            Canvas groupItemCanvas = Utilities.CanvasFromXaml("CanvasMenuItem.xaml");
            groupItemCanvas.Width = this.Width;
            groupItemCanvas.Height = this.Height;

            // Agrega los hijos al canvas
            groupItemCanvas.Children.Add(outputConnectionPoint.MyCanvas);
            ((TextBlock)groupItemCanvas.FindName("Label")).Text = this.Text;
            Canvas.SetLeft(outputConnectionPoint.MyCanvas, groupItemCanvas.Width);
            Canvas.SetTop(outputConnectionPoint.MyCanvas, groupItemCanvas.Height / 4);

            this.myCanvas = groupItemCanvas;
        }

        /// <summary>
        /// Función que obtiene un UIElement que representa al FormMenuItemWpf.
        /// </summary>
        /// <returns>UIElement que representa el FormMenuItemWpf</returns>
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
        /// Función llamada cuando se presiona en el punto de conexion de salida del formulario MENU.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outputConnectionPoint_Click(object sender, EventArgs e)
        {
            if (ConnectionClick != null)
            {
                ConnectionClick(this, new ConnectionPointClickEventArgs((ConnectionPoint)sender));
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler ConnectionClick;

        #endregion
    }
}