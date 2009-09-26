using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Janus.Client.PresentationLayer
{
    public partial class MessageForm : Form
    {
        /// <summary>
        /// El mensaje mostrado al usuario
        /// </summary>
        public Label LabelMessage
        {
            get
            {
                return labelMessage;
            }
        }
        /// <summary>
        /// Obtiene o establece el título del formulario
        /// </summary>
        public string FormTitle
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el icono de la imagen a mostrar
        /// </summary>
        public Image Icon
        {
            get
            {
                return pictureBoxIcon.Image;
            }
            set
            {
                pictureBoxIcon.Image = value;
            }
        }

        public MessageForm()
        {
            InitializeComponent();
        }

        private void menuItemBegin_Click(object sender, EventArgs e)
        {

        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {

        }
    }
}