using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Lógica de interacción para el botón.
    /// </summary>
    public partial class CustomButton:UserControl
    {
        /// <summary>
        /// Texto que se mostrará en el botón.
        /// </summary>
        public string Text
        {
            get
            {
                return Label.Text;
            }

            set
            {
                Label.Text = value;
            }
        }

        /// <summary>
        /// Rúta de la imagen que será mostrada en el botón.
        /// </summary>
        public string Image
        {
            get
            {
                return ((BitmapImage)Icon.Source).UriSource.AbsoluteUri;
            }

            set
            {
                Icon.Source = new BitmapImage(new Uri(value, UriKind.Relative));
            }
        }

        private bool enabled;
        /// <summary>
        /// Estado del botón.
        /// </summary>
        public bool Enabled
        {
            get
            {

                return enabled;

            }

            set
            {
                enabled = value;

                if (value)
                {
                    Cover.Fill.Opacity = 0.0;
                }
                else
                {
                    Cover.Fill.Opacity = 0.5;
                }
            }
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CustomButton()
        {
            this.InitializeComponent();
            Enabled = true;
        }

        /// <summary>
        /// Metodo llamado cuando el botón es cliqueado.
        /// </summary>
        /// <param name="sender">Objeto que emite el evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null && enabled)
            {
                Clicked(sender, e);
            }
        }

        #region Events

        /// <summary>
        /// Evento disparado cuando el botón es cliqueado.
        /// </summary>
        public event EventHandler Clicked;

        #endregion

        private void Cover_MouseEnter(object sender, MouseEventArgs e)
        {
            OnMouseEnter_Animation.Begin();
        }

        private void Cover_MouseLeave(object sender, MouseEventArgs e)
        {
            OnMouseLeave_Animation.Begin();
        }
    }
}