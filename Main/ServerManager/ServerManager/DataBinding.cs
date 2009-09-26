using System;
using System.Collections.Generic;
using System.Text;

namespace UtnEmall.ServerManager
{
    public class DataBinding
    {
        private string column;
        /// <summary>
        /// El nombre de la columna.
        /// </summary>
        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        private string field;
        /// <summary>
        /// El nombre del campo.
        /// </summary>
        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="column">El nombre de la columna.</param>
        /// <param name="field">El nombre del campo.</param>
        public DataBinding(string column, string field)
        {
            this.column = column;
            this.field = field;
        }
    }
}
