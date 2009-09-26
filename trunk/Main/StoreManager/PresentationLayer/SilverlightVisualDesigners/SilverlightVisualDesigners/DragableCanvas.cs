using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary.ServerDesignerClasses;

namespace SilverlightVisualDesigners
{
    public class DragAndDropCanvas:Canvas
    {
        /// <summary>
        /// Indica si fue seleccionada la acción de cambiar tamaño.
        /// </summary>
        private static bool isResizeAction;
        public static bool IsResizeAction 
        { 
            get {return isResizeAction;} 
            set {isResizeAction = value;}
        }
        /// <summary>
        ///  Indica si fue seleccionada la acción de arrastrar y soltar.
        /// </summary>
        private static bool isDragDropAction;
        public static bool IsDragDropAction 
        {
            get { return isDragDropAction;}
            set { isDragDropAction = value;}
        }
        
        private Point mousePosition;
        /// <summary>
        /// Indica la posición del mouse cuando los eventos MouseDown y MouseUp son disparados.
        /// </summary>
        public Point MousePosition
        {
            get { return mousePosition; }
            set { mousePosition = value; }
        }

        private UIElement currentElement;
        /// <summary>
        /// Indica el UIElement actualmente seleccionado.
        /// </summary>
        public UIElement CurrentElement
        {
            get { return currentElement; }
            set { currentElement = value; }
        }

        public DragAndDropCanvas()
        {
            this.mousePosition = new Point();
            this.MouseMove += new MouseEventHandler(DrawArea_MouseMove);
            this.SizeChanged += new SizeChangedEventHandler(DraggableCanvas_SizeChanged);
        }

        void DraggableCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipCanvas();
        }

        private void ClipCanvas()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            this.Clip = rectangleGeometry;
        }

        public void Add(UIElement uiElement)
        {
            AtachMoveEvents(uiElement);
            this.Children.Add(uiElement);
        }

        private void AtachMoveEvents(UIElement uiElement)
        {
            uiElement.MouseLeftButtonDown += new MouseButtonEventHandler(uiElement_MouseLeftButtonDown);
            uiElement.MouseLeftButtonUp += new MouseButtonEventHandler(uiElement_MouseLeftButtonUp);
        }

        void uiElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragAndDropCanvas.IsResizeAction= false;
                if (isDragDropAction)
                {
                    isDragDropAction = false;
                }
            }
            catch (NullReferenceException exception)
            {
                throw new NullReferenceException("currentElement is null in method uiElement_MouseLeftButtonUp in class DragableCanvas.\n" + exception.Message);
            }
        }

        void uiElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!isDragDropAction)
                {
                    isDragDropAction = true;
                    currentElement = sender as UIElement;

                    mousePosition.X = e.GetPosition(currentElement).X;
                    mousePosition.Y = e.GetPosition(currentElement).Y;
                }
            }
            catch (NullReferenceException exception)
            {
                throw new NullReferenceException("currentElement is null in method uiElement_MouseLeftButtonDown in class DragableCanvas.\n" + exception.Message);
            }
        }

        void DrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (IsResizeAction)
                {
                    double width = e.GetPosition(this).X - Canvas.GetLeft(currentElement);
                    double height = e.GetPosition(this).Y - Canvas.GetTop(currentElement);
                    TextField textField = currentElement as TextField;
                    if (textField!= null)
                    {
                        textField.LoadElementSize(width, height);
                    }
                }
                else if (isDragDropAction)
                {
                    Point newPosition = new Point();
                    newPosition.X = e.GetPosition(this).X - MousePosition.X;
                    newPosition.Y = e.GetPosition(this).Y - MousePosition.Y;
                    if (newPosition.X <= 0 || newPosition.Y <= 0)
                    {
                        return;
                    }
                    Canvas.SetLeft(currentElement, newPosition.X);
                    Canvas.SetTop(currentElement, newPosition.Y);
                }
            }
            catch (NullReferenceException exception)
            {
                throw new NullReferenceException("currentElement is null in method DrawArea_MouseMove in class DragableCanvas.\n" + exception.Message);
            }
        }
    }
}
