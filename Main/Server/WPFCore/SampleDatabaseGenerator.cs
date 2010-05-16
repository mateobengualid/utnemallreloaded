using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.DataModel;
using System.Diagnostics;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.WpfCore;
using System.Security.Cryptography;

namespace WpfCore
{
    class SampleDatabaseGenerator
    {
        static Random _rnd = new Random();
        static string sessionId;

        static string[] _maleNames = new string[] { 
                "Ambrosio", "Américo", "Amilcar", "Anastasio", "Andrés", "Ángel", "Aníbal", "Aniceto", "Anselmo", "Antolín", "Antón", "Antonello", "Antonio", "Apolo", "Apolonio", "Aquiles", "Aquilino", "Arcadio", "Arcángel", "Archibaldo", "Ariano", "Ariel", "Aristóbulo", "Aristóteles", "Armando", "Arnaldo", "Arquímedes", "Arsenio", "Artemio", "Arturo", "Astor", "Atahualpa", "Atanasio", "Ataúlfo", "Atila", "Augusto", "Aureliano", "Aurelio", "Axel",
                "Bernardino", "Bernardo", "Bienvenido", "Blas", "Bonifacio", "Bonifaz", "Borgia", "Boris", "Borja", "Braulio", "Bruno",
                "Cirilo", "Ciro", "Claudio", "Clemente", "Clodoveo", "Columbano", "Conrado", "Confucio", "Constancio", "Constantino", "Cornelio", "Cosme", "Crispo", "Crispín", "Críspulo", "Cristián", "Cristiano", "Cristo", "Cristóbal", "Cucufate",
                "Emiliano", "Emilio", "Emmanuel", "Eneas", "Enrique", "Enzo", "Epifanio", "Erasmo", "Eric", "Erico", "Ermegol", "Ernesto", "Eros", "Espartaco", "Estanislao", "Esteban", "Estefano", "Estíbaliz", "Estrabón", "Eugenio", "Eulogio", "Eusebio", "Eustaquio", "Evaristo", "Evelio", "Ezequiel",
                "Gelo", "Geminiano", "Gentil", "Genaro", "George", "Gerardo", "Germain", "Germán", "Germánico", "Germano", "Gerson", "Gervasio", "Getulio", "Giancarlo", "Gianfranco", "Gilberto", "Giordano", "Giorgio", "Glauco", "Godofredo", "Goliard", "Gonzalo", "Gordon", "Gotardo", "Graciano", "Graham", "Gregorio", "Gualterio", "Guido", "Guillermo", "Gunter", "Gustavo",
                "Lamberto", "Lamcelot", "Landelino", "Landolfo", "Lanfranco", "Lanzarote", "Laureano", "Laurencio", "Laurentino", "Lauro", "Lautaro", "Lázaro", "Leandro", "Learco", "Lelio", "Lenin", "Leo",
            };

