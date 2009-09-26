using LogicalLibrary.Widgets;
using System.Windows.Controls;
using LogicalLibrary.DataModelClasses;
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
    /// Esta clase define un origen de datos usado en un proyecto WPF
    /// </summary>
    public class DataSourceWpf : DataSource, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Ruta del lienzo de la tabla xaml
        /// </summary>
        private string canvasPath;

        /// <summary>
        /// Tabla de lienzos
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
        /// Establecer la ruta del lienzo e inicializar el punto de conexión de entrada y salida
        /// </summary>
        /// <param name="table">Tabla relacionada</param>
        public DataSourceWpf(Table table)
            : base(table)
        {
            this.canvasPath = "CanvasDataSource.xaml";
            this.InputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Input, this);
            this.OutputConnectionPoint = new ConnectionPointWpf(ConnectionPointType.Output, this);
        }

        #endregion
        
        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Hace el lienzo de origen de datos WPF y adjunta eventos del mouse
        /// </summary>
        public void MakeCanvas()
        {
            Canvas dataCanvas = Utilities.CanvasFromXaml(this.canvasPath);
            ConnectionPointWpf connectionPoint;

            dataCanvas.PreviewMouseDown += new MouseButtonEventHandler(formCanvas_PreviewMouseDown);
            dataCanvas.PreviewMouseUp += new MouseButtonEventHandler(formCanvas_PreviewMouseUp);

            if (this.RelatedTable.IsStorage == true)
            {
                connectionPoint = this.InputConnectionPoint as ConnectionPointWpf;
            }
            else
            {
                connectionPoint = this.OutputConnectionPoint as ConnectionPointWpf;
            }
            connectionPoint.MakeCanvas();
            connectionPoint.Click += new EventHandler(ConnectionPoint_Click);

            // Compone el grupo de lienzos.
            Canvas groupFormCanvas = new Canvas();
            groupFormCanvas.Background = Brushes.Transparent;
            groupFormCanvas.Width = dataCanvas.Width + (connectionPoint.MyCanvas.Width * 2);
            groupFormCanvas.Height = dataCanvas.Height;
            groupFormCanvas.MouseUp += new MouseButtonEventHandler(groupFormCanvas_MouseUp);

            groupFormCanvas.Children.Add(dataCanvas);
            Canvas.SetLeft(dataCanvas, connectionPoint.MyCanvas.Width);
            groupFormCanvas.Children.Add(connectionPoint.MyCanvas);

            // Establece la posición del ConnectionPoint.
            if (this.RelatedTable.IsStorage == true)
            {
                connectionPoint.XCoordinateRelativeToParent = 0;
                connectionPoint.YCoordinateRelativeToParent = (groupFormCanvas.Height - (connectionPoint.MyCanvas.Height / 3));
            }
            else
            {
                connectionPoint.XCoordinateRelativeToParent = groupFormCanvas.Width - connectionPoint.MyCanvas.Width;
                connectionPoint.YCoordinateRelativeToParent = (groupFormCanvas.Height - connectionPoint.MyCanvas.Height) / 3;
            }
            Canvas.SetLeft(connectionPoint.MyCanvas, connectionPoint.XCoordinateRelativeToParent);
            Canvas.SetTop(connectionPoint.MyCanvas, connectionPoint.YCoordinateRelativeToParent);

            TextBlock textBlockName = dataCanvas.FindName("TextBlockName") as TextBlock;
            textBlockName.Text = this.Name;

            GenerateContextMenu(groupFormCanvas);

            this.MyCanvas = groupFormCanvas;
        }

        /// <summary>
        /// Retorna el lienzo de origen de datos como UIElement
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
        /// Este método crea el menú de contexto para el formulario
        /// </summary>
        /// <param name="widget">El lienzo donde el menú sera agregado</param>
        private void GenerateContextMenu(Canvas widget)
        {
            ContextMenu contextMenu = new ContextMenu();
            
            MenuItem menuItemDelete = new MenuItem();
            menuItemDelete.Header = UtnEmall.ServerManager.Properties.Resources.Delete;
            menuItemDelete.Click += new RoutedEventHandler(menuItemDelete_Click);
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
        public event MouseButtonEventHandler WidgetClick;
        public event MouseButtonEventHandler WidgetPreviewMouseDown;
        public event MouseButtonEventHandler WidgetPreviewMouseUp;
        public event RoutedEventHandler WidgetContextMenuDeleteClick;

        #endregion
    }
}
