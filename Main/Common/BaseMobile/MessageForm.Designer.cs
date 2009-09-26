namespace UtnEmall.Client.PresentationLayer
{
    partial class MessageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemBegin = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.labelMessage = new System.Windows.Forms.Label();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemBegin);
            this.mainMenu1.MenuItems.Add(this.menuItemExit);
            // 
            // menuItemBegin
            // 
            this.menuItemBegin.Text = "Begin";
            this.menuItemBegin.Click += new System.EventHandler(this.menuItemBegin_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Segoe Condensed", 11F, System.Drawing.FontStyle.Bold);
            this.labelMessage.Location = new System.Drawing.Point(3, 105);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(150, 95);
            this.labelMessage.Text = "message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Location = new System.Drawing.Point(43, 56);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(100, 88);
            // 
            // title
            // 
            this.title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(137)))), ((int)(((byte)(145)))));
            this.title.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(176, 24);
            this.title.Text = "Message";
            // 
            // header
            // 
            this.header.Image = ((System.Drawing.Image)(resources.GetObject("header.Image")));
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(176, 50);
            this.header.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(169, 173);
            this.Controls.Add(this.title);
            this.Controls.Add(this.header);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.labelMessage);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "MessageForm";
            this.Text = "MessageForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.MenuItem menuItemBegin;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.PictureBox header;
    }
}