namespace UtnEmall.Client.PresentationLayer
{
    partial class MallConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuMallConfiguration;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MallConfigurationForm));
            this.mainMenuMallConfiguration = new System.Windows.Forms.MainMenu();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.labelServerIp = new System.Windows.Forms.Label();
            this.textBoxServerIp = new System.Windows.Forms.TextBox();
            this.header = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenuMallConfiguration
            // 
            this.mainMenuMallConfiguration.MenuItems.Add(this.menuItemSave);
            this.mainMenuMallConfiguration.MenuItems.Add(this.menuItemCancel);
            // 
            // menuItemSave
            // 
            resources.ApplyResources(this.menuItemSave, "menuItemSave");
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemCancel
            // 
            resources.ApplyResources(this.menuItemCancel, "menuItemCancel");
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // labelServerIp
            // 
            resources.ApplyResources(this.labelServerIp, "labelServerIp");
            this.labelServerIp.ForeColor = System.Drawing.Color.DimGray;
            this.labelServerIp.Name = "labelServerIp";
            // 
            // textBoxServerIp
            // 
            resources.ApplyResources(this.textBoxServerIp, "textBoxServerIp");
            this.textBoxServerIp.BackColor = System.Drawing.Color.Gainsboro;
            this.textBoxServerIp.Name = "textBoxServerIp";
            // 
            // header
            // 
            resources.ApplyResources(this.header, "header");
            this.header.Name = "header";
            // 
            // title
            // 
            resources.ApplyResources(this.title, "title");
            this.title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(137)))), ((int)(((byte)(145)))));
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Name = "title";
            // 
            // MallConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.Controls.Add(this.textBoxServerIp);
            this.Controls.Add(this.labelServerIp);
            this.Menu = this.mainMenuMallConfiguration;
            this.Name = "MallConfigurationForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.Label labelServerIp;
        private System.Windows.Forms.TextBox textBoxServerIp;
        private System.Windows.Forms.PictureBox header;
        private System.Windows.Forms.Label title;
    }
}