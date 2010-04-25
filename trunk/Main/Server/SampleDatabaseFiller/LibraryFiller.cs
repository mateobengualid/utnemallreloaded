using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.Base;
using UtnEmall.Server.DataModel;
using System.Data;
using UtnEmall.Store930.EntityModel;
using UtnEmall.Store930;

namespace SampleDatabaseFiller
{
    class LibraryFiller
    {
        IDbConnection conn;
        Random _random = new Random();
        Dictionary<string, int> _categorias = new Dictionary<string, int>();
        List<int> _origenes = new List<int>();
        List<int> _autores = new List<int>();

        public LibraryFiller(IDbConnection connection)
        {
            this.conn = connection;
        }

        public void Run()
        {
            FillCategorias();
            FillOrigenAutor();
            FillAutores();
            FillLibros();
        }

        private void FillOrigenAutor()
        {
            OrigenAutor_00967889501DataAccess da = new OrigenAutor_00967889501DataAccess();
            var origenes = da.LoadAll(false);
            if (origenes.Count == 0)
            {
                origenes = new System.Collections.ObjectModel.Collection<OrigenAutor_00967889501Entity>(
                    new OrigenAutor_00967889501Entity[]{
                        new OrigenAutor_00967889501Entity{ Origen = "Español" },
                        new OrigenAutor_00967889501Entity{ Origen = "Latinoamericano" },
                        new OrigenAutor_00967889501Entity{ Origen = "Italiano" },
                        new OrigenAutor_00967889501Entity{ Origen = "Portugues" },
                        new OrigenAutor_00967889501Entity{ Origen = "Ruso" },
                    }
                    );
            }
            foreach (var origen in origenes)
            {
                da.Save(origen);
                _origenes.Add(origen.Id);
            }
        }

