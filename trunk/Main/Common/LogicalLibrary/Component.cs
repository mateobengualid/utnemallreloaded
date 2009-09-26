// using UtnEmall.Server.EntityModel;

using LogicalLibrary.DataModelClasses;
using System;
using LogicalLibrary.ServerDesignerClasses;
namespace LogicalLibrary
{
    /// <summary>
    /// Una clase que representa un objeto que va a ser mostrado.
    /// </summary>
    public class Component
    {
        #region Constants, Variables and Properties

        private ConnectionPoint inputConnectionPoint;
        /// <summary>
        /// Un ConnectionPoint de entrada para el formulario.
        /// </summary>
        public ConnectionPoint InputConnectionPoint
        {
            get { return inputConnectionPoint; }
            set { inputConnectionPoint = value; }
        }

        private ConnectionPoint outputConnectionPoint;
        /// <summary>
        /// Un ConnectionPoint de salida para el formulario.
        /// </summary>
        public ConnectionPoint OutputConnectionPoint
        {
            get { return outputConnectionPoint; }
            set { outputConnectionPoint = value; }
        }

        private Table inputDataContext;
        /// <summary>
        /// Datos de entrada de contexto para el componente, esto es una tabla del modelo  de datos que representa una entrada de datos con la cual el componente    trabaja.
        /// </summary>
        public Table InputDataContext
        {
            get { return inputDataContext; }
            set { inputDataContext = value; }
        }

        private Table outputDataContext;
        /// <summary>
        /// Contexto de datos de salida para el componente, es una tabla del modelo de    datos que representa un dato de salida con el cual trabaja el componente.
        /// </summary>
        public Table OutputDataContext
        {
            get { return outputDataContext; }
            set { outputDataContext = value; }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        private double width;
        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        private double xCordenateRelativeToParent;
        /// <summary>
        /// Coordenada 'x' que representa la posición del componente dentro del padre.
        /// </summary>
        public double XCoordinateRelativeToParent
        {
            get { return xCordenateRelativeToParent; }
            set { xCordenateRelativeToParent = value; }
        }
        private double yCordenateRelativeToParent;
        /// <summary>
        /// Coordenada 'y' que representa la posición del componente dentro del padre.
        /// </summary>
        public double YCoordinateRelativeToParent
        {
            get { return yCordenateRelativeToParent; }
            set { yCordenateRelativeToParent = value; }
        }

        private double xFactorCordenateRelativeToParent;
        /// <summary>
        /// Un factor que representa una 'x' coordenada de posición independiente del   padre.
        /// </summary>
        public double XFactorCoordinateRelativeToParent
        {
            get { return xFactorCordenateRelativeToParent; }
            set { xFactorCordenateRelativeToParent = value; }
        }
        
        private double yFactorCordenateRelativeToParent;
        /// <summary>
        /// Un factor que representa una 'y' coordenada de posición independiente del    padre.
        /// </summary>
        public double YFactorCoordinateRelativeToParent
        {
            get { return yFactorCordenateRelativeToParent; }
            set { yFactorCordenateRelativeToParent = value; }
        }

        private double heightFactor;
        /// <summary>
        /// 
        /// </summary>
        public double HeightFactor
        {
            get { return heightFactor; }
            set { heightFactor = value; }
        }

        private double widthFactor;
        /// <summary>
        /// 
        /// </summary>
        public double WidthFactor
        {
            get { return widthFactor; }
            set { widthFactor = value; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Implementa algun comportamiento cuando un dato de contexto es detectado. Por  defecto, la tabla se copia en la entrada.
        /// </summary>
        public virtual void ManageInputDataContext(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            this.inputDataContext = table;
        }

        /// <summary>
        /// Implementa algun comportamiento cuando un dato de contexto es detectado. Por  defecto la tabla se copia en la salida.
        /// </summary>
        public virtual void ManageOutputDataContext(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            this.outputDataContext = table;
        }

        /// <summary>
        /// Método que "Reestablece" el componente. Es decir que pone el estado del  componente en Limpio. La implementación por defecto de este método no hace  nada.
        /// </summary>
        public virtual void Reset(ConnectionPoint previousConnectionPoint)
        {
            this.InputDataContext = null;
            this.OutputDataContext = null;
        }

        /// <summary>
        /// Retorna una copia simple del objeto
        /// </summary>
        /// <returns>Objeto clonado</returns>
        public Component Clone()
        {
            return (Component)this.MemberwiseClone();
        }

        #endregion
    }
}
