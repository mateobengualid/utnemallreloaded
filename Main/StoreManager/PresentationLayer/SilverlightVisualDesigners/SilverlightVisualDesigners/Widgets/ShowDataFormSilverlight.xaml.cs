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
    /// Clase que representa un artefacto visual ShowDataForm para el diseñador de servicios Silverlight. 
    /// Esta clase encapsula una instancia de ShowDataForm del proyecto LogicalLibrary.
    /// </summary>
	public partial class ShowDataFormSilverlight : UserControl,IDraw,IConnection
	{
        private ShowDataForm showDataForm;
        /// <summary>
        /// Obtiene el artefacto ShowDataForm encapsulado.
        /// </summary>
        public ShowDataForm ShowDataForm
        {
            get { return showDataForm; }
            set 
            { 
                showDataForm = value;
                Title.Text = value.Title;
            }
        }


        /// <summary>
        /// Cambia el título del ShowDataForm.
        /// </summary>
        /// <param name="title">Título que se establecerá.</param>
        public void ChangeTitle(string title)
        {
            Title.Text = title;
            showDataForm.Title = title;
        }

        /// <summary>
        /// Crea una instancia de ShowDataFormSilverlight.
        /// </summary>
		public ShowDataFormSilverlight()
		{
			// Inicializar variables.
			InitializeComponent();
            showDataForm = new ShowDataForm();
            showDataForm.Title = String.Empty;
            showDataForm.TemplateListFormDocument = new TemplateListFormDocument();
            showDataForm.InputConnectionPoint = new ConnectionPoint(ConnectionPointType.Input);
            showDataForm.InputConnectionPoint.Parent = showDataForm;
            showDataForm.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
            showDataForm.OutputConnectionPoint.Parent = showDataForm;
            AddMenu(canvasWidget);
		}

        /// <summary>
        /// Crea una instancia de ShowDataFormSilverlight basada en un ShowDataForm.
        /// </summary>
        /// <param name="showDataForm">ShowDataForm relativo a un ShowDataFormSilverlight.</param>
        public ShowDataFormSilverlight(ShowDataForm showDataForm)
        {
            InitializeComponent();
            ShowDataForm = showDataForm;
            AddMenu(canvasWidget);
        }

        /// <summary>
        /// Agrega un menú de opciones al artefacto.
        /// </summary>
        /// <param name="canvas">Canvas al que se le agregará el menú.</param>
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
                Configure(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.ShowDataForm));
            }
        }

        void menuWidget_DeletePressed(object sender, EventArgs e)
        {
            if (Deleted != null)
            {
                Deleted(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.ShowDataForm));
            }
        }

        /// <summary>
        /// Agrega un ítem al ShowDataForm.
        /// </summary>
        /// <param name="templateListItem">TemplteListItem que se añadirá.</param>
        public void AddItem(TemplateListItem templateListItem)
        {
            if (showDataForm == null)
            {
                throw new ArgumentNullException("templateListItem", "templateListItem can not be null in method AddItem in class ShowDataFormSilverlight");
            }
            showDataForm.TemplateListFormDocument.AddTemplateListItem(templateListItem);
        }

        /// <summary>
        /// Quita un ítem del ShowDataForm.
        /// </summary>
        /// <param name="templateListItem">TempleteListItem que se quitará.</param>
        public void RemoveItem(TemplateListItem templateListItem)
        {
            if (showDataForm == null)
            {
                throw new ArgumentNullException("templateListItem","templateLIstItem can not be null in method RemoveItem in class ShowDataFormSilverlight");
            }
            showDataForm.TemplateListFormDocument.RemoveTemplateListItem(templateListItem);
        }

        #region IDrawable Members

        /// <summary>
        /// Coordenada 'x' que representa la posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get
            {
                return showDataForm.XCoordinateRelativeToParent;
            }
            set
            {
                showDataForm.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordenada 'y' que representa la posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get
            {
                return showDataForm.YCoordinateRelativeToParent;
            }
            set
            {
                showDataForm.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get
            {
                return showDataForm.XFactorCoordinateRelativeToParent;
            }
            set
            {
                showDataForm.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get
            {
                return showDataForm.YFactorCoordinateRelativeToParent;
            }
            set
            {
                showDataForm.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                showDataForm.WidthFactor = value;
            }
            get
            {
                return showDataForm.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                showDataForm.HeightFactor = value;
            }
            get
            {
                return showDataForm.HeightFactor;
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
        /// Obtiene el punto gráfico de inicio de conexión.
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
        /// Obtiene el punto gráfico de fin de conexión.
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
        /// Obtiene el punto de inicio de conexión del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint InputConnectionPoint
        {
            get { return showDataForm.InputConnectionPoint; }
        }

        /// <summary>
        /// Obtiene el punto de fin de conexión del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint OutputConnectionPoint
        {
            get { return showDataForm.OutputConnectionPoint; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public Component Component
        {
            get { return showDataForm; }
        }

        #endregion

        #region IConnection Members


        public event MouseMenuWidgetClickEventHandler Deleted;

        public event MouseMenuWidgetClickEventHandler Configure;

        #endregion
    }
}