using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PresentationLayer.ServerDesignerClasses;

namespace LogicalLibrary.ServerDesignerClasses
{
    public class FormMenuItem : Component
    {
        #region Constants, Variables and Properties

        private MenuForm parent;
        /// <summary>
        /// MenuForm padre de los FormMenuItem.
        /// </summary>
        public MenuForm Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private string text;
        /// <summary>
        /// Texto que se mostrara en un MenuItem.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private TextAlign textAlign = TextAlign.Center;
        /// <summary>
        /// Un TextAlign para la propiedad Text. 
        /// </summary>
        public TextAlign TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        private FontName fontName = FontName.Arial;
        /// <summary>
        /// Una fuente para el FormMenuItem.
        /// </summary>
        public FontName FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        private string fontColor;
        
        /// <summary>
        /// Una cadena que representa el color de una fuente para el FormMenuItem.
        /// </summary>
        public string FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }
       
        private bool bold;
        /// <summary>
        /// Indica si el texto representado por el FormMenuItem tiene que ser  mostrado como negrita.
        /// </summary>
        public bool Bold
        {
            get { return bold; }
            set { bold = value; }
        }

        private string helpText;
        /// <summary>
        /// FormMenuItem de ayuda.
        /// </summary>
        public string HelpText
        {
            get { return helpText; }
            set { helpText = value; }
        }

        #endregion

        #region Constructors

        public FormMenuItem()
        {
            this.FontName = FontName.Arial;
        }

        /// <summary>
        /// Constructor que inicializa el FormMenuItem con un texto específico y una fuente por defecto ("Arial") y tamaño por defecto ("10").
        /// </summary>
        /// <param name="text">Texto que se mostrara</param>
        public FormMenuItem(string text)
        {
            this.Text = text;
            this.FontName = FontName.Arial;

        }

        /// <summary>
        /// Constructor que inicializa el FormMenuItem con un texto especifico y una  fuente y tamaño de de fuente por defecto ("10").
        /// </summary>
        /// <param name="text">Texto que sera mostrado</param>
        /// <param name="fontName">Fuente para el texto</param>
        public FormMenuItem(string text, string help, FontName fontName)
        {
            this.Text = text;
            this.HelpText = help;
            this.FontName = fontName;
        }

        /// <summary>
        /// Constructor que inicializa el FormMenuItem con un texto específico, 
        /// fuente y tamaño.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        public FormMenuItem(string text, string help, FontName fontName, int fontSize)
        {
            this.Text = text;
            this.HelpText = help;
            this.FontName = fontName;
        }

        #endregion

        #region Instance Methods

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                FormMenuItem formMenuItem = (FormMenuItem)obj;
                if (String.CompareOrdinal(this.text, formMenuItem.text) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Método que reinica el FormMenuItem, llama al método Reset del padre y al   método Reset del OutputConnectionPoint.
        /// </summary>
        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            base.Reset(previousConnectionPoint);
            this.OutputConnectionPoint.Reset(null);   
        }

        /// <summary>
        /// Implementación del método ToString.
        /// </summary>
        /// <returns>Texto del MenuItem</returns>
        public override string ToString()
        {
            return this.Text;
        }
        #endregion
    }
}
