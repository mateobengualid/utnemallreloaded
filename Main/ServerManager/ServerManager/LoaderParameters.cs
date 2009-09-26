using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.ServerManager;

namespace UtnEmall.ServerManager
{
    public class LoaderParameters
    {
        private UserControl1 control;
        /// <summary>
        /// El componente que crea el evento en la interfaz de usuario
        /// </summary>
        public UserControl1 Control
        {
            get { return control; }
            set { control = value; }
        }
        private ServerManager.LoadList loader;
        /// <summary>
        /// El delegado que realiza la carga
        /// </summary>
        public ServerManager.LoadList Loader
        {
            get { return loader; }
            set { loader = value; }
        }
        private string session;
        /// <summary>
        /// Clave de sesión
        /// </summary>
        public string Session
        {
            get { return session; }
            set { session = value; }
        }
        private bool loadRelations;
        /// <summary>
        /// Indica si se debe cargar también los objetos de referencia
        /// </summary>
        public bool LoadRelations
        {
            get { return loadRelations; }
            set { loadRelations = value; }
        }
        private ServerManager.OnFinished onFinished;
        /// <summary>
        /// Método invocado cuando finaliza el proceso de carga
        /// </summary>
        public ServerManager.OnFinished OnFinished
        {
            get { return onFinished; }
            set { onFinished = value; }
        }

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">El componente que emite el evento en la interfaz de usuario</param>
        /// <param name="loader">El delegado que realiza la carga</param>
        /// <param name="session">Clave de sesión</param>
        /// <param name="loadRelations">Indica si se deben cargar las relaciones para el objeto</param>
        /// <param name="onFinished">Método invocado cuando finaliza el proceso de carga</param>
        public LoaderParameters(UserControl1 control, ServerManager.LoadList loader, string session, bool loadRelations, ServerManager.OnFinished onFinished)
        {
            this.control = control;
            this.loader = loader;
            this.session = session;
            this.loadRelations = loadRelations;
            this.onFinished = onFinished;
        }
    }
}