        static string[] _femNames = new string[] { 
                "Amelia",  "América",  "Amparo",  "Ana",  "Anabel",  "Anabella",  "Anaclara",  "Anahí",  "Anáis",  "Anastasia",  "Andrea",  "Andreína",  "Ángela",  "Ángeles",  "Angélica",  "Angelina",  "Angie",  "Angustias",  "Ania",  "Antea",  "Antonela",  "Antolina",  "Antonieta",  "Antonia",  "Antonina",  "Anunciación",  "Anunciata",  "Apia",  "Apolonia",  "Aquilina",  "Arabela",  "Araceli",  "Arantxa",  "Aránzazu",  "Arcadia",  "Argentina",  "Ariadna",  "Ariana",  "Armonía",  "Artemis",  "Aroa",  "Ascensión",  "Astrid",  "Asunción",  "Asunta",  "Atala",  "Atalanta",  "Atanasia",  "Atenea",  "Augusta",  "Áurea",  "Aurelia",  "Aureliana",  "Aurora",  "Auxiliadora",  "Ayelen",  "Azalea",  "Azucena", 
                "Claribel",  "Clarisa",  "Clarinda",  "Clarita",  "Claudia",  "Clelia",  "Clementina",  "Cleopatra",  "Cleo", "Clío",  "Cloe",  "Cloris",  "Clotilde",  "Coleta",  "Concepción",  "Concha",  "Consolación",  "Constancia",  "Constanza",  "Consuelo",  "Copelia",  "Cora",  "Coral",  "Cordelia",  "Corina",  "Cornelia",  "Cósima",  "Covadonga",  "Creusa",  "Crispína",  "Cristal",  "Cristina",  "Cristiana", 
                "Fermina",  "Fernanda",  "Filis",  "Filomena",  "Fina",  "Fiona",  "Fiorela",  "Flaminia",  "Flavia",  "Flor",  "Flora",  "Florencia", "Florentina",  "Florida",  "Fortunata",  "Francesca",  "Francisca",  "Freya",  "Frida",  "Friné",  "Fructuosa",  "Fuenciscla",  "Fuensanta", 
                "Ioana",  "Iole",  "Iracema",  "Irene",  "Irina",  "Iris",  "Irma",  "Irmina",  "Isabel",  "Isabela",  "Isaura",  "Isidoro",  "Isis",  "Isolda",  "Isolina",  "Iva",  "Ivana",  "Ivon",  "Ivonne",  "Izaskum", 
                "Leticia",  "Leucofrina",  "Lía",  "Liana",  "Libera",  "Liberata",  "Libertad",  "Libia",  "Licia",  "Lidia",  "Liuvina",  "Ligia",  "Lila",  "Lili",  "Lilia",  "Lilian",  "Liliana",  "Lilit",  "Lina",  "Linda",  "Lioba",  "Lionela",  "Lis",  "Lisa",  "Liu",  "Liuba",  "Livia",  "Liza",  "Lola",  "Lora",  "Loreley",  "Lorena",  "Lorenza",  "Loreta",  "Lorna",  "Lourdes",  "Luana",  "Lucia",  "Luciana",  "Lucila",  "Lucina",  "Lucrecia",  "Lucy",  "Ludmila",  "Ludovica",  "Luisa",  "Luisina",  "Lujan",  "Luminosa",  "Luna",  "Lupe",  "Lutecia",  "Lutgarda",  "Luz",  "Lydia",  "Lyla", 
                "Marlene",  "Marta",  "Martina",  "Matilda",  "Matilde",  "Maura",  "Máxima",  "Maya",  "Mayra",  "Mayte",  "Medea",  "Melania",  "Melba",  "Melibea",  "Melinda",  "Melisa",  "Melisenda",  "Melitina",  "Melody",  "Mercedes",  "Meredith",  "Meritxell",  "Meryl",  "Mesalina",  "Mía",  "Micaela",  "Michelle",  "Mila",  "Milagros",  "Milagrosa",  "Mildred",  "Milena",  "Mimi",  "Minerva",  "Miranda",  "Mireya",  "Miriam",  "Mirinda",  "Mirra",  "Mirta",  "Miryan",  "Misericordia",  "Moira",  "Mona",  "Mónica",  "Montserrat",  "Morgana",  "Muriel",  "Myrian", 
            };

