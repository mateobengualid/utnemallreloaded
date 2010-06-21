using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace UTNEmallReports
{
    public partial class ServiciosPorSexo : Form
    {
        public ServiciosPorSexo()
        {
            InitializeComponent();
        }

        private void imprimir_Click(object sender, EventArgs e)
        {
            try
            {
                this.axChartSpace1.ExportPicture("test.png", "png", 1024, 768);
                new PrintReport("test.png");
            }
            catch (Exception error)
            {
            }
        }
    }
}
