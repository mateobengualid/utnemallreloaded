using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;

namespace LogicalLibrary
{
    public class EnterSingleDataForm : Form
    {
        #region Constants, Variables and Properties

        private DataType dataType;
        public DataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        private String descriptiveText;
        public String DescriptiveText
        {
            get { return descriptiveText; }
            set { descriptiveText = value; }
        }

        private string dataName;
        public string DataName
        {
            get { return dataName; }
            set { dataName = value; }
        }

        #endregion

        #region Instance Methods

        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            base.Reset(previousConnectionPoint);
            this.OutputConnectionPoint.Reset(null);
            this.DescriptiveText = "";
            this.DataName = "";
        }

        #endregion
    }
}
