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

namespace SilverlightVisualDesigners
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Drawable")]
    public interface IDraw
    {
        double XCoordinateRelativeToParent { get; set; }
        double YCoordinateRelativeToParent { get; set; }

        double XFactorCoordinateRelativeToParent { get; set; }
        double YFactorCoordinateRelativeToParent { get; set; }

        double WidthFactor { get;  set; }
        double HeightFactor { get; set; }

        void OnDrag();

        event MouseButtonEventHandler MouseLeftButtonDown;
        event MouseButtonEventHandler MouseLeftButtonUp;
        event MouseEventHandler MouseMove;
        event EventHandler Drag;
    }
}
