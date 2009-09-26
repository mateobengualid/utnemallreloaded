using System.Windows;
using PresentationLayer.Widgets;
using LogicalLibrary;
using System.Windows.Controls;
using System.IO;
using System;
using PresentationLayer.ServerDesignerClasses;
using UtnEmall.ServerManager;
using System.Collections.Generic;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Logica para WindowMenuForm.xaml
    /// </summary>
    public partial class WindowMenuForm : Window
    {
        #region Constants, Variables and Properties

        MenuFormWpf menuFormWPF;
        ServiceDocumentWpf document;
        //  
        //
        List<FormMenuItemWpf> menuItemsDeleted;
        //  
        //
        private bool isUpdateItemAction;
        #endregion

        #region Constructors

        public WindowMenuForm(MenuForm form, ServiceDocumentWpf document)
        {
            InitializeComponent();
            this.document = document;
            this.menuFormWPF = form as MenuFormWpf;
            
            this.menuItemsDeleted = new List<FormMenuItemWpf>();
            
            this.DataContext = menuFormWPF;
            this.listOptionMenuForm.SelectionMode = SelectionMode.Single;
            this.listOptionMenuForm.SelectionChanged += new SelectionChangedEventHandler(listOptionMenuForm_SelectionChanged);
        }

        private void loadRealtedTables()
        {
            if (menuFormWPF.InputDataContext == null)
            {
                comboBoxOutput.IsEnabled = false;
                radioList.IsEnabled = false;
                radioRegister.IsEnabled = false;
                radioNone.IsEnabled = true;
                radioNone.IsChecked = true;
                return;
            }
            List<LogicalLibrary.DataModelClasses.Table> relatedTables =
                document.DataModel.GetRelatedTables(menuFormWPF.InputDataContext);
            comboBoxOutput.Items.Clear();
            foreach (LogicalLibrary.DataModelClasses.Table table in relatedTables)
            {
                comboBoxOutput.Items.Add(table);
            }
            comboBoxOutput.SelectedIndex = 0;
            if (comboBoxOutput.Items.Count == 0)
            {
                comboBoxOutput.IsEnabled = false;
                radioList.IsEnabled = false;
                radioRegister.IsEnabled = true;
                radioRegister.IsChecked = true;
            }
        }
        #endregion

        #region Instance Methods

        void listOptionMenuForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FormMenuItemWpf item = listOptionMenuForm.SelectedItem as FormMenuItemWpf;
                if (item != null)
                {
                    textBoxText.Text = item.Text;
                    checkBoxBold.IsChecked = item.Bold;
                    textBoxHelp.Text = item.HelpText;
                    isUpdateItemAction = true;
                    this.buttonAdd.Content = "Update";

                    if (item.InputDataContext == null)
                    {
                        radioNone.IsChecked = true;
                    }
                    else
                    {
                        loadRealtedTables();
                        comboBoxOutput.SelectedItem = item.InputDataContext;
                    }

                    this.EnableOutputContext(false);
                }
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private FormMenuItemWpf generateItem()
        {
            FormMenuItemWpf item = new FormMenuItemWpf(this.textBoxText.Text, this.textBoxHelp.Text,
                FontName.Arial);

            if (radioRegister.IsChecked == true)
            {
                item.InputDataContext = this.menuFormWPF.InputDataContext;
                item.OutputDataContext = item.InputDataContext;
            }
            else if (radioList.IsChecked == true)
            {
                item.InputDataContext = comboBoxOutput.SelectedItem as LogicalLibrary.DataModelClasses.Table;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (FormMenuItemWpf formMenuItem in menuFormWPF.MenuItems)
            {
                listOptionMenuForm.Items.Add(formMenuItem);
            }
            this.loadRealtedTables();
        }

        private void clearTextBoxs()
        {
            this.textBoxHelp.Text = "";
            this.textBoxText.Text = "";
            this.checkBoxBold.IsChecked = false;
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (FormMenuItemWpf formMenuItem in menuItemsDeleted)
                {
                    formMenuItem.Reset(null);
                }
                foreach (FormMenuItemWpf formMenuItem in menuFormWPF.MenuItems)
                {
                    formMenuItem.Reset(null);
                }

                menuFormWPF.MenuItems.Clear();

                listOptionMenuForm.Items.Clear();
                this.EnableOutputContext(true);
                resetUpdate();
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

        private void EnableOutputContext(bool enableOutputContext)
        {
            if (enableOutputContext)
            {
                this.comboBoxOutput.IsEnabled = true;
                this.radioList.IsChecked = true;
                this.radioList.IsEnabled = true;

                this.radioNone.IsEnabled = true;
                this.radioNone.IsChecked = false;
                
                this.radioRegister.IsEnabled = true;
                this.radioRegister.IsChecked = false;
            }
            else
            {
                this.comboBoxOutput.IsEnabled = false;
                this.radioList.IsEnabled = false;
                this.radioNone.IsEnabled = false;
                this.radioRegister.IsEnabled = false;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxText.Text.Trim()))
                {
                    Util.ShowInformationDialog(UtnEmall.ServerManager.Properties.Resources.EmptyText, UtnEmall.ServerManager.Properties.Resources.Error);
                    return;
                }
                if (isUpdateItemAction)
                {
                    FormMenuItemWpf selectedItem = listOptionMenuForm.SelectedItem as FormMenuItemWpf;
                    selectedItem.Text = this.textBoxText.Text;
                    selectedItem.HelpText = this.textBoxHelp.Text;
                    resetUpdate();
                }
                else
                {
                    FormMenuItemWpf itemGenerated = generateItem();
                    listOptionMenuForm.Items.Add(itemGenerated);
                }
                clearTextBoxs();
                loadRealtedTables();
                UpdateListOption();
                buttonAdd.Content = UtnEmall.ServerManager.Properties.Resources.Add;
                EnableOutputContext(true);
                textBoxText.Focus();
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

        private void UpdateListOption()
        {
            listOptionMenuForm.Items.Refresh();
            listOptionMenuForm.UpdateLayout();
        }

        /// <summary>
        /// Metodo llamado cuando se presiona en guardar. El metodo guarda todos los cambios realizados en el formulario de menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (FormMenuItemWpf formMenuItem in menuItemsDeleted)
                {
                    formMenuItem.Reset(null);
                }

                menuFormWPF.MenuItems.Clear();
                foreach (FormMenuItemWpf item in listOptionMenuForm.Items)
                {
                    item.Parent = menuFormWPF;
                    menuFormWPF.MenuItems.Add(item);
                }
                menuFormWPF.MakeCanvas();
                document.RedrawDocument();
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        private void radioList_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxOutput.IsEnabled = true;
        }

        private void radioRegister_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxOutput.IsEnabled = false;
        }

        private void radioNone_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxOutput.IsEnabled = false;
        }
        #endregion

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            int index = listOptionMenuForm.SelectedIndex;

            if (index < 1)
            {
                return;
            }

            object previousItem = listOptionMenuForm.Items[index - 1];
            listOptionMenuForm.Items[index - 1] = listOptionMenuForm.SelectedItem;
            listOptionMenuForm.Items[index] = previousItem;
            listOptionMenuForm.SelectedIndex = index - 1;

            listOptionMenuForm.Items.Refresh();
            listOptionMenuForm.UpdateLayout();
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            int index = listOptionMenuForm.SelectedIndex;

            if (index == -1 || index + 1 == listOptionMenuForm.Items.Count)
            {
                return;
            }

            object nextItem = listOptionMenuForm.Items[index + 1];
            listOptionMenuForm.Items[index + 1] = listOptionMenuForm.SelectedItem;
            listOptionMenuForm.Items[index] = nextItem;
            listOptionMenuForm.SelectedIndex = index + 1;

            listOptionMenuForm.Items.Refresh();
            listOptionMenuForm.UpdateLayout();
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            menuItemsDeleted.Add(listOptionMenuForm.SelectedItem as FormMenuItemWpf);
            
            listOptionMenuForm.Items.Remove(listOptionMenuForm.SelectedItem);
            resetUpdate();
        }

        private void resetUpdate()
        {
            isUpdateItemAction = false;
            this.buttonAdd.Content = UtnEmall.ServerManager.Properties.Resources.Add;
            clearTextBoxs();
        }
    }
}
