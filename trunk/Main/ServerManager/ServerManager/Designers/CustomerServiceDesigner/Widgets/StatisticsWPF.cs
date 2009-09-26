using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UtnEmall.Server.EntityModel;
using LogicalLibrary;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesigner;
using UtnEmall.ServerManager.Statistics;
using System.Collections;
using UtnEmall.ServerManager;
using System.Globalization;

namespace PresentationLayer.Widgets
{
    public class StatisticsWpf : StatisticsForm, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        private string canvasPath;
        private Canvas parent;

        private Canvas myCanvas;
        /// <summary>
        /// Establece u obtiene el canvas que representa un StatisticsWpf.
        /// </summary>
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatisticsWpf(Form baseForm, Canvas parent, string summary)
            : base()
        {
            this.BaseForm = baseForm;
            this.canvasPath = "CanvasStatistics.xaml";
            this.parent = parent;

            this.MakeCanvas();
            ((TextBlock)myCanvas.FindName("textBlockComment")).Text = summary;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        public void MakeCanvas()
        {
            // Crea y agrega el canvas a la ventana
            Canvas canvas = Utilities.CanvasFromXaml(this.canvasPath);
            parent.Children.Add(canvas);

            // Cambiar el rectangulo de activacion escondido, asi cubre todo el formulario.
            FrameworkElement activationRectangle = (FrameworkElement)canvas.FindName("activationRectangle");
            IDrawAbleWpf baseFormAsIDrawableWpf = (BaseForm as IDrawAbleWpf);
            activationRectangle.Height = baseFormAsIDrawableWpf.MyCanvas.Height;
            activationRectangle.Width = baseFormAsIDrawableWpf.MyCanvas.Width;

            // Obtiene el ancho del viewerCanvas, es el maximo valor.
            Canvas viewerCanvas = (Canvas)canvas.FindName("viewerCanvas");
            double maxWidth = viewerCanvas.Width;

            // Posicion del rectangulo de activacion oculto justo encima del formulario base.
            XCoordinateRelativeToParent = BaseForm.XCoordinateRelativeToParent - (maxWidth - activationRectangle.Width) / 2;
            YCoordinateRelativeToParent = BaseForm.YCoordinateRelativeToParent;
            Canvas.SetLeft(canvas, XCoordinateRelativeToParent);
            Canvas.SetTop(canvas, YCoordinateRelativeToParent);

            this.myCanvas = canvas;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.UIElement UIElement
        {
            get
            {
                return myCanvas;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GenerateStatisticSummary(ComponentEntity component, string session)
        {
            StringBuilder resultString = new StringBuilder();

            resultString.Append(UtnEmall.ServerManager.Properties.Resources.HowManyCustomersUsedForm + ": ");
            resultString.AppendLine(Services.StatisticsAnalyzer.GetCustomerFormAccessAmount(component, session).ToString(CultureInfo.CurrentCulture));
            resultString.AppendLine();

            resultString.Append(UtnEmall.ServerManager.Properties.Resources.HowManyTimesUsedForm + ": ");
            resultString.AppendLine(Services.StatisticsAnalyzer.GetFormAccessAmount(component, session).ToString(CultureInfo.CurrentCulture));
            resultString.AppendLine();

            switch ((ComponentType)component.ComponentType)
            {
                case (ComponentType.MenuForm):
                    resultString.AppendLine(UtnEmall.ServerManager.Properties.Resources.HowManyTimesMenuWasUsed + ":");
                    foreach (ComponentEntity menuEntity in component.MenuItems)
                    {
                        FormMenuItemWpf menuItem = Utilities.ConvertEntityToFormMenuItem(menuEntity);
                        int count = Services.StatisticsAnalyzer.GetMenuItemAccessAmount(component, session);

                        resultString.AppendLine(menuItem.Text + " -> " + count);
                    }
                    break;
                case (ComponentType.ListForm):
                    List<DictionaryEntry> pairWithCount = Services.StatisticsAnalyzer.GetRegistersClickAmount(component, session);
                    List<DictionaryEntry> pairWithPercentage = Services.StatisticsAnalyzer.GetRegisterClickPercentageAmount(component, session);

                    resultString.AppendLine(UtnEmall.ServerManager.Properties.Resources.HowManyRegisterSelections + ":");

                    for (int i = 0; i < pairWithCount.Count; i++)
                    {
                        string percentage = ((double)pairWithPercentage[i].Value).ToString("P2", CultureInfo.CurrentCulture);
                        resultString.AppendLine(pairWithCount[i].Key + " -> " + pairWithCount[i].Value + "|" + percentage);
                    }

                    break;
            }

            return resultString.ToString();
        }

        #endregion
    }
}

