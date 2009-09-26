using System;
using System.Windows;
using System.Windows.Controls;
using UtnEmall.ServerManager;
using UtnEmall.Server.BusinessLogic;
using LogicalLibrary;
using PresentationLayer;
using PresentationLayer.ServerDesigner;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager.Statistics
{
    public class StatisticServiceDocumentWpf : ServiceDocumentWpf
    {
        public StatisticServiceDocumentWpf(Window window, string session)
            : base(window, session)
        {
        }

        public override void RedrawDocument()
        {
            DrawArea.Children.Clear();

            foreach (Widget widget in Components)
            {
                // Agregar el elemento al área de dibujo, y borrar su menú de contexto
                IDrawAbleWpf drawableWPF = widget as IDrawAbleWpf;
                DrawArea.Children.Add(drawableWPF.UIElement);
                drawableWPF.MyCanvas.ContextMenu = null;

                // Reposicionar el lienzo
                Canvas.SetLeft(drawableWPF.UIElement, widget.XCoordinateRelativeToParent);
                Canvas.SetTop(drawableWPF.UIElement, widget.YCoordinateRelativeToParent);
            }

            // Actualizar y redibujar el área de dibujo
            DrawArea.UpdateLayout();
            RedrawConnections();
        }
        
        public override bool Load()
        {
            try
            {
                if (this.ServiceEntity.CustomerServiceData != null)
                {
                    // Reemplazar la llamada con una llamada para convertir a un modelo de servicio con formularios de estadísticas.
                    PresentationLayer.Utilities.ConvertEntityToServiceModelWithStatistics(this.ServiceEntity.CustomerServiceData, this, Session);
                    return true;
                }
                throw new UtnEmallBusinessLogicException(Resources.NoServiceDesigned);
            }
            catch (InvalidCastException)
            {
                Util.ShowErrorDialog(Resources.LoadServiceDesignFailed);
            }
            return false;
        }
    }
}