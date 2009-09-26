using System.Windows;
using LogicalLibrary.DataModelClasses;
using Janus.Server.EntityModel;
using System.Windows.Input;
using System.Windows.Controls;
using PresentationLayer.Widgets;
using System.IO;
using System;
using System.Windows.Media;
using Janus;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Interacción lógica para ServiceStatistics.xaml
    /// </summary>
    public partial class ServiceStatistics : Window
    {
        #region Constants, Variables and Properties

        private ServiceDocumentWpf document;
        public ServiceDocumentWpf Document
        {
            get { return document; }
        }

        private DataModel dataModel;
        public DataModel DataModel
        {
            get { return dataModel; }
            set { dataModel = value; }
        }

        private bool result;
        public bool Result
        {
            get { return result; }
        }

        #endregion

        #region Constructors

        public ServiceStatistics(ServiceEntity serviceEntity, String session)
        {
            InitializeComponent();
            document = new ServiceDocumentWpf(this, session);
            document.ServiceEntity = serviceEntity;
            this.DataContext = document.ServiceEntity;

            this.Loaded += new RoutedEventHandler(ServiceStatistics_Loaded);
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
                    Util.ShowOKMessage("No se puede cargar el documento", "Advertencia");
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
                Util.ShowErrorMessage("No se pudo crear el canvas: FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorMessage("No se pudo crear el canvas: ArgumentNullException");
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
                Util.ShowErrorMessage("No se pudo crear el canvas: FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorMessage("No se pudo crear el canvas: ArgumentNullException");
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
                Util.ShowErrorMessage("No se pudo crear el canvas: FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorMessage("No se pudo crear el canvas: ArgumentNullException");
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
                Util.ShowErrorMessage("No se pudo crear el canvas: FileNotFoundException");
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorMessage("No se pudo crear el canvas: ArgumentNullException");
            }
            document.AddWidget(enterSingleDataForm);
        }

        private void btnDefineConnection_Click(object sender, RoutedEventArgs e)
        {
            document.IsMakeConnectionAction = true;
            document.ConnectionWidgetFrom = document.ConnectionWidgetTarget = null;
            textBlockStatusBar.Text = "Select Point of Origin Connection";
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            CustomerServiceDataEntity customerServiceDataEntity = document.Save();

            if (customerServiceDataEntity == null)
            {
                Util.ShowErrorMessage("No se pudo guardar el servicio");
                return;
            }

            document.ServiceEntity.CustomerServiceData = customerServiceDataEntity;
            Util.ShowOKMessage("Servicio guardado exitosamente", "Éxito");
            this.result = true;
            this.Close();
        }

        private void disableOptions()
        {
            this.buttonSave.IsEnabled = false;
        }

        private void showLabelNoDataModelLoaded()
        {
            Label labelNoDataModelLoaded = new Label();
            labelNoDataModelLoaded.Content = "< Sin modelo de datos >";
            labelNoDataModelLoaded.FontSize = 28;
            labelNoDataModelLoaded.Foreground = Brushes.Gray;
            Canvas.SetLeft(labelNoDataModelLoaded, canvasDraw.ActualWidth / 4);
            Canvas.SetTop(labelNoDataModelLoaded, canvasDraw.ActualHeight / 3);
            canvasDraw.Children.Add(labelNoDataModelLoaded);
        }

        #endregion
    }
}
