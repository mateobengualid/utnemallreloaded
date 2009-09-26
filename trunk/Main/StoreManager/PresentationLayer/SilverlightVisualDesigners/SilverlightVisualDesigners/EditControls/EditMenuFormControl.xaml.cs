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
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.Widgets;
using LogicalLibrary.DataModelClasses;
using System.Collections.Generic;
using SilverlightVisualDesigners.Widgets;
using UtnEmall.Server.EntityModel;

namespace SilverlightVisualDesigners
{
	public partial class EditMenuFormControl : UserControl,IWindow
	{
        private MenuFormSilverlight menuFormSilverlight;
        private MenuForm myMenuForm;
        private Dictionary<MenuItemSilverlight,FormMenuItem> menuItemsChanged;
        private List<MenuItemSilverlight> menuItemsDeleted;
        private DataModel dataModel;

        /// <summary>
        /// Clase que representa un UserControl para editar artefactos MenuFormSilverlight.
        /// </summary>
        public EditMenuFormControl(MenuFormSilverlight menuFormSilverlight, DataModel dataModel)
		{
			// Inicializar variables.
			InitializeComponent();
            this.menuFormSilverlight = menuFormSilverlight;
            this.myMenuForm = menuFormSilverlight.MenuForm;
            this.dataModel = dataModel;
            this.menuItemsChanged = new Dictionary<MenuItemSilverlight,FormMenuItem>();
            this.menuItemsDeleted = new List<MenuItemSilverlight>();
            LoadListBoxOutputData();
		}

        #region IWindow Members

        public event EventHandler Closed;

        #endregion

        /// <summary>
        /// Carga ComboBox con datos de salida.
        /// </summary>
        private void LoadListBoxOutputData()
        {
            if (myMenuForm.InputDataContext == null)
            {
                radioList.IsEnabled = false;
                radioRegister.IsEnabled = false;
                radioNone.IsEnabled = true;
                radioNone.IsChecked = true;
                return;
            }
            
            List<Table> relatedTables = dataModel.GetRelatedTables(myMenuForm.InputDataContext);
            listBoxOutput.ItemsSource = relatedTables;
            if (listBoxOutput.Items.Count == 0)
            {
                radioList.IsEnabled = false;
                radioRegister.IsEnabled = true;
                radioRegister.IsChecked = true;
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxEnabled.Items.Count == 0)
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.NoMenuItems, this.gridLayautRoot);
                    return;
                }

