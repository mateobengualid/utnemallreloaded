using System;
using System.Collections.Generic;
using System.Text;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define métodos estáticos de validación
    /// </summary>
    class Validator
    {
        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        private Validator()
        {

        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Verifica si una variable de cadena puede ser convertida a entero
        /// </summary>
        /// <param name="number">La cadena a verificar</param>
        /// <returns>true si la variable puede ser convertida</returns>
        public static bool IsNumber(string number)
        {
            int n;
            return int.TryParse(number, out n);
        }

        #endregion
    }
}
