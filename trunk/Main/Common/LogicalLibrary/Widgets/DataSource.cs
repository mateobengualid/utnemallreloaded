using System;
using System.Linq;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;

namespace LogicalLibrary.Widgets
{
    public enum OrderType
    {
        Ascendant = 1,
        Descendant = 2
    }

    public class DataSource : Widget
    {
        #region Constants, Variables and Properties

        public String Name
        {
            get { return RelatedTable.Name; }
        }

        public override string Title
        {
            get
            {
                return relatedTable.Name;
            }
            set
            {
                base.Title = value;
            }
        }

        private Field fieldToOrder;
        public Field FieldToOrder
        {
            get { return fieldToOrder; }
            set { fieldToOrder = value; }
        }

        private OrderType typeOrder;
        public OrderType TypeOrder
        {
            get { return typeOrder; }
            set { typeOrder = value; }
        }

        private Table relatedTable;
        public Table RelatedTable
        {
            get { return relatedTable; }
            set { relatedTable = value; }
        }

        #endregion

        #region Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification = "This situation was taken into account")]
        public DataSource(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table", "table can not be null");
            }
            this.TypeOrder = OrderType.Descendant;
            RelatedTable = table;
            if (table.Fields.Count > 0)
            {
                FieldToOrder = table.Fields.ElementAtOrDefault(0);
            }
            ManageInputDataContext(table);
            ManageOutputDataContext(table);
        }

        public DataSource()
        {
            this.FieldToOrder = new Field();
            this.InputConnectionPoint = new ConnectionPoint();
            this.OutputConnectionPoint = new ConnectionPoint();
            this.RelatedTable = new Table("");
            this.OutputDataContext = new Table("");
            this.InputDataContext = new Table("");
        }

        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            base.Reset(previousConnectionPoint);
            this.OutputConnectionPoint.Reset(null);
            
        }

        #endregion
    }
}

