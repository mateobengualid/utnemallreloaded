using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace Janus.Client.UserControls
{
    /// <summary>
    /// Implementa una lista de controles de usuario
    /// </summary>
    public partial class ListItemUserControl : UserControl
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties

        // Una lista de items propietarios
        private ArrayList itemsUserControl;
        private int selectedIndex;
        private Color unSelectedItemColor;
        private Color selectedItemColor;

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
        #endregion

        #endregion

        #region Constructors

        public ListItemUserControl(Form parent)
        {
            InitializeComponent();
            itemsUserControl = new ArrayList();
            Dock = DockStyle.Fill;
            selectedItemColor = Color.LightSteelBlue;
            unSelectedItemColor = Color.LightBlue;

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

        // Agrega un item al control
        public void AddItem(UserControl item)
        {
            itemsUserControl.Add(item);

            // Establece el ancho del item al mismo tamaño del formulario
            item.Width = (int)(this.Width * 0.94);
            item.BackColor = unSelectedItemColor;

            // Después de mostrar el item, relocalizarlo debajo del último
            int count = Controls.Count;
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
        #endregion

        #region Protected and Private Instance Methods

        // Cambia el color de fondo del item seleccionado
        private void selectItemAt(int index)
        {
            if (index >= 0 && index < itemsUserControl.Count)
            {
                UserControl selectedItem = this.SelectedItem;
                
                if (selectedItem != null)
                {
                    selectedItem.BackColor = unSelectedItemColor;
                }

                this.selectedIndex = index;
                selectedItem = (UserControl)itemsUserControl[index];
                selectedItem.BackColor = selectedItemColor;
                selectedItem.Focus();
            }
        }

        // Cambia el item seleccionado
        private void ListItemUserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                int index = 0;

                if (this.SelectedIndex != -1)
                {
                    // Cambia la selección del item de acuerdo a la tecla presionada por el usuario
                    index = SelectedIndex - 1;

                    if (index < 0)
                    {
                        // Mueve la selección al elemento actual
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
