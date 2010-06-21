namespace UTNEmallReports
{
    partial class ServiciosPorSexo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiciosPorSexo));
            this.axChartSpace1 = new AxMicrosoft.Office.Interop.Owc11.AxChartSpace();
            this.imprimir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axChartSpace1)).BeginInit();
            this.SuspendLayout();
            // 
            // axChartSpace1
            // 
            this.axChartSpace1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axChartSpace1.DataSource = null;
            this.axChartSpace1.Enabled = true;
            this.axChartSpace1.Location = new System.Drawing.Point(12, 12);
            this.axChartSpace1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.axChartSpace1.Name = "axChartSpace1";
            this.axChartSpace1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axChartSpace1.OcxState")));
            this.axChartSpace1.Size = new System.Drawing.Size(743, 441);
            this.axChartSpace1.TabIndex = 0;
            // 
            // imprimir
            // 
            this.imprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.imprimir.Location = new System.Drawing.Point(637, 458);
            this.imprimir.Name = "imprimir";
            this.imprimir.Size = new System.Drawing.Size(108, 27);
            this.imprimir.TabIndex = 1;
            this.imprimir.Text = "Imprimir";
            this.imprimir.UseVisualStyleBackColor = true;
            this.imprimir.Click += new System.EventHandler(this.imprimir_Click);
            // 
            // ServiciosPorSexo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 492);
            this.Controls.Add(this.imprimir);
            this.Controls.Add(this.axChartSpace1);
            this.Name = "ServiciosPorSexo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UTN Emall Reportes";
            ((System.ComponentModel.ISupportInitialize)(this.axChartSpace1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMicrosoft.Office.Interop.Owc11.AxChartSpace axChartSpace1;
        private System.Windows.Forms.Button imprimir;
    }
}

