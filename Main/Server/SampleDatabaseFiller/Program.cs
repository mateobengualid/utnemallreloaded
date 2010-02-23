using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.Base;
using UtnEmall.Server.DataModel;
using System.Data;

namespace SampleDatabaseFiller
{
    class Program
    {
        static IDbConnection conn;

        static void Main(string[] args)
        {
            Console.WriteLine("Abriendo conexion a base de datos");

            conn = DataAccessConnection.Instance.GetNewConnection();

            FillRegistersForLibrary();
            FillRegistersForCinema();
            FillRegistersForAccesories();
            FillRegistersForMall();

            Console.WriteLine("Fin");
            Console.Read();
        }

        private static void FillRegistersForMall()
        {
            Console.WriteLine("Llenando registros para Mall");
        }

        private static void FillRegistersForAccesories()
        {
            Console.WriteLine("Llenando registros para Accesorios");
            new AccesoriesFiller(conn).Run();
        }

        private static void FillRegistersForCinema()
        {
            Console.WriteLine("Llenando registros para Cine");
        }

        private static void FillRegistersForLibrary()
        {
            Console.WriteLine("Llenando registros para Libreria");
        }


    }
}
