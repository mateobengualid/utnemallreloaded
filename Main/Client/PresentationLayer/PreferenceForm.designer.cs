namespace UtnEmall.Client.PresentationLayer
{
    partial class PreferenceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuPreference;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferenceForm));
            this.mainMenuPreference = new System.Windows.Forms.MainMenu();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemBack = new System.Windows.Forms.MenuItem();
            this.header = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenuPreference
            // 
            this.mainMenuPreference.MenuItems.Add(this.menuItemSave);
            this.mainMenuPreference.MenuItems.Add(this.menuItemBack);
            // 
            // menuItemSave
            // 
            resources.ApplyResources(this.menuItemSave, "menuItemSave");
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemBack
            // 
            resources.ApplyResources(this.menuItemBack, "menuItemBack");
            this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
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
            // PreferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.Menu = this.mainMenuPreference;
            this.Name = "PreferenceForm";
            this.Load += new System.EventHandler(this.PreferenceForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemBack;
        private System.Windows.Forms.PictureBox header;
        private System.Windows.Forms.Label title;
    }
}