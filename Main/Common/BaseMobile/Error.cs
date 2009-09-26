#region Usage Declarations


#endregion


namespace UtnEmall.Client.EntityModel
{
    /// <summary>
    /// Representa un error ocurrido en los datos de una entidad
    /// </summary>
    public class Error
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties
        private string name;
        private string property;
        private string description;

        /// <summary>
        /// Obtiene o establece el nombre del error
        /// <summary>
        public string Name
        {
            get 
            { 
                return name; 
            }
            set 
            { 
                name = value; 
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la propiedad con error
        /// <summary>
        public string Property
        {
            get 
            {
                return property; 
            }
            set 
            { 
                property = value; 
            }
        }

        /// <summary>
        /// Obtiene o establece la descripción del error
        /// <summary>
        public string Description
        {
            get 
            { 
                return description; 
            }
            set 
            { 
                description = value; 
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Nombre del error</param>
        /// <param name="property">Propiedad a la que aplica el error</param>
        /// <param name="description">Descripción del error</param>
        public Error(string name, string property, string description)
        {
            this.name = name;
            this.property = property;
            this.description = description;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Error()
        {

        }
        #endregion

        #region Static Methods

        #region Public Static Methods

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        #endregion

        #region Protected and Private Instance Methods

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }
}
