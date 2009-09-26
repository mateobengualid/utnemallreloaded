namespace UtnEmall.Client.PresentationLayer
{
    partial class PrincipalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuPrincipal;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrincipalForm));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem();
            this.mainMenuPrincipal = new System.Windows.Forms.MainMenu();
            this.menuItemSelect = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.listViewOptions = new System.Windows.Forms.ListView();
            this.logo = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mainMenuPrincipal
            // 
            this.mainMenuPrincipal.MenuItems.Add(this.menuItemSelect);
            this.mainMenuPrincipal.MenuItems.Add(this.menuItemExit);
            // 
            // menuItemSelect
            // 
            resources.ApplyResources(this.menuItemSelect, "menuItemSelect");
            this.menuItemSelect.Click += new System.EventHandler(this.menuItemSelect_Click);
            // 
            // menuItemExit
            // 
            resources.ApplyResources(this.menuItemExit, "menuItemExit");
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // listViewOptions
            // 
            resources.ApplyResources(this.listViewOptions, "listViewOptions");
            this.listViewOptions.BackColor = System.Drawing.Color.White;
            listViewItem1.BackColor = System.Drawing.Color.White;
            listViewItem1.Checked = true;
            listViewItem1.ForeColor = System.Drawing.Color.DimGray;
            resources.ApplyResources(listViewItem1, "listViewItem1");
            listViewItem1.Text = resources.GetString("listViewOptions.Items");
            listViewItem2.BackColor = System.Drawing.Color.White;
            listViewItem2.ForeColor = System.Drawing.Color.DimGray;
            resources.ApplyResources(listViewItem2, "listViewItem2");
            listViewItem2.Text = resources.GetString("listViewOptions.Items1");
            listViewItem3.BackColor = System.Drawing.Color.White;
            listViewItem3.ForeColor = System.Drawing.Color.DimGray;
            resources.ApplyResources(listViewItem3, "listViewItem3");
            listViewItem3.Text = resources.GetString("listViewOptions.Items2");
            listViewItem4.BackColor = System.Drawing.Color.White;
            listViewItem4.ForeColor = System.Drawing.Color.DimGray;
            resources.ApplyResources(listViewItem4, "listViewItem4");
            listViewItem4.Text = resources.GetString("listViewOptions.Items3");
            this.listViewOptions.Items.Add(listViewItem1);
            this.listViewOptions.Items.Add(listViewItem2);
            this.listViewOptions.Items.Add(listViewItem3);
            this.listViewOptions.Items.Add(listViewItem4);
            this.listViewOptions.Name = "listViewOptions";
            this.listViewOptions.SelectedIndexChanged += new System.EventHandler(this.listViewOptions_SelectedIndexChanged);
            // 
            // logo
            // 
            resources.ApplyResources(this.logo, "logo");
            this.logo.BackColor = System.Drawing.Color.White;
            this.logo.Name = "logo";
            // 
            // PrincipalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.logo);
            this.Controls.Add(this.listViewOptions);
            this.Menu = this.mainMenuPrincipal;
            this.Name = "PrincipalForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSelect;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.ListView listViewOptions;
        private System.Windows.Forms.PictureBox logo;

    }
}

