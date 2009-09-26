using System;
using LogicalLibrary.DataModelClasses;
using PresentationLayer.ServerDesignerClasses;
using System.ComponentModel;
using System.Windows.Media;

namespace LogicalLibrary.ServerDesignerClasses
{
    public class TemplateListItem : Component
    {
        #region Constants, Variables and Properties

        public const double MinWidth = 80;
        public const double MinHeight = 30;

        private Field fieldAssociated;
        /// <summary>
        /// Un campo asociado al TempleteListItem.
        /// </summary>
        public Field FieldAssociated
        {
            get { return fieldAssociated; }
            set { fieldAssociated = value; }
        }

        private string backgroundColor;
        /// <summary>
        /// Una cadena que representa el color de fondo del TemplateListItem.
        /// </summary>
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private DataType dataType;
        /// <summary>
        /// Un tipo de dato para el TemplateListItem.
        /// </summary>
        public DataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        private FontName fontName;
        /// <summary>
        /// Una fuente para el TemplateListItem.
        /// </summary>
        public FontName FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        private FontSize fontSize;
        /// <summary>
        /// Un tamaño de fuente para el TemplateListItem.
        /// </summary>
        public FontSize FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        private bool bold;
        /// <summary>
        /// Indica si el texto representado por el TemplateListItem debe ser mostrado  en negrita.
        /// </summary>
        public bool Bold
        {
            get { return bold; }
            set { bold = value; }
        }

        private bool italic;
        /// <summary>
        /// Indica si el texto representado por el TemplateListItem tiene que ser  mostado en cursiva.
        /// </summary>
        public bool Italic
        {
            get { return italic; }
            set { italic = value; }
        }

        private bool underline;
        /// <summary>
        /// Indica si el texto representado por el TemplateListItem tiene que ser  mostrado como subrayado.
        /// </summary>
        public bool Underline
        {
            get { return underline; }
            set { underline = value; }
        }

        private string fontColor;
        /// <summary>
        /// Una cadena que representa un color de fuente para el TemplateListItem.
        /// </summary>
        public string FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor que inicializa el TemplateListItem con un campo y tipo de dato   específico.
        /// </summary>
        /// <param name="fieldAssociated">Campo a asociar</param>
        /// <param name="dataType">Tipo de dato a asociar</param>
        public TemplateListItem(Field fieldAssociated, DataType dataType)
        {
            if (fieldAssociated == null)
            {
                throw new ArgumentNullException("fieldAssociated", "fieldAssociated can not be null");
            }
            FieldAssociated = fieldAssociated;
            DataType = dataType;
            FontName = FontName.Arial;
            FontSize = FontSize.Medium;
            FontColor = "#FF000000";
        }

        /// <summary>
        /// Constructor que inicializa el TemplateListItem con un campo específico,  fuente, tamaño de fuente y tipo de dato.
        /// </summary>
        /// <param name="fieldAssociated">Campo a asociar</param>
        /// <param name="fontName">Fuente a asociar</param>
        /// <param name="fontSize">Tamaño de fuente a asociar</param>
        /// <param name="dataType">Tipo de dato a asociar</param>
        public TemplateListItem(Field fieldAssociated, FontName fontName, FontSize fontSize, DataType dataType)
        {
            if (fieldAssociated == null)
            {
                throw new ArgumentNullException("fieldAssociated", "fieldAssociated can not be null");
            }

            FontColor = "#FF000000";
            this.FieldAssociated = fieldAssociated;
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.DataType = dataType;
        }

        #endregion
    }
}

