using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicalLibrary.ServerDesignerClasses
{
    public enum ConnectionPointType
    {
        Input = 1,
        Output = 2,
    }

    /// <summary>
    /// Clase que representa un punto de conexión para el componente mediante el cual  este puede conectarse a otros componentes.
    /// </summary>
    public class ConnectionPoint
    {
        #region Constants, Variables and Properties

        private ConnectionPointType connectionPointType;
        /// <summary>
        /// Una clase de punto de conexión. Puede ser un punto de conexión de entrada o  salida.
        /// </summary>
        public ConnectionPointType ConnectionPointType
        {
            get { return connectionPointType; }
            set { connectionPointType = value; }
        }

        private List<Connection> connection;
        /// <summary>
        /// Conexión que contiene el ConnectionPoint
        /// </summary>
        public List<Connection> Connection
        {
            get 
            {
                if (connection == null)
                {
                    connection = new List<Connection>();
                }
                return connection; 
            }

        }

        private double xCoordinateRelativeToParent;
        public double XCoordinateRelativeToParent
        {
            get { return xCoordinateRelativeToParent; }
            set { xCoordinateRelativeToParent = value; }
        }

        private double yCoordinateRelativeToParent;
        public double YCoordinateRelativeToParent
        {
            get { return yCoordinateRelativeToParent; }
            set { yCoordinateRelativeToParent = value; }
        }

        private Component parent;
        /// <summary>
        /// Componente padre que contiene el ConnectionPoint
        /// </summary>
        public Component Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor que inicializa el ConnectionPointType para el ConnectionPoint 
        /// </summary>
        /// <param name="connectionPointType">ConnectionPointType para  el ConnectionPoint</param>
        /// <param name="parent">Componente adjuntado al punto de conexión</param>
        public ConnectionPoint(ConnectionPointType connectionPointType, Component parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent", "parent can not be null");
            }
            this.Parent = parent;

            this.ConnectionPointType = connectionPointType;            
        }
        /// <summary>
        /// Constructor que inicializa el ConnectionPointType para el ConnectionPoint 
        /// </summary>
        /// <param name="connectionPointType">ConnectionPointType para  el ConnectionPoint</param>
        public ConnectionPoint(ConnectionPointType connectionPointType)
        {
            this.ConnectionPointType = connectionPointType;
        }

        /// <summary>
        /// Constructor que inicializa el padre para el ConnectionPoint
        /// </summary>
        public ConnectionPoint()
        {
            this.Parent = new Component();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método que reinica el ConnectionPoint.
        /// Llama al método padre Reset si el ConnectionType es de entrada o llama al   método Reset Connection si el ConnectionType es de salida.
        /// </summary>
        public void Reset(Connection connectionCaller)
        {
            if (this.ConnectionPointType == ConnectionPointType.Input)
            {
                if (connectionCaller != null)
                {
                    bool otherInputContext = false;
                    foreach (Connection conn in this.Connection)
                    {
                        if (conn != connectionCaller && conn.DataContext != null)
                        {
                            otherInputContext = true;
                        }
                    }
                    if (!otherInputContext)
                    {
                        (Parent as Component).Reset(this);
                    }
                    this.Connection.Remove(connectionCaller);
                }
                else
                {
                    foreach (Connection conn in this.Connection)
                    {
                        conn.Reset(false, true);
                    }
                    this.Connection.Clear();
                }
            }
            else
            {
                if (connectionCaller == null)
                {
                    foreach (Connection conn in this.Connection)
                    {
                        conn.Reset(true, false);
                    }
                    this.Connection.Clear();
                }
                else
                {
                    this.Connection.Remove(connectionCaller);
                }
            }
        }

        #endregion
    }
}

