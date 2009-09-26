using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary;

namespace SilverlightVisualDesigners.Widgets
{
    /// <summary>
    /// Clase que representa un artefacto visual MenuItem para el diseñador de servicios Silverlight. 
    /// Esta clase encapsula una instancia de MenuItem del proyecto LogicalLibrary.
    /// </summary>
    public partial class MenuItemSilverlight : UserControl,IConnection
    {
        private FormMenuItem formMenuItem;
        /// <summary>
        /// Obtiene el artefacto FormMenuItem encapsulado.
        /// </summary>
        public FormMenuItem FormMenuItem 
        {
            get { return formMenuItem; } 
        }

        private MenuFormSilverlight menuParent;
        /// <summary>
        /// Obtiene el objeto padre MenuFormSilverlight de este objeto MenuItemSilverlight.
        /// </summary>
        public MenuFormSilverlight MenuParent
        {
            get { return menuParent; }
            set 
            { 
                menuParent = value;
                menuParent.Drag += new EventHandler(menuParent_Drag);
            }
        }

        /// <summary>
        /// Indica si el texto representado por este FormMenuItem tiene que mostrarse en negrita.
        /// </summary>
        public bool Bold
        {
            get { return formMenuItem.Bold; }
            set { formMenuItem.Bold = value; }
        }

        /// <summary>
        /// Fuente del objeto FormMenuItem.
        /// </summary>
        public FontName FontName
        {
            get { return formMenuItem.FontName; }
            set { formMenuItem.FontName = value; }
        }

        /// <summary>
        /// Una cadena que representa el color de fuente del objeto FormMenuItem.
        /// </summary>
        public String FontColor
        {
            get { return formMenuItem.FontColor; }
            set { formMenuItem.FontColor = value; }
        }

        /// <summary>
        /// La cadena que se mostrará en el objeto MenuItem.
        /// </summary>
        public string Text
        {
            get { return formMenuItem.Text; }
            set
            {
                formMenuItem.Text = value;
                this.label.Text = value;
            }

        }

        /// <summary>
        /// Cadena de ayuda del objeto FormMenuItem.
        /// </summary>
        public string HelpText
        {
            get { return formMenuItem.HelpText; }
            set { formMenuItem.HelpText = value; }
        }

        /// <summary>
        /// Accede a la alineación del texto.
        /// </summary>
        public TextAlign TextAlign
        {
            get { return formMenuItem.TextAlign; }
            set { formMenuItem.TextAlign = value; }
        }

        public Table OutputDataContext
        {
            get { return formMenuItem.OutputDataContext; }
            set { formMenuItem.OutputDataContext = value; }
        }

        public Table InputDataContext
        {
            get { return formMenuItem.InputDataContext; }
            set { formMenuItem.InputDataContext = value; }
        }

        public MenuItemSilverlight(string text)
        {
            InitializeComponent();
            formMenuItem = new FormMenuItem(text);
            this.label.Text = text;
            formMenuItem.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
            formMenuItem.OutputConnectionPoint.Parent = formMenuItem;
        }

        public MenuItemSilverlight(FormMenuItem formMenuItem)
        {
            InitializeComponent();
            this.label.Text = formMenuItem.Text;
            this.formMenuItem = formMenuItem;
            if (formMenuItem.InputConnectionPoint == null)
            {
                formMenuItem.InputConnectionPoint = new ConnectionPoint(ConnectionPointType.Input);
                formMenuItem.InputConnectionPoint.Parent = formMenuItem;
            }
            if (formMenuItem.OutputConnectionPoint==null)
            {
                formMenuItem.OutputConnectionPoint = new ConnectionPoint(ConnectionPointType.Output);
                formMenuItem.OutputConnectionPoint.Parent = formMenuItem;
            }
        }

        void menuParent_Drag(object sender, EventArgs e)
        {
            this.OnDrag();
        }

        public void OnDelete()
        {
            if (Deleted != null)
            {
                Deleted(this,new MouseDoubleClickEventArgs(WidgetType.MenuForm));
            }
        }

        public override string ToString()
        {
            return this.formMenuItem.Text;
        }

        #region IConnection Members

        /// <summary>
        /// Obtiene un punto gráfico de entrada de conexión.
        /// </summary>
        public Point VisualInputPoint
        {
            get { return new Point(); }
        }

        /// <summary>
        /// Obtiene el punto gráfico de salida de conexión.
        /// </summary>
        public Point VisualOutputPoint
        {
            get 
            {
                GeneralTransform generalTransform = CanvasConnectionPointOutput.TransformToVisual(canvasMenuItem);
                return generalTransform.Transform(new Point(CanvasConnectionPointOutput.ActualHeight, CanvasConnectionPointOutput.ActualWidth / 2)); 
            }
        }

        /// <summary>
        /// Obtiene el punto de entrada de conexión del artefacto.
        /// </summary>
        public ConnectionPoint InputConnectionPoint
        {
            get { return null; }
        }

        /// <summary>
        /// Obtiene el punto de conexión de salida del artefacto.
        /// </summary>
        public ConnectionPoint OutputConnectionPoint
        {
            get { return this.formMenuItem.OutputConnectionPoint; }
        }

        #endregion

        #region IDrawable Members

        /// <summary>
        /// Coordenada 'x' que representa la posición del componente en el padre.
        /// </summary>
        double IDraw.XCoordinateRelativeToParent
        {
            get
            {
                return formMenuItem.XCoordinateRelativeToParent;
            }
            set
            {
                formMenuItem.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordenada 'y' que representa la posición del componente en el padre.
        /// </summary>
        double IDraw.YCoordinateRelativeToParent
        {
            get
            {
                return formMenuItem.YCoordinateRelativeToParent;
            }
            set
            {
                formMenuItem.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        double IDraw.XFactorCoordinateRelativeToParent
        {
            get
            {
                return formMenuItem.XFactorCoordinateRelativeToParent;
            }
            set
            {
                formMenuItem.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        double IDraw.YFactorCoordinateRelativeToParent
        {
            get
            {
                return formMenuItem.YFactorCoordinateRelativeToParent;
            }
            set
            {
                formMenuItem.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                formMenuItem.WidthFactor = value;
            }
            get
            {
                return formMenuItem.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                formMenuItem.HeightFactor = value;
            }
            get
            {
                return formMenuItem.HeightFactor;
            }
        }

        /// <summary>
        /// Lanza el evento de arrastre manualmente.
        /// </summary>
        public void OnDrag()
        {
            if (Drag!=null)
            {
                Drag(this, new EventArgs());
            }
        }

        public event EventHandler Drag;

        #endregion

        #region IComponent Members

        /// <summary>
        /// Obtiene el componente encapsulado.
        /// </summary>
        public Component Component
        {
            get { return this.formMenuItem; }
        }

        #endregion

        #region IConnection Members

        #endregion

        #region IConnection Members

        public event MouseMenuWidgetClickEventHandler Deleted;

        #pragma warning disable 0067
        // Este evento está definido en la interface IConnection y que esta clase
        // implementa pero no usa.
        public event MouseMenuWidgetClickEventHandler Configure;
        #pragma warning restore 0067

        #endregion
    }
}
