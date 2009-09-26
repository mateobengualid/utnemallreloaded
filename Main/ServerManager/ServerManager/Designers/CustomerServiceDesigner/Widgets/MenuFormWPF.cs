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
    /// Clase que define un formulario de menu usado en un proyecto wpf
    /// </summary>
    public class MenuFormWpf : MenuForm, IDrawAbleWpf
    {
        #region Instance Variables and Properties

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
        /// Constructor
        /// </summary>
        public MenuFormWpf()
        {
            this.canvasPath = "CanvasMenuForm.xaml";
            InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="formNumber">Número de formulario</param>
        public MenuFormWpf(int formNumber)
        {
            this.canvasPath = "CanvasMenuForm.xaml";
            InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
            this.SetTitle(UtnEmall.ServerManager.Properties.Resources.MenuForm + " " + formNumber);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Función ToString redefinida para mostrar el titulo del formulario
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// Actualiza el titulo del formulario
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
        /// Construye el canvas del formulario de menu y agrega los eventos del mouse
        /// </summary>
        public void MakeCanvas()
        {
            Canvas formCanvas = Utilities.CanvasFromXaml(this.canvasPath);

            // Alarga el formulario de menu, usando 30 como alto.
            formCanvas.Height += MenuItems.Count * 30;

            formCanvas.PreviewMouseDown += new MouseButtonEventHandler(formCanvas_PreviewMouseDown);
            formCanvas.PreviewMouseUp += new MouseButtonEventHandler(formCanvas_PreviewMouseUp);
            
            ConnectionPointWpf inputConnectionPoint = this.InputConnectionPoint as ConnectionPointWpf;

            // Cargar el canvas desde el punto de conexión.
            inputConnectionPoint.MakeCanvas();

            // Agrega los eventos al punto de conexión
            inputConnectionPoint.Click += new EventHandler(ConnectionPoint_Click);

            // Agrega los items del menu al canvas
            int i = 1;
            foreach (FormMenuItemWpf item in this.MenuItems)
            {
                item.Width = formCanvas.Width;
                item.Height = 30;// formCanvas.Height;
                item.MakeCanvas();

                // Agrega los eventos.
                if (!item.ConnectionClickAttached)
                {
                    item.ConnectionClickAttached = true;
                    item.ConnectionClick += new EventHandler(ConnectionPoint_Click);
                }
                Canvas itemCanvas = (item as FormMenuItemWpf).MyCanvas;
                formCanvas.Children.Add(itemCanvas);
                Canvas.SetTop(itemCanvas, i * item.Height);
                i++;
            }

            // Compone el grupo de canvas
            Canvas groupFormCanvas = new Canvas();
            groupFormCanvas.Background = Brushes.Transparent;
            groupFormCanvas.Width = formCanvas.Width + (inputConnectionPoint.MyCanvas.Width * 2);
            groupFormCanvas.Height = formCanvas.Height;
            groupFormCanvas.MouseUp += new MouseButtonEventHandler(groupFormCanvas_MouseUp);

            groupFormCanvas.Children.Add(formCanvas);
            Canvas.SetLeft(formCanvas, inputConnectionPoint.MyCanvas.Width);
            groupFormCanvas.Children.Add(inputConnectionPoint.MyCanvas);

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

        /// <summary>
        /// Función llamada cuando un punto de conexión es presionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionPoint_Click(object sender, EventArgs e)
        {
            if (ConnectionClick != null)
            {
                if (sender is FormMenuItemWpf)
                {
                    FormMenuItemWpf menuItem = sender as FormMenuItemWpf;
                    ConnectionClick(menuItem, new ConnectionPointClickEventArgs(menuItem.OutputConnectionPoint));
                }
                else
                {
                    ConnectionClick(this, new ConnectionPointClickEventArgs((ConnectionPoint)sender));
                }
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