                // Verificar si el MenuForm tiene título.
                if (String.IsNullOrEmpty(textBoxTitle.Text))
                {
                    Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error,SilverlightVisualDesigners.Properties.Resources.TitleFormEmpty, this.gridLayautRoot);
                    return;
                }

                // Cambiar el título del MenuForm.
                menuFormSilverlight.ChangeTitle(textBoxTitle.Text.Trim());

                // Eliminar los ítems viejos eliminados si el usuario está editando
                // el Menuform.
                foreach (MenuItemSilverlight item in menuItemsDeleted.ToArray())
                {
                    menuFormSilverlight.RemoveItem(item);
                }

                // Busca los ítems editados y actualiza los datos.
                foreach (MenuItemSilverlight item in menuFormSilverlight.MenuItemsSilverlight)
                {
                    if (menuItemsChanged.ContainsKey(item))
                    {
                        FormMenuItem tempFormMenuItem = menuItemsChanged[item];
                        item.Text = tempFormMenuItem.Text;
                        item.HelpText = tempFormMenuItem.HelpText;
                        // Remover de la lista para no agregarlo en el siguiente foreach.
                        listBoxEnabled.Items.Remove(tempFormMenuItem);
                    }
                }

                // Para cada ítem en listBoxEnable que no haya sido creado, crear un nuevo
                // FormMenuItem y añadirlo al menú.
                foreach (FormMenuItem formMenuItem in listBoxEnabled.Items)
                {
                    MenuItemSilverlight menuItemSilverlightFinded = menuFormSilverlight.FindMenuItemSilverlight(formMenuItem);
                    // Si el formMenuItem es nuevo, crearlo y añadirlo al menú.
                    if (menuItemSilverlightFinded == null)
                    {
                        formMenuItem.Parent = this.menuFormSilverlight.MenuForm;
                        MenuItemSilverlight newMenuItemSilverlight = new MenuItemSilverlight(formMenuItem);
                        newMenuItemSilverlight.MenuParent = this.menuFormSilverlight;
                        this.menuFormSilverlight.AddItem(newMenuItemSilverlight);
                    }
                    
                    int indexFormMenuItem = listBoxEnabled.Items.IndexOf(formMenuItem);
                    int indexMenuItemSilverlightFinded = menuFormSilverlight.MenuItemsSilverlight.IndexOf(menuItemSilverlightFinded);
                    
                    // Si el formMenuItem ha cambiado de posición en el menú, reordenarlo
                    // también en el menú.
                    if (indexMenuItemSilverlightFinded != -1 && indexFormMenuItem != indexMenuItemSilverlightFinded)
                    {
                        MenuItemSilverlight menuItemToPosicionate = menuFormSilverlight.MenuItemsSilverlight[indexMenuItemSilverlightFinded];
                        menuFormSilverlight.MenuItemsSilverlight.Remove(menuItemToPosicionate);
                        menuFormSilverlight.MenuItemsSilverlight.Insert(indexFormMenuItem, menuItemToPosicionate);                        
                    }
                }

                // Reordenar el ítem para actualizar la visualización.
                menuFormSilverlight.ReorderItems();

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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, e);
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            clearTextBoxs();
            buttonAddItem.IsEnabled = true;
            buttonEdit.IsEnabled = false;
            listBoxEnabled.SelectedIndex = -1;
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar errores en los campos o nombres de tablas.
                Error error = Utilities.CheckFieldsOrTableNames(TextBoxText.Text);
                if (error == null)
                {
                    FormMenuItem itemGenerated = generateItem();
                    listBoxEnabled.Items.Add(itemGenerated);
                    clearTextBoxs();
                    this.EnableOutputContext(true);
                    return;
                }
                Dialog.ShowErrorDialog(error.Name, error.Description, this.gridLayautRoot);
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String textEdited = TextBoxText.Text.Trim();
                String stringHelpEdited = textBoxHelp.Text.Trim();

                FormMenuItem formMenuItem = listBoxEnabled.SelectedItem as FormMenuItem;
                // Buscar el objeto MenuItemSilverlight elegido en MenuFormSilverlight.
                MenuItemSilverlight menuItemSilverlight = menuFormSilverlight.FindMenuItemSilverlight(formMenuItem);
                if (menuItemSilverlight != null)
                {
                    FormMenuItem formMenuItemChanged;
                    // Si el MenuItemsToChange ya contiene el FormMenuItem, cambiar este
                    // FormMenuItem.
                    if (menuItemsChanged.ContainsKey(menuItemSilverlight))
                    {
                        formMenuItemChanged = menuItemsChanged[menuItemSilverlight];
                        formMenuItemChanged.Text = textEdited;
                        formMenuItemChanged.HelpText = stringHelpEdited;
                        listBoxEnabled.UpdateLayout();
                        return;
                    }
                    // Si no lo contiene, crear un nuevo formMenuItem.
                    formMenuItemChanged = new FormMenuItem();
                    formMenuItemChanged.Text = textEdited;
                    formMenuItemChanged.HelpText = stringHelpEdited;

                    // Agregar un nuevo formMenuItem a la lista menuItemsChanged.
                    menuItemsChanged.Add(menuItemSilverlight,formMenuItemChanged);
                    
                    int selected = listBoxEnabled.SelectedIndex;
                    // Quitar el ítem viejo de la lista listBoxEneable.
                    listBoxEnabled.Items.Remove(formMenuItem);
                    // Insertar el nuevo ítem en la misma posición que el ítem quitado.
                    listBoxEnabled.Items.Insert(selected, formMenuItemChanged);

                    return;
                }
                // Si el ítem seleccionado no está en el MenuForm, entonces el ítem está
                // siendo editado: completar sus datos.
                formMenuItem.Text = textEdited;
                formMenuItem.HelpText = stringHelpEdited;
                
                listBoxEnabled.Items.Remove(formMenuItem);
                listBoxEnabled.Items.Add(formMenuItem);

                clearTextBoxs();
                EnableOutputContext(true);
                buttonAddItem.IsEnabled = true;
                buttonEdit.IsEnabled = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Limpiar los TextBoxes en el UserControl.
        /// </summary>
        private void clearTextBoxs()
        {
            this.textBoxHelp.Text = "";
            this.TextBoxText.Text = "";
            this.checkBoxBold.IsChecked = false;
        }

        /// <summary>
        /// Crea un ítem basado en los datos ingresados en un EditMenuFromControl.
        /// </summary>
        /// <returns>Objeto MenuItem para un Form.</returns>
        private FormMenuItem generateItem()
        {
            FormMenuItem item = new FormMenuItem(this.TextBoxText.Text.Trim(), this.textBoxHelp.Text.Trim(),
                FontName.Arial);

            if (radioRegister.IsChecked.Value)
            {
                item.InputDataContext = this.myMenuForm.InputDataContext;
                item.OutputDataContext = item.InputDataContext;
            }
            else if (radioList.IsChecked.Value)
            {
                item.InputDataContext = listBoxOutput.SelectedItem as LogicalLibrary.DataModelClasses.Table;
                item.OutputDataContext = item.InputDataContext;
            }
            else
            {
                item.InputDataContext = null;
                item.OutputDataContext = null;
            }

            item.FontName = FontName.Arial;

            item.Bold = (bool)checkBoxBold.IsChecked;

            return item;
        }

        /// <summary>
        /// Verifica si los TextBoxes requeridos tienen datos válidos.
        /// </summary>
        /// <returns>Verdadero si los datos son válidos, falso si no.</returns>
        private bool ValidateTextBoxes()
        {
            string text = TextBoxText.Text.Trim();
            
            string wrongControl = null;

            if (String.IsNullOrEmpty(text))
            {
                wrongControl = "Text";
            }

            if (wrongControl != null)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, wrongControl + " Is Necessary", this.gridLayautRoot);
                return false;
            }
            return true;
        }

        private void listBoxEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FormMenuItem item = listBoxEnabled.SelectedItem as FormMenuItem;
            if (item != null)
            {
                // Carga la interfaz GUI en base a un ítem seleccionado.
                TextBoxText.Text = item.Text;
                checkBoxBold.IsChecked = item.Bold;
                if (item.HelpText!=null)
                {
                    textBoxHelp.Text = item.HelpText;
                }
 
                this.buttonEdit.IsEnabled = true;
                this.buttonAddItem.IsEnabled = false;
                this.EnableOutputContext(false);
            }
        }

        private void EnableOutputContext(bool enableOutputContext)
        {
            if (enableOutputContext)
            {
                this.listBoxOutput.Visibility = Visibility.Visible;
                this.radioList.IsEnabled = true;
                this.radioNone.IsEnabled = true;
                this.radioRegister.IsEnabled = true;
            }
            else
            {
                this.listBoxOutput.Visibility = Visibility.Collapsed;
                this.radioList.IsEnabled = false;
                this.radioNone.IsEnabled = false;
                this.radioRegister.IsEnabled = false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxTitle.Text = myMenuForm.Title;
            foreach (MenuItemSilverlight menuItemSilverlight in menuFormSilverlight.MenuItemsSilverlight)
            {
                listBoxEnabled.Items.Add(menuItemSilverlight.FormMenuItem);
            }
        }

        private void listBoxOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            radioList.IsChecked = true;
        }

        private void buttonRemoveOfList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormMenuItem formMenuItem = listBoxEnabled.SelectedItem as FormMenuItem;
                MenuItemSilverlight menuItemSilverlight = menuFormSilverlight.FindMenuItemSilverlight(formMenuItem);

                clearTextBoxs();
                buttonAddItem.IsEnabled = true;
                buttonEdit.IsEnabled = false;
                listBoxEnabled.SelectedIndex = -1;

                // Si el objeto FormMenuItem estuvo desde el comienzo y no fue modificado.
                if (menuItemSilverlight != null)
                {
                    // Agregar un formMenuItem para borrarlo de la lista ListBoxEneable.
                    menuItemsDeleted.Add(menuItemSilverlight);
                    listBoxEnabled.Items.Remove(formMenuItem);
                    return;
                }
                // El objeto formMenuItem estuvo desde el comienzo, pero fue modificado.
                if (menuItemsChanged.ContainsValue(formMenuItem))
                {
                    foreach (MenuItemSilverlight item in menuFormSilverlight.MenuItemsSilverlight)
                    {
                        if (menuItemsChanged[item].Equals(formMenuItem))
                        {
                            menuItemsDeleted.Add(item);
                            menuItemsChanged.Remove(item);
                            listBoxEnabled.Items.Remove(formMenuItem);
                            return;
                        }
                    }
                }
                // El objeto formMenuItem no estuvo desde el comienzo.
                listBoxEnabled.Items.Remove(formMenuItem);
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }

        private void buttonMoveUpOfList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listBoxEnabled.SelectedIndex;

                if (index < 1)
                {
                    return;
                }

                object previousItem = listBoxEnabled.Items[index - 1];
                listBoxEnabled.Items[index - 1] = listBoxEnabled.SelectedItem;
                listBoxEnabled.Items[index] = previousItem;

                listBoxEnabled.SelectedIndex = index - 1;

                listBoxEnabled.UpdateLayout();

            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }

        private void buttonMoveDownOfList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listBoxEnabled.SelectedIndex;

                if (index == -1 || index + 1 == listBoxEnabled.Items.Count)
                {
                    return;
                }

                object nextItem = listBoxEnabled.Items[index + 1];
                
                listBoxEnabled.Items[index + 1] = listBoxEnabled.SelectedItem;
                listBoxEnabled.Items[index] = nextItem;

                listBoxEnabled.SelectedIndex = index + 1;

                listBoxEnabled.UpdateLayout();
            }
            catch (NullReferenceException error)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.UnhandledError, error.Message, this.gridLayautRoot);
            }
        }   
    }
}