        private void FillLibros()
        {
            Libros_01133584615DataAccess da = new Libros_01133584615DataAccess();
            var libros = da.LoadAll(false);
            if (libros.Count == 0)
            {
                NewLibro(da, "Pensar Como Rico Para Hacerse Rico", "$ 28,00", "Administración", @".\Libros\libro1.jpg");
                NewLibro(da, "Historia Economica De La Empresa", "$ 154,00", "Administración", @".\Libros\libro2.jpg");
                NewLibro(da, "Su Jefe Esta Loco ?", "$ 49,00", "Administración", @".\Libros\libro3.jpg");
                NewLibro(da, "Administracion General", "$ 35,00", "Administración", @".\Libros\libro4.jpg");
                NewLibro(da, "Liderazgo Inteligente", "$ 29,70", "Administración", @".\Libros\libro5.jpg");
                NewLibro(da, "4. Manual De Autogestion Para Organizaciones Sin Fines De Lucro", "$ 44,00", "Administración", @".\Libros\libro1.jpg");
                NewLibro(da, "El Millonario Instantaneo", "$ 45,00", "Administración", @".\Libros\libro2.jpg");
                NewLibro(da, "Los Manuscritos Del Mar Muerto", "$ 120,00", "Arqueologia", @".\Libros\libro3.jpg");
                NewLibro(da, "Arqueologia", "$ 216,00", "Arqueologia", @".\Libros\libro4.jpg");
                NewLibro(da, "La Proteccion Del Patrimonio Cultural Argentino", "$ 150,00", "Arqueologia", @".\Libros\libro5.jpg");
                NewLibro(da, "El Gran Calentamiento", "$ 93,00", "Arqueologia", @".\Libros\libro1.jpg");
                NewLibro(da, "Tutankhamon", "$ 610,00", "Arqueologia", @".\Libros\libro2.jpg");
                NewLibro(da, "Arqueologia Del Lenguaje", "$ 133,00", "Arqueologia", @".\Libros\libro3.jpg");
                NewLibro(da, "El Poblamiento De America", "$ 25,00", "Arqueologia", @".\Libros\libro4.jpg");
                NewLibro(da, "El Caballo", "$ 109,00", "Equitación", @".\Libros\libro5.jpg");
                NewLibro(da, "Equitacion Centrada", "$ 82,00", "Equitación", @".\Libros\libro1.jpg");
                NewLibro(da, "Bienvenidos Al Mundo De Las Carreras De Caballos", "$ 95,00", "Equitación", @".\Libros\libro2.jpg");
                NewLibro(da, "Viajes A Caballo", "$ 179,00", "Equitación", @".\Libros\libro3.jpg");
                NewLibro(da, "Manual Completo De Equitacion", "$ 472,50", "Equitación", @".\Libros\libro4.jpg");
                NewLibro(da, "Iniciacion Al Salto", "$ 162,50", "Equitación", @".\Libros\libro5.jpg");
                NewLibro(da, "Como Ganarse Las Espuelas", "$ 240,00", "Equitación", @".\Libros\libro1.jpg");
                NewLibro(da, "Equitacion Manual De Trabajo Con Cavaletti", "$ 207,00", "Equitación", @".\Libros\libro2.jpg");
                NewLibro(da, "Horse Racing", "$ 44,00", "Equitación", @".\Libros\libro3.jpg");
                NewLibro(da, "Manual De Hipica", "$ 143,00", "Equitación", @".\Libros\libro4.jpg");
                NewLibro(da, "El Equipo Del Caballo", "$ 49,00", "Equitación", @".\Libros\libro5.jpg");
                NewLibro(da, "Estadistica Aplicada Basica", "$ 219,50", "Estadistica", @".\Libros\libro1.jpg");
                NewLibro(da, "Estadistica Para Las Ciencias Del Comportamiento", "$ 138,00", "Estadistica", @".\Libros\libro2.jpg");
                NewLibro(da, "Tratamiento De Datos", "$ 130,00", "Estadistica", @".\Libros\libro3.jpg");
                NewLibro(da, "Diseño De Experimentos", "$ 169,00", "Estadistica", @".\Libros\libro4.jpg");
                NewLibro(da, "Prediccion Estadistica En Condiciones De Incertidumbre", "$ 99,00", "Estadistica", @".\Libros\libro5.jpg");
                NewLibro(da, "Analisis Multivariante Aplicado", "$ 175,00", "Estadistica", @".\Libros\libro1.jpg");
                NewLibro(da, "Problemas De Inferencia Estadistica", "$ 149,00", "Estadistica", @".\Libros\libro2.jpg");
                NewLibro(da, "Inferencia Estadistica", "$ 142,00", "Estadistica", @".\Libros\libro3.jpg");
                NewLibro(da, "55 Respuestas A Dudas Tipicas De Estadistica", "$ 98,00", "Estadistica", @".\Libros\libro4.jpg");
                NewLibro(da, "Estadistica Aplicada", "$ 108,00", "Estadistica", @".\Libros\libro5.jpg");
                NewLibro(da, "Los Cataros", "$ 113,00", "Historia", @".\Libros\libro1.jpg");
                NewLibro(da, "Las Supersticiones En La Edad Media", "$ 49,00", "Historia", @".\Libros\libro2.jpg");
                NewLibro(da, "Mahoma Y Carlomagno", "$ 135,50", "Historia", @".\Libros\libro3.jpg");
                NewLibro(da, "Cruzadas", "$ 80,00", "Historia", @".\Libros\libro4.jpg");
                NewLibro(da, "Los Intelectuales En La Edad Media", "$ 71,00", "Historia", @".\Libros\libro5.jpg");
                NewLibro(da, "El Otoño De La Edad Media", "$ 234,50", "Historia", @".\Libros\libro1.jpg");
                NewLibro(da, "La Edad Media Explicada A Los Jovenes", "$ 49,00", "Historia", @".\Libros\libro2.jpg");
                NewLibro(da, "Amor En La Edad Media", "$ 139,00", "Historia", @".\Libros\libro3.jpg");
                NewLibro(da, "La Caballeria", "$ 149,00", "Historia", @".\Libros\libro4.jpg");
                NewLibro(da, "Las Ciudades De La Edad Media", "$ 54,00", "Historia", @".\Libros\libro5.jpg");
                NewLibro(da, "El Oso", "$ 139,00", "Historia", @".\Libros\libro1.jpg");
                NewLibro(da, "Una Historia Simbolica De La Edad Media Occidental", "$ 100,00", "Historia", @".\Libros\libro2.jpg");
                NewLibro(da, "Una Larga Edad Media", "$ 117,00", "Historia", @".\Libros\libro3.jpg");
                NewLibro(da, "Inteligencia Artificial", "$ 74,90", "Inteligencia Artificial", @".\Libros\libro4.jpg");
                NewLibro(da, "Redes Neuronales", "$ 71,00", "Inteligencia Artificial", @".\Libros\libro5.jpg");
                NewLibro(da, "Inteligencia Artificial", "$ 152,00", "Inteligencia Artificial", @".\Libros\libro1.jpg");
                NewLibro(da, "Redes Neurales", "$ 144,00", "Inteligencia Artificial", @".\Libros\libro2.jpg");
                NewLibro(da, "Redes Neuronales Y Sistemas Borrosos", "$ 72,00", "Inteligencia Artificial", @".\Libros\libro3.jpg");
                NewLibro(da, "Aprendizaje Automatico", "$ 159,00", "Inteligencia Artificial", @".\Libros\libro4.jpg");
                NewLibro(da, "Inteligencia Artificial E Ingenieria Del Conocimiento", "$ 72,00", "Inteligencia Artificial", @".\Libros\libro5.jpg");
                NewLibro(da, "Inteligencia Natural Y Sintetica", "$ 52,00", "Inteligencia Artificial", @".\Libros\libro1.jpg");
                NewLibro(da, "Musica Proyecto Clave A Eso", "$ 49,00", "Música", @".\Libros\libro2.jpg");
                NewLibro(da, "Musica I", "$ 22,00", "Música", @".\Libros\libro3.jpg");
                NewLibro(da, "Musica Ii", "$ 22,00", "Música", @".\Libros\libro4.jpg");
                NewLibro(da, "Bulebu Con Soda", "$ 49,00", "Música", @".\Libros\libro5.jpg");
                NewLibro(da, "Didactica De La Musica", "$ 41,00", "Música", @".\Libros\libro1.jpg");
                NewLibro(da, "Musijugando 1", "$ 25,00", "Música", @".\Libros\libro2.jpg");
                NewLibro(da, "Didactica De La Musica Para Primaria", "$ 119,00", "Música", @".\Libros\libro3.jpg");
                NewLibro(da, "El Proceso Psicosomatico", "$ 163,00", "Psicoanálisis", @".\Libros\libro4.jpg");
                NewLibro(da, "Femenino Melancolico", "$ 99,00", "Psicoanálisis", @".\Libros\libro5.jpg");
                NewLibro(da, "Lo Politico Y El Psicoanalisis", "$ 89,00", "Psicoanálisis", @".\Libros\libro1.jpg");
                NewLibro(da, "Mito Y Poesia En El Psicoanalisis", "$ 119,00", "Psicoanálisis", @".\Libros\libro2.jpg");
                NewLibro(da, "Psiquiatria Y Psicoanalisis", "$ 89,00", "Psicoanálisis", @".\Libros\libro3.jpg");
                NewLibro(da, "Sujeto Encarnado Sujeto Desencarnado", "$ 99,00", "Psicoanálisis", @".\Libros\libro4.jpg");
                NewLibro(da, "Contra La Eternidad", "$ 49,00", "Psicoanálisis", @".\Libros\libro5.jpg");
                NewLibro(da, "2. Conferencias Porteñas", "$ 76,00", "Psicoanálisis", @".\Libros\libro1.jpg");
            }
        }

