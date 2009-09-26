using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PresentationLayer.ServerDesignerClasses;

namespace LogicalLibrary.DataModelClasses
{
    public class Relation : Component
    {
        #region Constants, Variables and Properties

        private RelationType relationType;
        public RelationType RelationType
        {
            get { return relationType; }
            set { relationType = value; }
        }

        private Table source;
        public Table Source
        {
            get { return source; }
            set { source = value; }
        }

        private Table target;
        public Table Target
        {
            get { return target; }
            set { target = value; }
        }

        #endregion

        #region Constructors

        public Relation(Table source, Table target, RelationType relationType)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "source can not be null");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target", "target can not be null");
            }
            this.Source = source;
            this.Target = target;
            this.RelationType = relationType;
        }

        #endregion
    }
}

