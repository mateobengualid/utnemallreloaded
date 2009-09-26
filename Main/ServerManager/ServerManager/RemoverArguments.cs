using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    public class RemoverArguments
    {
        private bool succeed;
        /// <summary>
        /// El estado del proceso
        /// </summary>
        public bool Succeed
        {
            get { return succeed; }
            set { succeed = value; }
        }

        
        private Collection<Error> message;
        /// <summary>
        /// Un listado de mensajes de errores
        /// </summary>
        public Collection<Error> Message
        {
            get { return message; }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="succeed">El estado del proceso</param>
        /// <param name="message">Un listado de mensajes de error</param>
        public RemoverArguments(bool succeed, Collection<Error> message)
        {
            this.succeed = succeed;
            this.message = message;
        }
    }
}
