using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define la ventana que contiene el componente Main.xaml
    /// </summary>
    public partial class ServerManagerWindow
    {
        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public ServerManagerWindow()
        {
            this.InitializeComponent();
            this.Hide();

            UtnEmall.ServerManager.MainWindow window = new UtnEmall.ServerManager.MainWindow();
            window.Closed += new EventHandler(window_Closed);
            window.Show();
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