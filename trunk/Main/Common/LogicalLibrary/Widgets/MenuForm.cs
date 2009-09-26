using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;

namespace LogicalLibrary
{
    public class MenuForm : Form
    {
        #region Constants, Variables and Properties

        public int NumberOfItems
        {
            get { return MenuItems.Count; }
        }

        private List<FormMenuItem> menuItems;
        public List<FormMenuItem> MenuItems
        {
            get { return menuItems; }
        }

        #endregion

        #region Constructors

        public MenuForm()
        {
            menuItems = new List<FormMenuItem>();
        }

        #endregion

        #region Instance Methods

        public void AddMenuItem(FormMenuItem menuItem)
        {
            menuItem.Parent = this;
            this.MenuItems.Add(menuItem);
        }

        public void RemoveMenuItem(FormMenuItem menuItem)
        {
            menuItem.Reset(null);
            this.MenuItems.Remove(menuItem);
        }

        public override void Reset(ConnectionPoint previousConnectionPoint)
        {
            foreach (FormMenuItem formMenuItem in this.MenuItems)
            {
                formMenuItem.Reset(previousConnectionPoint);
            }
            this.MenuItems.Clear();
            base.Reset(previousConnectionPoint);
        }

        #endregion
    }
}
