namespace UtnEmall.Client.PresentationLayer
{
    partial class HelpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuHelp;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.mainMenuHelp = new System.Windows.Forms.MainMenu();
            this.menuItemOk = new System.Windows.Forms.MenuItem();
            this.textBoxHelp = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpText = new System.Windows.Forms.TextBox();
            this.header = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuHelp
            // 
            this.mainMenuHelp.MenuItems.Add(this.menuItemOk);
            // 
            // menuItemOk
            // 
            resources.ApplyResources(this.menuItemOk, "menuItemOk");
            this.menuItemOk.Click += new System.EventHandler(this.menuItemOk_Click);
            // 
            // textBoxHelp
            // 
            resources.ApplyResources(this.textBoxHelp, "textBoxHelp");
            this.textBoxHelp.Name = "textBoxHelp";
            this.textBoxHelp.ReadOnly = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.helpText);
            this.panel1.Name = "panel1";
            // 
            // helpText
            // 
            resources.ApplyResources(this.helpText, "helpText");
            this.helpText.BackColor = System.Drawing.Color.White;
            this.helpText.ForeColor = System.Drawing.Color.Black;
            this.helpText.Name = "helpText";
            this.helpText.ReadOnly = true;
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
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxHelp);
            this.Menu = this.mainMenuHelp;
            this.Name = "HelpForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxHelp;
        private System.Windows.Forms.MenuItem menuItemOk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox header;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.TextBox helpText;
    }
}