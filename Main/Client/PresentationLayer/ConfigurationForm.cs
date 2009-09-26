using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Esta clase implementa el formulario de Configuración
    /// </summary>
    public partial class ConfigurationForm : Form
    {
        /// <summary>
        /// Inicializa el formulario
        /// </summary>
        public ConfigurationForm()
        {
            InitializeComponent();
            int height, width, logoHeight, iconSize;

            height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            logoHeight = width / 3;
            iconSize = (int)(width / 4.5);

            this.Height = height;
            this.Width = width;

            logo.Width = width;
            logo.Height = logoHeight;

            listViewConfigurationOption.Top = logoHeight;
            listViewConfigurationOption.Left = 0;
            listViewConfigurationOption.Height = Utilities.VisibleScreenSize.Height - logoHeight;
            listViewConfigurationOption.Width = width;

            ImageList images = new ImageList();
            
            // Agregar algunas imagenes
            Bitmap icon;
            try
            {
                images.ImageSize = new Size(iconSize, iconSize);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_services.png");
                // Agregar las opciones del servicio
                images.Images.Add(icon);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_options.png");
                // Agregar la opción de configuración
                images.Images.Add(icon);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_help.png");
                // Agregar la opción de ayuda
                images.Images.Add(icon);

                listViewConfigurationOption.LargeImageList = images;
            }
            catch (IOException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.ImageNotFoundError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
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
        /// Método llamado cuando se selecciona "Seleccionar".
        /// Abre el formulario adecuado de acuerdo a la selección del usuario.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            ListViewItem itemSelected = listViewConfigurationOption.FocusedItem;

            if (itemSelected == null)
            {
                return;
            }
            
            switch (itemSelected.Index)
            {
                case 0:
                    MallConfigurationForm mallConfigurationForm = new MallConfigurationForm();
                    mallConfigurationForm.Owner = this;
                    mallConfigurationForm.ShowDialog();
                    break;

                case 1:
                    CustomerConfigurationForm customerConfigurationForm = new CustomerConfigurationForm();
                    customerConfigurationForm.Owner = this;
                    customerConfigurationForm.ShowDialog();
                    break;

                case 2:
                    HelpForm helpForm = new HelpForm(global::PresentationLayer.GeneralResources.ConfigurationHelp);
                    helpForm.ShowDialog();
                    break;

                default: break;
            }
        }
    }
}