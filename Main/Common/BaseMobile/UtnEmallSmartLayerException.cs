using System;
using System.Collections.Generic;
using System.Text;

namespace UtnEmall.Client.SmartClientLayer
{
    /// <summary>
    /// Excepción a ser usada en la capa SmartClient de la aplicación.
    /// </summary>
    public class UtnEmallSmartLayerException : System.Exception
    {
        public UtnEmallSmartLayerException() { }
        public UtnEmallSmartLayerException(string message)
            : base(message)
        {

        }
        public UtnEmallSmartLayerException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }

}
