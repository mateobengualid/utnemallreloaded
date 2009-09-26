using System.Windows;
using LogicalLibrary.DataModelClasses;
using UtnEmall.Server.EntityModel;
using System.Windows.Input;
using System.Windows.Controls;
using PresentationLayer.Widgets;
using System.IO;
using System;
using System.Windows.Media;
using UtnEmall.ServerManager;
using LogicalLibrary.Widgets;
using LogicalLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Logica para WindowDesigner.Xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1708:IdentifiersShouldDifferByMoreThanCase")]
    public partial class WindowDesigner : Window
    {
        #region Constants, Variables and Properties

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected ServiceDocumentWpf document;
        private int formNumber;

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
            set { result = value; }
            get { return result; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// contructor.
        /// </summary>
        /// <param name="serviceEntity">Servicio a editar</param>
        /// <param name="session">Identificador de sesion</param>
        public WindowDesigner(ServiceEntity serviceEntity, String session)
        {
            InitializeComponent();
            document = new ServiceDocumentWpf(this, session);
            document.ServiceEntity = serviceEntity;
            this.DataContext = document.ServiceEntity;

            this.Loaded += new RoutedEventHandler(WindowDesignerLoaded);
            this.document.DataSourceDeleted += new EventHandler(document_DataSourceDeleted);
            this.document.FormDeleted += new EventHandler(document_FormDeleted);
            canvasDrawPrincipal.MouseMove += new MouseEventHandler(CanvasDraw_MouseMove);
            textBoxServiceName.Text = serviceEntity.Name;
        }
                
        #endregion

        #region Instance Methods


        void document_FormDeleted(object sender, EventArgs e)
        {
            this.comboBoxForms.Items.Remove(sender);
            this.comboBoxForms.UpdateLayout();
        }

        void document_DataSourceDeleted(object sender, EventArgs e)
        {
            DataSource dataSource = sender as DataSource;
            if (dataSource == null)
            {
                throw new ArgumentException("sender must be a DataSource","sender");
            }
            this.comboBoxDataModels.Items.Add(dataSource.RelatedTable);
        }

        protected void WindowDesignerLoaded(object sender, RoutedEventArgs e)
        {
            document.DataModel = document.LoadDataModel();
            if (document.DataModel == null)
            {
                this.disableOptions();
                this.showLabelNoDataModelLoaded();
            }
            loadComboBoxDataModel(document.DataModel);

            if (document.ServiceEntity.CustomerServiceData != null)
            {
                if (document.Load())
                {
                    document.RedrawDocument(true);
                    UpdateComboBoxDataModels();
                }
                else
                {
                    Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.LoadServiceDesignFailed, "Warning");
                    this.disableOptions();
                }
            }
            loadComboBoxStartForm();
            formNumber = document.Components.Count + 1;
        }
                
        private void UpdateComboBoxDataModels()
        {
            foreach (Widget widget in document.Components)
            {
                if (widget is DataSource)
                {
                    this.comboBoxDataModels.Items.Remove(widget.OutputDataContext);
                }
            }
        }

        private void CanvasDraw_MouseMove(object sender, MouseEventArgs e)
        {
            this.comboBoxForms.Items.Refresh();
            this.comboBoxForms.UpdateLayout();

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

        private void ButtonAddWidgetDataModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogicalLibrary.DataModelClasses.Table table = comboBoxDataModels.SelectedItem as LogicalLibrary.DataModelClasses.Table;
                if (table != null)
                {
                    DataSourceWpf dataSourceWPF = new DataSourceWpf(table);
                    dataSourceWPF.MakeCanvas();
                    
                    document.AddWidget(dataSourceWPF);

                    comboBoxDataModels.Items.RemoveAt(comboBoxDataModels.SelectedIndex);
                    comboBoxDataModels.SelectedIndex = 0;
                }
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                document.StartWidget = comboBoxForms.SelectedItem as Widget;
                if (document.StartWidget == null)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoStartForm, "");
                    return;
                }

                Collection<Error> listOfDesignerError = document.CheckDesignerLogic();
                if (listOfDesignerError.Count > 0)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ServiceDesignLogicIncorrect, listOfDesignerError);
                    return;
                }
                
                if (!document.CheckValidPathForms())
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InvalidServicePath);
                    return;
                }
                CustomerServiceDataEntity customerServiceDataEntity = document.Save();
                
                if (customerServiceDataEntity == null)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.SaveServiceError);
                    return;
                }

                document.ServiceEntity.CustomerServiceData = customerServiceDataEntity;
                this.result = true;
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void loadComboBoxStartForm()
        {
            if (Document != null)
            {
                foreach (Widget widget in Document.Components)
                {
                    if (widget is ListFormWpf || widget is MenuFormWpf)
                    {
                        comboBoxForms.Items.Add(widget);
                        if (Document.StartWidget == widget)
                        {
                            comboBoxForms.SelectedItem = widget;
                        }
                    }
                }
                
            }
        }

        private void loadComboBoxDataModel(DataModel recivedDataModel)
        {
            if (recivedDataModel != null)
            {
                foreach (LogicalLibrary.DataModelClasses.Table table in recivedDataModel.Tables)
                {
                    comboBoxDataModels.Items.Add(table);
                }
                comboBoxDataModels.SelectedIndex = 0;
            }
        }

        private void disableOptions()
        {
            List.Enabled = false;
            ShowData.Enabled = false;
            Input.Enabled = false;
            Menu.Enabled = false;
            Connection.Enabled = false;

            this.buttonSave.IsEnabled = false;
            this.buttonAddWidgetDataModel.IsEnabled = false;
            this.comboBoxDataModels.IsEnabled = false;
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

        private void OnListClicked(object sender, EventArgs e)
        {
            try
            {
                ListFormWpf listFormWPF = new ListFormWpf(formNumber);
                formNumber++;
                
                listFormWPF.MakeCanvas();
                comboBoxForms.Items.Add(listFormWPF);
                document.AddWidget(listFormWPF);
                Canvas.SetLeft(listFormWPF.UIElement, 300);
                Canvas.SetTop(listFormWPF.UIElement, 300);
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void OnShowDataClicked(object sender, EventArgs e)
        {
            try
            {
                ShowDataFormWpf showDataForm = new ShowDataFormWpf(formNumber);
                formNumber++;
                
                showDataForm.MakeCanvas();
                
                document.AddWidget(showDataForm);

                Canvas.SetLeft(showDataForm.UIElement, 300);
                Canvas.SetTop(showDataForm.UIElement, 300);
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void OnMenuClicked(object sender, EventArgs e)
        {
            try
            {
                MenuFormWpf menuForm = new MenuFormWpf(formNumber);
                formNumber++;
                menuForm.MakeCanvas();
               
                comboBoxForms.Items.Add(menuForm);
                document.AddWidget(menuForm);
                Canvas.SetLeft(menuForm.UIElement, 300);
                Canvas.SetTop(menuForm.UIElement, 300);
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void OnInputClicked(object sender, EventArgs e)
        {
            try
            {
                EnterSingleDataFormWpf enterSingleDataForm = new EnterSingleDataFormWpf(formNumber);
                formNumber++;
                
                enterSingleDataForm.MakeCanvas();
                
                document.AddWidget(enterSingleDataForm);
                Canvas.SetLeft(enterSingleDataForm.UIElement, 300);
                Canvas.SetTop(enterSingleDataForm.UIElement, 300);
            }
            catch (FileNotFoundException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (ArgumentNullException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.InternalError);
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void OnConnectionClicked(object sender, EventArgs e)
        {
            try
            {
                document.IsMakeConnectionAction = true;
                document.ConnectionWidgetFrom = document.ConnectionWidgetTarget = null;
                textBlockStatusBar.Text = UtnEmall.ServerManager.Properties.Resources.SelectFormOriginConnection;
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void comboBoxForms_MouseEnter(object sender, MouseEventArgs e)
        {
            this.comboBoxForms.Items.Refresh();
            this.comboBoxForms.UpdateLayout();
        }
        
        #endregion
    }
}
