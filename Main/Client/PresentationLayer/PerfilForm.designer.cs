namespace Janus.Client.PresentationLayer
{
    partial class PerfilForm
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
            this.mainMenuPerfil = new System.Windows.Forms.MainMenu();
            this.menuItemChange = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPreferences = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mainMenuPerfil
            // 
            this.mainMenuPerfil.MenuItems.Add(this.menuItemChange);
            this.mainMenuPerfil.MenuItems.Add(this.menuItemCancel);
            // 
            // menuItemChange
            // 
            this.menuItemChange.Text = "Change";
            this.menuItemChange.Click += new System.EventHandler(this.menuItemChange_Click);
            // 
            // menuItemCancel
            // 
            this.menuItemCancel.Text = "Cancel";
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 22);
            this.label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtName.Location = new System.Drawing.Point(0, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(176, 22);
            this.txtName.TabIndex = 2;
            this.txtName.GotFocus += new System.EventHandler(this.txtName_GotFocus);
            this.txtName.LostFocus += new System.EventHandler(this.txtName_LostFocus);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 22);
            this.label2.Text = "Surname";
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(0, 81);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(176, 22);
            this.txtSurname.TabIndex = 4;
            this.txtSurname.GotFocus += new System.EventHandler(this.txtSurname_GotFocus);
            this.txtSurname.LostFocus += new System.EventHandler(this.txtSurname_LostFocus);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(0, 185);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(176, 22);
            this.txtAddress.TabIndex = 8;
            this.txtAddress.GotFocus += new System.EventHandler(this.txtAddress_GotFocus);
            this.txtAddress.LostFocus += new System.EventHandler(this.txtAddress_LostFocus);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 22);
            this.label3.Text = "Address";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Location = new System.Drawing.Point(0, 132);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(176, 22);
            this.txtPhoneNumber.TabIndex = 7;
            this.txtPhoneNumber.GotFocus += new System.EventHandler(this.txtPhoneNumber_GotFocus);
            this.txtPhoneNumber.LostFocus += new System.EventHandler(this.txtPhoneNumber_LostFocus);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 22);
            this.label4.Text = "Phone Number";
            // 
            // txtPreferences
            // 
            this.txtPreferences.Location = new System.Drawing.Point(0, 209);
            this.txtPreferences.Name = "txtPreferences";
            this.txtPreferences.ReadOnly = true;
            this.txtPreferences.Size = new System.Drawing.Size(176, 22);
            this.txtPreferences.TabIndex = 11;
            this.txtPreferences.Text = "Preferences";
            // 
            // PerfilForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(169, 173);
            this.Controls.Add(this.txtPreferences);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSurname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Menu = this.mainMenuPerfil;
            this.Name = "PerfilForm";
            this.Text = "Customer Profile";
            this.Load += new System.EventHandler(this.PerfilForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PerfilForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemChange;
        private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPreferences;
    }
}