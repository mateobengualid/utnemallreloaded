using System.Windows;
using PresentationLayer.Widgets;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using PresentationLayer.ServerDesignerClasses;
using System.Windows.Media;
using LogicalLibrary.DataModelClasses;
using System.ComponentModel;
using LogicalLibrary.ServerDesignerClasses;
using UtnEmall.ServerManager;
using System;
using System.Windows.Shapes;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Logica para WindowListFrom.xaml
    /// </summary>
    public partial class WindowListForm : Window
    {
        #region Constants, Variables and Properties

        ListFormWpf listformWPF;
        ServiceDocumentWpf serviceDocument;
        TemplateListFormDocumentWpf templateListFormDocument;
        List<LogicalLibrary.Component> tempComponents;
        private TemplateListItemWpf templateListItemWpfSelected;
        private Table previousDataContext;

        #endregion

        #region Constructors
        /// <summary>
        ///constructor
        /// </summary>
        /// <param name="form">El formulario de lista a editar</param>
        /// <param name="serviceDocument">El documento de servicio que contiene los formularios y las relaciones</param>
        public WindowListForm(ListFormWpf form, ServiceDocumentWpf serviceDocument)
        {
            InitializeComponent();
            listBoxFields.SelectedIndex = 0;

            this.Loaded += new RoutedEventHandler(WindowListForm_Loaded);
            canvasDraw.ClipToBounds = true;
            this.canvasDraw.MouseMove += new MouseEventHandler(canvasDraw_MouseMove);

            radioRegister.Checked += new RoutedEventHandler(radioRegister_Checked);
            radioList.Checked += new RoutedEventHandler(radioList_Checked);

            this.listformWPF = form;
            this.previousDataContext = this.listformWPF.OutputDataContext;
            templateListFormDocument = listformWPF.TemplateListFormDocument as TemplateListFormDocumentWpf;

            tempComponents = new List<LogicalLibrary.Component>();
            foreach (TemplateListItemWpf component in this.listformWPF.TemplateListFormDocument.Components)
            {
                tempComponents.Add(component.Clone());
            }

            this.templateListFormDocument.CanvasDraw = this.canvasDraw;
            this.serviceDocument = serviceDocument;
        }

        #endregion

        #region Instance Methods
        
        /// <summary>
        ///Este metodo carga los combos e inicializa el diseñador visual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WindowListForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxTitle.Text = listformWPF.Title;
            this.loadListBoxFields();
            this.loadRealtedTables();
            this.loadComboFontName();
            this.loadComboFontSize();
            this.loadListBoxFontColors();
            loadVisualizationDesigner();
            this.loadOutputDataContext();
        }

        void radioList_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxOutput.IsEnabled = true;
        }

        void radioRegister_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxOutput.IsEnabled = false;
        }

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

        private void loadOutputDataContext()
        {            
            if (previousDataContext == null)
            {
                radioRegister.IsChecked = true;
            }
            else if (previousDataContext.Name == listformWPF.InputDataContext.Name)
            {
                radioRegister.IsChecked = true;
            }
            else
            {
                this.comboBoxOutput.SelectedItem = previousDataContext;
                radioList.IsChecked = true;
            }
        }

        private void loadListBoxFields()
        {
            this.listBoxFields.ItemsSource = listformWPF.InputDataContext.Fields;
        }

        private void loadRealtedTables()
        {
            List<LogicalLibrary.DataModelClasses.Table> relatedTables = serviceDocument.DataModel.GetRelatedTables(listformWPF.InputDataContext);
            foreach (LogicalLibrary.DataModelClasses.Table table in relatedTables)
            {
                comboBoxOutput.Items.Add(table);
            }
            comboBoxOutput.SelectedIndex = 0;
            if (comboBoxOutput.Items.Count == 0)
            {
                comboBoxOutput.IsEnabled = false;
                radioList.IsEnabled = false;
                radioRegister.IsChecked = true;
            }
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
            comboBoxFontSize.SelectedIndex = 1;
        }

        private void loadListBoxFontColors()
        {
            foreach (SolidColorBrush brush in LogicalLibrary.Utilities.ColorFonts)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Background = brush;
                listBoxItem.Content = "";
                listBoxFontColor.Items.Add(listBoxItem);
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

        void templateListItem_ComponentPreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        /// <summary>
        /// Metodo llamado cuando se presiona el boton cancelar. Cierra la ventana sin guardar los cambios
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
            else if(templateListFormDocument.IsResizeAction)
            {
                TemplateListItemWpf element = templateListFormDocument.CurrentComponent as TemplateListItemWpf;
                double width = e.GetPosition(canvasDraw).X - Canvas.GetLeft(element.MyCanvas);
                double height = e.GetPosition(canvasDraw).Y - Canvas.GetTop(element.MyCanvas);
                element.LoadElementSize(width, height);
            }
        }

        /// <summary>
        /// Metodo llamado cuando se presiona el boton guardar. Guarda los cambios realizados sobre el formulario de lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty( textBoxTitle.Text ))
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.EmptyText);
                    return;
                }

                if (tempComponents.Count == 0)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoDisplayedField);
                    return;
                }

                if (radioRegister.IsChecked == true)
                {
                    listformWPF.OutputDataContext = listformWPF.InputDataContext;
                }
                else
                {
                    listformWPF.OutputDataContext = comboBoxOutput.SelectedItem as LogicalLibrary.DataModelClasses.Table;
                }
                
                if (this.previousDataContext != null && String.Compare(this.previousDataContext.Name, this.listformWPF.OutputDataContext.Name, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    bool result = Util.ShowConfirmDialog(UtnEmall.ServerManager.Properties.Resources.ResetConnectionAndForm, UtnEmall.ServerManager.Properties.Resources.OutputDataContextChanged);
                    if (result)
                    {
                        this.listformWPF.OutputConnectionPoint.Reset(null);
                        this.serviceDocument.RedrawDocument();
                    }
                    else
                    {
                        return;
                    }
                }

                
                listformWPF.TemplateListFormDocument.ClearAllComponents();

                foreach (TemplateListItemWpf item in tempComponents)
                {
                    listformWPF.TemplateListFormDocument.AddTemplateListItem(item);
                }

                templateListFormDocument.SaveComponentPositions();
                this.listformWPF.Title = this.textBoxTitle.Text;
                listformWPF.ChangeTitle();
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
