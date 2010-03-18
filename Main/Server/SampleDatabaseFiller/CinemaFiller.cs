using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.Base;
using UtnEmall.Server.DataModel;
using System.Data;
using UtnEmall.Store855.EntityModel;
using UtnEmall.Store855;

namespace SampleDatabaseFiller
{
    class CinemaFiller
    {
        IDbConnection conn;

        Dictionary<string, int> _categorias = new Dictionary<string, int>();
        List<int> _salas = new List<int>();
        Dictionary<string, int> _peliculas = new Dictionary<string, int>();


        public CinemaFiller(IDbConnection connection)
        {
            this.conn = connection;
        }

        public void Run()
        {
            // Llenar salas
            FillSalas();
            // Llenar categorias
            FillCategorias();
            // Llenar peliculas
            FillPeliculas();
            // Llenar categorias a peliculas
            FillCategoriaXPeliculas();
            // Llenar funciones
            FillFunciones();
        }

        private void FillFunciones()
        {
        }

        private void FillCategoriaXPeliculas()
        {
        }

        private void FillPeliculas()
        {
        }

        private void FillCategorias()
        {
        }

        private void FillSalas()
        {
            var da = new Salas_01199597104DataAccess();
            var salas = da.LoadAll(false);
            if (salas.Count == 0)
            {
                salas = new System.Collections.ObjectModel.Collection<Salas_01199597104Entity>(
                    new Salas_01199597104Entity[]{
                        new Salas_01199597104Entity{ Nombre = "A1", Capacidad = 80 },
                        new Salas_01199597104Entity{ Nombre = "A2", Capacidad = 80 },
                        new Salas_01199597104Entity{ Nombre = "A3", Capacidad = 60 },
                        new Salas_01199597104Entity{ Nombre = "B1", Capacidad = 100 },
                        new Salas_01199597104Entity{ Nombre = "B2", Capacidad = 100 },
                    });
            }

            foreach (var item in salas)
            {
                da.Save(item);
                _salas.Add(item.Id);
            }
        }
    }
}
