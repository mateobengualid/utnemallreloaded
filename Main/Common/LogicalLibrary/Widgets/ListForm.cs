using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;

namespace LogicalLibrary
{
    public class ListForm : Form
    {
        #region Constants, Variables and Properties

        private TemplateListFormDocument templateListFormDocument;
        public TemplateListFormDocument TemplateListFormDocument
        {
            get { return templateListFormDocument; }
            set { templateListFormDocument = value; }
        }

        #endregion

        #region Instance Methods

        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            base.Reset(previousConnectionPoint);
            this.OutputConnectionPoint.Reset(null);
            this.TemplateListFormDocument.Reset();
        }

        #endregion
    }
}
