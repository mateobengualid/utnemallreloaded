using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.ServerDesignerClasses;

namespace LogicalLibrary
{
    public class StatisticsForm: Widget
    {
        private Form baseForm;
        /// <summary>
        /// El formulario en que se basa el formulario de estadísticas.
        /// </summary>
        public Form BaseForm
        {
            get { return baseForm; }
            set { baseForm = value; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
