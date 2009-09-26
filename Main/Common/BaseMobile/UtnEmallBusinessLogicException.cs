using System;
using System.Collections.Generic;
using System.Text;

namespace UtnEmall.Client.BusinessLogic
{
    /// <summary>
    /// Excepción a ser usada en la capa de negocios de la aplicación
    /// </summary>
    public class UtnEmallBusinessLogicException : System.Exception
    {
        public UtnEmallBusinessLogicException() { }
        public UtnEmallBusinessLogicException(string message)
            : base(message)
        {
        }
        public UtnEmallBusinessLogicException(string message, System.Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
