

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverlightVisualDesigners.EditControls;
using LogicalLibrary;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.ServerDesignerClasses;
using LogicalLibrary.DataModelClasses;
using System.Collections.Generic;
using System.ComponentModel;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un UserControl para editar los datos de un artefacto ListForm.
    /// </summary>
	public partial class EditListFormControl : UserControl, IWindow
	{
        private ListFormSilverlight listFormSilverlight;
        private ListForm listForm;
        private List<TextField> tempSingleItems;
        private DataModel dataModel;
        
        /// <summary>
        /// Guarda el ítem seleccionado en ListDesigner.
        /// </summary>
        private IDraw currentElement;

        /// <summary>
        /// Crea un EditListFormControl basado en un ListFormSilverlight.
        /// </summary>
        /// <param name="listFormSilverlight">listFormSilverlight usado para crear.</param>
        /// <param name="dataModel">Modelo de datos usado para completar la interfaz.</param>
		public EditListFormControl(ListFormSilverlight listFormSilverlight,DataModel dataModel)
		{
			// Inicializar variables.
			InitializeComponent();
            this.dataModel = dataModel;
            this.listFormSilverlight = listFormSilverlight;
            this.listForm = listFormSilverlight.ListForm;
            
            // Una lista usada para guardar la configuración de ítems mientras el
            // usuario configura la lista, esto no se actualiza en el momento, porque
            // el usuario puede cancelar la edición.
            this.tempSingleItems = new List<TextField>();
            
            this.canvasDraw.MouseLeftButtonUp += new MouseButtonEventHandler(canvasDraw_MouseLeftButtonUp);
		}

        void canvasDraw_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DragAndDropCanvas.IsDragDropAction = false;
            DragAndDropCanvas.IsResizeAction = false;
        }

        #region IWindow Members

        public event EventHandler Closed;

        #endregion

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxTitle.Text))
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.TitleFormEmpty, this.gridLayautRoot);
                    return;
                }
                if (tempSingleItems.Count == 0)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.NoListItems, this.gridLayautRoot);
                    return;
                }
                SaveInformationToWidget();
                if (Closed != null)
                {
                    Closed(this, e);
                }
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }

        /// <summary>
        /// Establece los datos ingresados en el ListFormControl, en el ListFormSilverlight.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void SaveInformationToWidget()
        {
            listFormSilverlight.ChangeTitle(textBoxTitle.Text.Trim());

            listForm.TemplateListFormDocument.Components.Clear();
            foreach (TextField textField in tempSingleItems)
            {
                listForm.TemplateListFormDocument.AddTemplateListItem(textField.TemplateListItem);

                // Guarda las coordenadas y factores posición para cada TextField, y
                // cachea ambas asignaciones de casteo.
                IDraw selectedItemAsIDraw = textField as IDraw;

                selectedItemAsIDraw.XCoordinateRelativeToParent = Canvas.GetLeft(textField);
                selectedItemAsIDraw.YCoordinateRelativeToParent = Canvas.GetTop(textField);

                // Asigna XFactorCoordinateRelativeToParent
                // and YFactorCoordinateRelativeToParent.
                selectedItemAsIDraw.XFactorCoordinateRelativeToParent = selectedItemAsIDraw.XCoordinateRelativeToParent / canvasDraw.ActualWidth;
                selectedItemAsIDraw.YFactorCoordinateRelativeToParent = selectedItemAsIDraw.YCoordinateRelativeToParent / canvasDraw.ActualHeight;

                selectedItemAsIDraw.HeightFactor = (textField.ActualHeight / canvasDraw.ActualHeight);
                selectedItemAsIDraw.WidthFactor = (textField.ActualWidth / canvasDraw.ActualWidth);
            }
            listFormSilverlight.ListForm = listForm;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Lanza el evento Closed.
            if (Closed != null)
            {
                Closed(this, e);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInformationToUI();
        }

        /// <summary>
        /// Actualiza los datos de ListFormControl desde el objeto ListFormSilverlight.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadInformationToUI()
        {
            LoadListBoxOutputData();
            LoadListBoxFields();
            LoadComboFontSize();
            LoadComboFontName();
            LoadListBoxFontColors();

            // Carga los ítems en el CanvasDraw
            foreach (TemplateListItem item in listForm.TemplateListFormDocument.Components)
            {
                AddSingleItem(new TextField((TemplateListItem)item.Clone()));
            }

            if (listForm.OutputDataContext != null)
            {
                if (listForm.OutputDataContext.Equals(listForm.InputDataContext))
                {
                    radioButtonRegister.IsChecked = true;
                }
                else
                {
                    radioButtonList.IsChecked = true;
                    listBoxOutputData.SelectedItem = listForm.OutputDataContext;
                }
            }
            else
            {
                radioButtonRegister.IsChecked = true;
            }

            textBoxTitle.Text = listForm.Title;
            
        }

        /// <summary>
        /// Carga los datos de ListBox desde el contexto de datos de entrada de
        /// listForm.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadListBoxFields()
        {
            // Informar si la lista no tiene un contexto.
            if (listForm.InputDataContext != null)
            {
                this.listBoxFields.ItemsSource = listFormSilverlight.ListForm.InputDataContext.Fields;
                listBoxFields.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Carga el ComboBox con el nombre de fuente.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadComboFontName()
        {
            for (int i = 1; i < (int)FontName.LastIndex; i++)
            {
                listBoxFontName.Items.Add((FontName)i);
            }
            listBoxFontName.SelectedIndex = 0;
        }

        /// <summary>
        /// Carga el ComboBox con el tamaño de la fuente.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadComboFontSize()
        {
            for (int i = 1; i < (int)PresentationLayer.ServerDesignerClasses.FontSize.LastIndex; i++)
            {
                listBoxFontSize.Items.Add((PresentationLayer.ServerDesignerClasses.FontSize)i);
            }
            listBoxFontSize.SelectedIndex = 1;
        }


        /// <summary>
        /// Carga el ListBox con el color de la fuente.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadListBoxFontColors()
        {
            foreach (SolidColorBrush brush in LogicalLibrary.Utilities.ColorFonts)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Background = brush;
                listBoxItem.Foreground = brush;
                listBoxItem.Content = " ";
                listBoxFontColor.Items.Add(listBoxItem);
            }
        }

        /// <summary>
        /// Carga el ListBox con datos del outputDataContext relativo.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void LoadListBoxOutputData()
        {
            if (listForm.InputDataContext != null)
            {
                List<Table> relatedTables = dataModel.GetRelatedTables(listForm.InputDataContext);
                listBoxOutputData.ItemsSource = relatedTables;
            }

        }

        private void buttonAddField_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Field selectedField = listBoxFields.SelectedItem as Field;
                TextField textField = new TextField(selectedField, selectedField.DataType);
                AddSingleItem(textField);
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }

        /// <summary>
        /// Agrega un nuevo TextField para insertarse en la lista.
        /// </summary>
        /// <param name="textField">TextField que se agregará.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is Called from others private method in the same Class")]
        private void AddSingleItem(TextField textField)
        {
            textField.Deleted += new MouseButtonEventHandler(textField_Deleted);
            textField.ResizeUp += new MouseEventHandler(textField_ResizeUp);
            textField.ResizeDown += new MouseEventHandler(textField_ResizeDown);
            textField.TextFieldMouseMove += new MouseEventHandler(textField_TextFieldMouseMove);
            AtachSingleItemEvents(textField);
            tempSingleItems.Add(textField);
            MarkSelectedItem(textField);
            canvasDraw.Add(textField);
            Canvas.SetLeft(textField, textField.XCoordinateRelativeToParent);
            Canvas.SetTop(textField, textField.YCoordinateRelativeToParent);
        }

        void textField_TextFieldMouseMove(object sender, MouseEventArgs e)
        {
            currentElement.OnDrag();
        }

        void textField_ResizeDown(object sender, MouseEventArgs e)
        {
            DragAndDropCanvas.IsResizeAction = true;
        }

        void textField_ResizeUp(object sender, MouseEventArgs e)
        {
            DragAndDropCanvas.IsResizeAction = false;
        }

        void textField_Deleted(object sender, MouseButtonEventArgs e)
        {
            TextField textField = sender as TextField;
            tempSingleItems.Remove(textField);
            canvasDraw.Children.Remove(textField);
        }

        /// <summary>
        /// Enlaza los eventos MouseLeftButtonUp y MouseLeftbuttonDown events de  iDraw.
        /// </summary>
        /// <param name="iDrawable">iDraw al que enlazarán los eventos.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is Called from other private method in the same Class")]
        private void AtachSingleItemEvents(IDraw iDraw)
        {
            if (iDraw == null)
            {
                throw new ArgumentNullException("iDraw", "iDraw can not be null");
            }
            // iDraw.MouseLeftButtonUp += new MouseButtonEventHandler(iDrawable_MouseLeftButtonUp);
            //
            iDraw.MouseLeftButtonDown += new MouseButtonEventHandler(iDrawable_MouseLeftButtonDown);
        }

        /// <summary>
        /// Marca un TextField como seleccionado.
        /// </summary>
        /// <param name="selected">TextField a marcarse.</param>
        private void MarkSelectedItem(TextField selected)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                textField.rectanglePrincipal.Stroke = new SolidColorBrush(Colors.Black);
            }
            currentElement = selected;
            TextField selectedItem = selected as TextField;
            
            // Marca un objeto Rectangule alrededor del TextField.
            selectedItem.rectanglePrincipal.Stroke = new SolidColorBrush(Colors.Red);
            
            // Establece el estado de los checkBox basado en datos del TextField.
            checkBoxBold.IsChecked = selected.TemplateListItem.Bold;
            checkBoxItalic.IsChecked = selected.TemplateListItem.Italic;
        }

        /// <summary>
        /// Actualiza las opciones relativas al ítem seleccionado.
        /// </summary>
        private void SetItemSelectedOptions()
        {
            TextField textField = currentElement as TextField;
            if (textField != null)
            {
                listBoxFontName.SelectedIndex = (int)textField.TemplateListItem.FontName;
                listBoxFontSize.SelectedIndex = (int)textField.TemplateListItem.FontSize - 1;

                int i = 0;
                foreach (SolidColorBrush color in Utilities.ColorFonts)
                {
                    if (String.CompareOrdinal(color.Color.ToString(),textField.TemplateListItem.FontColor)==0)
                    {
                        listBoxFontColor.SelectedIndex = i;
                        return;
                    }
                    i++;
                }

                checkBoxBold.IsChecked = textField.TemplateListItem.Bold;
                checkBoxItalic.IsChecked = textField.TemplateListItem.Italic;
            }
        }

        void iDrawable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextField textField = sender as TextField;
            if (textField != null)
            {
                MarkSelectedItem(textField);
                // Establece el ítem seleccionado como contexto de datos para
                // poder enlazar en XAML.
                DataContext = textField.TemplateListItem;
                SetItemSelectedOptions();
            }
        }

        private void listBoxFontName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                if (textField != null)
                {
                    textField.TemplateListItem.FontName = (FontName)listBoxFontName.SelectedIndex;
                }
            }
        }

        private void listBoxFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                if (textField != null && listBoxFontSize.SelectedIndex>=0)
                {
                    textField.TemplateListItem.FontSize = (FontSize)listBoxFontSize.SelectedIndex + 1;
                    textField.ChangeTextSize((FontSize)listBoxFontSize.SelectedIndex + 1);
                }
            }
            
        }

        private void checkBoxBold_Checked(object sender, RoutedEventArgs e)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                if (textField != null)
                {
                    textField.TemplateListItem.Bold = true;
                    textField.label.FontWeight = FontWeights.Bold;
                }
            }
            
        }

        private void checkBoxItalic_Checked(object sender, RoutedEventArgs e)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                if (textField != null)
                {
                    textField.TemplateListItem.Italic = true;
                    textField.label.FontStyle = FontStyles.Italic;
                }
            }
        }
                
        private void checkBoxItalic_Unchecked(object sender, RoutedEventArgs e)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                if (textField != null)
                {
                    textField.TemplateListItem.Italic = false;
                    textField.label.FontStyle = FontStyles.Normal;
                }
            }
        }


        private void listBoxFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.rectanglePreviewColor.Fill = (listBoxFontColor.SelectedItem as ListBoxItem).Background;

                if (currentElement != null)
                {
                    TextField textField = currentElement as TextField;
                    if (textField != null)
                    {
                        textField.TemplateListItem.FontColor = (this.rectanglePreviewColor.Fill as SolidColorBrush).Color.ToString();
                        textField.label.Foreground = this.rectanglePreviewColor.Fill;
                    }
                }
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }

        private void radioButtonList_Checked(object sender, RoutedEventArgs e)
        {
            listBoxOutputData.Visibility = Visibility.Visible;
        }

        private void radioButtonRegister_Checked(object sender, RoutedEventArgs e)
        {
            listBoxOutputData.Visibility = Visibility.Collapsed;
            listForm.OutputDataContext = listForm.InputDataContext;
        }

        private void listBoxOutputData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Table table = listBoxOutputData.SelectedItem as Table;
            if (table!=null)
            {
                listForm.OutputDataContext = table;
            }
        }

        private void checkBoxBold_Unchecked(object sender, RoutedEventArgs e)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                if (textField != null)
                {
                    textField.TemplateListItem.Bold = false;
                    textField.label.FontWeight = FontWeights.Normal;
                }
            }
        }
    }
}