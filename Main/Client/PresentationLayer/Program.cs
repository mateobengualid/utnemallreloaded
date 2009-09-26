using System;
using System.Windows.Forms;
using System.Reflection;
using UtnEmall.Client.DataModel;
using UtnEmall.Client.BusinessLogic;

namespace UtnEmall.Client.PresentationLayer
{
    static class Program
    {
        /// <summary>
        /// El punto de entrada de la aplicación
        /// </summary>
        [MTAThread]
        static void Main()
        {
            UtnEmallClientApplication.Instance.Run();
        }
    }
}