using System;
using System.Runtime.Serialization;

namespace UtnEmall.Server.DataModel
{
    /// <summary>
    /// Excepción a ser usada en la capa de acceso a datos UtnEmall Server.
    /// </summary>
    [Serializable]
    public class UtnEmallDataAccessException : System.Exception
    {
        #region Constructors

        public UtnEmallDataAccessException()
        {
        }

        public UtnEmallDataAccessException(string message)
            : base(message)
        {
        }

        public UtnEmallDataAccessException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        protected UtnEmallDataAccessException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
        }

        #endregion
    }
}
