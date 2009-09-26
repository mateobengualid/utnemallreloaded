using System;
using System.Collections.Generic;
using System.Text;
using UtnEmall.Server.EntityModel;
using System.Windows;
using UtnEmall.ServerManager;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager.Properties;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define métodos estáticos que son usados en el proyecto.
    /// </summary>
    static class Util
    {
        #region Static Methods

        /// <summary>
        /// Muestra un mensaje de diálogo mostrando un error
        /// </summary>
        /// <param name="Message">El mensaje a mostrar</param>
        public static void ShowErrorDialog(string Message)
        {
            ShowErrorDialog(Message, Resources.Error);
        }

        /// <summary>
        /// Muestra un cuadro de diálogo de error
        /// </summary>
        /// <param name="Message">El mensaje a mostrar</param>
        /// <param name="errors">Una lista de errores a mostrar en detalle</param>
        public static void ShowErrorDialog(string Message, Collection<Error> errors)
        {
            ErrorDialog error = new ErrorDialog(Message, errors);
            error.ShowDialog();
        }

        /// <summary>
        /// Muestra un cuadro de diálogo de error
        /// </summary>
        /// <param name="Message">El mensaje a mostrar</param>
        /// <param name="Title">El título del cuadro de diálogo</param>
        public static void ShowErrorDialog(string Message, string Title)
        {
            ErrorDialog error = new ErrorDialog(Message);
            error.Title = Title;
            error.ShowHideButton.Visibility = Visibility.Hidden;
            
            error.ShowDialog();
        }

        /// <summary>
        /// Muestra un cuadro de diálogo de confirmación
        /// </summary>
        /// <param name="Message">El mensaje a confirmar</param>
        /// <param name="Title">El título del cuadro de diálogo</param>
        /// <returns>True si el usuario confirma</returns>
        public static bool ShowConfirmDialog(string Message, string Title)
        {
            return MessageBox.Show(Message, Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Muestra un cuadro de diálogo de información
        /// </summary>
        /// <param name="Message">El mensaje a mostrar</param>
        /// <param name="Title">El título del cuadro de diálogo</param>
        public static void ShowInformationDialog(string Message, string Title)
        {
            MessageBox.Show(Message,
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
        }

        #endregion
    }
}
