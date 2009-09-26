using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PresentationLayer.ServerDesignerClasses;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace LogicalLibrary.DataModelClasses
{
    public class Field
    {
        #region Constants, Variables and Properties

        private DataType dataType;
        public DataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set 
            {
                name = value; 
            }
        }

        private Table table;
        public Table Table
        {
            get { return table; }
            set { table = value; }
        }

        #endregion

        #region Constructors

        public Field(string name, DataType dataType)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name","name can not be null or Empty");
            }
            this.Name = name;
            this.DataType = dataType;
        }

        public Field()
        {
        }

		#endregion

        #region Instance Methods

        public override string ToString()
        {
            return this.Name + ":" + LogicalLibrary.Utilities.GetDataType(this.DataType);
        }

        public override bool Equals(object obj)
        {
            Field compareField = obj as Field;
            if (compareField == null)
            {
                return false;
            }
            if (compareField.Table != this.Table)
            {
                return false;
            }
            if (compareField.Name != this.Name)
            {
                return false;
            }
            if (compareField.DataType != this.DataType)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return (this.Name.GetHashCode() + (int)this.DataType);
        }

        public void Validate(Collection<Error> errors)
        {
            if (String.IsNullOrEmpty(this.name))
            {
                errors.Add(new Error(LogicalLibrary.Properties.Resources.FieldError, "", LogicalLibrary.Properties.Resources.NoFieldName + " " + this.table.Name));
            }
        }
        #endregion
    }
}
