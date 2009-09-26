namespace UtnEmall.Client.PresentationLayer
{
    partial class CustomerConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuCustomerConfiguration;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerConfigurationForm));
            this.mainMenuCustomerConfiguration = new System.Windows.Forms.MainMenu();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenuCustomerConfiguration
            // 
            this.mainMenuCustomerConfiguration.MenuItems.Add(this.menuItemSave);
            this.mainMenuCustomerConfiguration.MenuItems.Add(this.menuItemCancel);
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
            // labelUsername
            // 
            resources.ApplyResources(this.labelUsername, "labelUsername");
            this.labelUsername.ForeColor = System.Drawing.Color.DimGray;
            this.labelUsername.Name = "labelUsername";
            // 
            // textBoxUserName
            // 
            resources.ApplyResources(this.textBoxUserName, "textBoxUserName");
            this.textBoxUserName.BackColor = System.Drawing.Color.Gainsboro;
            this.textBoxUserName.Name = "textBoxUserName";
            // 
            // textBoxPassword
            // 
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.textBoxPassword.Name = "textBoxPassword";
            // 
            // labelPassword
            // 
            resources.ApplyResources(this.labelPassword, "labelPassword");
            this.labelPassword.ForeColor = System.Drawing.Color.DimGray;
            this.labelPassword.Name = "labelPassword";
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
            // CustomerConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelUsername);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.Menu = this.mainMenuCustomerConfiguration;
            this.Name = "CustomerConfigurationForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.PictureBox header;
        private System.Windows.Forms.Label title;
    }
}