using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.DataModelClasses;

namespace LogicalLibrary.ServerDesignerClasses
{
    /// <summary>
    /// Clase que representa una conexión
    /// </summary>
    public class Connection
    {
        #region Constants, Variables and Properties

        /// <summary>
        /// Contexto de datos de salida para un componente, es una tabla del modelo de   datos que representa un dato de salida con el cual trabaja un componente.
        /// </summary>
        public Table DataContext
        {
            get { return Source.Parent.OutputDataContext; }
        }

        private ConnectionPoint source;
        /// <summary>
        /// Un ConnectionPoint que representa el origen de la conexión.
        /// </summary>
        public ConnectionPoint Source
        {
            get { return source; }
            set { source = value; }
        }

        private ConnectionPoint target;
        /// <summary>
        /// Un ConnectionPoint que representa el objetivo de la conexión
        /// </summary>
        public ConnectionPoint Target
        {
            get { return target; }
            set { target = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor que inicializa el formulario ConnectionPoint y ConnectionPoint  objetivo
        /// </summary>
        /// <param name="from">ConnectionPoint origen de la conexión que sera creada</param>
        /// <param name="target">ConnectionPoint objetivo de la conexión que sera creada</param>
        public Connection(ConnectionPoint from, ConnectionPoint target)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from", "from can not be null");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target", "target can not be null");
            }
            this.Source = from;
            this.Target = target;
            Source.Connection.Add(this);
            Target.Connection.Add(this);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método que reinicia la conexión. Este método llama al método Reset del  ConnectionPoint objetivo y emite el evento Delete.
        /// </summary>
        public void Reset(bool resetTarget, bool resetSource)
        {
            if (resetSource)
            {
                Source.Reset(this);
            }
            if (resetTarget)
            {
                Target.Reset(this);
            }
            if (Delete != null)
            {
                Delete(this, new EventArgs());
            }
        }

        #endregion

        #region Events
        public event EventHandler Delete;
        #endregion
    }
}

