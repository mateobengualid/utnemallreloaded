using System.Windows;
using PresentationLayer.Widgets;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.DataModelClasses;
using System;
using System.Windows.Controls;
using UtnEmall.ServerManager;
using System.Collections.Generic;
using UtnEmall.Server.EntityModel;

namespace PresentationLayer.DataModelDesigner
{
    /// <summary>
    /// Clase que define una tabla del modelo de datos en wpf y permite agregar columnas asociadas a la tabla
    /// </summary>
    public partial class WindowTable : Window
    {
        #region Instance Variables and Properties

        TableWpf tableWPF;
        DataModelDocumentWpf dataModelDocumentWpf;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="table">La tabla a modificar</param>
        /// <param name="dataModelDocumentWpf">El modelo de datos con tablas y relaciones.</param>
        public WindowTable(TableWpf table,DataModelDocumentWpf dataModelDocumentWpf)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            if (dataModelDocumentWpf == null)
            {
                throw new ArgumentNullException("dataModelDocumentWpf", "dataModelDocumentWpf can not be null");
            }
            InitializeComponent();
            this.dataModelDocumentWpf = dataModelDocumentWpf;
            this.tableWPF = table;
            this.DataContext = tableWPF;
            this.textTableName.LostFocus += new RoutedEventHandler(textTableName_LostFocus);
            this.checkIsStorage.Checked += new RoutedEventHandler(checkIsStorage_Checked);
            this.checkIsStorage.Unchecked += new RoutedEventHandler(checkIsStorage_Unchecked);
            this.Loaded += new RoutedEventHandler(WindowTable_Loaded);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Función llamada cuando se deselecciona la opcion IsStorage(tabla de almacenamiento)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkIsStorage_Unchecked(object sender, RoutedEventArgs e)
        {
            buttonAdd.IsEnabled = true;
            buttonRemove.IsEnabled = true;
            textBoxFieldName.IsEnabled = true;
        }

        /// <summary>
        /// Función llamada cuando se selecciona la opcion IsStorage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkIsStorage_Checked(object sender, RoutedEventArgs e)
        {

            if ((this.listBoxField.Items.Count != 0) && !Util.ShowConfirmDialog(UtnEmall.ServerManager.Properties.Resources.ResetConnectionAndFields, ""))
            {
                checkIsStorage.IsChecked = false;
                return;
            }
            // Elimina todas las relaciones con esta tabla
            dataModelDocumentWpf.RemoveRelationWithTable(tableWPF as Table);

            // Elimina todos los campos de la tabla
            this.tableWPF.RemoveAllFields();
            this.listBoxField.Items.Clear();
            buttonAdd.IsEnabled = false;
            buttonRemove.IsEnabled = false;
            textBoxFieldName.IsEnabled = false;

            return;
        }

        /// <summary>
        /// Función llamada cuando el textbox donde se ingresa el nombre de la tabla pierde el foco. Esta función controla si el nombre de la tabla ya existe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textTableName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dataModelDocumentWpf.ExistsTableName(this.tableWPF, textTableName.Text))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.TableNameDupplicated);
                textTableName.Text = textTableName.Text.Substring(0, textTableName.Text.Length - 1);
                textTableName.Focus();
            }
        }

        /// <summary>
        /// Función llamada cuando la ventana se carga. Inicializa los campos y los combos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WindowTable_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxDataType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Text));
            comboBoxDataType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Numeric));
            comboBoxDataType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Date));
            comboBoxDataType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.YesNo));
            comboBoxDataType.Items.Add(LogicalLibrary.Utilities.GetDataType(DataType.Image));
            comboBoxDataType.SelectedIndex = 0;

            textTableName.Text = tableWPF.Name;
            checkIsStorage.IsChecked = tableWPF.IsStorage;
            foreach (Field field in tableWPF.Fields)
            {
                listBoxField.Items.Add(field);
            }
        }

        /// <summary>
        /// Función llamada cuando se presiona el boton cancelar
        ///Cierra la ventana sin guardar los cambios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Valida si ya existe una columna de la tabla con ese nombre
        /// </summary>
        /// <param name="field">La nueva columna a agregar</param>
        /// <returns>Retorna true si ya existe una columna con ese nombre</returns>
        private bool ValidateIfFieldExists(Field field)
        {
            foreach (Field itemField in listBoxField.Items)
            {
                if (String.CompareOrdinal(field.Name, itemField.Name) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Función llamada cuando se presiona el boton agregar.
        /// Esta función agrega una nueva columna a la lista de columnas de la tabla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxFieldName.Text))
            {
                Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.FieldNameRequired, "Warning");
                return;
            }
            
            string fieldName = textBoxFieldName.Text;

            Error error = LogicalLibrary.Utilities.CheckFieldsOrTableNames(fieldName);
            if (error != null)
            {
                Util.ShowInformationDialog(error.Description, "Warning");
                return;
            }

            Field newField = new Field(fieldName, (DataType)(comboBoxDataType.SelectedIndex+1));
            if (ValidateIfFieldExists(newField))
            {
                Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.FieldNameDuplicated,"Error");
                return;

            }
            listBoxField.Items.Add(newField);
            textBoxFieldName.Clear();
        }

        /// <summary>
        /// Función llamada cuando se presiona el botón eliminar.
        ///Esta función elimina un campo de la lista de campos de la tabla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            Field selectedField = listBoxField.SelectedItem as Field;
            listBoxField.Items.Remove(selectedField);
            listBoxField.SelectedIndex = listBoxField.Items.Count - 1;
        }

        /// <summary>
        /// Función llamada cuando el botón Ok es presionado
        /// Crea una nueva tabla o la actualiza con las respectivas columnas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textTableName.Text))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.TableNameRequired);
                textTableName.Focus();
                return;
            }
            Error error = LogicalLibrary.Utilities.CheckFieldsOrTableNames(textTableName.Text);
            if (error != null)
            {
                Util.ShowInformationDialog(error.Description, "Warning");
                textTableName.Focus();
                return;
            }

            tableWPF.Name = textTableName.Text.Trim();
            tableWPF.IsStorage = checkIsStorage.IsChecked.Value;

            // Borra las viejas columnas y agrega las nuevas
            tableWPF.Fields.Clear();
            foreach (Field item in listBoxField.Items)
            {
                tableWPF.Fields.Add(item);
            }

            // Cambia el nombre de la tabla en miniatura del diseñador
            TextBlock textBlockTableName = (tableWPF.UIElement as Canvas).FindName("textBlockTableName") as TextBlock;
            textBlockTableName.Text = tableWPF.Name;

            this.Close();
        }

        #endregion
    }
}
