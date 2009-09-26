using System;
using System.Runtime.Serialization;

namespace UtnEmall.Server.BusinessLogic
{
    /// <summary>
    /// Excepción a ser usada en la capa de negocios de UtnEmall Server.
    /// </summary>
    [Serializable]
    public class UtnEmallBusinessLogicException : System.Exception
    {
        #region Constructors

        public UtnEmallBusinessLogicException()
        {
        }

        public UtnEmallBusinessLogicException(string message) :
            base(message)
        {
        }

        public UtnEmallBusinessLogicException(string message, System.Exception inner) :
            base(message, inner)
        {

        }

        protected UtnEmallBusinessLogicException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
        }

        #endregion
    }
}
