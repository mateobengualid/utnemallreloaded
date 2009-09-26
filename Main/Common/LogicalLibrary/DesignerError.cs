using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicalLibrary
{
    public class DesignerError
    {
        private String message;
        public String Message 
        {
            get { return message; }
        }
        private String componentName;
        public String ComponentName 
        {
            get { return componentName; }
        }

        public DesignerError(String message, String componentName)
        {
            this.message = message;
            this.componentName = componentName;
        }
    }
}
