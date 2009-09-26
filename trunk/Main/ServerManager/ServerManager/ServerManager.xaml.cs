using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace JanusMain
{
    /// <summary>
    /// Esta clase define la ventana que contiene el componente Main.xaml
    /// </summary>
    public partial class ServerManager
    {
        #region Constructors

        /// <summary>
        /// Cosntructor de clase
        /// </summary>
        public ServerManager()
        {
            this.InitializeComponent();
            this.Hide();
            Janus.MainWindow window = new Janus.MainWindow();
            window.Closed += new EventHandler(window_Closed);
            window.Show();

            //  Insert code required on object creation below this point.
            // TODO Put the code, if it is necessary.
            //
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método invocado cuando la ventana principal se cierra
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera al evento
        /// </param>
        /// <param name="e">
        /// Contiene infomación adicional acerca del evento
        /// </param>
        private void window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion
    }
}