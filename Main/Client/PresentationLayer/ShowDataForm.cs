using System;
using System.Windows.Forms;
using Janus.Client.UserControls;

namespace Janus.Client.PresentationLayer
{
    public partial class ShowDataForm : Form
    {
        // ItemUserControl itemUserControl;
        //public ShowDataForm(ItemUserControl itemUserControl)
        //{
        //    InitializeComponent();
        //    this.itemUserControl = itemUserControl;
        //    this.itemUserControl.Width = this.Width;
        //    this.itemUserControl.Height = this.Height;
        //    Controls.Add(itemUserControl);
        //}

        private void menuItemOption_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void menuItemBack_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}