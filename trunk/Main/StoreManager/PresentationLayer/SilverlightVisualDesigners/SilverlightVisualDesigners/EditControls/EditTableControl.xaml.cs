using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.DataModelClasses;
using SilverlightVisualDesigners.EditControls;
using System.Collections.Generic;
using UtnEmall.Server.EntityModel;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un UserControl para editar los datos de un artefacto  Table.
    /// </summary>
	public partial class EditTableControl : UserControl,IWindow
	{
        private TableSilverlight tableSilverlight;
        public TableSilverlight TableSilverlight 
        {
            get { return tableSilverlight; }
            set 
            { 
                tableSilverlight = value;
                this.UpdateUI();
            }
        }

        private UserControl userControl;

        public EditTableControl(TableSilverlight tableSilverlight, UserControl userControl)
        {
            if (tableSilverlight == null)
            {
                throw new ArgumentNullException("tableSilverlight","tableSilverlight can not be null");
            }

            // Inicializar variables.
            InitializeComponent();
            this.TableSilverlight = tableSilverlight;
            this.userControl = userControl;
            LoadDataTypeList();
        }

        public void UpdateUI()
        {
            if (this.tableSilverlight != null)
            {
                this.textBoxName.Text = tableSilverlight.TableName;
                
                // Quitar los campos mostrados actualmente.
                this.listBoxFields.Items.Clear();
                foreach (Field field in this.tableSilverlight.Fields)
                {
                    AddField(field);
                }
                checkIsStorage.IsChecked = tableSilverlight.IsStorage;
                if (checkIsStorage.IsChecked.Value)
                {
                    buttonAddField.IsEnabled = false;
                    buttonRemoveField.IsEnabled = false;
                }
            }
        }

        private void LoadDataTypeList()
        {
            listBoxFieldType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Text));
            listBoxFieldType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Numeric));
            listBoxFieldType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Date));
            listBoxFieldType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.YesNo));
            listBoxFieldType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Image));
        }

        #region events
            public event EventHandler Closed;
            public event EventHandler TableNameChanged;
        #endregion

        private void buttonAddField_Click(object sender, RoutedEventArgs e)
        {
            String fieldName = this.textBoxFieldName.Text;

            if (this.listBoxFieldType.SelectedIndex < 0)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidFieldNameError, SilverlightVisualDesigners.Properties.Resources.NoSelectedDataType, this.LayoutRoot);
                return;
            }

            // listIndex comienza en 0, y la enumeración DataType comienza en 1, es por
            // esto que hay desfasaje.
            DataType fieldType = (DataType)this.listBoxFieldType.SelectedIndex+1;
            if (String.IsNullOrEmpty(fieldName))
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidFieldNameError, SilverlightVisualDesigners.Properties.Resources.InvalidFieldNameMessage, this.LayoutRoot);
                return;
            }

            Error nameError = LogicalLibrary.Utilities.CheckFieldsOrTableNames(fieldName);
            if (nameError != null)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidFieldNameError, nameError.Description, this.LayoutRoot);
                return;
            }

            Field newField = new Field(fieldName,fieldType);
            AddField(newField);

            textBoxFieldName.Text = string.Empty;
        }

        private void buttonRemoveField_Click(object sender, RoutedEventArgs e)
        {
            Field fieldToRemove = listBoxFields.SelectedItem as Field;
            if (fieldToRemove != null)
            {
                RemoveField(fieldToRemove);
            }
        }

        private void AddField(Field field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field", "field can not be null");
            }
            if (ValidateIfFieldExists(field))
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.FieldSameNameError, SilverlightVisualDesigners.Properties.Resources.FieldSameNameMessage, this.LayoutRoot);
                return;
            }
            
            listBoxFields.Items.Add(field);
        }

        private bool ValidateIfFieldExists(Field field)
        {
            foreach (Field itemField in listBoxFields.Items)
            {
                if (String.CompareOrdinal(field.Name,itemField.Name) == 0 )
                {
                    return true;
                }
            }
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is called from other method in the same class")]
        private void RemoveField(Field field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field", "field can not be null");
            }
            listBoxFields.Items.Remove(field);
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            String tableName = textBoxName.Text.Trim();
            if (String.IsNullOrEmpty(tableName))
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidTableNameError, SilverlightVisualDesigners.Properties.Resources.InvalidTableNameMessage,this.LayoutRoot);
                return;
            }
            Error nameError = LogicalLibrary.Utilities.CheckFieldsOrTableNames(tableName);
            if (nameError != null)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.InvalidTableNameError, nameError.Description, this.LayoutRoot);
                return;
            }

            tableSilverlight.TableName = tableName;

            tableSilverlight.IsStorage = checkIsStorage.IsChecked.Value;
            if (tableSilverlight.IsStorage)
            {
                tableSilverlight.Table.RemoveAllFields();
                this.listBoxFields.Items.Clear();
            }

            tableSilverlight.Fields.Clear();
            foreach (Field field in listBoxFields.Items)
            {
                tableSilverlight.Fields.Add(field);
            }
            
            if (Closed != null)
            {
                Closed(this, e);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, e);
            }
        }

        private void checkIsStorage_Checked(object sender, RoutedEventArgs e)
        {
            DataModelDesignerSilverlight dataModelDesignerSilverlight = this.userControl as DataModelDesignerSilverlight;
            if (dataModelDesignerSilverlight != null && dataModelDesignerSilverlight.HasRelations(tableSilverlight.Table))
            {
                Dialog.ShowInformationDialog(SilverlightVisualDesigners.Properties.Resources.Information, SilverlightVisualDesigners.Properties.Resources.TheTableHasRelations, this.LayoutRoot);
                this.checkIsStorage.IsChecked = false;
                return;
            }

            buttonAddField.IsEnabled = false;
            buttonRemoveField.IsEnabled = false;
            
        }

        private void checkIsStorage_Unchecked(object sender, RoutedEventArgs e)
        {
            buttonAddField.IsEnabled = true;
            buttonRemoveField.IsEnabled = true;
        }

        private void textBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            textBoxName.Text = textBoxName.Text.Replace(" ", string.Empty);
            if (TableNameChanged != null)
            {
                TableNameChanged(this,e);
            }
        }

        private void textBoxFieldName_LostFocus(object sender, RoutedEventArgs e)
        {
            textBoxFieldName.Text = textBoxFieldName.Text.Replace(" ",string.Empty);
        }
    }
}