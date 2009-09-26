using System;
using System.Windows;
using System.Windows.Controls;
using Janus;
using Janus.Server.BusinessLogic;
using LogicalLibrary;
using PresentationLayer;
using PresentationLayer.ServerDesigner;

namespace ServerManager.Statistics
{
    public class StatisticServiceDocumentWPF : ServiceDocumentWpf
    {
        public StatisticServiceDocumentWPF(Window window, string session)
            : base(window, session)
        {
        }

        public override void RedrawDocument()
        {
            DrawArea.Children.Clear();
            foreach (Widget widget in Components)
            {
                IDrawAbleWpf drawableWPF = widget as IDrawAbleWpf;
                DrawArea.Children.Add(drawableWPF.GetUIElement());
                drawableWPF.MyCanvas.ContextMenu = null;

                Canvas.SetLeft(drawableWPF.GetUIElement(), widget.XCoordinateRelativeToParent);
                Canvas.SetTop(drawableWPF.GetUIElement(), widget.YCoordinateRelativeToParent);
            }
            DrawArea.UpdateLayout();
            RedrawConnections();
        }

        public override bool Load()
        {
            try
            {
                if (this.ServiceEntity.CustomerServiceData != null)
                {
                    PresentationLayer.Utilities.ConvertEntityToServiceModelWithStatistics(this.ServiceEntity.CustomerServiceData, this, session);
                    return true;
                }
                throw new JanusBusinessLogicException("CustomerServiceData es null");
            }
            catch (InvalidCastException)
            {
                Util.ShowErrorDialog("No se pudo cargar Load Service Design");
            }
            return false;
        }
    }
}