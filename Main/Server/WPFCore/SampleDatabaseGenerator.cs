using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.DataModel;
using System.Diagnostics;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;

namespace WpfCore
{
    class SampleDatabaseGenerator
    {
        static Dictionary<string, CategoryEntity> _categories;
        static Random rnd = new Random();

        static string[] maleNames = new string[] { 
                "Ambrosio", "Américo", "Amilcar", "Anastasio", "Andrés", "Ángel", "Aníbal", "Aniceto", "Anselmo", "Antolín", "Antón", "Antonello", "Antonio", "Apolo", "Apolonio", "Aquiles", "Aquilino", "Arcadio", "Arcángel", "Archibaldo", "Ariano", "Ariel", "Aristóbulo", "Aristóteles", "Armando", "Arnaldo", "Arquímedes", "Arsenio", "Artemio", "Arturo", "Astor", "Atahualpa", "Atanasio", "Ataúlfo", "Atila", "Augusto", "Aureliano", "Aurelio", "Axel",
                "Bernardino", "Bernardo", "Bienvenido", "Blas", "Bonifacio", "Bonifaz", "Borgia", "Boris", "Borja", "Braulio", "Bruno",
                "Cirilo", "Ciro", "Claudio", "Clemente", "Clodoveo", "Columbano", "Conrado", "Confucio", "Constancio", "Constantino", "Cornelio", "Cosme", "Crispo", "Crispín", "Críspulo", "Cristián", "Cristiano", "Cristo", "Cristóbal", "Cucufate",
                "Emiliano", "Emilio", "Emmanuel", "Eneas", "Enrique", "Enzo", "Epifanio", "Erasmo", "Eric", "Erico", "Ermegol", "Ernesto", "Eros", "Espartaco", "Estanislao", "Esteban", "Estefano", "Estíbaliz", "Estrabón", "Eugenio", "Eulogio", "Eusebio", "Eustaquio", "Evaristo", "Evelio", "Ezequiel",
                "Gelo", "Geminiano", "Gentil", "Genaro", "George", "Gerardo", "Germain", "Germán", "Germánico", "Germano", "Gerson", "Gervasio", "Getulio", "Giancarlo", "Gianfranco", "Gilberto", "Giordano", "Giorgio", "Glauco", "Godofredo", "Goliard", "Gonzalo", "Gordon", "Gotardo", "Graciano", "Graham", "Gregorio", "Gualterio", "Guido", "Guillermo", "Gunter", "Gustavo",
                "Lamberto", "Lamcelot", "Landelino", "Landolfo", "Lanfranco", "Lanzarote", "Laureano", "Laurencio", "Laurentino", "Lauro", "Lautaro", "Lázaro", "Leandro", "Learco", "Lelio", "Lenin", "Leo",
            };

