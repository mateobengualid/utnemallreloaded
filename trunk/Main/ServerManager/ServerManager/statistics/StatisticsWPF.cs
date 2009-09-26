using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Janus.Server.EntityModel;
using LogicalLibrary;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesigner;
using ServerManager.Statistics;
using System.Collections;
using Janus;

namespace PresentationLayer.Widgets
{
    public class StatisticsWpf : Widget, IDrawAbleWpf
    {
        #region Instance Variables and Properties

        private string canvasPath;
        private ComponentEntity formEntity;
        private IDrawAbleWpf form;
        private Canvas parent;

        private Canvas myCanvas;
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="formEntity">ComponentEntity</param>
        /// <param name="form">Formulario involucrado</param>
        /// <param name="session">Identificador de sesión</param>
        /// <param name="parent">Canvas padre</param>
        public StatisticsWpf(ComponentEntity formEntity, IDrawAbleWpf form, string session, Canvas parent)
            : base()
        {
            this.form = form;
            this.formEntity = formEntity;
            this.canvasPath = "CanvasStatistics.xaml";
            this.parent = parent;

            this.MakeCanvas();
            ((TextBlock)myCanvas.FindName("textBlockComment")).Text = GenerateStatisticSummary(formEntity, session);
        }

        #endregion

        #region Instance Methods

        public void MakeCanvas()
        {
            // Crea y agrega el canvas a la ventana
            Canvas canvas = Utilities.CanvasFromXaml(this.canvasPath);
            parent.Children.Add(canvas);

            // Si el formulario es un menu, tiene una variable de altura y el formulario debe alargarse.
            if ((form is MenuFormWpf) && (canvas.Height - form.MyCanvas.Height < 25))
            {
                double difference = 25 + form.MyCanvas.Height - canvas.Height;
                canvas.Height += difference;
                (canvas.FindName("rectangle") as FrameworkElement).Height += difference;
                (canvas.FindName("scrollViewer") as FrameworkElement).Height += difference;
            }

            XCoordinateRelativeToParent = (form as Form).XCoordinateRelativeToParent + (form.MyCanvas.Width - canvas.Width) / 2;
            YCoordinateRelativeToParent = (form as Form).YCoordinateRelativeToParent + (form.MyCanvas.Height - canvas.Height) / 2;
            Canvas.SetLeft(canvas, XCoordinateRelativeToParent);
            Canvas.SetTop(canvas, YCoordinateRelativeToParent);

            this.myCanvas = canvas;
        }

        public System.Windows.UIElement GetUIElement()
        {
            return myCanvas;
        }

        public static string GenerateStatisticSummary(ComponentEntity component, string session)
        {
            StringBuilder resultString = new StringBuilder();

            resultString.AppendLine(ServerManager.Properties.Resources.HowManyCustomersUsedForm + ":");
            resultString.AppendLine(Services.StatisticsAnalyzer.GetCustomerFormAccessAmount(component, session).ToString());
            resultString.AppendLine(ServerManager.Properties.Resources.HowManyTimesUsedForm + ":");
            resultString.AppendLine(Services.StatisticsAnalyzer.GetFormAccessAmount(component, session).ToString());

            switch ((ComponentType)component.ComponentType)
            {
                case (ComponentType.MenuForm):
                    resultString.AppendLine(ServerManager.Properties.Resources.HowManyTimesMenuWasUsed + ":");
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
                    resultString.AppendLine(ServerManager.Properties.Resources.HowManyRegisterSelections + ":");

                    for (int i = 0; i < pairWithCount.Count; i++)
                    {
                        double percentage = (double)pairWithPercentage[i].Value;                        

                        resultString.AppendLine(pairWithCount[i].Key + " -> " + pairWithCount[i].Value + "|" + percentage.ToString("P2"));
                    }

                    break;
            }

            return resultString.ToString();
        }

        #endregion
    }
}