        static string[] _lastNames = new string[]{
                "González", "Rodríguez", "Gómez", "Fernández", "López", "Díaz", "Martínez", "Pérez", "García", "Sánchez", "Romero", "Sosa", "Álvarez", "Torres", "Ruiz", "Ramírez", "Flores", "Acosta", "Benítez", "Medina", "Suárez", "Herrera", "Aguirre", "Pereyra", "Gutiérrez", "Giménez", "Molina", "Silva", "Castro", "Rojas", "Ortíz", "Núñez", "Luna", "Juárez", "Cabrera", "Ríos", "Ferreyra", "Godoy", "Morales", "Domínguez", "Moreno", "Peralta", "Vega", "Carrizo", "Quiroga", "Castillo", "Ledesma", "Muñoz", "Ojeda", "Ponce", "Vera", "Vázquez", "Villalba", "Cardozo", "Navarro", "Ramos", "Arias", "Coronel", "Córdoba", "Figueroa", "Correa", "Cáceres", "Vargas", "Maldonado", "Mansilla", "Farías", "Rivero", "Paz", "Miranda", "Roldán", "Méndez", "Lucero", "Cruz", "Hernández", "Agüero", "Páez", "Blanco", "Mendoza", "Barrios", "Escobar", "Ávila", "Soria", "Leiva", "Acuña", "Martin", "Maidana", "Moyano", "Campos", "Olivera", "Duarte", "Soto", "Franco", "Bravo", "Valdéz", "Toledo", "Velázquez", "Montenegro", "Leguizamón", "Chávez", "Arce",
            };

        static string[] _addresses = new string[]{
                "Siempre viva", "Las mojarritas", "Uspallata", "Mendoza", "Allende", "Linux", "Windows", "Mapple", "Es dell", "Sin nombre", "La Internet" 
            };

        static string[] _stores = new string[]{
            "Abstracta", "Acento", "Acqua", "AF JEANS", "Akiabara", "Alpiste", "Alto Pie", "Andrea Francescini", "Animal Shop", "Athletic", "Aupesa", "Ave Caesar",
            "A`Gustino Cueros", "B-52", "Babbylon", "Banco Macro", "Banco Provincia de Córdoba", "Bazar", "Be More", "Betos Lomitos", "Bocatto", "Brooksfield", "Burger King", "Buryak", "Cachavacha", "Caro Cuore", "Casa Chica", "Chammas", "Cheeky Baby & Kids", "Chexa", "Chinchorro", "Christian Dior - Rochas", "Claro", "Clinique", "Cómo quieres que te quiera", "Conte", "Delicity", "Delta", "Disco", "Doña Chacha", "Ecco", "Esencias del Boticario", "Eyelit", "Fabrizzi", "Fábulas y Cuentos", "Falabella", "Farmacia Taleb", "Ferreterí a Universal", "Flash Service", "Foto Shop", "Foto Shop Stand", "Fuencarral", "Full Love", "Funny Ville", "Garbarino", "Gizona", "Goya Calzados", "Graffity", "Grido", "Gridosoft", "Grupo Shopping", "Guayacan", "Hábito", "Havanna", "Hoyts General Cinema", "Hush Puppies", "Il Gatto Trattoria", "IL Panino", "Indonesia", "Inside", "Insomnio", "Isadora", "Jc", "Julia Saúl", "Jurado Golf", "Kevingston", "La Bs.As Seguros", "Lacoste", "Lady Stork", "Legacy", "Límite", "Maipu Ford", "Maipu VW", "Mallorca", "Mannequins", "Marfil", "Mc Donald`s", "Mediterráneo Automotores", "MH Clean Service", "Mimo", "Mirangi", "Mistral", "Montesco", "Motcor", "Muaa", "Musimundo", "Narda",
            "Narrow", "Negro el 11", "Neverland Park", "Nextel", "Nike", "Optica Tustanoski", "Ossira", "Parra", "Pass Time Drugstore", "Peralta Seguros", "Perfumerías Juleriaque", "Personal", "Personal Stand", "Peuque", "Pluto`s", "Polo Casa de Chocolate", "Portsaid", "Prune", "Puma", "Rai Remis", "Red Link Cajeros", "Rincón de las Flores", "Roberto Giordano", "Rossetti Concepts", "Rossetti Deportes", "Secco Rap", "Service Express", "Sigfrido", "Silenzio", "Ski & Summershop", "Soho", "Soleil Bijouterie", "Sólido", "Sport`s Car", "Style Watch", "Sunny Center Solarium", "Sweet Home", "Sweet Sweet Way", "Tarjeta Naranja", "Telecom", "The Kickback", "Timberland", "Tinta Amarilla", "Todo Moda", "Transatlántica", "Traruwe", "Tromso", "Tuareg", "Tucci", "Utzzia", "Van Gansen", "Vaqueria", "Ver", "Vesna", "Vía Verde", "Wanama & Cook", "Wild Fire", "XL Extra Large", "Yenny", "Zantina", "Zona Cero"
        };

