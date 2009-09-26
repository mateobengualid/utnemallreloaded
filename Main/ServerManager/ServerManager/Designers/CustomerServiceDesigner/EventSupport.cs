

using System;
using LogicalLibrary.ServerDesignerClasses;
namespace VisualDesignerPresentationLayer
{
    public class ConnectionPointClickEventArgs : EventArgs
    {
        #region Constants, Variables and Properties

        ConnectionPoint connectionPoint;
        public ConnectionPoint ConnectionPoint
        {
            get { return connectionPoint; }
            set { connectionPoint = value; }
        }

        #endregion

        #region Instance Methods

        public ConnectionPointClickEventArgs(ConnectionPoint value)
        {
            connectionPoint = value;
        }

        #endregion
    }

    public class ItemClickEventArgs : EventArgs
    {
        #region Constants, Variables and Properties

        TemplateListItem templateListItem;
        public TemplateListItem TemplateListItem
        {
            get { return templateListItem; }
            set { templateListItem = value; }
        }

        #endregion

        #region Instance Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ItemClickEventArgs(TemplateListItem value)
        {
            templateListItem = value;
        }

        #endregion
    }

}
