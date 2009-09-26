namespace UtnEmall.Client.PresentationLayer
{
    partial class ProfileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuPerfil;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileForm));
            this.mainMenuPerfil = new System.Windows.Forms.MainMenu();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemPreferences = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenuPerfil
            // 
            this.mainMenuPerfil.MenuItems.Add(this.menuItemSave);
            this.mainMenuPerfil.MenuItems.Add(this.menuItemOptions);
            // 
            // menuItemSave
            // 
            resources.ApplyResources(this.menuItemSave, "menuItemSave");
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemOptions
            // 
            resources.ApplyResources(this.menuItemOptions, "menuItemOptions");
            this.menuItemOptions.MenuItems.Add(this.menuItemPreferences);
            this.menuItemOptions.MenuItems.Add(this.menuItemCancel);
            // 
            // menuItemPreferences
            // 
            resources.ApplyResources(this.menuItemPreferences, "menuItemPreferences");
            this.menuItemPreferences.Click += new System.EventHandler(this.menuItemPreferences_Click);
            // 
            // menuItemCancel
            // 
            resources.ApplyResources(this.menuItemCancel, "menuItemCancel");
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Name = "label1";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Name = "txtName";
            this.txtName.GotFocus += new System.EventHandler(this.txtName_GotFocus);
            this.txtName.LostFocus += new System.EventHandler(this.txtName_LostFocus);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Name = "label2";
            // 
            // txtSurname
            // 
            resources.ApplyResources(this.txtSurname, "txtSurname");
            this.txtSurname.BackColor = System.Drawing.Color.Gainsboro;
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.GotFocus += new System.EventHandler(this.txtSurname_GotFocus);
            this.txtSurname.LostFocus += new System.EventHandler(this.txtSurname_LostFocus);
            // 
            // txtAddress
            // 
            resources.ApplyResources(this.txtAddress, "txtAddress");
            this.txtAddress.BackColor = System.Drawing.Color.Gainsboro;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.GotFocus += new System.EventHandler(this.txtAddress_GotFocus);
            this.txtAddress.LostFocus += new System.EventHandler(this.txtAddress_LostFocus);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Name = "label3";
            // 
            // txtPhoneNumber
            // 
            resources.ApplyResources(this.txtPhoneNumber, "txtPhoneNumber");
            this.txtPhoneNumber.BackColor = System.Drawing.Color.Gainsboro;
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.GotFocus += new System.EventHandler(this.txtPhoneNumber_GotFocus);
            this.txtPhoneNumber.LostFocus += new System.EventHandler(this.txtPhoneNumber_LostFocus);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Name = "label4";
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
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSurname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.KeyPreview = true;
            this.Menu = this.mainMenuPerfil;
            this.Name = "ProfileForm";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.MenuItem menuItemPreferences;
        private System.Windows.Forms.PictureBox header;
        private System.Windows.Forms.Label title;
    }
}