using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace UTNEmallReports
{
    class PrintReport
    {
        PrintDocument _doc;
        string _imageFileName;

        public PrintReport(string imageFileName)
        {
            _doc = new PrintDocument();
            _imageFileName = imageFileName;
            try
            {
                _doc.PrintPage += new PrintPageEventHandler(_doc_PrintPage);
                _doc.PrinterSettings.DefaultPageSettings.Landscape = true;
                _doc.PrinterSettings.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
                var dialog = new PrintDialog();
                dialog.Document = _doc;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PrintPreviewDialog prev = new PrintPreviewDialog();
                    prev.Document = _doc;
                    prev.ShowDialog();
                    prev.Dispose();
                    //_doc.Print();
                }
                dialog.Dispose();
                _doc.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error imprimiendo.", "UTN Emall", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void _doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var image = Image.FromFile(_imageFileName);
            var rect = e.MarginBounds;

            var pageUnit = e.Graphics.PageUnit;
            e.Graphics.DrawImage(image,
                rect,
                image.GetBounds(ref pageUnit), pageUnit);
            e.Graphics.DrawString(
                "UTN Mobile Mall", new Font("Arial", 14, FontStyle.Bold), Brushes.Black,
                new PointF(e.MarginBounds.Left, e.MarginBounds.Top != 0 ? e.MarginBounds.Top / 2 : 1));
        }

    }
}
