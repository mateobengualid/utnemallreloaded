using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    public class LoaderArguments
    {
        private bool succeed;
        /// <summary>
        /// indica el estado del proceso
        /// </summary>
        public bool Succeed
        {
            get { return succeed; }
            set { succeed = value; }
        }

        private string message;
        /// <summary>
        /// Un mensaje del estado del proceso
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        private ReadOnlyCollection<IEntity> items;
        /// <summary>
        /// La lista de elementos cargados
        /// </summary>
        public ReadOnlyCollection<IEntity> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Cosntructor de clase
        /// </summary>
        /// <param name="succeed">Indica el estado del proceso</param>
        /// <param name="message">Un mensaje del estado del proceso</param>
        /// <param name="items">Una lista de elementos cargados</param>
        public LoaderArguments(bool succeed, string message, ReadOnlyCollection<IEntity> items)
        {
            this.succeed = succeed;
            this.message = message;
            this.items = items;
        }
    }
}
