using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario principal de la aplicación
    /// </summary>
    public partial class PrincipalForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrincipalForm()
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

            listViewOptions.Left = 0;
            listViewOptions.Top = logoHeight;

            listViewOptions.Height = Utilities.VisibleScreenSize.Height - logoHeight;
            listViewOptions.Width = width;
            

            ImageList images = new ImageList();
            // Agregar algunas imagenes
            Bitmap icon;
            try
            {
                images.ImageSize = new Size(iconSize, iconSize);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_services.png");
                images.Images.Add(icon);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_options.png");
                images.Images.Add(icon);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_help.png");
                images.Images.Add(icon);
                icon = new Bitmap(Utilities.AppPath + @"images\ico_profile.png");
                images.Images.Add(icon);

                listViewOptions.LargeImageList = images;
            } 
            catch (IOException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.ErrorTitle,
                    global::PresentationLayer.GeneralResources.ImageNotFoundError);
            }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"
            , Justification = "This is last change exception controler on user interface event.")]
        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem itemSelected = listViewOptions.FocusedItem;

                if (itemSelected == null)
                {
                    return;
                }

                switch (itemSelected.Index)
                {
                    // Servicios
                    case 0:
                        ServicesForm servicesForm = new ServicesForm();
                        servicesForm.Owner = this;
                        servicesForm.ShowDialog();
                        break;

                    // Ayuda
                    case 1:
                        HelpForm helpForm = new HelpForm(global::PresentationLayer.GeneralResources.ApplicationHelp);
                        helpForm.Owner = this;
                        helpForm.ShowDialog();
                        break;

                    // Perfil
                    case 2:
                        ProfileForm perfilForm = new ProfileForm();
                        perfilForm.Owner = this;
                        perfilForm.ShowDialog();
                        break;

                    // Configuración
                    case 3:
                        ConfigurationForm configurationForm = new ConfigurationForm();
                        configurationForm.Owner = this;
                        configurationForm.ShowDialog();
                        break;

                    default: break;
                }

            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        /// <summary>
        /// Método llamado cuando se presiona "Salir".
        /// Cierra el formulario y limpia los recursos utilizados
        /// Cierra la aplicación
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void listViewOptions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}