        static string[] _categories = new string[]{
            "Agencia de Modelos", "Agencia de Viajes", "Automotriz", "Bancos", "Bijouterie y Accesorios", 
            "Bombonerías y Dulces", "Calzado Hombre y Mujer", "Centro de Estética", "Cerrajería", "Cines", "Compostura de Calzado", "Confiterías y cafeterías", "Decoración y regalos", 
            "Deportes", "Electrodomésticos", "Electrónica", "Farmacia y perfumería", "Ferretería", "Fotografía y óptica", 
            "Gastronomía", "Indumentaria Jóven", "Joyería y relojería", "Juguetería", "Kiosco & Drugstore", "Lencería y medias", 
            "Librería", "Marroquinería", "Muebles y Blanco", "Música y audio", "Otros", "Parque Infantil", "Seguros", "Servicio para el auto", 
            "Servicios", "Supermercado", "Telefonía", "Tienda Departamental", "Tintorería/Lavandería", "Vestimenta femenina", 
            "Vestimenta hombre y mujer", "Vestimenta masculina formal", "Vestimenta masculina informal", "Veterinaria", 
        };

        enum Categories
        {
            Agencia_de_Modelos, Agencia_de_Viajes, Automotriz, Bancos, Bijouterie_y_Accesorios, 
            Bombonerías_y_Dulces, Calzado_Hombre_y_Mujer, Centro_de_Estética, Cerrajería, Cines, Compostura_de_Calzado, Confiterías_y_cafeterías, Decoración_y_regalos, 
            Deportes, Electrodomésticos, Electrónica, Farmacia_y_perfumería, Ferretería, Fotografía_y_óptica, 
            Gastronomía, Indumentaria_Jóven, Joyería_y_relojería, Juguetería, Kiosco___Drugstore, Lencería_y_medias, 
            Librería, Marroquinería, Muebles_y_Blanco, Música_y_audio, Otros, Parque_Infantil, Seguros, Servicio_para_el_auto, 
            Servicios, Supermercado, Telefonía, Tienda_Departamental, Tintorería_Lavandería, Vestimenta_femenina, 
            Vestimenta_hombre_y_mujer, Vestimenta_masculina_formal, Vestimenta_masculina_informal, Veterinaria, 
        }

        static List<CategoryEntity> _categoryList = new List<CategoryEntity>();

        public static void Run()
        {
            Trace.WriteLine("Inserting sample data into database.");

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashByteArray = sha.ComputeHash(Encoding.ASCII.GetBytes("admin"));
            sessionId = SessionManager.Instance.ValidateLogin("admin", hashByteArray, false);

            // Categories
            CreateCategories();

            // Customers
            CreateCustomers();

            // Stores
            CreateStores();

            // Users
            CreateUsers();

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
            var sda = new StoreDataAccess();

            for (int n = 0; n < _stores.Length; n++)
            {
                var store = new StoreEntity();
                store.Name = _stores[n];
                store.TelephoneNumber = GetPhoneNumber();
                store.OwnerName = GetMaleName();
                store.InternalPhoneNumber = GetPhoneNumber();
                store.ContactName = GetMaleName();
                store.LocalNumber = n.ToString();
                store.WebAddress = "www." + store.Name.ToLower().Replace(' ', '-') + ".com.ar";
                store.Email = "elmejormail@gmail.com";
                
                sda.Save(store);
            }
        }

