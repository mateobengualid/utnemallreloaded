using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicalLibrary.ServerDesignerClasses
{
    /// <summary>
    /// Clase que representa un formulario dentro del Diseñador Visual. Es la   representación de un formulario antes que sea mostrado en un SmarthPhone.
    /// </summary>
    public class Form : Widget
    {
        #region Constants, Variables and Properties
               
        private string backgroundColor;
        /// <summary>
        /// Un color para el fondo del formulario
        /// </summary>
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string stringHelp;
        /// <summary>
        /// Una cadena que representa el texto que se mostrará si se necesita ayuda para   este formulario.  
        /// </summary>
        public string StringHelp
        {
            get { return stringHelp; }
            set { stringHelp = value; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método que reinicia el formulario, llama al métod padre Reset y limpia el   título, BackgroundColor, StringHelp y OutputConnectionPoint
        /// </summary>
        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            base.Reset(previousConnectionPoint);
            this.Title = "";
            this.BackgroundColor = "";
            this.StringHelp = "";
        }

        #endregion
    }
}
