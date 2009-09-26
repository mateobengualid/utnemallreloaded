using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary;
using LogicalLibrary.ServerDesignerClasses;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un artefacto EnterSingleData para el diseñador de servicios de Silverlight. 
    /// Esta clase encapsula una instancia de EnterSingleData del proyecto LogicalLibrary.
    /// </summary>
    public partial class EnterSingleDataFormSilverlight : UserControl, IDraw, IConnection
	{

        private EnterSingleDataForm enterSingleDataForm;
        /// <summary>
        /// Obtiene el artefacto EnterSingleDataForm encapsulado.
        /// </summary>
        public EnterSingleDataForm EnterSingleDataForm
        {
            get { return enterSingleDataForm; }
            set 
            {
                enterSingleDataForm = value;
                Title.Text = value.Title;
            }
        }

        /// <summary>
        /// Cambia el título del EnterSingleDataForm.
        /// </summary>
        /// <param name="title">Título a asignar.</param>
        public void ChangeTitle(string title)
        {
            Title.Text = title;
            enterSingleDataForm.Title = title;
        }

        /// <summary>
        /// Crea una instancia de EnterSingleDataFormSilverlight.
        /// </summary>
		public EnterSingleDataFormSilverlight()
		{
			// Inicializar variables.
			InitializeComponent();
            enterSingleDataForm = new EnterSingleDataForm();
            enterSingleDataForm.InputConnectionPoint = new ConnectionPoint(ConnectionPointType.Input);
            enterSingleDataForm.InputConnectionPoint.Parent = enterSingleDataForm;
            enterSingleDataForm.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
            enterSingleDataForm.OutputConnectionPoint.Parent = enterSingleDataForm;
            this.DataContext = enterSingleDataForm;
            AddMenu(canvasWidget);
		}

        /// <summary>
        /// Crea una instancia de EnterSingleDataFormSilverlight basado en un enterSingleDataForm.
        /// </summary>
        /// <param name="enterSingleDataForm">EnterSingleDataForm relacionado con un enterSingleDataFormSilverlight.</param>
        public EnterSingleDataFormSilverlight(EnterSingleDataForm enterSingleDataForm)
        {
            // Inicializar variables.
            InitializeComponent();
            EnterSingleDataForm = enterSingleDataForm;
            this.DataContext = enterSingleDataForm;
            AddMenu(canvasWidget);
        }

        /// <summary>
        /// Agregar un menú de opciones al artefacto.
        /// </summary>
        /// <param name="canvas">Canvas sobre el que se agregará el menú.</param>
        private void AddMenu(Canvas canvas)
        {
            MenuWidget menuWidget = new MenuWidget(MenuType.Both);
            canvasWidget.Children.Add(menuWidget);
            menuWidget.DeletePressed += new EventHandler(menuWidget_DeletePressed);
            menuWidget.OpenWindowsPressed += new EventHandler(menuWidget_OpenWindowsPressed);
        }

        void menuWidget_OpenWindowsPressed(object sender, EventArgs e)
        {
            if (Configure != null)
            {
                Configure(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.EnterSingleDataForm));
            }
        }

        void menuWidget_DeletePressed(object sender, EventArgs e)
        {
            if (Deleted != null)
            {
                Deleted(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.EnterSingleDataForm));
            }
        }

        #region IDrawable Members

        /// <summary>
        /// Coordenada 'x' que representa una posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get
            {
                return enterSingleDataForm.XCoordinateRelativeToParent;
            }
            set
            {
                enterSingleDataForm.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordenada 'y' que representa una posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get
            {
                return enterSingleDataForm.YCoordinateRelativeToParent;
            }
            set
            {
                enterSingleDataForm.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get
            {
                return enterSingleDataForm.XFactorCoordinateRelativeToParent;
            }
            set
            {
                enterSingleDataForm.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get
            {
                return enterSingleDataForm.YFactorCoordinateRelativeToParent;
            }
            set
            {
                enterSingleDataForm.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                enterSingleDataForm.WidthFactor = value;
            }
            get
            {
                return enterSingleDataForm.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                enterSingleDataForm.HeightFactor = value;
            }
            get
            {
                return enterSingleDataForm.HeightFactor;
            }
        }

        /// <summary>
        /// Lanza el evento de arrastre manualmente.
        /// </summary>
        public void OnDrag()
        {
            if (Drag != null)
            {
                Drag(this, new EventArgs());
            }
        }

        public event EventHandler Drag;

        #endregion

        #region IConnection Members

        /// <summary>
        /// Obtiene un punto gráfico de entrada de conexión.
        /// </summary>
        public Point VisualInputPoint
        {
            get
            {
                GeneralTransform g = CanvasConnectionPointInput.TransformToVisual(this);
                return g.Transform(new Point(CanvasConnectionPointInput.ActualWidth / 2, CanvasConnectionPointInput.ActualHeight / 2));
            }
        }

        /// <summary>
        /// Obtiene el punto gráfico de salida de conexión.
        /// </summary>
        public Point VisualOutputPoint
        {
            get
            {
                GeneralTransform g = CanvasConnectionPointOutput.TransformToVisual(this);
                return g.Transform(new Point(CanvasConnectionPointOutput.ActualWidth / 2, CanvasConnectionPointOutput.ActualHeight / 2));
            }
        }

        /// <summary>
        /// Obtiene el punto de conexión de entrada del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint InputConnectionPoint
        {
            get { return enterSingleDataForm.InputConnectionPoint; }
        }

        /// <summary>
        /// Obtiene el punto de conexión de salida del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint OutputConnectionPoint
        {
            get { return enterSingleDataForm.OutputConnectionPoint; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public Component Component
        {
            get { return enterSingleDataForm; }
        }

        #endregion

        #region IConnection Members

        public event MouseMenuWidgetClickEventHandler Deleted;

        public event MouseMenuWidgetClickEventHandler Configure;

        #endregion
    }
}