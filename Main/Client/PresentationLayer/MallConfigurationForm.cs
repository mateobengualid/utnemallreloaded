using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using UtnEmall.Client.PresentationLayer;
using System.Net;
using System.Net.Sockets;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario para configuración del servidor
    /// </summary>
    public partial class MallConfigurationForm : Form
    {
        public MallConfigurationForm()
        {
            InitializeComponent();
            int width, headerHeight;

            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);

            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;

            labelServerIp.Top = 5 + headerHeight;
            labelServerIp.Left = 5;
            textBoxServerIp.Top = 35 + headerHeight;
            textBoxServerIp.Left = 5;
            textBoxServerIp.Width = width - 10;
        }

        /// <summary>
        /// Metodo llamado cuando se presiona "Guardar" en el ménu.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string ipServer = textBoxServerIp.Text;

            try
            {
                // Intenta resolver por nombre o ip
                IPHostEntry hostEntry = Dns.GetHostEntry(ipServer);

                if (hostEntry.AddressList.Length > 0)
                {
                    ipServer = hostEntry.AddressList[0].ToString();
                    if (ipServer.StartsWith("::", StringComparison.Ordinal)) ipServer = "[" + ipServer + "]";
                }
                else
                {
                    // Controlar la dirección IP
                    IPAddress ip = IPAddress.Parse(ipServer);
                    ipServer = ip.ToString();
                }

                string portServer = BackgroundBroadcastService.BackgroundBroadcast.DefaultServerPort;
                string pingTime = BackgroundBroadcastService.BackgroundBroadcast.DefaultPingTime;

                UtnEmallClientApplication.CreateMallServerFile(ipServer, portServer, pingTime);
                UtnEmallClientApplication.Instance.ReconfigureBackgroundBroadcastService();

                Cursor.Current = Cursors.Default;
                BaseForm.ShowMessage(
                    global::PresentationLayer.GeneralResources.SaveMallSuccess,
                    global::PresentationLayer.GeneralResources.SuccessTitle);

                this.Close();
            }
            catch (SocketException)
            {
                Cursor.Current = Cursors.Default;
                BaseForm.ShowErrorMessage(
                global::PresentationLayer.GeneralResources.InvalidHostAddressError + ipServer,
                global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            catch (System.FormatException)
            {
                Cursor.Current = Cursors.Default;
                BaseForm.ShowErrorMessage(
                global::PresentationLayer.GeneralResources.InvalidHostAddressError + ipServer,
                global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            
        }

        /// <summary>
        /// Metodo llamado cuando se presiona "Cancelar".
        /// Cierra el formulario y limpia los recursos utilizados
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}