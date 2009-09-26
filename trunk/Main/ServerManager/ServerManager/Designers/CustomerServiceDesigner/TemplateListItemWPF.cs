using LogicalLibrary.ServerDesignerClasses;
using System.Windows.Controls;
using LogicalLibrary.DataModelClasses;
using PresentationLayer.ServerDesignerClasses;
using System.Windows.Input;
using System.Windows.Ink;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Clase que representa un TemplateListItemWpf gráfico
    /// </summary>
    public class TemplateListItemWpf : TemplateListItem, IDrawAbleWpf
    {
        #region Constants, Variables and Properties

        private string canvasPath;

        private Canvas myCanvas;
        /// <summary>
        /// Establece u obtiene el canvas que representa un TemplateListItemWpf.
        /// </summary>
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        /// <summary>
        /// Representa el borde gráfico de un TemplateListItem
        /// </summary>
        public Brush Border {
            
            get 
            {
                Rectangle rectangle = MyCanvas.FindName("rectangle") as Rectangle;
                return rectangle.Stroke; 
            }
            set 
            {
                Rectangle rectangle = MyCanvas.FindName("rectangle") as Rectangle;
                rectangle.Stroke = value; 
            }
        }

        /// <summary>
        /// Representa un rectangulo grafico de un TemplateListItem.
        /// </summary>
        public Rectangle ResizableBorder
        {
            get
            {
                Rectangle rectangle = MyCanvas.FindName("rectangleResize") as Rectangle;
                return rectangle;
            }
        }

        /// <summary>
        /// Representa un rectangulo grafico de un TemplateListItem.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                Rectangle rectangle = MyCanvas.FindName("rectangle") as Rectangle;
                return rectangle;
            }
        }

        public TextBlock TextBlockName
        {
            get
            {
                TextBlock textBlockName = this.MyCanvas.FindName("TextBlockName") as TextBlock;
                return textBlockName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fieldAssociated">Campo a asociar.</param>
        /// <param name="fontName">Nombre de la fuente.</param>
        /// <param name="fontSize">Tamaño de la fuente.</param>
        /// <param name="dataType">Tipo de dato.</param>
        public TemplateListItemWpf(Field fieldAssociated, FontName fontName, FontSize fontSize, DataType dataType)
            : base(fieldAssociated, fontName, fontSize, dataType)
        {
                canvasPath = "CanvasTemplateListItemTextBox.xaml";
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Función que modifica el tamaño de un canvas de un template list item.
        /// </summary>
        /// <param name="width">ancho</param>
        /// <param name="height">alto</param>
        #region Public Instance Methods
        public void LoadElementSize(double width, double height)
        {
            if (width == 0 || height == 0)
            {
                this.Width = Rectangle.Width;
                this.Height = Rectangle.Height;
                return;
            }

            if (width > TemplateListItem.MinWidth) 
            {
                Rectangle.Width = width;
                myCanvas.Width = width;
                TextBlockName.Width = width - 5;
                this.Width = width;
            }
            if (height > TemplateListItem.MinHeight)
            {
                Rectangle.Height = height;
                myCanvas.Height = height;
                this.Height = height;
            }
        }

        /// <summary>
        ///Función que modifica el estilo del texto
        /// </summary>
        public void ChangeTextStyle()
        {
            if (TextBlockName == null)
            {
                return;
            }
            TextBlockName.Text = FieldAssociated.Name;
            TextBlockName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(FontColor));
            if (DataType == DataType.Image)
            {
                TextBlockName.UpdateLayout();
                return;
            }

            switch (FontName)
            {
                case FontName.Arial:
                    TextBlockName.FontFamily = new FontFamily("Arial");
                    break;
                case FontName.Currier:
                    TextBlockName.FontFamily = new FontFamily("Courier New");
                    break;
                case FontName.Times:
                    TextBlockName.FontFamily = new FontFamily("Times New Roman");
                    break;
            }

            switch(FontSize)
            {
                case FontSize.Small:
                    TextBlockName.FontSize = 9;
                    break;
                case FontSize.Medium:
                    TextBlockName.FontSize = 13;
                    break;
                case FontSize.Large:
                    TextBlockName.FontSize = 17;
                    break;
            }

            if (Italic)
            {
                TextBlockName.FontStyle = FontStyles.Italic;
            }
            else
            {
                TextBlockName.FontStyle = FontStyles.Normal;
            }

            if (Bold)
            {
                TextBlockName.FontWeight = FontWeights.UltraBold;
            }
            else
            {
                TextBlockName.FontWeight = FontWeights.Normal;
            }

            if (Underline)
            {
                TextDecoration myUnderline = new TextDecoration();

                myUnderline.Pen = new Pen(TextBlockName.Foreground, 1);
                myUnderline.PenThicknessUnit = TextDecorationUnit.FontRecommended;

                // Set the underline decoration to a TextDecorationCollection and add it to the text block.
                TextDecorationCollection myCollection = new TextDecorationCollection();
                myCollection.Add(myUnderline);
                TextBlockName.TextDecorations = myCollection;
            }
            else
            {
                TextBlockName.TextDecorations.Clear();
            }

            TextBlockName.UpdateLayout();
        }
        /// <summary>
        /// Función que construye un canvas que representa un TemplateListItemWpf.
        /// </summary>
        public void MakeCanvas()
        {
            Canvas itemCanvas = Utilities.CanvasFromXaml(this.canvasPath);
            this.MyCanvas = itemCanvas;
            
            ChangeTextStyle();

            if (TextBlockName != null)
            {
                TextBlockName.PreviewMouseDown += new MouseButtonEventHandler(labelFieldName_PreviewMouseDown);
                TextBlockName.PreviewMouseUp += new MouseButtonEventHandler(labelFieldName_PreviewMouseUp);
            }
            if (Rectangle != null)
            {
                Rectangle.PreviewMouseDown += new MouseButtonEventHandler(formCanvas_PreviewMouseDown);
                Rectangle.PreviewMouseUp += new MouseButtonEventHandler(formCanvas_PreviewMouseUp);
            }
            if (ResizableBorder != null)
            {
                ResizableBorder.PreviewMouseDown += new MouseButtonEventHandler(formCanvas_PreviewImageMouseDown);
                ResizableBorder.PreviewMouseUp += new MouseButtonEventHandler(formCanvas_PreviewImageMouseUp);
            }
            
            LoadElementSize(this.Width, this.Height);
        }

        /// <summary>
        /// Función que devuelve un UIElement que representa un TemplateListItemWpf.
        /// </summary>
        /// <returns>UIElement que representa un TemplateListItemWpf</returns>
        public System.Windows.UIElement UIElement
        {
            get
            {
                return MyCanvas;
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        void formCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ComponentPreviewMouseUp != null)
            {
                ComponentPreviewMouseUp(this, e);
            }
        }

        void formCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ComponentPreviewMouseDown != null)
            {
                ComponentPreviewMouseDown(this, e);
            }
        }

        void formCanvas_PreviewImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ComponentPreviewImageMouseUp != null)
            {
                ComponentPreviewImageMouseUp(this, e);
            }
        }

        void formCanvas_PreviewImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ComponentPreviewImageMouseDown != null)
            {
                ComponentPreviewImageMouseDown(this, e);
            }
        }

        void labelFieldName_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ComponentPreviewMouseUp != null)
            {
                ComponentPreviewMouseUp(this, e);
            }
        }

        void labelFieldName_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ComponentPreviewMouseDown != null)
            {
                ComponentPreviewMouseDown(this, e);
            }
        }

        #endregion

        #endregion

        #region Events

        public event MouseButtonEventHandler ComponentPreviewMouseDown;
        public event MouseButtonEventHandler ComponentPreviewMouseUp;
        public event MouseButtonEventHandler ComponentPreviewImageMouseDown;
        public event MouseButtonEventHandler ComponentPreviewImageMouseUp;
        #endregion
    }
}