        static string[] femNames = new string[] { 
                "Amelia",  "América",  "Amparo",  "Ana",  "Anabel",  "Anabella",  "Anaclara",  "Anahí",  "Anáis",  "Anastasia",  "Andrea",  "Andreína",  "Ángela",  "Ángeles",  "Angélica",  "Angelina",  "Angie",  "Angustias",  "Ania",  "Antea",  "Antonela",  "Antolina",  "Antonieta",  "Antonia",  "Antonina",  "Anunciación",  "Anunciata",  "Apia",  "Apolonia",  "Aquilina",  "Arabela",  "Araceli",  "Arantxa",  "Aránzazu",  "Arcadia",  "Argentina",  "Ariadna",  "Ariana",  "Armonía",  "Artemis",  "Aroa",  "Ascensión",  "Astrid",  "Asunción",  "Asunta",  "Atala",  "Atalanta",  "Atanasia",  "Atenea",  "Augusta",  "Áurea",  "Aurelia",  "Aureliana",  "Aurora",  "Auxiliadora",  "Ayelen",  "Azalea",  "Azucena", 
                "Claribel",  "Clarisa",  "Clarinda",  "Clarita",  "Claudia",  "Clelia",  "Clementina",  "Cleopatra",  "Cleo", "Clío",  "Cloe",  "Cloris",  "Clotilde",  "Coleta",  "Concepción",  "Concha",  "Consolación",  "Constancia",  "Constanza",  "Consuelo",  "Copelia",  "Cora",  "Coral",  "Cordelia",  "Corina",  "Cornelia",  "Cósima",  "Covadonga",  "Creusa",  "Crispína",  "Cristal",  "Cristina",  "Cristiana", 
                "Fermina",  "Fernanda",  "Filis",  "Filomena",  "Fina",  "Fiona",  "Fiorela",  "Flaminia",  "Flavia",  "Flor",  "Flora",  "Florencia", "Florentina",  "Florida",  "Fortunata",  "Francesca",  "Francisca",  "Freya",  "Frida",  "Friné",  "Fructuosa",  "Fuenciscla",  "Fuensanta", 
                "Ioana",  "Iole",  "Iracema",  "Irene",  "Irina",  "Iris",  "Irma",  "Irmina",  "Isabel",  "Isabela",  "Isaura",  "Isidoro",  "Isis",  "Isolda",  "Isolina",  "Iva",  "Ivana",  "Ivon",  "Ivonne",  "Izaskum", 
                "Leticia",  "Leucofrina",  "Lía",  "Liana",  "Libera",  "Liberata",  "Libertad",  "Libia",  "Licia",  "Lidia",  "Liuvina",  "Ligia",  "Lila",  "Lili",  "Lilia",  "Lilian",  "Liliana",  "Lilit",  "Lina",  "Linda",  "Lioba",  "Lionela",  "Lis",  "Lisa",  "Liu",  "Liuba",  "Livia",  "Liza",  "Lola",  "Lora",  "Loreley",  "Lorena",  "Lorenza",  "Loreta",  "Lorna",  "Lourdes",  "Luana",  "Lucia",  "Luciana",  "Lucila",  "Lucina",  "Lucrecia",  "Lucy",  "Ludmila",  "Ludovica",  "Luisa",  "Luisina",  "Lujan",  "Luminosa",  "Luna",  "Lupe",  "Lutecia",  "Lutgarda",  "Luz",  "Lydia",  "Lyla", 
                "Marlene",  "Marta",  "Martina",  "Matilda",  "Matilde",  "Maura",  "Máxima",  "Maya",  "Mayra",  "Mayte",  "Medea",  "Melania",  "Melba",  "Melibea",  "Melinda",  "Melisa",  "Melisenda",  "Melitina",  "Melody",  "Mercedes",  "Meredith",  "Meritxell",  "Meryl",  "Mesalina",  "Mía",  "Micaela",  "Michelle",  "Mila",  "Milagros",  "Milagrosa",  "Mildred",  "Milena",  "Mimi",  "Minerva",  "Miranda",  "Mireya",  "Miriam",  "Mirinda",  "Mirra",  "Mirta",  "Miryan",  "Misericordia",  "Moira",  "Mona",  "Mónica",  "Montserrat",  "Morgana",  "Muriel",  "Myrian", 
            };

        static string[] lastNames = new string[]{
                "González", "Rodríguez", "Gómez", "Fernández", "López", "Díaz", "Martínez", "Pérez", "García", "Sánchez", "Romero", "Sosa", "Álvarez", "Torres", "Ruiz", "Ramírez", "Flores", "Acosta", "Benítez", "Medina", "Suárez", "Herrera", "Aguirre", "Pereyra", "Gutiérrez", "Giménez", "Molina", "Silva", "Castro", "Rojas", "Ortíz", "Núñez", "Luna", "Juárez", "Cabrera", "Ríos", "Ferreyra", "Godoy", "Morales", "Domínguez", "Moreno", "Peralta", "Vega", "Carrizo", "Quiroga", "Castillo", "Ledesma", "Muñoz", "Ojeda", "Ponce", "Vera", "Vázquez", "Villalba", "Cardozo", "Navarro", "Ramos", "Arias", "Coronel", "Córdoba", "Figueroa", "Correa", "Cáceres", "Vargas", "Maldonado", "Mansilla", "Farías", "Rivero", "Paz", "Miranda", "Roldán", "Méndez", "Lucero", "Cruz", "Hernández", "Agüero", "Páez", "Blanco", "Mendoza", "Barrios", "Escobar", "Ávila", "Soria", "Leiva", "Acuña", "Martin", "Maidana", "Moyano", "Campos", "Olivera", "Duarte", "Soto", "Franco", "Bravo", "Valdéz", "Toledo", "Velázquez", "Montenegro", "Leguizamón", "Chávez", "Arce",
            };

        static string[] addresses = new string[]{
                "Siempre viva", "Las mojarritas", "Uspallata", "Mendoza", "Allende", "Linux", "Windows", "Mapple", "Es dell", "Sin nombre", "La Internet" 
            };

        public static void Run()
        {
            Trace.WriteLine("Inserting sample data into database.");

            // Categories
            CreateCategories();

            // Customers
            CreateCustomers();

            // Users
            CreateUsers();

            // Stores
            CreateStores();

            // DataModels
            CreateDataModels();

            // Data for DataModels
            CreateDataForDataModels();

            // Services
            CreateServices();

            // Create usage data for services
            CreateUsageDataForServices();

            // Campaigns
            CreateCampaigns();

        }

        private static void CreateCampaigns()
        {
        }

        private static void CreateUsageDataForServices()
        {
        }

