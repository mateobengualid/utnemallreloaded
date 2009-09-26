namespace UtnEmall.Client.PresentationLayer
{
    partial class DeviceProfileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenuDeviceProfile;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceProfileForm));
            this.mainMenuDeviceProfile = new System.Windows.Forms.MainMenu();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.textBoxDeviceType = new System.Windows.Forms.TextBox();
            this.textBoxDeviceModel = new System.Windows.Forms.TextBox();
            this.textBoxMacAddress = new System.Windows.Forms.TextBox();
            this.labelType = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.labelMacAddress = new System.Windows.Forms.Label();
            this.labelWindowsMobileVersion = new System.Windows.Forms.Label();
            this.textBoxWindowsMobileVersion = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mainMenuDeviceProfile
            // 
            this.mainMenuDeviceProfile.MenuItems.Add(this.menuItemSave);
            this.mainMenuDeviceProfile.MenuItems.Add(this.menuItemCancel);
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
            // textBoxDeviceType
            // 
            resources.ApplyResources(this.textBoxDeviceType, "textBoxDeviceType");
            this.textBoxDeviceType.Name = "textBoxDeviceType";
            // 
            // textBoxDeviceModel
            // 
            resources.ApplyResources(this.textBoxDeviceModel, "textBoxDeviceModel");
            this.textBoxDeviceModel.Name = "textBoxDeviceModel";
            // 
            // textBoxMacAddress
            // 
            resources.ApplyResources(this.textBoxMacAddress, "textBoxMacAddress");
            this.textBoxMacAddress.Name = "textBoxMacAddress";
            this.textBoxMacAddress.ReadOnly = true;
            // 
            // labelType
            // 
            resources.ApplyResources(this.labelType, "labelType");
            this.labelType.Name = "labelType";
            // 
            // labelModel
            // 
            resources.ApplyResources(this.labelModel, "labelModel");
            this.labelModel.Name = "labelModel";
            // 
            // labelMacAddress
            // 
            resources.ApplyResources(this.labelMacAddress, "labelMacAddress");
            this.labelMacAddress.Name = "labelMacAddress";
            // 
            // labelWindowsMobileVersion
            // 
            resources.ApplyResources(this.labelWindowsMobileVersion, "labelWindowsMobileVersion");
            this.labelWindowsMobileVersion.Name = "labelWindowsMobileVersion";
            // 
            // textBoxWindowsMobileVersion
            // 
            resources.ApplyResources(this.textBoxWindowsMobileVersion, "textBoxWindowsMobileVersion");
            this.textBoxWindowsMobileVersion.Name = "textBoxWindowsMobileVersion";
            // 
            // DeviceProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.labelWindowsMobileVersion);
            this.Controls.Add(this.textBoxWindowsMobileVersion);
            this.Controls.Add(this.labelMacAddress);
            this.Controls.Add(this.labelModel);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.textBoxMacAddress);
            this.Controls.Add(this.textBoxDeviceModel);
            this.Controls.Add(this.textBoxDeviceType);
            this.Menu = this.mainMenuDeviceProfile;
            this.Name = "DeviceProfileForm";
            this.Load += new System.EventHandler(this.DeviceProfile_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDeviceType;
        private System.Windows.Forms.TextBox textBoxDeviceModel;
        private System.Windows.Forms.TextBox textBoxMacAddress;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label labelMacAddress;
        private System.Windows.Forms.Label labelWindowsMobileVersion;
        private System.Windows.Forms.TextBox textBoxWindowsMobileVersion;
        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemCancel;
    }
}