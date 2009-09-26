using System;
using System.Collections.Generic;
using System.Text;

namespace UtnEmall.Client.DataModel
{
    /// <summary>
    /// Excepción a ser usada en la capa de datos de la aplicación.
    /// </summary>
    public class UtnEmallDataAccessException : System.Exception
    {
        public UtnEmallDataAccessException() { }
        public UtnEmallDataAccessException(string message): base(message)
        {

        }
        public UtnEmallDataAccessException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }

}
