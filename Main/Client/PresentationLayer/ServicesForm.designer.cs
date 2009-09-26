namespace UtnEmall.Client.PresentationLayer
{
    partial class ServicesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuServices;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicesForm));
            this.mainMenuServices = new System.Windows.Forms.MainMenu();
            this.menuItemSelect = new System.Windows.Forms.MenuItem();
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemBack = new System.Windows.Forms.MenuItem();
            this.menuItemRemove = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemShowRemovedServices = new System.Windows.Forms.MenuItem();
            this.listViewServices = new System.Windows.Forms.ListView();
            this.title = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mainMenuServices
            // 
            this.mainMenuServices.MenuItems.Add(this.menuItemSelect);
            this.mainMenuServices.MenuItems.Add(this.menuItemOptions);
            // 
            // menuItemSelect
            // 
            resources.ApplyResources(this.menuItemSelect, "menuItemSelect");
            this.menuItemSelect.Click += new System.EventHandler(this.menuItemSelect_Click);
            // 
            // menuItemOptions
            // 
            resources.ApplyResources(this.menuItemOptions, "menuItemOptions");
            this.menuItemOptions.MenuItems.Add(this.menuItemBack);
            this.menuItemOptions.MenuItems.Add(this.menuItemRemove);
            this.menuItemOptions.MenuItems.Add(this.menuItemHelp);
            this.menuItemOptions.MenuItems.Add(this.menuItem1);
            this.menuItemOptions.MenuItems.Add(this.menuItemShowRemovedServices);
            // 
            // menuItemBack
            // 
            resources.ApplyResources(this.menuItemBack, "menuItemBack");
            this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
            // 
            // menuItemRemove
            // 
            resources.ApplyResources(this.menuItemRemove, "menuItemRemove");
            this.menuItemRemove.Click += new System.EventHandler(this.menuItemRemove_Click);
            // 
            // menuItemHelp
            // 
            resources.ApplyResources(this.menuItemHelp, "menuItemHelp");
            this.menuItemHelp.Click += new System.EventHandler(this.menuItemHelp_Click);
            // 
            // menuItem1
            // 
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // menuItemShowRemovedServices
            // 
            resources.ApplyResources(this.menuItemShowRemovedServices, "menuItemShowRemovedServices");
            this.menuItemShowRemovedServices.Click += new System.EventHandler(this.menuItemShowRemovedServices_Click);
            // 
            // listViewServices
            // 
            resources.ApplyResources(this.listViewServices, "listViewServices");
            this.listViewServices.ForeColor = System.Drawing.Color.DimGray;
            this.listViewServices.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewServices.Name = "listViewServices";
            this.listViewServices.View = System.Windows.Forms.View.SmallIcon;
            // 
            // title
            // 
            resources.ApplyResources(this.title, "title");
            this.title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(137)))), ((int)(((byte)(145)))));
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Name = "title";
            // 
            // header
            // 
            resources.ApplyResources(this.header, "header");
            this.header.Name = "header";
            // 
            // ServicesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.Controls.Add(this.listViewServices);
            this.Menu = this.mainMenuServices;
            this.Name = "ServicesForm";
            this.Load += new System.EventHandler(this.ServicesForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewServices;
        private System.Windows.Forms.MenuItem menuItemSelect;
        private System.Windows.Forms.MenuItem menuItemOptions;
        private System.Windows.Forms.MenuItem menuItemRemove;
        private System.Windows.Forms.MenuItem menuItemBack;
        private System.Windows.Forms.MenuItem menuItemHelp;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.PictureBox header;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemShowRemovedServices;
    }
}