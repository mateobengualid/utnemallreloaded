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
    /// Clase que representa un artefacto ListForm para el diseñador de servicios Silverlight. 
    /// Esta clase encapsula una instancia de ListForm del proyecto LogicalLibrary.
    /// </summary>
	public partial class ListFormSilverlight : UserControl,IDraw,IConnection
	{
        private ListForm listForm;
        /// <summary>
        /// Obtiene el artefacto ListForm encapsulado.
        /// </summary>
        public ListForm ListForm 
        {
            get { return listForm; }
            set 
            { 
                listForm = value;
                Title.Text = value.Title;
            } 
        }


        /// <summary>
        /// Cambia el título del ListForm.
        /// </summary>
        /// <param name="title">Título que será asignado.</param>
        public void ChangeTitle(string title)
        {
            Title.Text = title;
            listForm.Title = title;
        }

        /// <summary>
        /// Crea una instancia de ListFormSilverlight.
        /// </summary>
		public ListFormSilverlight()
		{
			// Inicializar variables.
			InitializeComponent();
            listForm = new ListForm();
            listForm.TemplateListFormDocument = new TemplateListFormDocument();
            listForm.InputConnectionPoint = new ConnectionPoint(ConnectionPointType.Input);
            listForm.InputConnectionPoint.Parent = listForm;
            listForm.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
            listForm.OutputConnectionPoint.Parent = listForm;
            AddMenu(canvasWidget);
		}

        /// <summary>
        /// Crea una instancia de ListFormSilverlight basada en un ListForm.
        /// </summary>
        /// <param name="listForm">ListForm relativo al ListFormSilverlight.</param>
        public ListFormSilverlight(ListForm listForm)
        {
            InitializeComponent();
            ListForm = listForm;
            AddMenu(canvasWidget);
        }

        /// <summary>
        /// Agregar un menú con opciones para el artefacto.
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
                Configure(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.ListForm));
            }
        }

        void menuWidget_DeletePressed(object sender, EventArgs e)
        {
            if (Deleted!=null)
            {
                Deleted(this,new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.ListForm));
            }
        }

        /// <summary>
        /// Quitar un ítem de la lista.
        /// </summary>
        /// <param name="templateListItem">Objeto templateListItem a quitar.</param>
        public void RemoveItem(TemplateListItem templateListItem)
        {
            if (templateListItem == null)
            {
                throw new ArgumentNullException("templateListItem", "templateListItem can not be null in method RemoveItem in class ListFormSilverlight");
            }
            listForm.TemplateListFormDocument.RemoveTemplateListItem(templateListItem);
        }

        #region IDrawable Members

        /// <summary>
        /// Coordenada 'x' que representa una posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get
            {
                return listForm.XCoordinateRelativeToParent;
            }
            set
            {
                listForm.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordenada 'y' que representa una posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get
            {
                return listForm.YCoordinateRelativeToParent;
            }
            set
            {
                listForm.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get
            {
                return listForm.XFactorCoordinateRelativeToParent;
            }
            set
            {
                listForm.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get
            {
                return listForm.YFactorCoordinateRelativeToParent;
            }
            set
            {
                listForm.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                listForm.WidthFactor = value;
            }
            get
            {
                return listForm.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                listForm.HeightFactor = value;
            }
            get
            {
                return listForm.HeightFactor;
            }
        }

        /// <summary>
        /// Lanza el evento de arrastre manualmente.
        /// </summary>
        public void OnDrag()
        {
            if (Drag != null)
            {
                Drag(this,new EventArgs());
            }
        }

        public event EventHandler Drag;

        #endregion

        #region IConnection Members

        /// <summary>
        /// Obtiene el punto gráfico del punto de conexión de entrada.
        /// </summary>
        public Point VisualInputPoint
        {
            get 
            {
                GeneralTransform g = CanvasConnectionPointInput.TransformToVisual(this);
                return g.Transform(new Point(CanvasConnectionPointInput.ActualWidth/2,CanvasConnectionPointInput.ActualHeight/2)); 
            }
        }

        /// <summary>
        /// Obtiene un punto gráfico de conexión de salida.
        /// </summary>
        public Point VisualOutputPoint
        {
            get 
            {
                GeneralTransform g = CanvasConnectionPointOutput.TransformToVisual(this);
                return g.Transform(new Point(CanvasConnectionPointOutput.ActualWidth/2,CanvasConnectionPointOutput.ActualHeight/2));
            }
        }

        public event MouseMenuWidgetClickEventHandler Deleted;

        public event MouseMenuWidgetClickEventHandler Configure;

        #endregion

        #region IConnection Members

        /// <summary>
        /// Obtiene el punto de conexión de entrada del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint InputConnectionPoint
        {
            get { return listForm.InputConnectionPoint; }
        }

        /// <summary>
        /// Obtiene el punto de conexión de salida del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint OutputConnectionPoint
        {
            get { return listForm.OutputConnectionPoint; }
        }

        #endregion

        #region IDrawable Members

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public Component Component
        {
            get { return listForm; }
        }

        #endregion

    }
}