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
using SilverlightVisualDesigners.Widgets;
using System.Collections.Generic;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un artefacto visual MenuForm para el diseñador de servicios Silverlight. 
    /// Esta clase encapsula una instancia de MenuForm del proyecto LogicalLibrary.
    /// </summary>
	public partial class MenuFormSilverlight : UserControl,IDraw,IConnection
	{
        private MenuForm menuForm;
        /// <summary>
        /// Establece el artefacto MenuForm encapsulado.
        /// </summary>
        public MenuForm MenuForm
        {
            get { return menuForm; }
            set 
            {
                menuForm = value;
                Title.Text = value.Title;
            }
        }

        private MenuItemSilverlight clickedMenuItem;

        /// <summary>
        /// Obtiene el ítem clickado.
        /// </summary>
        public MenuItemSilverlight ClickedMenuItem
        {
            get { return clickedMenuItem; }
        }

        private List<MenuItemSilverlight> menuItemsSilverlight;
        /// <summary>
        /// Obtiene el elemento MenuItemSilverlight de la lista.
        /// </summary>
        public List<MenuItemSilverlight> MenuItemsSilverlight
        {
            get { return menuItemsSilverlight; }
        }

        private MenuWidget menuWidget;


        /// <summary>
        /// Cambia el título del MenuForm.
        /// </summary>
        /// <param name="title">El título que será asignado.</param>
        public void ChangeTitle(string title)
        {
            Title.Text = title;
            menuForm.Title = title;
        }

        /// <summary>
        /// Crea una instancia de MenuFormSilverlight.
        /// </summary>
		public MenuFormSilverlight()
		{
			// Inicializar variables.
			InitializeComponent();
            menuForm = new MenuForm();
            menuItemsSilverlight = new List<MenuItemSilverlight>();
            menuForm.InputConnectionPoint = new ConnectionPoint(ConnectionPointType.Input);
            menuForm.InputConnectionPoint.Parent = menuForm;
            menuForm.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
            menuForm.OutputConnectionPoint.Parent = menuForm;
            AddMenu(canvasWidget);
		}

        /// <summary>
        /// Crea una instancia de MenuFormSilverlight basado en un MenuForm.
        /// </summary>
        /// <param name="menuForm">MenuForm relativo a MenuFormSilverlight</param>
        public MenuFormSilverlight(MenuForm menuForm)
        {
            // Inicializar variables.
            InitializeComponent();
            menuItemsSilverlight = new List<MenuItemSilverlight>();
            MenuForm = menuForm;
            AddMenu(canvasWidget);
            LoadMenuItems();
        }

        /// <summary>
        /// Encuentra un MenuItemSilverlight de un MenuForm. Este método usa Equals
        /// para comparar el FormMenuItem encapsulado.
        /// </summary>
        /// <param name="menuItemSilverlight">Objeto MenuItemSilverlight a encontrar.</param>
        /// <returns>Objeto MenuItemSilverlight si se lo encuentra, null si no.</returns>
        public MenuItemSilverlight FindMenuItemSilverlight(MenuItemSilverlight menuItemSilverlight)
        {
            foreach (MenuItemSilverlight itemSilverlight in menuItemsSilverlight)
            {
                if (itemSilverlight.FormMenuItem.Equals(menuItemSilverlight.FormMenuItem))
                {
                    return itemSilverlight;
                }
            }
            return null;
        }

        /// <summary>
        /// Encuentra un FormMenuItem en un MenuForm. Este método usa Equals
        /// para comparar el FormMenuItem encapsulado.
        /// </summary>
        /// <param name="formMenuItem">Objeto FormMenuItem a ser encontrado.</param>
        /// <returns>Objeto MenuItemSilverlight si fue encontrado, null si no.</returns>
        public MenuItemSilverlight FindMenuItemSilverlight(FormMenuItem formMenuItem)
        {
            MenuItemSilverlight menuItemSivlerlight = new MenuItemSilverlight(formMenuItem);
            return FindMenuItemSilverlight(menuItemSivlerlight);
        }

        /// <summary>
        /// Dibuja el MenuForm basado en sus ítems.
        /// </summary>
        public void LoadMenuItems()
        {
            int menuNumber = 0;
            foreach (FormMenuItem formMenuItem in menuForm.MenuItems)
            {
                MenuItemSilverlight menuItemSilverlight = new MenuItemSilverlight(formMenuItem);
                // Agregar ítem de menú.
                menuItemsSilverlight.Add(menuItemSilverlight);
                menuItemSilverlight.MouseLeftButtonUp += new MouseButtonEventHandler(item_MouseLeftButtonUp);
                menuItemSilverlight.MenuParent = this;
                this.canvasMenuItems.Children.Add(menuItemSilverlight);
                Canvas.SetZIndex(menuItemSilverlight, Canvas.GetZIndex(menuWidget)-1);
                Canvas.SetTop(menuItemSilverlight, menuNumber * 27);
                menuNumber++;
            }
        }

        /// <summary>
        /// Agrega un menú de opciones al artefacto.
        /// </summary>
        /// <param name="canvas">Canvas donde se añadirá el menú.</param>
        private void AddMenu(Canvas canvas)
        {
            menuWidget = new MenuWidget(MenuType.Both);
            canvasWidget.Children.Add(menuWidget);
            menuWidget.DeletePressed += new EventHandler(menuWidget_DeletePressed);
            menuWidget.OpenWindowsPressed += new EventHandler(menuWidget_OpenWindowsPressed);
        }

        void menuWidget_OpenWindowsPressed(object sender, EventArgs e)
        {
            if (Configure != null)
            {
                Configure(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.MenuForm));
            }
        }

        void menuWidget_DeletePressed(object sender, EventArgs e)
        {
            if (Deleted != null)
            {
                foreach (MenuItemSilverlight menuItemSilverlight in MenuItemsSilverlight)
                {
                    menuItemSilverlight.OnDelete();
                }
                Deleted(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.MenuForm));
            }
        }

        /// <summary>
        /// Agrega un ítem al MenuForm.
        /// </summary>
        /// <param name="item">Ítem a agregar.</param>
        public void AddItem(MenuItemSilverlight item)
        {
            // Enlazar cuando el ítem es clickado.
            item.MouseLeftButtonUp += new MouseButtonEventHandler(item_MouseLeftButtonUp);
            this.menuItemsSilverlight.Add(item);
            this.MenuForm.AddMenuItem(item.FormMenuItem);
            item.MenuParent = this;
            this.canvasMenuItems.Children.Add(item);
            Canvas.SetZIndex(item,Canvas.GetZIndex(item)-1);
            Canvas.SetTop(item, (canvasMenuItems.Children.Count-1) * (this.ActualHeight));
        }

        /// <summary>
        /// Quitar un ítem al MenuForm.
        /// </summary>
        /// <param name="item">Objeto MenuItemSilverlight a quitar.</param>
        public void RemoveItem(MenuItemSilverlight item)
        {
            // Lanza un evento de eliminación desde el ítem.
            item.OnDelete();
            this.menuItemsSilverlight.Remove(item);
            this.MenuForm.RemoveMenuItem(item.FormMenuItem);
            this.canvasMenuItems.Children.Remove(item);
            ReorderItems();
        }

        void item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clickedMenuItem = sender as MenuItemSilverlight;
        }

        /// <summary>
        /// Reorganiza las visualizaciones del ítem.
        /// </summary>
        public void ReorderItems()
        {
            // Ubica los ítems de menú.
            int i = 0;
            foreach (MenuItemSilverlight menuItemSilverlight in MenuItemsSilverlight)
            {
                Canvas.SetTop(menuItemSilverlight, i * (this.ActualHeight));
                i++;
            }
            // Lanza el evento de arrastre para actualizar las relaciones.
            if (Drag != null)
            {
                Drag(this,new EventArgs());
            }
        }

        #region IDrawable Members

        /// <summary>
        /// Coordenada 'x' que representa la posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get
            {
                return menuForm.XCoordinateRelativeToParent;
            }
            set
            {
                menuForm.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordinada 'y' que representa una posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get
            {
                return menuForm.YCoordinateRelativeToParent;
            }
            set
            {
                menuForm.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get
            {
                return menuForm.XFactorCoordinateRelativeToParent;
            }
            set
            {
                menuForm.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get
            {
                return menuForm.YFactorCoordinateRelativeToParent;
            }
            set
            {
                menuForm.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                menuForm.WidthFactor = value;
            }
            get
            {
                return menuForm.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                menuForm.HeightFactor = value;
            }
            get
            {
                return menuForm.HeightFactor;
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
        /// Obtiene el punto gráfico de la entrada de la conexión.
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
        /// Obtiene el punto gráfico de la salida de la conexión.
        /// </summary>
        public Point VisualOutputPoint
        {
            get { return new Point(); }
        }

        /// <summary>
        /// Obtiene el punto de entrada de conexión del artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint InputConnectionPoint
        {
            get { return menuForm.InputConnectionPoint; }
        }

        /// <summary>
        /// Obtiene el punto de conexión de salida para el artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint OutputConnectionPoint
        {
            get { return menuForm.OutputConnectionPoint; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public Component Component
        {
            get { return menuForm; }
        }

        #endregion

        #region IConnection Members


        public event MouseMenuWidgetClickEventHandler Deleted;

        public event MouseMenuWidgetClickEventHandler Configure;

        #endregion
    }
}