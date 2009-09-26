using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Janus.Client.EntityModel;

namespace Janus.Client.UserControls
{
    public class ItemUserControl : UserControl
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties
        private IEntity entity;
        private System.ComponentModel.IContainer components = null;

        public IEntity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }
        #endregion

        #endregion

        #region Constructors
        public ItemUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Static Methods

        #region Public Static Methods

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        #endregion

        #region Protected and Private Instance Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }
        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }
}
