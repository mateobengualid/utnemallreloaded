using LogicalLibrary.ServerDesignerClasses;
using System.Windows;
using System.Windows.Controls;
using LogicalLibrary;
using System.Windows.Input;
using VisualDesignerPresentationLayer;
using System.Windows.Media;

namespace PresentationLayer.ServerDesigner
{
    // Clase que representa un document para un Formulario de lista en wpf, esta clase puede contener TemplateListItem y permite agregar y eliminar estos TemplateListItem.
    public class TemplateListFormDocumentWpf : TemplateListFormDocument
    {
        #region Constants, Variables and Properties

        /// <summary>
        /// true si es una acción de drag and drop
        /// </summary>
        private bool isDragDropAction;
        public bool IsDragDropAction
        {
            get { return isDragDropAction; }
            set { isDragDropAction = value; }
        }

        /// <summary>
        /// true si es una acción para modificar el tamaño de un template
        /// </summary>
        private bool isResizeAction;
        public bool IsResizeAction
        {
            get { return isResizeAction; }
            set { isResizeAction = value; }
        }

        private Point mousePosition = new Point();
        public Point MousePosition
        {
            get { return mousePosition; }
            set { mousePosition = value; }
        }

        private UIElement currentElement;
        public UIElement CurrentElement
        {
            get { return currentElement; }
            set { currentElement = value; }
        }

        private Component currentComponent;
        public Component CurrentComponent
        {
            get { return currentComponent; }
            set { currentComponent = value; }
        }

        private Canvas canvasDraw;
        public Canvas CanvasDraw
        {
            get { return canvasDraw; }
            set { canvasDraw = value; }
        }

        #endregion

        #region Constructors

        public TemplateListFormDocumentWpf()
            : base()
        {
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Agrega un nuevo item a la lista de templatelistitem.
        /// </summary>
        /// <param name="templateListItemWpf">Item a agregar a la lista</param>
        public void AddTemplateListItem(TemplateListItemWpf templateListItemWpf)
        {
            base.AddComponent(templateListItemWpf);
            this.AttachTemplateListItemEvents(templateListItemWpf);
            this.AddCanvasToCanvasDraw(templateListItemWpf);
        }

        /// <summary>
        ///Elimina un template item desde la lista de item a mostrar
        /// </summary>
        /// <param name="item"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void RemoveTemplateListItem(TemplateListItemWpf item)
        {
            base.AddComponent(item);
        }

        /// <summary>
        /// Guarda la posicion de cada uno de los elementos de la lista de templateitem
        /// </summary>
        public void SaveComponentPositions()
        {
            foreach (TemplateListItemWpf listItem in this.Components)
            {
                UIElement element = listItem.MyCanvas;
                listItem.XCoordinateRelativeToParent = Canvas.GetLeft(element);
                listItem.YCoordinateRelativeToParent = Canvas.GetTop(element);
                listItem.XFactorCoordinateRelativeToParent = listItem.XCoordinateRelativeToParent / canvasDraw.ActualWidth;
                listItem.YFactorCoordinateRelativeToParent = listItem.YCoordinateRelativeToParent / canvasDraw.ActualHeight;
                listItem.HeightFactor = (listItem.Height / canvasDraw.ActualHeight);
                listItem.WidthFactor = (listItem.Width / canvasDraw.ActualWidth);
            }
        }

        /// <summary>
        ///Agrega eventos a los template list item
        /// </summary>
        /// <param name="templateListItemWpf">Item a agregar a la lista</param>
        public void AttachTemplateListItemEvents(TemplateListItemWpf templateListItemWpf)
        {
            templateListItemWpf.ComponentPreviewMouseDown += new MouseButtonEventHandler(templateListItemWpf_ComponentPreviewMouseDown);
            templateListItemWpf.ComponentPreviewMouseUp += new MouseButtonEventHandler(templateListItemWpf_ComponentPreviewMouseUp);
            templateListItemWpf.ComponentPreviewImageMouseDown += new MouseButtonEventHandler(templateListItemWpf_ComponentPreviewImageMouseDown);
            templateListItemWpf.ComponentPreviewImageMouseUp += new MouseButtonEventHandler(templateListItemWpf_ComponentPreviewMouseUp);
        }

        public void AddCanvasToCanvasDraw(TemplateListItemWpf templateListItemWpf)
        {
            this.canvasDraw.Children.Add(templateListItemWpf.MyCanvas);
            Canvas.SetLeft(templateListItemWpf.MyCanvas, templateListItemWpf.XCoordinateRelativeToParent);
            Canvas.SetTop(templateListItemWpf.MyCanvas, templateListItemWpf.YCoordinateRelativeToParent);
        }

        public void RemoveCanvasToCanvasDraw(TemplateListItemWpf templateListItemWpf)
        {
            this.canvasDraw.Children.Remove(templateListItemWpf.MyCanvas);
        }
        #endregion

        #region Protected and Private Instance Methods

        private void templateListItemWpf_ComponentPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsDragDropAction || IsResizeAction)
            {
                IsResizeAction = false;
                IsDragDropAction = false;
                TemplateListItemWpf templateListItemWpf = sender as TemplateListItemWpf;
                templateListItemWpf.XCoordinateRelativeToParent = Canvas.GetLeft(templateListItemWpf.UIElement);
                templateListItemWpf.YCoordinateRelativeToParent = Canvas.GetTop(templateListItemWpf.UIElement);
            }
        }

        private void templateListItemWpf_ComponentPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            IDrawAbleWpf widget = sender as IDrawAbleWpf;
            if (!IsDragDropAction)
            {
                IsResizeAction = false;
                IsDragDropAction = true;
                CurrentElement = widget.UIElement;
                mousePosition.X = e.GetPosition(widget.UIElement).X;
                mousePosition.Y = e.GetPosition(widget.UIElement).Y;
                CurrentComponent = sender as Component;
            }
        }

        private void templateListItemWpf_ComponentPreviewImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsResizeAction)
            {
                IsDragDropAction = false;
                IsResizeAction = true;
                IDrawAbleWpf widget = sender as IDrawAbleWpf;
                CurrentElement = widget.UIElement;
                mousePosition.X = e.GetPosition(widget.UIElement).X;
                mousePosition.Y = e.GetPosition(widget.UIElement).Y;
                CurrentComponent = sender as Component;
            }
        }
        #endregion

        #endregion

        #region Events

        // public event ItemClickEventHandler ItemClick;

        #endregion
    }
}
