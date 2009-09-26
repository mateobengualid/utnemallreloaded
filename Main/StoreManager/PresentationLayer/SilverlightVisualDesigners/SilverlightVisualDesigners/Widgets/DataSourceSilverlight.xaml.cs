using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary.Widgets;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;
using LogicalLibrary;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un DataSource para el diseñador de servicios en Silverlight. 
    /// Esta clase encapsula una instancia de DataSource del proyecto LogicalLibrary.
    /// </summary>
	public partial class DataSourceSilverlight : UserControl,IDraw,IConnection
	{
        private DataSource dataSource;
        /// <summary>
        /// Accede al artefacto DataSource.
        /// </summary>
        public DataSource DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }
        
        /// <summary>
        /// Crea un nuevo DataSourceSilverlight vinculado con la tabla.
        /// </summary>
        /// <param name="table">Tabla a vincular con DataSourceSilverlight</param>
		public DataSourceSilverlight(Table table)
		{
			// Inicializar variables.
			InitializeComponent();
            dataSource = new DataSource(table);
            drawCorrectConnectionPoint();
            textBlockName.Text = table.Name;
            dataSource.InputConnectionPoint = new ConnectionPoint(ConnectionPointType.Input);
            dataSource.InputConnectionPoint.Parent = dataSource;
            dataSource.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
            dataSource.OutputConnectionPoint.Parent = dataSource;
            AddMenu(canvasWidget);
		}

        /// <summary>
        /// Crear un nuevo DataSourceSilverlight basado en un objeto DataSource.
        /// </summary>
        /// <param name="dataSource">DataSource relativo al DataSourceSilverlight</param>
        public DataSourceSilverlight(DataSource dataSource)
        {
            // Inicializar variables.
            InitializeComponent();
            DataSource = dataSource;
            drawCorrectConnectionPoint();
            textBlockName.Text = dataSource.RelatedTable.Name;
            AddMenu(canvasWidget);
        }

        /// <summary>
        /// Verificar si DataSourceSilverlight es un almacenamiento y en base a ello  dibujar correctamente el punto (de entrada o de salida).
        /// </summary>
        private void drawCorrectConnectionPoint()
        {
            if (this.dataSource.RelatedTable.IsStorage)
            {
                this.CanvasConnectionPointOutput.Visibility = Visibility.Collapsed;
                return;
            }
            this.CanvasConnectionPointInput.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Agregar un menú de opciones al artefacto.
        /// </summary>
        /// <param name="canvas">Canvas sobre el cual se agregará el menú.</param>
        private void AddMenu(Canvas canvas)
        {
            MenuWidget menuWidget = new MenuWidget(MenuType.MenuDelete);
            canvasWidget.Children.Add(menuWidget);
            Grid.SetColumn(menuWidget, 1);
            menuWidget.DeletePressed += new EventHandler(menuWidget_DeletePressed);
        }


        void menuWidget_DeletePressed(object sender, EventArgs e)
        {
            // Lanzar el evento de eliminación.
            if (Deleted != null)
            {
                Deleted(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.DataSource));
            }
        }


        #region IDrawable Members

        /// <summary>
        /// Coordinate 'x' que representa la posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get
            {
                return dataSource.XCoordinateRelativeToParent;
            }
            set
            {
                dataSource.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordenada 'y' que representa la posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get
            {
                return dataSource.YCoordinateRelativeToParent;
            }
            set
            {
                dataSource.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa la coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get
            {
                return dataSource.XFactorCoordinateRelativeToParent;
            }
            set
            {
                dataSource.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una cordenada 'y' independiente del padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get
            {
                return dataSource.YFactorCoordinateRelativeToParent;
            }
            set
            {
                dataSource.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente del padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                dataSource.WidthFactor = value;
            }
            get
            {
                return dataSource.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente del padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                dataSource.HeightFactor = value;
            }
            get
            {
                return dataSource.HeightFactor;
            }
        }

        /// <summary>
        /// Lanza evento de arrastre manualmente.
        /// </summary>
        public void OnDrag()
        {
            // Lanzar evento de arrastre.
            if (Drag != null)
            {
                Drag(this, new EventArgs());
            }
        }

        public event EventHandler Drag;

        #endregion

        #region IConnection Members

        /// <summary>
        /// Obtiene un punto gráfico del punto de entrada de conexión.
        /// </summary>
        public Point VisualInputPoint
        {
            get 
            {
                GeneralTransform g = CanvasConnectionPointInput.TransformToVisual(this);
                return g.Transform(new Point(0, CanvasConnectionPointInput.ActualHeight / 2));
            }
        }

        /// <summary>
        /// Obtiene un punto gráfico del destino de la conexión.
        /// </summary>
        public Point VisualOutputPoint
        {
            get 
            {
                GeneralTransform g = CanvasConnectionPointOutput.TransformToVisual(this);
                return g.Transform(new Point(CanvasConnectionPointOutput.Width, CanvasConnectionPointOutput.Height / 2));
            }
        }

        /// <summary>
        /// Obtiene el punto de entrada del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint InputConnectionPoint
        {
            get { return dataSource.InputConnectionPoint; }
        }

        /// <summary>
        /// Obtiene el punto de salida del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint OutputConnectionPoint
        {
            get { return dataSource.OutputConnectionPoint; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public Component Component
        {
            get { return dataSource; }
        }

        #endregion

        #region IConnection Members


        public event MouseMenuWidgetClickEventHandler Deleted;
        #pragma warning disable 67
        public event MouseMenuWidgetClickEventHandler Configure;
        #pragma warning restore 67
        #endregion
    }
}