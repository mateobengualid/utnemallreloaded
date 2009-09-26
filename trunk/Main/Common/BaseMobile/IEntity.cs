#region Usage Declarations

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace UtnEmall.Client.EntityModel
{
    /// <summary>
    /// Interfaz para las clases de entidades
    /// </summary>
    public interface IEntity
    {
        // El identificador de las entidades
        int Id
        {
            get;
            set;
        }
        // Establece si la entidad cambio
        bool Changed
        {
            get;
            set;
        }
        // Establece si la entidad es nueva
        bool IsNew
        {
            get;
            set;
        }
     
        // Colección de errores de la entidad
        Collection<Error> Errors
        {
            get;
        }
        // Timestamp
        DateTime Timestamp
        {
            get;
        }
    }
}
