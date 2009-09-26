namespace Janus.Client.PresentationLayer
{
    partial class ShowDataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuShowData;

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
            this.mainMenuShowData = new System.Windows.Forms.MainMenu();
            this.menuItemOption = new System.Windows.Forms.MenuItem();
            this.menuItemBack = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenuShowData
            // 
            this.mainMenuShowData.MenuItems.Add(this.menuItemOption);
            this.mainMenuShowData.MenuItems.Add(this.menuItemBack);
            // 
            // menuItemOption
            // 
            this.menuItemOption.Text = "Option";
            this.menuItemOption.Click += new System.EventHandler(this.menuItemOption_Click);
            // 
            // menuItemBack
            // 
            this.menuItemBack.Text = "Back";
            this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
            // 
            // ShowDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(176, 180);
            this.Menu = this.mainMenuShowData;
            this.Name = "ShowDataForm";
            this.Text = "ShowDataForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemOption;
        private System.Windows.Forms.MenuItem menuItemBack;
    }
}