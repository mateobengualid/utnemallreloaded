using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace LogicalLibrary.DataModelClasses
{
    /// <summary>
    /// Esta clase define un modelo de datos. Un modelo de datos es un grupo de  tablas y relaciones.
    /// </summary>
    public class DataModel
    {
        #region Constants, Variables and Properties

        /// <summary>
        /// Lista de relaciones del modelo de datos
        /// </summary>
        private List<Relation> relations;
        public List<Relation> Relations
        {
            get { return relations; }
        }

        /// <summary>
        /// Lista de tablas del modelo de datos.
        /// </summary>
        private List<Table> tables;
        public List<Table> Tables
        {
            get { return tables; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Clase constructora. Inicializa la lista de relaciones y tablas.
        /// </summary>
        public DataModel()
        {
            this.relations = new List<Relation>();
            this.tables = new List<Table>();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Verifica si usted esta haciendo una conexión con una tabla de almacenamiento.
        /// </summary>
        /// <param name="table">La tabla a verificar</param>
        /// <returns>Un objeto de error si la tabla es de almacenamiento. Nulo si no lo  es
        /// </returns>
        public static Error CheckConnectionWithStorage(Table table)
        {
            if (table != null && table.IsStorage)
            {
                return new Error("", "", LogicalLibrary.Properties.Resources.NoConnectionStorage);
            }

            return null;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Agrega una nueva relación al modelo de datos 
        /// </summary>
        /// <param name="newRelation">La nueva relación a agregar</param>
        public void AddRelation(Relation newRelation)
        {
            if (newRelation == null)
            {
                throw new ArgumentNullException("newRelation", "newRelation can not be null");
            }
            Relations.Add(newRelation);
        }

        /// <summary>
        /// Remueve una relación desde el modelo de datos
        /// </summary>
        /// <param name="relation">La relación a remover</param>
        public void RemoveRelation(Relation relation)
        {
            if (relation == null)
            {
                throw new ArgumentNullException("relation", "relation can not be null");
            }
            Relations.Remove(relation);
        }

        /// <summary>
        /// Agrega una nueva tabla al modelo de datos
        /// </summary>
        /// <param name="newTable">La nueva tabla a agregar</param>
        public void AddTable(Table newTable)
        {
            if (newTable == null)
            {
                throw new ArgumentNullException("newTable", "newTable can not be null");
            }
            Tables.Add(newTable);
        }

        /// <summary>
        /// Remueve una tabla del modelo de datos
        /// </summary>
        /// <param name="table">La tabla a remover</param>
        public void RemoveTable(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            Tables.Remove(table);
        }

        /// <summary>
        /// Limpia la lista de tablas del modelo de datos.
        /// </summary>
        public void RemoveAllTables()
        {
            this.Tables.Clear();
        }

        /// <summary>
        /// Obtiene una tabla por su nombre
        /// </summary>
        /// <param name="nameTable">El nombre de la tabla a buscar</param>
        /// <returns>La tabla encontrada</returns>
        public Table GetTable(String nameTable)
        {
            Table tableReturn = null;
            foreach (Table item in Tables)
            {
                if (string.CompareOrdinal(item.Name, nameTable) == 0)
                {
                    tableReturn = item;
                    break;
                }
            }
            return tableReturn;
        }

        /// <summary>
        /// Esta método busca una lista de tablas que tienen una relación con los  parámetros de la tabla.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>Una lista de tablas que tiene una relación con una tabla  específica
        /// </returns>
        public List<Table> GetRelatedTables(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }

            List<Table> tables = new List<Table>();
            foreach (Relation realtion in Relations)
            {
                if (String.CompareOrdinal(realtion.Source.Name, table.Name) == 0)
                {
                    tables.Add(realtion.Target);
                }
                else if (String.CompareOrdinal(realtion.Target.Name, table.Name) == 0)
                {
                    tables.Add(realtion.Source);
                }
            }
            return tables;
        }

        /// <summary>
        /// Valida la lista de tablas del modelo de datos.
        /// </summary>
        /// <param name="errors">Una colección para agregar al grupo de nuevos  errores
        /// </param>
        public void Validate(Collection<Error> errors)
        {
            foreach (Table table in Tables)
            {
                table.Validate(errors);
            }
        }

        /// <summary>
        /// Verifica si existe una relación entre una tabla de origen y una de destino
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public Error CheckDuplicatedConnection(Table from, Table target)
        {
            foreach (Relation rel in Relations)
            {
                if ((rel.Source == from && rel.Target == target) || (rel.Source == target && rel.Target == from))
                {
                    return new Error("", "", LogicalLibrary.Properties.Resources.DuplicatedRelation);
                }
            }
            return null;
        }

        #endregion
    }
}
