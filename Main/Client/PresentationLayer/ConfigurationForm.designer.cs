namespace UtnEmall.Client.PresentationLayer
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuConfiguration;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem();
            this.mainMenuConfiguration = new System.Windows.Forms.MainMenu();
            this.menuItemSelect = new System.Windows.Forms.MenuItem();
            this.menuItemBack = new System.Windows.Forms.MenuItem();
            this.listViewConfigurationOption = new System.Windows.Forms.ListView();
            this.logo = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mainMenuConfiguration
            // 
            this.mainMenuConfiguration.MenuItems.Add(this.menuItemSelect);
            this.mainMenuConfiguration.MenuItems.Add(this.menuItemBack);
            // 
            // menuItemSelect
            // 
            resources.ApplyResources(this.menuItemSelect, "menuItemSelect");
            this.menuItemSelect.Click += new System.EventHandler(this.menuItemSelect_Click);
            // 
            // menuItemBack
            // 
            resources.ApplyResources(this.menuItemBack, "menuItemBack");
            this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
            // 
            // listViewConfigurationOption
            // 
            resources.ApplyResources(this.listViewConfigurationOption, "listViewConfigurationOption");
            this.listViewConfigurationOption.ForeColor = System.Drawing.Color.DimGray;
            listViewItem4.Checked = true;
            resources.ApplyResources(listViewItem4, "listViewItem4");
            listViewItem4.Text = resources.GetString("listViewConfigurationOption.Items");
            resources.ApplyResources(listViewItem5, "listViewItem5");
            listViewItem5.Text = resources.GetString("listViewConfigurationOption.Items1");
            resources.ApplyResources(listViewItem6, "listViewItem6");
            listViewItem6.Text = resources.GetString("listViewConfigurationOption.Items2");
            this.listViewConfigurationOption.Items.Add(listViewItem4);
            this.listViewConfigurationOption.Items.Add(listViewItem5);
            this.listViewConfigurationOption.Items.Add(listViewItem6);
            this.listViewConfigurationOption.Name = "listViewConfigurationOption";
            // 
            // logo
            // 
            resources.ApplyResources(this.logo, "logo");
            this.logo.Name = "logo";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.logo);
            this.Controls.Add(this.listViewConfigurationOption);
            this.Menu = this.mainMenuConfiguration;
            this.Name = "ConfigurationForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSelect;
        private System.Windows.Forms.MenuItem menuItemBack;
        private System.Windows.Forms.ListView listViewConfigurationOption;
        private System.Windows.Forms.PictureBox logo;
    }
}