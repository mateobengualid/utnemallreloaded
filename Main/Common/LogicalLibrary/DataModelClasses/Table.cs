using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace LogicalLibrary.DataModelClasses
{
    public class Table : Widget
    {
        #region Constants, Variables and Properties

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<Field> fields;
        public List<Field> Fields
        {
            get { return fields; }
        }

        private bool isStorage;
        public bool IsStorage
        {
            get { return isStorage; }
            set { isStorage = value; }
        }

        #endregion

        #region Constructors

        public Table(string name)
        {
            fields = new List<Field>();
            this.Name = name;
        }

        #endregion

        #region Instance Methods

        public void AddField(Field newField)
        {
            if (newField == null)
            {
                throw new ArgumentNullException("newField", "newField can not be null");
            }
            Fields.Add(newField);
        }

        public void RemoveField(Field field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field", "field can not be null");
            }
            Fields.Remove(field);
        }

        public void RemoveAllFields()
        {
            this.Fields.Clear();
        }

        public Field GetField(String fieldName)
        {
            if (String.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("Param can no be Empty", "fieldName");
            }

            Field returnField = null;
            foreach (Field field in Fields)
            {
                if (string.CompareOrdinal(field.Name, fieldName) == 0)
                {
                    returnField = field;
                    break;
                }
            }
            return returnField;
        }

        public override bool Equals(object obj)
        {
            Table compareObject = obj as Table;

            if (compareObject == null)
            {
                return false;
            }
            if (this.Name != compareObject.Name)
            {
                return false;
            }

            foreach (Field field in Fields)
            {
                bool isEquals = false;
                foreach (Field field_Compare in compareObject.Fields)
                {
                    if (field.Equals(field_Compare) == true)
                    {
                        isEquals = true;
                        break;
                    }
                }
                if (isEquals == false)
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = this.name.GetHashCode();
            foreach (Field field in Fields)
            {
                hashCode += (field.Name.GetHashCode() + (int)field.DataType);
            }
            return hashCode;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void Validate(Collection<Error> errors)
        {
            if (String.IsNullOrEmpty(this.name))
            {
                errors.Add(new Error(LogicalLibrary.Properties.Resources.TableError,"Name",LogicalLibrary.Properties.Resources.NoNameForTable));
            }
            if (!this.isStorage)
            {
                if (this.fields.Count == 0)
                {
                    errors.Add(new Error(LogicalLibrary.Properties.Resources.TableError,"Fields",LogicalLibrary.Properties.Resources.NoFieldsForTable + " " + this.name ));
                }
                foreach (Field field in fields)
                {
                    field.Validate(errors);
                }
            }
        }

        #endregion
    }
}
