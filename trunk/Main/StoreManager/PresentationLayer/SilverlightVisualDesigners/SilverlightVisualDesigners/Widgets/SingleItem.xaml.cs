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
	public partial class SingleItem : UserControl,IDrawable
	{
        private TemplateListItem templateListItem;
        public TemplateListItem TemplateListItem 
        {
            get { return templateListItem; }
            set { templateListItem = value; }
        }

		public SingleItem(Field fieldAsociated,DataType dataType)
		{
			// Inicializar variables.
			InitializeComponent();
            templateListItem = new TemplateListItem(fieldAsociated, dataType);
            this.textBlockName.Text = fieldAsociated.Name + ":" + dataType;
		}

        public SingleItem(TemplateListItem templateListItem)
        {
            this.templateListItem = templateListItem;
            this.textBlockName.Text = templateListItem.FieldAssociated.Name + ":" + templateListItem.DataType;
        }

        #region IDrawable Members

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

        public void OnDrag()
        {
            if (Drag != null)
            {
                Drag(this, new MouseEventArgs());
            }
        }

        public event MouseDoubleClickEventHandler DoubleClick;

        public event MouseEventHandler Drag;

        #endregion
    }
}