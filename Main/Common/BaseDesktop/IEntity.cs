using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{
    /// <summary>
    /// Interfaz global para las clases de entidades.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id de la entidad.
        /// </summary>
        int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Indica si la entidad cambio.
        /// </summary>
        bool Changed
        {
            get;
            set;
        }

        /// <summary>
        /// Indica si la entidad es nueva.
        /// </summary>
        bool IsNew
        {
            get;
            set;
        }

        /// <summary>
        /// Colección de errores relacionados con los datos de la entidad.
        /// </summary>
        Collection<Error> Errors
        {
            get;
        }
        
        /// <summary>
        /// Una marca de tiempo indicando la última modificación en la base de datos.
        /// </summary>
        DateTime Timestamp
        {
            get;
        }
    }
}
