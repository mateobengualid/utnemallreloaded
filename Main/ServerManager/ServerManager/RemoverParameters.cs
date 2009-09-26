using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.ServerManager
{
    public class RemoverParameters
    {
        private UserControl1 control;
        /// <summary>
        /// El componente que es usado para invocar al evento
        /// </summary>
        public UserControl1 Control
        {
            get { return control; }
            set { control = value; }
        }
        private RemoveEntity remover;
        /// <summary>
        /// El delegado que realiza la eliminación
        /// </summary>
        public RemoveEntity Remover
        {
            get { return remover; }
            set { remover = value; }
        }
        private string session;
        /// <summary>
        /// La clave de sesión
        /// </summary>
        public string Session
        {
            get { return session; }
            set { session = value; }
        }

        private OnRemoveFinished onFinished;
        /// <summary>
        /// Método a ser invocado al finalizar la eliminación
        /// </summary>
        public OnRemoveFinished OnFinished
        {
            get { return onFinished; }
            set { onFinished = value; }
        }

        private IEntity entity;
        /// <summary>
        /// La entidad a eliminar
        /// </summary>
        public IEntity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">El componente que invoca al evento</param>
        /// <param name="remover">El delegado que realiza la eliminación</param>
        /// <param name="session">La clave de sesión</param>
        /// <param name="entity">La entidad a eliminar</param>
        /// <param name="onFinished">Método a ser invocado cuando finalice la eliminación</param>
        public RemoverParameters(UserControl1 control, RemoveEntity remover, string session, IEntity entity, OnRemoveFinished onFinished)
        {
            this.control = control;
            this.remover = remover;
            this.session = session;
            this.entity = entity;
            this.onFinished = onFinished;
        }
    }
}
