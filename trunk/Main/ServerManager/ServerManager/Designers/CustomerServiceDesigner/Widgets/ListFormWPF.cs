using LogicalLibrary;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System;
using VisualDesignerPresentationLayer;
using PresentationLayer.ServerDesigner;
using LogicalLibrary.ServerDesignerClasses;
using System.Windows.Media;

namespace PresentationLayer.Widgets
{
    /// <summary>
    /// Esta clase define un formulario de lista usado en un projecto WPF
    /// </summary>
    public class ListFormWpf : ListForm, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        private string canvasPath;
        private Canvas myCanvas;

        /// <summary>
        /// Lienzo de formulario de lista
        /// </summary>
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de la clase 
        /// </summary>
        public ListFormWpf()
        {
            this.TemplateListFormDocument = new TemplateListFormDocumentWpf();
            this.canvasPath = "CanvasListForm.xaml";
            this.InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            this.OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="formNumber">Número de formulario de servicio</param>
        public ListFormWpf(int formNumber)
        {
            this.TemplateListFormDocument = new TemplateListFormDocumentWpf();
            this.canvasPath = "CanvasListForm.xaml";
            this.InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            this.OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
            this.SetTitle(UtnEmall.ServerManager.Properties.Resources.List + formNumber);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Sobreescribe el metodo ToString con el fin de mostrar el título del formulario de lista
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// Actualiza el título del formulario WPF
        /// </summary>
        public void ChangeTitle()
        {
            Canvas formCanvas = (MyCanvas.Children[0] as Canvas);
            TextBlock TextBlockName = formCanvas.FindName("Title") as TextBlock;
            if (String.IsNullOrEmpty(this.Title))
            {
                TextBlockName.Text = UtnEmall.ServerManager.Properties.Resources.NoTitle;
            }
            else
            {
                TextBlockName.Text = this.Title;
            }
            formCanvas.UpdateLayout();
        }

        /// <summary>
        /// Hace el lienzo del formulario de lista WPF y adjunta eventos del mouse
        /// </summary>
        public void MakeCanvas()
        {
            Canvas formCanvas = Utilities.CanvasFromXaml(this.canvasPath);

            formCanvas.PreviewMouseDown += new MouseButtonEventHandler(formCanvas_PreviewMouseDown);
            formCanvas.PreviewMouseUp += new MouseButtonEventHandler(formCanvas_PreviewMouseUp);

            ConnectionPointWpf inputConnectionPoint = this.InputConnectionPoint as ConnectionPointWpf;
            ConnectionPointWpf outputConnectionPoint = this.OutputConnectionPoint as ConnectionPointWpf;

            // Carga el lienzo desde los puntos de conexión.
            inputConnectionPoint.MakeCanvas();
            outputConnectionPoint.MakeCanvas();

            // Adjunta eventos a los puntos de conexión.
            inputConnectionPoint.Click += new EventHandler(ConnectionPoint_Click);
            outputConnectionPoint.Click += new EventHandler(ConnectionPoint_Click);

            // Compone el grupo de lienzos.
            Canvas groupFormCanvas = new Canvas();
            groupFormCanvas.Background = Brushes.Transparent;
            groupFormCanvas.Width = formCanvas.Width + (inputConnectionPoint.MyCanvas.Width * 2);
            groupFormCanvas.Height = formCanvas.Height;
            groupFormCanvas.MouseUp += new MouseButtonEventHandler(groupFormCanvas_MouseUp);

            groupFormCanvas.Children.Add(formCanvas);
            Canvas.SetLeft(formCanvas, inputConnectionPoint.MyCanvas.Width);
            groupFormCanvas.Children.Add(inputConnectionPoint.MyCanvas);
            groupFormCanvas.Children.Add(outputConnectionPoint.MyCanvas);

            // Establece la posición del ConnectionPoint.
            outputConnectionPoint.XCoordinateRelativeToParent = groupFormCanvas.Width - outputConnectionPoint.MyCanvas.Width;
            outputConnectionPoint.YCoordinateRelativeToParent = groupFormCanvas.Height - outputConnectionPoint.MyCanvas.Height;
            Canvas.SetLeft(outputConnectionPoint.MyCanvas, outputConnectionPoint.XCoordinateRelativeToParent);
            Canvas.SetTop(outputConnectionPoint.MyCanvas, outputConnectionPoint.YCoordinateRelativeToParent);

            GenerateContextMenu(groupFormCanvas);

            this.MyCanvas = groupFormCanvas;
            ChangeTitle();
        }

        /// <summary>
        /// Retorna el lienzo como un UIElement
        /// </summary>
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
        /// Crea el menú de contexto para el formulario
        /// </summary>
        /// <param name="widget">El lienzo donde el menú sera agregado</param>
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
        /// Método llamado cuando el item de menú Eliminar es seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (WidgetContextMenuDeleteClick != null)
            {
                WidgetContextMenuDeleteClick(this, e);
            }
        }

        /// <summary>
        /// Método llamado cuando el item de menú Editar es seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            if (WidgetContextMenuEditClick != null)
            {
                WidgetContextMenuEditClick(this, e);
            }
        }


        /// <summary>
        /// Ocurre cuando el botón del ratón es liberado en la zona del item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupFormCanvas_MouseUp(object sender, MouseButtonEventArgs e)
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
        private void formCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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
        private void formCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WidgetPreviewMouseDown != null)
            {
                WidgetPreviewMouseDown(this, e);
            }
        }

        /// <summary>
        /// Método llamado cuando un punto de conexión es seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConnectionPoint_Click(object sender, EventArgs e)
        {
            if (ConnectionClick != null)
            {
                ConnectionClick(this, new ConnectionPointClickEventArgs((ConnectionPoint)sender));
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler<ConnectionPointClickEventArgs> ConnectionClick;
        public event MouseButtonEventHandler WidgetPreviewMouseDown;
        public event MouseButtonEventHandler WidgetPreviewMouseUp;
        public event MouseButtonEventHandler WidgetClick;
        public event RoutedEventHandler WidgetContextMenuEditClick;
        public event RoutedEventHandler WidgetContextMenuDeleteClick;
        

        #endregion
    }
}
