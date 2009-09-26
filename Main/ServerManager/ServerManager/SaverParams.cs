using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;

namespace UtnEmall.ServerManager
{
    public class SaverParameters
    {
        private UserControl1 control;
        /// <summary>
        /// El componente que se usa para invocar al hilo
        /// </summary>
        public UserControl1 Control
        {
            get { return control; }
            set { control = value; }
        }
        private SaveEntity saver;
        /// <summary>
        /// El delegado que realiza el proceso
        /// </summary>
        public SaveEntity Saver
        {
            get { return saver; }
            set { saver = value; }
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

        private OnSaveFinished onFinished;
        /// <summary>
        /// Método invocado cuando finaliza el proceso
        /// </summary>
        public OnSaveFinished OnFinished
        {
            get { return onFinished; }
            set { onFinished = value; }
        }

        private IEntity entity;
        /// <summary>
        /// La entidad a guardar
        /// </summary>
        public IEntity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">El componente que invoca el proceso</param>
        /// <param name="saver">El delegado que realiza el proceso</param>
        /// <param name="session">La clave de sesión</param>
        /// <param name="entity">La entidad a guardar</param>
        /// <param name="onFinished">Método invocado cuando finaliza el proceso</param>
        public SaverParameters(UserControl1 control, SaveEntity saver, string session, IEntity entity, OnSaveFinished onFinished)
        {
            this.control = control;
            this.saver = saver;
            this.session = session;
            this.entity = entity;
            this.onFinished = onFinished;
        }
    }
}
