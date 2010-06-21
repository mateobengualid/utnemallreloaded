namespace UTNEmallReports
{
    partial class RegistrosMasVistos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistrosMasVistos));
            this.axPivotTable1 = new AxMicrosoft.Office.Interop.Owc11.AxPivotTable();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axPivotTable1)).BeginInit();
            this.SuspendLayout();
            // 
            // axPivotTable1
            // 
            this.axPivotTable1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axPivotTable1.DataSource = null;
            this.axPivotTable1.Enabled = true;
            this.axPivotTable1.Location = new System.Drawing.Point(12, 12);
            this.axPivotTable1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 50);
            this.axPivotTable1.Name = "axPivotTable1";
            this.axPivotTable1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPivotTable1.OcxState")));
            this.axPivotTable1.Size = new System.Drawing.Size(930, 443);
            this.axPivotTable1.TabIndex = 0;
            this.axPivotTable1.Enter += new System.EventHandler(this.axPivotTable1_Enter);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(800, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Exportar a Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RegistrosMasVistos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 495);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.axPivotTable1);
            this.Name = "RegistrosMasVistos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registros más vistos por sexo y edad";
            ((System.ComponentModel.ISupportInitialize)(this.axPivotTable1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMicrosoft.Office.Interop.Owc11.AxPivotTable axPivotTable1;
        private System.Windows.Forms.Button button1;
    }
}