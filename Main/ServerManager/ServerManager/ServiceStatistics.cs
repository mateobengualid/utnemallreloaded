using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using LogicalLibrary.DataModelClasses;
using PresentationLayer.ServerDesigner;
using PresentationLayer.Widgets;

namespace UtnEmall.ServerManager.Statistics
{
    /// <summary>
    /// Lógica de interacción para ServiceStatistics.xaml
    /// </summary>
    public partial class ServiceStatistics : WindowDesigner
    {
        #region Constants, Variables and Properties

        private new StatisticServiceDocumentWpf document;

        public new StatisticServiceDocumentWpf Document
        {
            get { return document; }
        }

        #endregion

        #region Constructors

        public ServiceStatistics(ServiceEntity serviceEntity, String session)
            : base(serviceEntity, session)
        {
            InitializeComponent();

            document = new StatisticServiceDocumentWpf(this, session);
            document.ServiceEntity = serviceEntity;

            this.DataContext = document.ServiceEntity;
            this.canvasDraw.MouseMove -= new MouseEventHandler(canvasDraw_MouseMove);
            this.Loaded -= new RoutedEventHandler(base.WindowDesignerLoaded);
            this.Loaded += new RoutedEventHandler(ServiceStatistics_Loaded);

            Connection.Visibility = Visibility.Collapsed;
            Input.Visibility = Visibility.Collapsed;
            List.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            ShowData.Visibility = Visibility.Collapsed;
            labelStartForm.Visibility = Visibility.Collapsed;
            comboBoxForms.Visibility = Visibility.Collapsed;

            canvasDataModel.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Instance Methods

        void ServiceStatistics_Loaded(object sender, RoutedEventArgs e)
        {
            document.DataModel = document.LoadDataModel();
            if (document.DataModel == null)
            {
                this.disableOptions();
            }

            if (document.ServiceEntity.CustomerServiceData != null)
            {
                if (document.Load())
                {
                    document.RedrawDocument();
                }
                else
                {
                    Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.ImpossibleLoadDocument, "Warning");
                    this.disableOptions();
                }
            }
        }

        private void canvasDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (document.IsDragDropAction == true)
            {
                Point newPosition = new Point();
                newPosition.X = e.GetPosition(canvasDraw).X - document.MousePosition.X;
                newPosition.Y = e.GetPosition(canvasDraw).Y - document.MousePosition.Y;

                UIElement element = document.CurrentElement;
                Canvas.SetLeft(element, newPosition.X);
                Canvas.SetTop(element, newPosition.Y);
                document.RedrawConnections();
            }
        }

        private void btnListForm_Click(object sender, RoutedEventArgs e)
        {
            ListFormWpf listFormWPF = new ListFormWpf();
            try
            {
                listFormWPF.MakeCanvas();
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": ArgumentNullException");
            }
            document.AddWidget(listFormWPF);
        }

        private void btnShowDataForm_Click(object sender, RoutedEventArgs e)
        {
            ShowDataFormWpf showDataForm = new ShowDataFormWpf();
            try
            {
                showDataForm.MakeCanvas();
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": ArgumentNullException");
            }
            document.AddWidget(showDataForm);
        }

        private void btnMenuForm_Click(object sender, RoutedEventArgs e)
        {
            MenuFormWpf menuForm = new MenuFormWpf();
            try
            {
                menuForm.MakeCanvas();
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": ArgumentNullException");
            }
            document.AddWidget(menuForm);
        }

        private void btnEnterSingleDataForm_Click(object sender, RoutedEventArgs e)
        {
            EnterSingleDataFormWpf enterSingleDataForm = new EnterSingleDataFormWpf();
            try
            {
                enterSingleDataForm.MakeCanvas();
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ErrorMakingCanvas + ": ArgumentNullException");
            }
            document.AddWidget(enterSingleDataForm);
        }

        private void btnDefineConnection_Click(object sender, RoutedEventArgs e)
        {
            document.IsMakeConnectionAction = true;
            document.ConnectionWidgetFrom = document.ConnectionWidgetTarget = null;
            textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectPointOriginConnection;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            CustomerServiceDataEntity customerServiceDataEntity = document.Save();

            if (customerServiceDataEntity == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ImpossibleSaveService);
                return;
            }

            document.ServiceEntity.CustomerServiceData = customerServiceDataEntity;
            Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.ServiceSavedSuccessfully, UtnEmall.ServerManager.Properties.Resources.SaveSuccessfully);
            this.Result = true;
            this.Close();
        }

        private void disableOptions()
        {
            this.buttonSave.IsEnabled = false;
        }

        private void showLabelNoDataModelLoaded()
        {
            Label labelNoDataModelLoaded = new Label();
            labelNoDataModelLoaded.Content = UtnEmall.ServerManager.Properties.Resources.NoDataModelLoaded;
            labelNoDataModelLoaded.FontSize = 28;
            labelNoDataModelLoaded.Foreground = Brushes.Gray;
            Canvas.SetLeft(labelNoDataModelLoaded, canvasDraw.ActualWidth / 4);
            Canvas.SetTop(labelNoDataModelLoaded, canvasDraw.ActualHeight / 3);
            canvasDraw.Children.Add(labelNoDataModelLoaded);
        }

        #endregion
    }
}
