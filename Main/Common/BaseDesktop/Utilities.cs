using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace UtnEmall.Server.Base
{
    public static class Utilities
    {
        #region Static Methods

        /// <summary>
        /// Calcula el hash del string indicado usando SHA1 y retorna la cadena en Base64.
        /// </summary>
        /// <param name="value">El string a hashear.</param>
        /// <returns>Un string en formato Base64 con el hash.</returns>
        public static string CalculateHashString(string value)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashByteArray = sha.ComputeHash(Encoding.ASCII.GetBytes(value));
            return System.Convert.ToBase64String(hashByteArray, 0, hashByteArray.Length);
        }

        /// <summary>
        /// Convierte una imagen, codificada como base64 a un objeto System.Drawing.Image.
        /// </summary>
        /// <param name="strImage">El string representando la imagen.</param>
        /// <returns>Un objeto System.Drawing.Image.</returns>
        public static Image StringToImage(string image)
        {
            Image img = null;
            char[] imageAsChars;
            byte[] imageAsBytes;

            // Transformar el string Base64 a un array de bytes
            imageAsChars = image.ToCharArray();
            imageAsBytes = Convert.FromBase64CharArray(imageAsChars, 0, imageAsChars.Length);

            MemoryStream ms = new MemoryStream(imageAsBytes);

            // Intentar transformar los bytes a un bitmap
            try
            {
                img = new Bitmap(ms);
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return img;
        }

        /// <summary>
        /// Retorna un identificador válido sin espacios.
        /// </summary>
        /// <param name="value">El valor a validar.</param>
        /// <param name="usedAsString">Si el valor será usado dentro de un literal de cadena.</param>
        /// <returns>Un string para ser usado de forma segura como identificador.</returns>
        public static string GetValidIdentifier(string value, bool usedAsString)
        {
            return GetValidIdentifier(value, usedAsString, true);
        }

        /// <summary>
        /// Retorna un identificador válido sin espacios.
        /// </summary>
        /// <param name="value">El valor a validar.</param>
        /// <param name="usedAsString">Si el valor será usado dentro de un literal de cadena.</param>
        /// <param name="alwaysUseHash">Si se debe anexar un hash al final del identificador</param>
        /// <returns>Un string para ser usado de forma segura como identificador.</returns>
        public static string GetValidIdentifier(string value, bool usedAsString, bool alwaysUseHash)
        {
            string returnString = String.Empty;
            bool requireHash = false;
            for (int n = 0; n < value.Length; n++)
            {
                char letter = value[n];
                // Si es un letra o número ASCII, de lo contrario se requiere hash
                if (letter >= 'a' && letter <= 'z' || letter >= 'A' && letter <= 'Z' || letter >= '0' && letter <= '9')
                {
                    if (n == 0 && letter >= '0' && letter <= '9') returnString += '_';
                    returnString += letter;
                }
                else
                {
                    if (letter == ' ')
                    {
                        returnString += '_';
                    }
                    else
                    {
                        requireHash = true;
                    }
                }
            }
            if (alwaysUseHash || requireHash)
            {
                // Agregar el hash para asegurarse que el identificador no genera conflicto con clases existentes
                returnString += "_" + value.GetHashCode().ToString("00000000000", NumberFormatInfo.InvariantInfo).Replace("-", "_");
            }
            if (usedAsString)
            {
                return returnString;
            }
            else
            {
                return "@" + returnString;
            }
        }

        /// <summary>
        /// Retorna un string seguro a ser usado en la generación de assemblies.
        /// </summary>
        /// <param name="value">El valor a controlar.</param>
        /// <returns>Un string seguro a ser usado en el programa LayerD.</returns>
        public static string GetValidStringValue(string value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            // Eliminar espacios y codificar como base64 para escapar caracteres unicode
            value = value.Trim();
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value)); 
            // Para decodificar usar lo siguiente :
            // System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        #endregion
    }
}
