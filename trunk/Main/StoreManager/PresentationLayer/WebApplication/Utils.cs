using System;

using UtnEmall.Server.EntityModel;
using System.ServiceModel;
using WebApplication;
using System.Drawing;

namespace SilverlightVisualDesigners
{
    public enum DesignerMode
    {
        // Asigna el modo de diseño para el diseñador visual en Silverlight.
        ServiceDesigner = 1,
        DataModelDesigner = 2,
        StatisticViewer = 3
    }
}