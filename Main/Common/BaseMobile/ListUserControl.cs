using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace UtnEmall.Client.UserControls
{
    /// <summary>
    /// Control de usuario para mostrar una lista de controles propia
    /// </summary>
    public partial class ListUserControl : UserControl
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties

        // Una lista de items personalizados
        private ArrayList itemsUserControl;
        private int selectedIndex;
        private Color unselectedItemColor;
        private Color selectedItemColor;
        private Color alternateRowColor;

        public UserControl SelectedItem
        {
            get
            {
                if (selectedIndex >= 0 && itemsUserControl.Count > 0)
                {
                    return (UserControl)itemsUserControl[selectedIndex];
                }
                return null;
            }
            set
            {
                if (value != null && itemsUserControl.Contains(value))
                {
                    int index = itemsUserControl.IndexOf(value);
                    this.SelectedIndex = index;
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (selectedIndex >= 0)
                {
                    return selectedIndex;
                }

                return -1;
            }
            set
            {
                selectItemAt(value);
            }
        }

        public Color UnselectedItemColor
        {
            get
            {
                return this.unselectedItemColor;
            }
            set
            {
                this.unselectedItemColor = value;
            }
        }

        public Color SelectedItemColor
        {
            get
            {
                return this.selectedItemColor;
            }
            set
            {
                //this.selectedItemColor = value;
            }
        }

        public Color AlternateRowColor
        {
            get
            {
                return this.alternateRowColor;
            }
            set
            {
                this.alternateRowColor = value;
            }
        }
        #endregion

        #endregion

        #region Constructors

        public ListUserControl(Form parent)
        {
            InitializeComponent();
            itemsUserControl = new ArrayList();
            selectedItemColor = Color.FromArgb(160, 192, 222);
            unselectedItemColor = Color.FromArgb(227, 234, 235);
            alternateRowColor = Color.White;
            
            if (parent == null)
            {
                throw new System.ArgumentException("parent cannot be null", "parent");
            }

            this.Parent = parent;
            ((Form)this.Parent).KeyDown += new KeyEventHandler(ListItemUserControl_KeyDown);
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

        // Agrega un utem al control
        public void AddItem(UserControl item)
        {
            itemsUserControl.Add(item);
            item.Tag = itemsUserControl.Count - 1;
            item.Click += new System.EventHandler(item_Click);

            // Establece el ancho al mismo tamaño que el formulario
            item.Width = this.Width;
            item.BackColor = unselectedItemColor;

            // antes de mostrar el item, relocalizarlo debajo del último
            int count = Controls.Count;

            if ((count % 2) != 0)
            {
                item.BackColor = alternateRowColor;
            }

            if (count > 0)
            {
                UserControl lastItem = (UserControl)Controls[count - 1];
                int newYPos = (lastItem.Height * count) + 1;
                item.Location = new System.Drawing.Point(0, newYPos);
            }
            else
            {
                selectItemAt(0);
            }

            Controls.Add(item);
        }

        void item_Click(object sender, System.EventArgs e)
        {
            try
            {
                selectItemAt((int)((UserControl)sender).Tag);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Protected and Private Instance Methods

        // cambia el color de fondo del item seleccionado
        private void selectItemAt(int index)
        {
            if (index >= 0 && index < itemsUserControl.Count)
            {
                UserControl selectedItem = this.SelectedItem;
                
                if (selectedItem != null)
                {
                    selectedItem.BackColor = unselectedItemColor;
                    if ((selectedIndex % 2) != 0)
                    {
                        selectedItem.BackColor = alternateRowColor;
                    }
                }

                this.selectedIndex = index;
                selectedItem = (UserControl)itemsUserControl[index];
                selectedItem.BackColor = selectedItemColor;
                selectedItem.Focus();
            }
        }

        // cambia el item seleccionado de acuerdo al pedido del usuario
        private void ListItemUserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                int index = 0;

                if (this.SelectedIndex != -1)
                {
                    // mueve la selección al elemento anterior
                    index = SelectedIndex - 1;

                    if (index < 0)
                    {
                        // Mueve el la selección al último item
                        index = itemsUserControl.Count - 1;
                    }
                }

                selectItemAt(index);
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                int index = 0;

                if (this.SelectedIndex != -1)
                {
                    // Mueve la selección al siguiente item
                    index = SelectedIndex + 1;

                    if (index == itemsUserControl.Count)
                    {
                        // Mueve la selección al primer item
                        index = 0;
                    }
                }

                selectItemAt(index);
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Izquierda
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Derecha
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }
        }
        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }
}
