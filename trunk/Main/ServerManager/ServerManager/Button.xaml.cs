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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Lógica de interacción para Button.xaml.
    /// </summary>
    public partial class Button
    {
        #region Constants, Variables and Properties

        /// <summary>
        /// El texto que se mostrará en el botón.
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
        /// La ubicación de la imagen que se mostrará en el botón.
        /// </summary>
        public string Image
        {
            get
            {
                return ((BitmapImage)Icon.Source).BaseUri.AbsoluteUri;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public Button()
        {
            this.InitializeComponent();
            Enabled = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método invocado cuando el botón se clickea.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null && enabled)
            {
                Clicked(sender, e);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento lanzado cuando el botón es clickeado.
        /// </summary>
        public event EventHandler Clicked;

        #endregion
    }
}