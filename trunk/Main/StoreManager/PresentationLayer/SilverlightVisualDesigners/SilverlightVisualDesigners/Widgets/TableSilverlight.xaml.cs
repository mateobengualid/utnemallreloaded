using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary.DataModelClasses;
using System.Collections.Generic;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un artefacto visual Table Widget para el diseñador de servicios Silverlight. 
    /// Esta clase encapsula una instancia de Table del proyecto LogicalLibrary.
    /// </summary>
	public partial class TableSilverlight : UserControl,IDraw,IConnection
	{
        private Table table;
        /// <summary>
        /// Accede al artefacto Table encapsulado.
        /// </summary>
        public Table Table
        {
            get { return table; }
            set { 
                    table = value;
                    textBlockTableName.Text = value.Name;
                }
        }

        /// <summary>
        /// Accede a una colección de campos.
        /// </summary>
        public List<Field> Fields {
            get { return table.Fields; } 
        }

        /// <summary>
        /// Accede al InputDataContext de Table.
        /// </summary>
        public Table InputDataContext {
            get { return table.InputDataContext; }
            set { table.InputDataContext = value; }
        }

        /// <summary>
        /// Accede al OutputDataContext de Table.
        /// </summary>
        public Table OutputDataContext {
            get { return table.OutputDataContext; }
            set { table.OutputDataContext = value; }
        }

        /// <summary>
        /// Determina si es un almacén.
        /// </summary>
        public bool IsStorage {
            get { return table.IsStorage; }
            set { table.IsStorage = value; }
        }

        public string TableName {
            get { return table.Name; }
            set 
            { 
                table.Name = value;
                textBlockTableName.Text = value;
            }
        }

        /// <summary>
        /// Coordenada 'x' que representa la posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent {
            get { return table.XCoordinateRelativeToParent; }
            set { table.XCoordinateRelativeToParent = value; }
        }

        /// <summary>
        /// Coordenada 'y' que representa la posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get { return table.YCoordinateRelativeToParent; }
            set { table.YCoordinateRelativeToParent = value; }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get { return table.XFactorCoordinateRelativeToParent; }
            set { table.XFactorCoordinateRelativeToParent = value; }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get { return table.YFactorCoordinateRelativeToParent; }
            set { table.YFactorCoordinateRelativeToParent = value; }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                table.WidthFactor = value;
            }
            get
            {
                return table.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                table.HeightFactor = value;
            }
            get
            {
                return table.HeightFactor;
            }
        }

        /// <summary>
        /// Obtiene el punto visual que representa el punto que debe conectarse a la  entrada de un ConnectionSilverlight, en coordenadas relativas al
        /// TableSilverlight UserControl.
        /// </summary>
        public Point VisualInputPoint 
        {
            get 
            {
                return new Point(LayoutRoot.ActualWidth/2,LayoutRoot.ActualHeight/2);
            }
        }

        /// <summary>
        /// Obtiene un punto visual que representa un punto que conectar a la salida de un ConnectionSilverlight, en coordenadas relativas al
        /// TableSilverlight UserControl.
        /// </summary>
        public Point VisualOutputPoint
        {
            get {return VisualInputPoint;}
        }

        /// <summary>
        /// Constructor con el nombre de la tabla.
        /// </summary>
        /// <param name="name">Nombre de la tabla.</param>
        public TableSilverlight(string name)
        {
            // Inicializar variables.
            InitializeComponent();
            this.table = new Table(name);
            AddMenu(canvasWidget);
        }

        /// <summary>
        /// Crea una instancia de TableSilverlight basada en un Table.
        /// </summary>
        /// <param name="table">Table relativa a un TableSilverlight.</param
        public TableSilverlight(Table table)
        {
            InitializeComponent();
            Table = table;
            AddMenu(canvasWidget);
        }

        /// <summary>
        /// Crea un menú de opciones al artefacto.
        /// </summary>
        /// <param name="canvas">Canvas donde se añadirá el menú.</param>
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
            if (Deleted != null)
            {
                Deleted(this, new MouseDoubleClickEventArgs(PresentationLayer.ServerDesignerClasses.WidgetType.ListForm));
            }
        }

        #region Events

        public event EventHandler Drag;

        #endregion

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

        #region IConnection Members

        /// <summary>
        /// Obtiene el punto de entrada de conexión para el artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint InputConnectionPoint
        {
            get { return table.InputConnectionPoint; }
        }

        /// <summary>
        /// Obtiene el punto de conexión de salida para el artefacto.
        /// </summary>
        public LogicalLibrary.ServerDesignerClasses.ConnectionPoint OutputConnectionPoint
        {
            get { return table.InputConnectionPoint; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public LogicalLibrary.Component Component
        {
            get { return table; }
        }

        #endregion

        #region IConnection Members


        public event MouseMenuWidgetClickEventHandler Deleted;

        public event MouseMenuWidgetClickEventHandler Configure;

        #endregion
    }
}