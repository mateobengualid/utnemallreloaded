namespace Janus.Client.PresentationLayer
{
    partial class ListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuListForm;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenuListForm = new System.Windows.Forms.MainMenu();
            this.menuItemSelect = new System.Windows.Forms.MenuItem();
            this.menuItemBack = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenuListForm
            // 
            this.mainMenuListForm.MenuItems.Add(this.menuItemSelect);
            this.mainMenuListForm.MenuItems.Add(this.menuItemBack);
            // 
            // menuItemSelect
            // 
            this.menuItemSelect.Text = "Select";
            this.menuItemSelect.Click += new System.EventHandler(this.menuItemSelect_Click);
            // 
            // menuItemBack
            // 
            this.menuItemBack.Text = "Back";
            this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(176, 180);
            this.KeyPreview = true;
            this.Menu = this.mainMenuListForm;
            this.Name = "ListForm";
            this.Text = "ListForm";
            this.Load += new System.EventHandler(this.ListForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSelect;
        private System.Windows.Forms.MenuItem menuItemBack;

    }
}