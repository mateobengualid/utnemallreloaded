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
using System.Collections.Generic;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary;
using System.ComponentModel;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un UserControl para editar un artefacto  ShowDataForm.
    /// </summary>
	public partial class EditShowDataFormControl : UserControl,IWindow
	{

        private ShowDataFormSilverlight showDataFormSilverlight;
        private List<TextField> tempSingleItems;

        /// <summary>
        /// Guarda el ítem seleccionado en ListDesigner.
        /// </summary>
        private IDraw currentElement;

        /// <summary>
        /// Crea un EditListFormControl basado en un ListFormSilverlight.
        /// </summary>
        /// <param name="showDataFormSilverlight">ShowDataFormSilverlight usado para  crearlo.</param>
        /// <param name="dataModel">Modelo de datos usado para completar los datos de la interfaz.</param>
		public EditShowDataFormControl(ShowDataFormSilverlight showDataFormSilverlight,DataModel dataModel)
		{
			// Inicializar variables.
			InitializeComponent();
            this.tempSingleItems = new List<TextField>();
            this.showDataFormSilverlight = showDataFormSilverlight;
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
                saveInformationToWidget();
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
        /// Ingresa los datos del ListFormControl en el ShowDataFormSilverlight.
        /// </summary>
        private void saveInformationToWidget()
        {
            
            showDataFormSilverlight.ChangeTitle(this.textBoxTitle.Text.Trim());
            showDataFormSilverlight.ShowDataForm.OutputDataContext = showDataFormSilverlight.ShowDataForm.InputDataContext;

            // Copiar la lista tempCollectionsItems en la lista listItems.
            showDataFormSilverlight.ShowDataForm.TemplateListFormDocument.Components.Clear();
            foreach (TextField textField in tempSingleItems)
            {
                showDataFormSilverlight.ShowDataForm.TemplateListFormDocument.AddTemplateListItem(textField.TemplateListItem);

                // Guardar coordenadas y factores posición para cada TextField.
                // Cachear ambos objetos casteados.
                IDraw selectedItemAsIDraw = textField as IDraw;

                selectedItemAsIDraw.XCoordinateRelativeToParent = Canvas.GetLeft(textField);
                selectedItemAsIDraw.YCoordinateRelativeToParent = Canvas.GetTop(textField);

                // Asignar XFactorCoordinateRelativeToParent y  YFactorCoordinateRelativeToParent.
                selectedItemAsIDraw.XFactorCoordinateRelativeToParent = selectedItemAsIDraw.XCoordinateRelativeToParent / canvasDraw.ActualWidth;
                selectedItemAsIDraw.YFactorCoordinateRelativeToParent = selectedItemAsIDraw.YCoordinateRelativeToParent / canvasDraw.ActualHeight;

                selectedItemAsIDraw.HeightFactor = (textField.ActualHeight / canvasDraw.ActualHeight);
                selectedItemAsIDraw.WidthFactor = (textField.ActualWidth / canvasDraw.ActualWidth);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadInformationToUI();
        }

        /// <summary>
        /// Actualizar los datos de ShowDataFormControl del ShowDataFormSilverlight.
        /// </summary>
        private void loadInformationToUI()
        {
            // Cargar listItems en CanvasDraw.
            foreach (TemplateListItem item in showDataFormSilverlight.ShowDataForm.TemplateListFormDocument.Components)
            {
                AddSingleItem(new TextField((TemplateListItem)item.Clone()));
            }

            if (!String.IsNullOrEmpty(showDataFormSilverlight.ShowDataForm.Title))
            {
                textBoxTitle.Text = showDataFormSilverlight.ShowDataForm.Title;
            }

            loadListBoxFields();
            loadComboFontSize();
            loadComboFontName();
            loadListBoxFontColors();
        }

        private void loadListBoxFields()
        {
            if (showDataFormSilverlight.ShowDataForm.InputDataContext != null)
            {
                this.listBoxFields.ItemsSource = showDataFormSilverlight.ShowDataForm.InputDataContext.Fields;
                listBoxFields.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Cargar el ComboBox con el nombre de fuente.
        /// </summary>
        private void loadComboFontName()
        {
            for (int i = 1; i < (int)FontName.LastIndex; i++)
            {
                listBoxFontName.Items.Add((FontName)i);
            }
        }

        /// <summary>
        /// Carga ComboBox con datos del tamaño.
        /// </summary>
        private void loadComboFontSize()
        {
            for (int i = 1; i < (int)PresentationLayer.ServerDesignerClasses.FontSize.LastIndex; i++)
            {
                listBoxFontSize.Items.Add((PresentationLayer.ServerDesignerClasses.FontSize)i);
            }
        }

        /// <summary>
        /// Carga ListBox con el color de la fuente.
        /// </summary>
        private void loadListBoxFontColors()
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
        /// Agrega un nuevo TextField para insertarlo en un showDataForm.
        /// </summary>
        /// <param name="textField">TextField que se agregará.</param>
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
        /// Enlaza los eventos MouseLeftButtonUp y MouseLeftbuttonDown de iDraw.
        /// </summary>
        /// <param name="iDrawable">iDraw que enlazará sus eventos.</param>
        private void AtachSingleItemEvents(IDraw iDrawable)
        {
            if (iDrawable == null)
            {
                throw new ArgumentNullException("iDrawable", "iDrawable can not be null");
            }
            iDrawable.MouseLeftButtonDown += new MouseButtonEventHandler(iDrawable_MouseLeftButtonDown);
        }

        /// <summary>
        /// Marca un TextField como seleccionado.
        /// </summary>
        /// <param name="selected">TextField que se marcará seleccionado.</param>
        private void MarkSelectedItem(TextField selected)
        {
            if (currentElement != null)
            {
                TextField textField = currentElement as TextField;
                textField.rectanglePrincipal.Stroke = new SolidColorBrush(Colors.Black);
            }
            currentElement = selected;
            TextField selectedItem = selected as TextField;
            selectedItem.rectanglePrincipal.Stroke = new SolidColorBrush(Colors.Red);

            checkBoxBold.IsChecked = selected.TemplateListItem.Bold;
            checkBoxItalic.IsChecked = selected.TemplateListItem.Italic;
        }

        /// <summary>
        /// Actualiza las opciones relativas al objeto TempletListItem seleccionado.
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
                    if (String.CompareOrdinal(color.Color.ToString(), textField.TemplateListItem.FontColor) == 0)
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
                if (textField != null && listBoxFontSize.SelectedIndex >= 0)
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
    }
}