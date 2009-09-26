using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Clase general para utilidades.
    /// </summary>
    public class Utilities
    {
        #region Nested Classes and Structures
        #endregion

        #region Constants, Variables and Properties
        private static Utilities instance;
        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties
        // El path de la aplicación en formato string
        private string appPath;
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        private Utilities()
        {
            appPath = GetAppPath();
        }
        #endregion

        
        #region Static Methods

        /// <summary>
        /// Devuelve la sección visible de la pantalla
        /// </summary>
        public static Size VisibleScreenSize
        {
            get
            {
                int height, width;

                height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;

                Size visibleScreen = new Size(width, height);
                if (visibleScreen.Height > 250)
                {
                    visibleScreen.Height = visibleScreen.Height - 26;
                }
                return visibleScreen;
            }
        }

        #region Public Static Methods
        /// <summary>
        /// Obtiene el path actual de la aplicación
        /// </summary>
        public static string AppPath
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utilities();
                }
                return instance.appPath;
            }
        }
        /// <summary>
        /// Muestra una advertencia al usuario
        /// </summary>
        /// <param name="title">Título de la advertencia</param>
        /// <param name="message">Mensaje al usuario</param>
        public static void ShowWarning(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// Muestra un error al usuario
        /// </summary>
        /// <param name="title">Título del dialogo de error</param>
        /// <param name="message">Mensaje al usuario</param>
        public static void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// Realiza una pregunta al usuario
        /// </summary>
        /// <param name="title">Título del dialogo</param>
        /// <param name="message">Mensaje al usuario</param>
        /// <returns>El resultado de la pregunta</returns>
        public static DialogResult ShowQuestion(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Retorna un hash del string original
        /// </summary>
        /// <param name="originalString">String original</param>
        /// <returns>Un hash del string original</returns>
        public static string CalculateHashString(string value)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashByteArray = sha.ComputeHash(Encoding.ASCII.GetBytes(value));
            return System.Convert.ToBase64String(hashByteArray, 0, hashByteArray.Length);
        }

        public static Image String64ToImage(string imageValue)
        {
            if (imageValue == null || imageValue.Trim().Length == 0)
            {
                throw new System.ArgumentException("strImage is not a valid image", "imageValue");
            }

            Image image = null;
            char[] imageAsChars;
            byte[] imageAsBytes;

            // Obtener un string
            imageAsChars = imageValue.ToCharArray();
            // Obtener una matriz de bytes
            imageAsBytes = System.Convert.FromBase64CharArray(imageAsChars, 0, imageAsChars.Length);

            MemoryStream stream = new MemoryStream(imageAsBytes);

            try
            {
                image = new Bitmap(stream);
            }
            catch
            {
                throw new System.ArgumentException("strImage is not a valid image", "imageValue");
            }

            return image;
        }

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        #endregion

        #region Protected and Private Instance Methods
        /// <summary>
        /// Obtiene el path actual de la aplicación
        /// </summary>
        /// <returns>El path de la aplicación</returns>
        static private string GetAppPath()
        {
            Module currentModule = Assembly.GetExecutingAssembly().GetModules()[0];
            string fullPath = currentModule.FullyQualifiedName;
            string path = fullPath.Substring(0, fullPath.IndexOf(currentModule.Name, StringComparison.Ordinal));
            return path;
        }
        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion

    }
}