        private static void CreateServices()
        {
        }

        private static void CreateDataForDataModels()
        {
        }

        private static void CreateDataModels()
        {
        }

        private static void CreateStores()
        {
        }

        private static void CreateUsers()
        {
        }

        private static void CreateCustomers()
        {
            for (int n = 0; n < 150; n++) CreateCustomer();
        }

        private static void CreateCustomer()
        {
            var cda = new CustomerDataAccess();
            // crear clientes
            var ce = new CustomerEntity();

            bool male = rnd.NextDouble() >= 0.5 ? true : false;

            ce.Gender = (int)(male ? Gender.Male : Gender.Female);
            ce.Birthday = new DateTime(rnd.Next(1920,1995), rnd.Next(1,12), rnd.Next(1, 28));

            int age = DateTime.Now.Year - ce.Birthday.Year;

            ce.Name = male ? GetMaleName() : GetFemenineName();
            ce.Surname = GetLastName();

            if (age > 18 && rnd.NextDouble() > 0.7)
            {
                ce.HowManyChildren = rnd.Next(1, 5);
                var csp = rnd.NextDouble();
                ce.CivilState = (int)(csp < 0.7 ? CivilState.Married : csp > 0.85 ? CivilState.Single : CivilState.Divorced);
                if (age > 50)
                {
                    if (rnd.NextDouble() < 0.2) ce.CivilState = (int)CivilState.Widow;
                }
            }
            else
            {
                ce.CivilState = (int)CivilState.Single;
            }

            ce.UserName = GetUserName(ce);
            ce.Password = Utilities.CalculateHashString("123");
            ce.PhoneNumber = GetPhoneNumber();
            ce.Address = GetAddress();
            
            // save the customer
            cda.Save(ce);
        }

        private static string GetPhoneNumber()
        {
            return GetRandomNumber(3, rnd) + "-" + GetRandomNumber(4, rnd);
        }

        private static string GetAddress()
        {
            return addresses[rnd.Next(addresses.Length - 1)] + " " + GetRandomNumber(4, rnd);
        }

        private static string GetLastName()
        {
            return lastNames[rnd.Next(lastNames.Length - 1)];
        }

        private static string GetFemenineName()
        {
            return femNames[rnd.Next(femNames.Length - 1)] + " " + femNames[rnd.Next(femNames.Length - 1)];
        }

        private static string GetMaleName()
        {
            return maleNames[rnd.Next(maleNames.Length - 1)] + " " + femNames[rnd.Next(maleNames.Length - 1)];
        }

        private static string GetRandomNumber(int digits, Random rnd)
        {
            string cad = "";
            for (int n = 0; n < digits; n++) cad += rnd.Next(1, 9);
            return cad;
        }

        private static string GetUserName(CustomerEntity ce)
        {
            var names = ce.Name.Split(' ');
            return names[0].ToLower() + "." + ce.Surname.ToLower();
        }

        private static void CreateCategories()
        {
            CategoryEntity[] cats = new CategoryEntity[] {
                new CategoryEntity{
                    Name = "Autos",
                    Description = "Todo para los autos."
                },
                new CategoryEntity{
                    Name = "Bebes",
                    Description = "Todo para los bebes."
                },
                new CategoryEntity{
                    Name = "Belleza",
                    Description = "Artículos para la mujer."
                },
                new CategoryEntity{
                    Name = "Deporte",
                    Description = "Todo para los deportes."
                },
                new CategoryEntity{
                    Name = "Entretenimiento",
                    Description = "Cine, Teatro, Libros, Recreación y tiempo libre.",
                    Childs = {
                        new CategoryEntity{
                            Name = "Cine"
                        },
                        new CategoryEntity{
                            Name = "Libros"
                        },
                        new CategoryEntity{
                            Name = "Tiempo libre"
                        },
                    }
                },
                new CategoryEntity{
                    Name = "Electrónica e Informática",
                    Description = "Electrónicos, computadoras y accesorios.",
                    Childs = {
                        new CategoryEntity{
                            Name = "Electrónicos"
                        },
                        new CategoryEntity{
                            Name = "Computación"
                        },
                    }
                },                
                new CategoryEntity{
                    Name = "Ropa",
                    Description = "Indumentaria femenina y masculina."
                },

                new CategoryEntity{
                    Name = "Viajes",
                    Description = "Turismo y viajes."
                },
            };

            var cda = new CategoryDataAccess();
            _categories = new Dictionary<string, CategoryEntity>();
            foreach (var category in cats)
            {
                cda.Save(category);
                _categories.Add(category.Name, category);
                foreach(var childCategory in category.Childs){
                    _categories.Add(childCategory.Name, childCategory);
                }
            }
        }
    }
}
