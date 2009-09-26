using System;
using System.Drawing;
using System.Windows.Forms;

namespace Janus.Client.PresentationLayer
{
    public partial class AddService : Form
    {
        public AddService()
        {
            InitializeComponent();
        }

        private void AddService_Load(object sender, EventArgs e)
        {
           icon.Image = new Bitmap(@"storage\windows\question.jpg");
           
        }

        private void menuItemBack_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void menuItemAdd_Click(object sender, EventArgs e)
        {
            // Buscar el ensamblado, descargarlo y correr el servicio
            JanusClientApplication.GetInstance().RunService("", "");
        }
    }
}