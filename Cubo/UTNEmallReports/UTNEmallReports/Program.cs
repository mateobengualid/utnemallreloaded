using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace UTNEmallReports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("es-AR");

            Form init = null;

            string arg = args.Length > 0 ? args[0] : String.Empty;
            switch (arg.ToLower())
            {
                case "historialusodeservicios":
                    init = new HistorialUsoDeServicios();
                    break;
                case "registrosmasvistos":
                    init = new RegistrosMasVistos();
                    break;
                case "serviciosporestadocivil":
                    init = new ServiciosPorEstadoCivil();
                    break;
                case "informepersonalizado":
                    init = new InformePersonalizado();
                    break;
                case "graficopersonalizado":
                    break;
                default:
                    init = new ServiciosPorSexo();
                    break;
            }

            Application.Run(init);
        }

        internal static void ExportToExcel(AxMicrosoft.Office.Interop.Owc11.AxPivotTable axPivotTable)
        {
            try
            {
                axPivotTable.Export(null, Microsoft.Office.Interop.Owc11.PivotExportActionEnum.plExportActionOpenInExcel);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al intentar exportar a Excel.", "No se pudo exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine("Error: " + error);
            }
        }
    }
}
