using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UtnEmall.Client.PresentationLayer
{
    public partial class MessageForm : BaseForm
    {
        /// <summary>
        /// Obtiene el Label que muestra el mensaje al usuario
        /// </summary>
        public Label LabelMessage
        {
            get
            {
                return labelMessage;
            }
        }

        /// <summary>
        /// Obtiene o establece el título
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
        /// Obtiene o establece la imagen a mostrar
        /// </summary>
        public new Image Icon
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
            if (this.Owner != null)
            {
                ((BaseForm) this.Owner).GoBackToBegin = true;
            }
            this.GoBackToBegin = true;
            this.Close();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                ((BaseForm) this.Owner).FinalizeService = true;
            }
            this.FinalizeService = true;
            this.Close();
        }

        public new DialogResult ShowDialog()
        {
            // relocaliza los componentes
            int width, headerHeight, step;

            // height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);

            // stepBase = (int)(height / 11.0);
            step = headerHeight;

            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;

            pictureBoxIcon.Top = step;
            pictureBoxIcon.Left = (width / 2) - (pictureBoxIcon.Width / 2);
            pictureBoxIcon.SizeMode = PictureBoxSizeMode.StretchImage;

            labelMessage.Top = pictureBoxIcon.Bottom + 2;
            labelMessage.Width = width;
            labelMessage.Left = 0;

            // muestra el dialogo
            return base.ShowDialog();
        }

        private void MessageForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                this.AutoScrollPosition = new Point(0, -this.AutoScrollPosition.Y - 10);
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                this.AutoScrollPosition = new Point(0, -this.AutoScrollPosition.Y + 10);
            }
        }

        /// <summary>
        /// Selecciona un mensaje a mostrar desde el archivo de recursos
        /// </summary>
        /// <param name="key">Clave del recurso a buscar</param>
        /// <returns></returns>
        public static string GetMessage(int key)
        {
            string message = BaseMobile.BaseMobileResources.DefaultMessage;
            switch (key)
            {
                case 1:
                    message = BaseMobile.BaseMobileResources.SuccessTitle;
                    break;
                case 2:
                    message = BaseMobile.BaseMobileResources.FailureTitle;
                    break;
                case 3:
                    message = BaseMobile.BaseMobileResources.ProblemsMessage;
                    break;
                case 4:
                    message = BaseMobile.BaseMobileResources.EndingMessage;
                    break;
                case 5:
                    message = BaseMobile.BaseMobileResources.ConnectionProblemMessage;
                    break;
                case 6:
                    message = BaseMobile.BaseMobileResources.WrongDataFormatMessage;
                    break;
                case 7:
                    message = BaseMobile.BaseMobileResources.EndingTitle;
                    break;
            }

            return message;
        }
    }
}