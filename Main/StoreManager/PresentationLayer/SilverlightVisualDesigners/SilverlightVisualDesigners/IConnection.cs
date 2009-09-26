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
using LogicalLibrary.ServerDesignerClasses;

namespace SilverlightVisualDesigners
{
    public interface IConnection:IDraw,IComponent
    {
        Point VisualInputPoint { get; }
        Point VisualOutputPoint { get; }

        ConnectionPoint InputConnectionPoint { get; }
        ConnectionPoint OutputConnectionPoint { get; }

        event MouseMenuWidgetClickEventHandler Deleted;
        event MouseMenuWidgetClickEventHandler Configure;
    }
}