        private Libros_01133584615Entity NewLibro(Libros_01133584615DataAccess da, string title, string price, string categories, string imagePath)
        {
            var libro = new Libros_01133584615Entity
            {
                Titulo = title,
                Precio = price,
                Imagen = Utilities.ImageToString(imagePath),
            };
            da.Save(libro);
            // add authors
            var ada = new Libros_01133584615_To_Autor_01251655725DataAccess();
            int authorCount = _random.Next(1, 3);
            for (int n = 0; n < authorCount; n++)
            {
                var lib2Aut = new Libros_01133584615_To_Autor_01251655725Entity
                {
                    Autor_01251655725Id = this._autores[this._random.Next(this._autores.Count-1)],
                    Libros_01133584615Id = libro.Id,
                };
                ada.Save(lib2Aut);
            }
            // add categories
            var cda = new Categorias__01697024398_To_Libros_01133584615DataAccess();
            var categoryNames = categories.Split(',');
            foreach(var categoryName in categoryNames){
                var cat2Libro = new Categorias__01697024398_To_Libros_01133584615Entity
                {
                    Libros_01133584615Id = libro.Id,
                    Categorias__01697024398Id = this._categorias[categoryName],
                };
                cda.Save(cat2Libro);
            }
            return libro;
        }

        private void FillCategorias()
        {
            Categorias__01697024398DataAccess da = new Categorias__01697024398DataAccess();
            var categorias = da.LoadAll(false);
            if (categorias.Count == 0)
            {
                categorias = new System.Collections.ObjectModel.Collection<Categorias__01697024398Entity>(
                    new Categorias__01697024398Entity[]{
                        new Categorias__01697024398Entity{ Categoria = "Administración" }, 
                        new Categorias__01697024398Entity{ Categoria = "Arqueologia" }, 
                        new Categorias__01697024398Entity{ Categoria = "Equitación" }, 
                        new Categorias__01697024398Entity{ Categoria = "Estadistica" }, 
                        new Categorias__01697024398Entity{ Categoria = "Historia" }, 
                        new Categorias__01697024398Entity{ Categoria = "Inteligencia Artificial" }, 
                        new Categorias__01697024398Entity{ Categoria = "Música" }, 
                        new Categorias__01697024398Entity{ Categoria = "Psicoanálisis" }, 
                    });
            }
            foreach (var item in categorias)
            {
                da.Save(item);
                _categorias.Add(item.Categoria, item.Id);
            }
        }

        private void FillAutores()
        {
            Autor_01251655725DataAccess da = new Autor_01251655725DataAccess();
            var autores = da.LoadAll(false);
            if (autores.Count == 0)
            {
                autores = new System.Collections.ObjectModel.Collection<Autor_01251655725Entity>(
                    new Autor_01251655725Entity[]{
                        new Autor_01251655725Entity{ Nombre = @"Gonzalez Roldan Benjamin", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Valdaliso Jesus Maria", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Bing Stanley", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Narvaez Jorge Luis", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Blanchard Ken", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Giomi Claudio", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Fisher Mark", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Puech Emile", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Renfrew Colin", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en España desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Berberian Eduardo E.", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en Argentina desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Fagan Brian", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en Argentina desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Hawass Zahi", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en Argentina desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                        new Autor_01251655725Entity{ Nombre = @"Rivera Arrizabalaga", OrigenAutor_00967889501Id = _origenes[_random.Next(_origenes.Count - 1)],	Biografia = @"Nacido en Argentina desde pequeño destaco en como literario prolifico escribiendo su primera obra a los 15 años de edad."},
                    });
            }
            foreach (var item in autores)
            {
                da.Save(item);
                _autores.Add(item.Id);
            }
        }

    }
}
