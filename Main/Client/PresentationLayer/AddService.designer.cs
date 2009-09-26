namespace Janus.Client.PresentationLayer
{
    partial class AddService
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuAddService;

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
            this.mainMenuAddService = new System.Windows.Forms.MainMenu();
            this.menuItemYes = new System.Windows.Forms.MenuItem();
            this.menuItemNo = new System.Windows.Forms.MenuItem();
            this.icon = new System.Windows.Forms.PictureBox();
            this.LblInstall = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenuAddService
            // 
            this.mainMenuAddService.MenuItems.Add(this.menuItemYes);
            this.mainMenuAddService.MenuItems.Add(this.menuItemNo);
            // 
            // menuItemYes
            // 
            this.menuItemYes.Text = "Yes";
            this.menuItemYes.Click += new System.EventHandler(this.menuItemAdd_Click);
            // 
            // menuItemNo
            // 
            this.menuItemNo.Text = "No";
            this.menuItemNo.Click += new System.EventHandler(this.menuItemBack_Click);
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(68, 20);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(38, 40);
            // 
            // LblInstall
            // 
            this.LblInstall.Location = new System.Drawing.Point(0, 80);
            this.LblInstall.Name = "LblInstall";
            this.LblInstall.Size = new System.Drawing.Size(173, 31);
            this.LblInstall.Text = "¿Confirm install Service?";
            this.LblInstall.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AddService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(176, 180);
            this.Controls.Add(this.LblInstall);
            this.Controls.Add(this.icon);
            this.Menu = this.mainMenuAddService;
            this.Name = "AddService";
            this.Text = "All Services";
            this.Load += new System.EventHandler(this.AddService_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemYes;
        private System.Windows.Forms.MenuItem menuItemNo;
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.Label LblInstall;
    }
}