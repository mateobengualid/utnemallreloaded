using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTNEmallReports
{
    public partial class HistorialUsoDeServicios : Form
    {
        public HistorialUsoDeServicios()
        {
            InitializeComponent();
        }

        private void imprimir_Click(object sender, EventArgs e)
        {
            try
            {
                this.axChartSpace1.ExportPicture("historialusodeservicios.png", "png", 1024, 768);
                new PrintReport("historialusodeservicios.png");
            }
            catch (Exception error)
            {
            }

        }
    }
}
