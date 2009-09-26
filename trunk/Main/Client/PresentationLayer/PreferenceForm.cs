using System;
using System.Windows.Forms;
using UtnEmall.Client.EntityModel;
using System.Collections;
using UtnEmall.Client.BusinessLogic;
using System.Collections.Generic;
using System.ServiceModel;
using System.Reflection;
using System.Collections.ObjectModel;
using UtnEmall.Client.SmartClientLayer;
using System.Diagnostics;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario de preferencias del Clietne
    /// </summary>
    public partial class PreferenceForm : Form
    {
        private List<KeyValuePair<CheckBox, PreferenceEntity>> list;
        private CustomerEntity customer;

        /// <summary>
        /// Constructor
        /// Inicializa el componente y la lista de preferencias
        /// </summary>
        /// <param name="customerEntity">
        /// La entidad que representa el cliente que esta usando la aplicación
        /// </param>
        public PreferenceForm(CustomerEntity customerEntity)
        {
            InitializeComponent();

            int height, width, headerHeight;

            height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);

            this.Height = height;
            this.Width = width;

            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;

            if (customerEntity == null)
            {
                throw new ArgumentException("Customer's preferences cant'be null");
            }
            else
            {
                customer = customerEntity;
                if (customer.Preferences == null)
                {
                    // Si no hay preferencias se usa una lista vacia
                    customer.Preferences = new Collection<PreferenceEntity>();
                }
                ListPreferences();
            }
        }

        /// <summary>
        /// Muestra una lista de categorias. Primero muestra las preferencias del cliente y luego el resto
        /// </summary>
        /// <param name="customerEntity">
        /// La entidad que representa el cliente que esta usando la aplicación
        /// </param>
        private void ListPreferences()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                list = new List<KeyValuePair<CheckBox, PreferenceEntity>>();
                int lastTop = header.Height + 5;

                // Muestra las preferencias del cliente. Un checkbox se utiliza para cada categoria
                foreach (PreferenceEntity preference in customer.Preferences)
                {
                    CheckBox checkBoxPreference = new CheckBox();

                    // Establecer el nombre del checkbox como el nombre de la categoria
                    checkBoxPreference.Text = preference.Category.Name;
                    // Si la preferencia es activa marcar el checkbox
                    if (preference.Active)
                    {
                        checkBoxPreference.Checked = true;
                    }
                    else
                    {
                        checkBoxPreference.Checked = false;
                    }
                    checkBoxPreference.Top = lastTop;
                    lastTop += checkBoxPreference.Height;
                    this.Controls.Add(checkBoxPreference);

                    list.Add(new KeyValuePair<CheckBox, PreferenceEntity>(checkBoxPreference, preference));
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Metodo llamda cuando se presiona "Atras".
        /// Cierra el formulario y limpia los recursos utilizados
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
               

        /// <summary>
        /// Metodo llamado cuando se preciona "Guardar".
        /// Actualiza las preferencias del cliente.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemSave_Click(object sender, EventArgs e)
        {
            foreach ( KeyValuePair<CheckBox,PreferenceEntity> keyvalue in list)
            {
                PreferenceEntity preference = keyvalue.Value;
                // Si la categoria se encuentra seleccinada marcar como activa en las preferencias del cliente
                if (keyvalue.Key.Checked)
                {
                    preference.Active = true;
                }
                else
                {
                    preference.Active = false;
                }
            }
            // Cerrar el formulario
            this.Close();
        }

        private void PreferenceForm_Load(object sender, EventArgs e)
        {

        }
    }
}