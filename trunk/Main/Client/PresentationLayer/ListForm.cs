using System;
using System.Windows.Forms;
using Janus.Client.UserControls;


namespace Janus.Client.PresentationLayer
{
    public partial class ListForm : Form
    {
        // private ListItemUserControl listItemUserControl;
        //public ListForm(ListItemUserControl listItemUserControl)
        //{
        //    this.listItemUserControl = listItemUserControl;
        //    InitializeComponent();
        //    listItemUserControl.showItems();
        //    Controls.Add(listItemUserControl);
        //}

        private void menuItemBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            // ItemUserControl selectedItem = (ItemUserControl)listItemUserControl.getSelectedItem();
            //ItemUserControl item = new ItemUserControl(selectedItem.Template);
            //ShowDataForm showDataForm = new ShowDataForm(item);
            //showDataForm.Visible = true;
        }

        private void ListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.F1))
            {
                // Soft Key 1
                // No capturada.
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F2))
            {
                // Soft Key 2
                // No capturada.
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // listItemUserControl.changeSelection(true);
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // listItemUserControl.changeSelection(false);
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D1))
            {
                // 1
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D2))
            {
                // 2
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D3))
            {
                // 3
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D4))
            {
                // 4
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D5))
            {
                // 5
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D6))
            {
                // 6
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D7))
            {
                // 7
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D8))
            {
                // 8
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D9))
            {
                // 9
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F8))
            {
                // *
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D0))
            {
                // 0
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F9))
            {
                // #
            }

        }

        private void ListForm_Load(object sender, EventArgs e)
        {






              
        }
    }
}