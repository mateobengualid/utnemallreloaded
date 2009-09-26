using System;
using System.Runtime.Serialization;


namespace UtnEmall.Server.BusinessLogic
{
    /// <summary>
    /// Excepción a ser usada en la capa de negocios de UtnEmall Server.
    /// </summary>
    [Serializable]
    public class UtnEmallPermissionException : Exception
    {
        #region Constructors

        public UtnEmallPermissionException()
        {
        }

        public UtnEmallPermissionException(string message) :
            base(message)
        {
        }

        public UtnEmallPermissionException(string message, System.Exception inner) :
            base(message, inner)
        {

        }

        protected UtnEmallPermissionException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
        }

        #endregion
    }
}
