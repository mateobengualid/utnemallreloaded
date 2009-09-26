using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PresentationLayer.ServerDesignerClasses;

namespace SilverlightVisualDesigners
{
    public class MouseDoubleClickEventArgs : EventArgs
    {
        WidgetType widgetType;
        public WidgetType WidgetType
        {
            get { return widgetType; }
            set { widgetType = value; }
        }

        public MouseDoubleClickEventArgs(WidgetType value)
        {
            widgetType = value;
        }
    }

    public delegate void MouseMenuWidgetClickEventHandler(object sender, MouseDoubleClickEventArgs e);
}
