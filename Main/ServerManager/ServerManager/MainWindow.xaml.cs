using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.ServerManager
{
    public partial class MainWindow
    {
        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            Closed += new EventHandler(MainWindow_Closed);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método invoacdo cuando la ventana principal es cerrada, o cuando se finaliza la sesión con el servidor
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            main.LogOff();
        }

        #endregion
    }
}