using System;
using System.Windows.Forms;
using System.IO;
namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario de Ayuda
    /// </summary>
    public partial class HelpForm : Form
    {
        /// <summary>
        /// Constructor. Abre el archivo de ayuda indicado.
        /// </summary>
        /// <param name="help">
        /// Nombre del archivo de ayuda
        /// </param>
        public HelpForm(string help)
        {
            if (String.IsNullOrEmpty(help))
            {
                throw new ArgumentException(
                    global::PresentationLayer.GeneralResources.HelpNullArgument);
            }

            InitializeComponent();

            int height, width, headerHeight;

            height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);

            this.Height = height;
            this.Width = width;

            helpText.Height = Utilities.VisibleScreenSize.Height - headerHeight;
            helpText.Top = headerHeight;

            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;
            string pathFolder = Path.Combine(Utilities.AppPath, "help");
            string path = pathFolder + "\\"+ help +".txt";

            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                helpText.Text = file.ReadToEnd();
            }
            else
            {
                helpText.Text = global::PresentationLayer.GeneralResources.UtnEmallProject;
            }
        }

        /// <summary>
        /// Metodo llamado cuando se presiona "Aceptar".
        /// Cierra el formulario y limpia los recursos utilizados
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpText_ParentChanged(object sender, EventArgs e)
        {

        }

        private void txtName_GotFocus(object sender, EventArgs e)
        {

        }

        private void txtName_LostFocus(object sender, EventArgs e)
        {

        }
    }
}