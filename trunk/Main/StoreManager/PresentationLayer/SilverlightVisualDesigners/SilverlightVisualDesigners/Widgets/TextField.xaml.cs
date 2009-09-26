using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary.ServerDesignerClasses;
using LogicalLibrary.DataModelClasses;
using PresentationLayer.ServerDesignerClasses;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un artefacto visual TemplateListItem para el diseñador de servicios Silverlight. 
    /// Esta clase encapsula una instancia de TempletListItem del proyecto LogicalLibrary.
    /// </summary>
	public partial class TextField : UserControl,IDraw
	{
        private TemplateListItem templateListItem;
        
        /// <summary>
        /// Obtiene el artefacto TemplateListItem encapsulado.
        /// </summary>
        public TemplateListItem TemplateListItem
        {
            get { return templateListItem; }
            set { templateListItem = value; }
        }

        public void LoadElementSize(double width, double height)
        {
            if (width == 0 || height == 0)
            {
                this.Width = TemplateListItem.MinWidth;
                this.Height = TemplateListItem.MinHeight;
                return;
            }

            if (width > TemplateListItem.MinWidth)
            {
                TemplateListItem.Width = width;
                this.Width = width;
            }
            if (height > TemplateListItem.MinHeight)
            {
                TemplateListItem.Height = height;
                this.Height = height;
            }

            this.UpdateLayout();
        }

		public TextField()
		{
			// Inicializar variables.
			InitializeComponent();
            attachEvents();
            LoadElementSize(this.ActualWidth, this.ActualHeight);
		}

        /// <summary>
        /// Crea un objeto TextField basado en un TemplateListItem.
        /// </summary>
        /// <param name="templateListItem">TempleteListItem relativo al TextField.</param>
        public TextField(TemplateListItem templateListItem)
        {
            InitializeComponent();
            this.templateListItem = templateListItem;
            this.label.Text = templateListItem.FieldAssociated.Name;
            imageCerrar.MouseLeftButtonUp += new MouseButtonEventHandler(imageCerrar_MouseLeftButtonUp);

            // Cargar aspecto visual.
            if (templateListItem.Bold) label.FontWeight = FontWeights.Bold;
            if (templateListItem.Italic) label.FontStyle = FontStyles.Italic;
            label.Foreground = LogicalLibrary.Utilities.ConvertStringToSolidColorBrush(templateListItem.FontColor);
            ChangeTextSize(templateListItem.FontSize);
            attachEvents();
            LoadElementSize(templateListItem.Width, templateListItem.Height);
        }

        /// <summary>
        /// Crea un TextField con un campo y tipo específicos.
        /// </summary>
        /// <param name="associatedField">Nombre del campo asociado.</param>
        /// <param name="dataType">Tipo de dato del campo.</param>
		public TextField(Field associatedField,DataType dataType)
		{
			// Inicializar variables.
			InitializeComponent();
            templateListItem = new TemplateListItem(associatedField, dataType);
            this.label.Text = associatedField.Name;
            attachEvents();
            LoadElementSize(this.ActualWidth, this.ActualHeight);
		}

        /// <summary>
        /// Cambia el tamaño de letra del TextField.
        /// </summary>
        /// <param name="size">Tamaño a establecer.</param>
        public void ChangeTextSize(PresentationLayer.ServerDesignerClasses.FontSize size)
        {
            switch (size)
            {
                case PresentationLayer.ServerDesignerClasses.FontSize.Small:
                    label.FontSize = 9;
                    break;
                case PresentationLayer.ServerDesignerClasses.FontSize.Medium:
                    label.FontSize = 12;
                    break;
                case PresentationLayer.ServerDesignerClasses.FontSize.Large:
                    label.FontSize = 15;
                    break;
                default:
                    label.FontSize = 12;
                    break;
            }
        }

        private void attachEvents()
        {
            imageCerrar.MouseLeftButtonUp += new MouseButtonEventHandler(imageCerrar_MouseLeftButtonUp);
            rectangleResize.MouseLeftButtonDown += new MouseButtonEventHandler(imageResize_MouseLeftButtonDown);
            rectangleResize.MouseLeftButtonUp += new MouseButtonEventHandler(imageResize_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(TextField_MouseMove);
        }
           
        void TextField_MouseMove(object sender, MouseEventArgs e)
        {
            if (TextFieldMouseMove != null)
            {
                TextFieldMouseMove(this, e);
            }
        }

        void imageResize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ResizeDown != null)
            {
                ResizeDown(this, e);
            }
        }

        void imageResize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ResizeUp != null)
            {
                ResizeUp(this, e);
            }
        }

        void imageCerrar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Deleted!=null)
            {
                Deleted(this,e);
            }
        }

        
        public event MouseButtonEventHandler Deleted;

        #region IDrawable Members

        /// <summary>
        /// Coordenada 'x' que representa la posición del componente en el padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get
            {
                return templateListItem.XCoordinateRelativeToParent;
            }
            set
            {
                templateListItem.XCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Coordenada 'y' que representa la posición del componente en el padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get
            {
                return templateListItem.YCoordinateRelativeToParent;
            }
            set
            {
                templateListItem.YCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'x' independiente al padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get
            {
                return templateListItem.XFactorCoordinateRelativeToParent;
            }
            set
            {
                templateListItem.XFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa una coordenada 'y' independiente al padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get
            {
                return templateListItem.YFactorCoordinateRelativeToParent;
            }
            set
            {
                templateListItem.YFactorCoordinateRelativeToParent = value;
            }
        }

        /// <summary>
        /// Un factor que representa el ancho independiente al padre.
        /// </summary>
        public double WidthFactor
        {
            set
            {
                templateListItem.WidthFactor = value;
            }
            get
            {
                return templateListItem.WidthFactor;
            }
        }

        /// <summary>
        /// Un factor que representa la altura independiente al padre.
        /// </summary>
        public double HeightFactor
        {
            set
            {
                templateListItem.HeightFactor = value;
            }
            get
            {
                return templateListItem.HeightFactor;
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
        public event MouseEventHandler ResizeUp;
        public event MouseEventHandler ResizeDown;
        public event MouseEventHandler TextFieldMouseMove;

        #endregion
    }
}