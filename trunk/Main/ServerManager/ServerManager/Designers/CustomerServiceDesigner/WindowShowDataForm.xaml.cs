using System.Windows;
using PresentationLayer.Widgets;
using LogicalLibrary;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;
using System.Windows.Media;
using LogicalLibrary.DataModelClasses;
using System.ComponentModel;
using UtnEmall.ServerManager;
using System;
using System.Windows.Shapes;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Logica para WindowShowDataForm.xaml
    /// </summary>
    public partial class WindowShowDataForm : Window
    {
        #region Constants, Variables and Properties

        ShowDataFormWpf showDataFormWPF;
        TemplateListFormDocumentWpf templateListFormDocument;
        List<LogicalLibrary.Component> tempComponents;
        ServiceDocumentWpf serviceDocument;
        private TemplateListItemWpf templateListItemWpfSelected;
        private Table previousDataContext;

        #endregion

        #region Constructors

        /// <summary>
        /// contructor.
        /// </summary>
        /// <param name="showDataFormWpf">El formulario show data form a editar</param>
        /// <param name="serviceDocument">El documento de servicio que contiene los formularios y las relaciones</param>
        public WindowShowDataForm(ShowDataFormWpf showDataFormWpf, ServiceDocumentWpf serviceDocument)
        {
            InitializeComponent();
            this.showDataFormWPF = showDataFormWpf;
            this.DataContext = this.showDataFormWPF;
            templateListFormDocument = showDataFormWpf.TemplateListFormDocument as TemplateListFormDocumentWpf;
            this.templateListFormDocument.CanvasDraw = this.canvasDraw;
            listBoxFields.SelectedIndex = 0;
            canvasDraw.ClipToBounds = true;
            tempComponents = new List<LogicalLibrary.Component>();
            foreach (TemplateListItemWpf component in this.showDataFormWPF.TemplateListFormDocument.Components)
            {
                tempComponents.Add(component.Clone());
            }

            this.canvasDraw.MouseMove += new MouseEventHandler(canvasDraw_MouseMove);
            this.previousDataContext = showDataFormWpf.OutputDataContext;
            this.serviceDocument = serviceDocument;
            this.Loaded += new RoutedEventHandler(WindowListForm_Loaded);
        }

        #endregion

        #region Instance Methods

        private void loadVisualizationDesigner()
        {
            foreach (LogicalLibrary.Component component in tempComponents)
            {
                TemplateListItemWpf templateListItemWPF = component as TemplateListItemWpf;
                templateListItemWPF.Rectangle.Width = component.Width;
                templateListItemWPF.Rectangle.Height = component.Height;
                templateListItemWPF.MakeCanvas();
                templateListItemWPF.ComponentPreviewMouseDown += new MouseButtonEventHandler(templateListItem_ComponentPreviewMouseDown);
                templateListFormDocument.AttachTemplateListItemEvents(templateListItemWPF);
                templateListFormDocument.AddCanvasToCanvasDraw(templateListItemWPF);
            }
        }

        private void loadListBoxFields()
        {
            this.listBoxFields.ItemsSource = showDataFormWPF.InputDataContext.Fields;
        }

        private void loadComboFontName()
        {
            for (int i = 1; i < (int)FontName.LastIndex; i++)
            {
                comboBoxFont.Items.Add((FontName)i);
            }
            comboBoxFont.SelectedIndex = 0;
        }

        private void loadComboFontSize()
        {
            for (int i = 1; i < (int)PresentationLayer.ServerDesignerClasses.FontSize.LastIndex; i++)
            {
                comboBoxFontSize.Items.Add((PresentationLayer.ServerDesignerClasses.FontSize)i);
            }
            comboBoxFont.SelectedIndex = 1;
        }

        private void loadListBoxFontColors()
        {
            foreach (Brush brush in LogicalLibrary.Utilities.ColorFonts)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Background = brush;
                listBoxItem.Content = "";
                listBoxFontColor.Items.Add(listBoxItem);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvasDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (templateListFormDocument.IsDragDropAction)
            {
                Point newPosition = new Point();
                newPosition.X = e.GetPosition(canvasDraw).X - templateListFormDocument.MousePosition.X;
                newPosition.Y = e.GetPosition(canvasDraw).Y - templateListFormDocument.MousePosition.Y;
                UIElement element = templateListFormDocument.CurrentElement;
                if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.Y > canvasDraw.ActualHeight || newPosition.X > canvasDraw.Width)
                {
                    return;
                }
                Canvas.SetLeft(element, newPosition.X);
                Canvas.SetTop(element, newPosition.Y);
            }
            else if (templateListFormDocument.IsResizeAction)
            {
                TemplateListItemWpf element = templateListFormDocument.CurrentComponent as TemplateListItemWpf;
                double width = e.GetPosition(canvasDraw).X - Canvas.GetLeft(element.MyCanvas);
                double height = e.GetPosition(canvasDraw).Y - Canvas.GetTop(element.MyCanvas);
                element.LoadElementSize(width, height);

            }
        }

        /// <summary>
        /// Metodo llamado cuando se presiona el boton cancelar. Se cierra la ventana sin cambios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.canvasDraw.Children.Clear();
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        /// <summary>
        /// Metodo llamado cuando se presiona guardar. Se guardan los cambios y se cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tempComponents.Count == 0)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoDisplayedField);
                    return;
                }

                showDataFormWPF.OutputDataContext = showDataFormWPF.InputDataContext;
                this.showDataFormWPF.Title = this.textBoxTitle.Text;
                showDataFormWPF.TemplateListFormDocument.ClearAllComponents();

                foreach (TemplateListItemWpf item in tempComponents)
                {
                    showDataFormWPF.TemplateListFormDocument.AddTemplateListItem(item);
                }

                templateListFormDocument.SaveComponentPositions();
                showDataFormWPF.ChangeTitle();
                serviceDocument.RedrawDocument();

                this.Close();
            }
            catch (NullReferenceException error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        /// <summary>
        /// Funcion llamada cuando el boton agregar es presionado. Este metodo agrega un template list item a la lista de items a mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Field selectedField = listBoxFields.SelectedItem as Field;
                TemplateListItemWpf templateListItem = new TemplateListItemWpf(selectedField, FontName.Arial, PresentationLayer.ServerDesignerClasses.FontSize.Medium, selectedField.DataType);
                templateListItem.MakeCanvas();
                templateListItem.ComponentPreviewMouseDown += new MouseButtonEventHandler(templateListItem_ComponentPreviewMouseDown);
                templateListFormDocument.AttachTemplateListItemEvents(templateListItem);
                tempComponents.Add(templateListItem);
                templateListFormDocument.AddCanvasToCanvasDraw(templateListItem);
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void templateListItem_ComponentPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.templateListItemWpfSelected != null)
            {
                this.templateListItemWpfSelected.Border = Brushes.Black;
            }
            this.templateListItemWpfSelected = sender as TemplateListItemWpf;
            templateListItemWpfSelected.Border = Brushes.Red;
            this.CanvasColorSelected.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(templateListItemWpfSelected.FontColor));
            this.DataContext = templateListItemWpfSelected;
        }

        private void WindowListForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxTitle.Text = showDataFormWPF.Title;
            loadVisualizationDesigner();
            this.loadListBoxFields();
            this.loadComboFontName();
            this.loadComboFontSize();
            this.loadListBoxFontColors();
        }

        private void listBoxFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.CanvasColorSelected.Background = (listBoxFontColor.SelectedItem as ListBoxItem).Background;
                TypeConverter typeConverter = new TypeConverter();
                if (templateListItemWpfSelected != null)
                {
                    if (templateListItemWpfSelected.DataType == DataType.Image)
                    {
                        return;
                    }
                    templateListItemWpfSelected.FontColor = typeConverter.ConvertToInvariantString((CanvasColorSelected.Background as SolidColorBrush).Color.ToString());
                    templateListItemWpfSelected.ChangeTextStyle();
                }
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        #endregion

        private void changeTextStyle(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (templateListItemWpfSelected != null)
                {
                    templateListItemWpfSelected.ChangeTextStyle();
                }
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        /// <summary>
        /// El metodo elimina un template list item de la lista de items visibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (templateListItemWpfSelected != null)
                {
                    tempComponents.Remove(templateListItemWpfSelected);
                    templateListFormDocument.RemoveCanvasToCanvasDraw(templateListItemWpfSelected);
                }
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void changeTextStyle(object sender, RoutedEventArgs e)
        {
            try
            {
                if (templateListItemWpfSelected != null)
                {
                    templateListItemWpfSelected.ChangeTextStyle();
                }
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }
    }
}