        private static void CreateUsers()
        {
            // cargar tiendas
            var stores = new StoreDataAccess().LoadAll(false);
            // cargar admin group
            var adminGroup = new GroupDataAccess().LoadAll(false)[0];

            UserDataAccess uda = new UserDataAccess();
            foreach (StoreEntity store in stores)
            {
                var ue = new UserEntity();
                ue.Name = GetMaleName();
                ue.Surname = GetLastName();
                ue.UserName = GetUserName(ue.Name, ue.Surname);
                ue.IsUserActive = true;
                ue.Password = Utilities.CalculateHashString("123");
                ue.PhoneNumber = GetPhoneNumber();
                ue.Charge = "Administrador";
                ue.Store = store;

                // add user group
                var ug = new UserGroupEntity();
                ug.Group = adminGroup;
                ug.User = ue;
                ue.UserGroup.Add(ug);

                // save user
                uda.Save(ue);
            }
        }

        private static void CreateCustomers()
        {
            for (int n = 0; n < 150; n++) CreateCustomer();
        }

        private static void CreateCustomer()
        {
            var cda = new Customer();
            // crear clientes
            var ce = new CustomerEntity();

            bool male = _rnd.NextDouble() >= 0.5 ? true : false;

            ce.Gender = (int)(male ? Gender.Male : Gender.Female);
            ce.Birthday = new DateTime(_rnd.Next(1920,1995), _rnd.Next(1,12), _rnd.Next(1, 28));

            int age = DateTime.Now.Year - ce.Birthday.Year;

            ce.Name = male ? GetMaleName() : GetFemenineName();
            ce.Surname = GetLastName();

            if (age > 18 && _rnd.NextDouble() > 0.7)
            {
                ce.HowManyChildren = _rnd.Next(1, 5);
                var csp = _rnd.NextDouble();
                ce.CivilState = (int)(csp < 0.7 ? CivilState.Married : csp > 0.85 ? CivilState.Single : CivilState.Divorced);
                if (age > 50)
                {
                    if (_rnd.NextDouble() < 0.2) ce.CivilState = (int)CivilState.Widow;
                }
            }
            else
            {
                ce.CivilState = (int)CivilState.Single;
            }

            ce.UserName = GetUserName(ce.Name, ce.Surname);
            ce.Password = Utilities.CalculateHashString("123");
            ce.PhoneNumber = GetPhoneNumber();
            ce.Address = GetAddress();
            
            // save the customer
            cda.Save(ce, sessionId);
        }

        private static string GetPhoneNumber()
        {
            return GetRandomNumber(3, _rnd) + "-" + GetRandomNumber(4, _rnd);
        }

        private static string GetAddress()
        {
            return _addresses[_rnd.Next(_addresses.Length - 1)] + " " + GetRandomNumber(4, _rnd);
        }

        private static string GetLastName()
        {
            return _lastNames[_rnd.Next(_lastNames.Length - 1)];
        }

        private static string GetFemenineName()
        {
            return _femNames[_rnd.Next(_femNames.Length - 1)] + " " + _femNames[_rnd.Next(_femNames.Length - 1)];
        }

        private static string GetMaleName()
        {
            return _maleNames[_rnd.Next(_maleNames.Length - 1)] + " " + _femNames[_rnd.Next(_maleNames.Length - 1)];
        }

        private static string GetRandomNumber(int digits, Random rnd)
        {
            string cad = "";
            for (int n = 0; n < digits; n++) cad += rnd.Next(1, 9);
            return cad;
        }

        private static string GetUserName(string firstName, string lastName)
        {
            var names = firstName.Split(' ');
            return names[0].ToLower() + "." + lastName.ToLower();
        }

        private static void CreateCategories()
        {
            /*
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
            */

            var cda = new CategoryDataAccess();
            // _categories = new Dictionary<string, CategoryEntity>();
            foreach (var categoryStr in _categories)
            {
                var category = new CategoryEntity();
                category.Name = categoryStr;

                cda.Save(category);
                _categoryList.Add(category);
                //_categories.Add(category.Name, category);
                //foreach(var childCategory in category.Childs){
                //    _categories.Add(childCategory.Name, childCategory);
                //}
            }
        }
    }
}
