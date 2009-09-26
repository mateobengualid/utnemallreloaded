using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;

namespace LogicalLibrary
{
    /// <summary>
    /// Una clase que representa un objeto que puede ser mostrado en un   ServiceDocument y es posible arrastrar y soltar dentro del ServiceDesigner.
    /// </summary>
    public abstract class Widget : Component
    {
        #region Constants, Variables and Properties
        private string title;
        /// <summary>
        /// El título para el formulario
        /// </summary>
        public virtual string Title
        {
            get 
            { 
                return title; 
            }
            set 
            { 
                title = value; 
            }
        }

        public void SetTitle(string newTitle)
        {
            this.title = newTitle;
        }

        public override string ToString()
        {
            return this.Title;
        }
        
        #endregion

        #region Instance Methods
        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            if (previousConnectionPoint == null || previousConnectionPoint.ConnectionPointType == ConnectionPointType.Output)
            {
                this.InputConnectionPoint.Reset(null);
            }
            base.Reset(previousConnectionPoint);
        }
        #endregion
    }
}
