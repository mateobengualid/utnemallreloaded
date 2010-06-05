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
        Dictionary<string,string> _categoriasXPeliculas = new Dictionary<string,string>();
        Random _random = new Random();

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
            var da = new Funciones_00933784564DataAccess();
            var funcs = da.LoadAll(false);
            if (funcs.Count > 0) return;
            
            foreach (string pelicula in _peliculas.Keys)
            {
                var funciones = new Funciones_00933784564Entity[]{
                    NewFuncion(_peliculas[pelicula], "10:00", "Lunes"), 
                    NewFuncion(_peliculas[pelicula], "15:00", "Lunes"), 
                    NewFuncion(_peliculas[pelicula], "18:00", "Lunes"), 
                    NewFuncion(_peliculas[pelicula], "10:00", "Martes"), 
                    NewFuncion(_peliculas[pelicula], "15:00", "Martes"), 
                    NewFuncion(_peliculas[pelicula], "18:00", "Martes"),
                    NewFuncion(_peliculas[pelicula], "10:00", "Miercoles"), 
                    NewFuncion(_peliculas[pelicula], "15:00", "Miercoles"), 
                    NewFuncion(_peliculas[pelicula], "18:00", "Miercoles"), 
                    NewFuncion(_peliculas[pelicula], "15:00", "Viernes"), 
                    NewFuncion(_peliculas[pelicula], "18:00", "Viernes"),
                    NewFuncion(_peliculas[pelicula], "22:00", "Viernes"), 
                    NewFuncion(_peliculas[pelicula], "15:00", "Sabado"), 
                    NewFuncion(_peliculas[pelicula], "18:00", "Sabado"),
                    NewFuncion(_peliculas[pelicula], "22:00", "Sabado"), 
                    NewFuncion(_peliculas[pelicula], "15:00", "Domingo"), 
                    NewFuncion(_peliculas[pelicula], "18:00", "Domingo"),
                    NewFuncion(_peliculas[pelicula], "22:00", "Domingo"), 
                };

                var da2 = new Funciones_00933784564_To_Salas_01199597104DataAccess();
                foreach (var funcion in funciones)
                {
                    da.Save(funcion);
                    var fts = new Funciones_00933784564_To_Salas_01199597104Entity();
                    fts.Funciones_00933784564Id = funcion.Id;
                    fts.Salas_01199597104Id = _salas[_random.Next(_salas.Count - 1)];
                    da2.Save(fts);
                }
            }
        }

        private Funciones_00933784564Entity NewFuncion(int idPelicula, string hora, string fecha)
        {
            var funcion = new Funciones_00933784564Entity
            {
                Peliculas_00945930857Id = idPelicula,
                Hora = hora,
                Fecha = fecha,
            };
            return funcion;
        }

        private void FillCategoriaXPeliculas()
        {
            var da = new Peliculas_00945930857_To_Categorias__01697024398DataAccess();
            var peliculasKey = _peliculas.Keys.ToArray();
            var peliculasId = _peliculas.Values.ToArray();
            var peliculasXCategorias = da.LoadAll(false);
            if (peliculasXCategorias.Count == 0)
            {
                for(int n = 0; n < _peliculas.Count; n++){
                    string categorias = _categoriasXPeliculas[peliculasKey[n]];
                    foreach (var categoria in categorias.Split(','))
                    {
                        var cxp = new Peliculas_00945930857_To_Categorias__01697024398Entity();
                        cxp.Categorias__01697024398Id = _categorias[categoria.Trim()];
                        cxp.Peliculas_00945930857Id = peliculasId[n];
                        da.Save(cxp);
                    }
                }
            }
        }

        private void FillPeliculas()
        {
            var da = new Peliculas_00945930857DataAccess();
            var peliculas = da.LoadAll(false);
            if (peliculas.Count == 0)
            {
                peliculas = new System.Collections.ObjectModel.Collection<Peliculas_00945930857Entity>(
                    new Peliculas_00945930857Entity[]{
                        NewItem("Invictus",	@".\Peliculas\11166.jpg",	@"Clint Eastwood",	@"Matt Damon, Morgan Freeman. Leleti Khumalo, Marguerite Wheatley, Adjoa Andoh, Julian Lewis Jones, Matt Stern, Patrick Mofokeng, Tony Kgoroge",	@"133 min",	@"Cuenta la vida de Nelson Mandela después de la caida del apartheid en Sudáfrica, durante su primer semestre como presidente que coincidió con el mundial de Rugby de 1995. Mandela fue el lider que",	@"Apta todo público",	@"Drama, Histórica"),
                        NewItem("Sherlock Holmes",	@".\Peliculas\11259.jpg",	@"Guy Ritchie",	@"Jude Law, Robert Downey Jr.. Eddie Marsan, Mark Strong, Kelly Reilly,Rachel McAdams.",	@"126 min",	@"El legendario detective Sherlock Holmes (Robert Downey Jr) y su incondicional compañero Watson (Jude Law), un doctor y veterano de guerra, estarán frente a su más reciente desafío. ",	@"Apta mayores de 13 años",	@"Acción, Suspenso"),
                        NewItem("Alvin y las ardillas 2",	@".\Peliculas\11438.jpg",	@"Betty Thomas",	@"Jason Lee , David Cross, Zachary Levi.",	@"88 min",	@"El trio más famoso de ardillas cantantes regresan en una nueva aventura donde tendrán que aprender a lidiar con las presiones del insituto, la fama y un grupo rival femenino conocido como The ",	@"Apta todo público",	@"Animación, Familiar"),
                        NewItem("Avatar",	@".\Peliculas\11439.jpg",	@"James Cameron",	@"Zoe Saldana, Sam Worthington. Matt Gerald, Dileep Rao, Joel Moore,Stephen Lang, Sigourney Weaver, Michelle Rodriguez, Giovanni Ribisi.",	@"165 min",	@"Ambientada en el Siglo XXII en una pequeña luna llamada Pandora, que gira alrededor de un planeta gigante de gas, viven unas criaturas azules de 3 metros de estatura, con un temperamento pacífico:",	@"Apta mayores de 13 años",	@"Ciencia Ficción"),
                        NewItem("Amor sin escalas",	@".\Peliculas\11440.jpg",	@"Jason Reitman",	@"Vera Farmiga, George Clooney. Jason Bateman, Anna Kendrick, Danny McBride, J.K. Simmons, Melanie Lynskey",	@"108 min",	@"Ryan Bingham (George Clooney) se especializa en despedir empleados para grandes empresas, lo cual lo lleva a viajar por todo el mundo. Después de tantos años pasados en el aire, se encuentra listo",	@"Apta mayores de 13 años",	@"Comedia, Drama"),
                        NewItem("La princesa y el sapo",	@".\Peliculas\11452.jpg",	@"Ron Clements",	@"Desconocido",	@"98 min",	@"Walt Disney Animation Studios presentan el musical LA PRINCESA Y EL SAPO, una comedia animada situada en la gran ciudad de Nueva Orleans. De los creadores de La Sirenita y Aladin, llega esta",	@"Apta todo público",	@"Animación, Familiar"),
                        NewItem("Toy Story 1 3D",	@".\Peliculas\11555.jpg",	@"John Lasseter",	@"Desconocido",	@"79 min",	@"Un niño llamado Andy, ama estar en su habitación jugando con sus juguetes, especialmente con Woody. Pero, ¿que pasa con los juguetes cuando Andy no está con ellos?: están vivos! Woody lo tiene todo",	@"Apta mayores de 13 años",	@"Animación, Familiar"),
                        NewItem("Enamorandome de mi ex",	@".\Peliculas\11619.jpg",	@"Nancy Meyers",	@"Meryl Streep, Steve Martin, Alec Baldwin. James Patrick Stuart, Robert Curtis Brown, Bruce Altman, Lake Bell, John Krasinski, Rita Wilson.",	@"119 min",	@"Jane (Meryl Streep), madre de 3 hijos mayores, tiene un negocio gastronómico que funciona muy bien en Santa Bárbara. Lleva 10 años divorciada y tiene una buena relación con su ex marido Jake (Alec",	@"Apta mayores de 16 años",	@"Comedia, Romance"),
                        NewItem("Papás a la fuerza",	@".\Peliculas\11621.jpg",	@"Walt Becker",	@"Robin Williams, John Travolta. Luis Guzmán, Bernie Mac, Ella Bleu Travolta,Rita Wilson, Matt Dillon, Kelly Preston, Seth Green, Justin Long.",	@"88 min",	@"Dan (Robin Williams) y Charlie (John Travolta) son compañeros de trabajo, además de buenos amigos. Creen tener la vida bajo control y disfrutan de ella al máximo ahora que han entrado en los",	@"Apta todo público",	@"Comedia, Familiar"),
                        NewItem("Días de ira",	@".\Peliculas\11622.jpg",	@"F. Gary Gray",	@"Gerard Butler, Jamie Foxx. Viola Davis, Regina Hall, Leslie Bibb, Bruce McGill, Colm Meany.",	@"107 min",	@"Un hombre, luego de 10 años de que su mujer y su hija hayan sido brutalmente asesinadas, regresa para realizar justicia a mano propia con el fiscal del distrito que llevo a cabo el caso contra los",	@"Apta mayores de 13 años",	@"Drama, Suspenso"),
                        NewItem("Vivir al límite",	@".\Peliculas\12136.jpg",	@"Kathryn Bigelow",	@"Ralph Fiennes, Jeremy Renner, Guy Pearce. Evangeline Lilly, Ralph Fiennes.",	@"131 min",	@"Si ir a la Guerra es estar en el infierno... ¿por que tantos hombres eligen ir a pelear? En una época donde ser soldado no es obligatorio, sino voluntario, y los hombres por motus propio deciden",	@"Apta mayores de 16 años",	@"Drama, Bélica"),
                        NewItem("Día de los enamorados",	@".\Peliculas\12168.jpg",	@"Garry Marshall",	@"Ashton Kutcher, Jennifer Garner, Julia Roberts, Jessica Biel, Jessica Alba,Anne Hathaway. Carter Jenkins, Taylor Swift, Emma Roberts, Hector Elizondo, Kathy Bates, Taylor Lautner, Queen Latifah, Shirley MacLaine, Jamie Foxx, Topher Grace,Patrick Dempsey, Bradley Cooper.",	@"90 min",	@"En Día de Los Enamorados actúa un reparto de grandes estrellas, y ellos dan vida a las entrelazadas historias de un grupo diverso de habitantes de Los Ángeles, y sus vicisitudes y angustias durante",	@"Apta mayores de 13 años",	@"Comedia, Romance"),
                    });
            }
            foreach (var item in peliculas)
            {
                da.Save(item);
                _peliculas.Add(item.Titulo, item.Id);
            }
        }

        private Peliculas_00945930857Entity NewItem(string title, string image, string director, string actores, string duracion, string sinopsis, string audiencia, string categorias)
        {
            var pelicula = new Peliculas_00945930857Entity
            {
                Titulo = title,
                Imagen = Utilities.ImageToString(image),
                Director = director,
                Actores = actores,
                Duracion = duracion,
                Sinopsis = sinopsis,
                Audiencia = audiencia,
            };
            _categoriasXPeliculas.Add(title, categorias);
            return pelicula;
        }

        private void FillCategorias()
        {
            var da = new Categorias__01697024398DataAccess();
            var categorias = da.LoadAll(false);
            if (categorias.Count == 0)
            {
                categorias = new System.Collections.ObjectModel.Collection<Categorias__01697024398Entity>(
                    new Categorias__01697024398Entity[]{
                        new Categorias__01697024398Entity{ Nombre = "Acción" },
                        new Categorias__01697024398Entity{ Nombre = "Animación" }, 
                        new Categorias__01697024398Entity{ Nombre = "Bélica" },
                        new Categorias__01697024398Entity{ Nombre = "Ciencia Ficción" },
                        new Categorias__01697024398Entity{ Nombre = "Comedia" }, 
                        new Categorias__01697024398Entity{ Nombre = "Drama" },
                        new Categorias__01697024398Entity{ Nombre = "Familiar" },
                        new Categorias__01697024398Entity{ Nombre = "Histórica" },
                        new Categorias__01697024398Entity{ Nombre = "Romance" },
                        new Categorias__01697024398Entity{ Nombre = "Suspenso" },
                    });
            }
            foreach (var item in categorias)
            {
                da.Save(item);
                _categorias.Add(item.Nombre, item.Id);
            }
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
