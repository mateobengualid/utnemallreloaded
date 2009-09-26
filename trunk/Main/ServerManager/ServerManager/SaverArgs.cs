using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    public class SaverArgs
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
        /// Una lista de mensajes de error
        /// </summary>
        public Collection<Error> Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="succeed">El estado del proceso</param>
        /// <param name="message">Una lista de mensajes de error</param>
        public SaverArgs(bool succeed, Collection<Error> message)
        {
            this.succeed = succeed;
            this.message = message;
        }
    }
}
