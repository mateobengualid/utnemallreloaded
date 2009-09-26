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
    /// Clase que define un formulario de visualizacion de datos en wpf
    /// </summary>
    public class ShowDataFormWpf : ShowDataForm, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        private string canvasPath;

        /// <summary>
        /// El canvas para un formulario de visualizacion de datos
        /// </summary>
        private Canvas myCanvas;
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor. Inicializa los puntos de conexión de salida y entrada.
        /// </summary>
        public ShowDataFormWpf()
        {
            this.TemplateListFormDocument = new TemplateListFormDocumentWpf();
            this.canvasPath = "CanvasShowDataForm.xaml";
            this.InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            this.OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
        }

        /// <summary>
        /// Crea el formulario de visualización de datos
        /// </summary>
        /// <param name="formNumber">Número de formulario</param>
        public ShowDataFormWpf(int formNumber)
        {
            this.TemplateListFormDocument = new TemplateListFormDocumentWpf();
            this.canvasPath = "CanvasShowDataForm.xaml";
            this.InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            this.OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
            this.SetTitle(UtnEmall.ServerManager.Properties.Resources.ShowData + formNumber);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Actualiza el titulo del formulario en wpf
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
        /// Crea el canvas para un formulario en wpf y agrega los eventos
        /// </summary>

        public void MakeCanvas()
        {
            Canvas formCanvas = Utilities.CanvasFromXaml(this.canvasPath);
            formCanvas.PreviewMouseDown += new MouseButtonEventHandler(formCanvas_PreviewMouseDown);
            formCanvas.PreviewMouseUp += new MouseButtonEventHandler(formCanvas_PreviewMouseUp);

            ConnectionPointWpf inputConnectionPoint = this.InputConnectionPoint as ConnectionPointWpf;
            ConnectionPointWpf outputConnectionPoint = this.OutputConnectionPoint as ConnectionPointWpf;

            // Cargar el canvas desde el punto de conexión.
            inputConnectionPoint.MakeCanvas();
            outputConnectionPoint.MakeCanvas();

            // Agrega los eventos al punto de conexión
            inputConnectionPoint.Click += new EventHandler(ConnectionPoint_Click);
            outputConnectionPoint.Click += new EventHandler(ConnectionPoint_Click);

            // Compone el grupo de canvas
            Canvas groupFormCanvas = new Canvas();
            groupFormCanvas.Background = Brushes.Transparent;
            groupFormCanvas.Width = formCanvas.Width + (inputConnectionPoint.MyCanvas.Width * 2);
            groupFormCanvas.Height = formCanvas.Height;
            groupFormCanvas.MouseUp += new MouseButtonEventHandler(groupFormCanvas_MouseUp);

            groupFormCanvas.Children.Add(formCanvas);
            Canvas.SetLeft(formCanvas, inputConnectionPoint.MyCanvas.Width);
            groupFormCanvas.Children.Add(inputConnectionPoint.MyCanvas);
            groupFormCanvas.Children.Add(outputConnectionPoint.MyCanvas);

            // Establece la posicion del punto de conexión
            outputConnectionPoint.XCoordinateRelativeToParent = groupFormCanvas.Width - outputConnectionPoint.MyCanvas.Width;
            outputConnectionPoint.YCoordinateRelativeToParent = groupFormCanvas.Height - outputConnectionPoint.MyCanvas.Height;
            Canvas.SetLeft(outputConnectionPoint.MyCanvas, outputConnectionPoint.XCoordinateRelativeToParent);
            Canvas.SetTop(outputConnectionPoint.MyCanvas, outputConnectionPoint.YCoordinateRelativeToParent);

            GenerateContextMenu(groupFormCanvas);

            this.MyCanvas = groupFormCanvas;
            ChangeTitle();
        }
        
        /// <summary>
        /// Retorna el canvas como un UIElement
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
        /// Crea el menu de opciones para el formulario
        /// </summary>
        /// <param name="widget">El canvas donde se agregara el menu</param>
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
        /// Función llamada cuando el boton elimina es presionado
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
        /// Función llamada cuando el boton editar es presionado
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
        /// Ocurre cuando el boton del mouse pasa por encima del area del item
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

        private void formCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (WidgetPreviewMouseUp != null)
            {
                WidgetPreviewMouseUp(this, e);
            }
        }

        private void formCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WidgetPreviewMouseDown != null)
            {
                WidgetPreviewMouseDown(this, e);
            }
        }

        /// <summary>
        /// Función llamada cuando un punto de conexión es presionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionPoint_Click(object sender, EventArgs e)
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
