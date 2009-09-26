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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace UtnEmall.ServerManager
{
    public partial class ErrorDialog
    {
        private bool isShow;
        private Collection<Error> errors;
        /// <summary>
        /// Una lista de errores para mostrar como detalle.
        /// </summary>
        public Collection<Error> Errors
        {
            get { return errors; }
            set
            {
                errors = value;

                ErrorList.Items.Clear();
                if (errors != null)
                {
                    foreach (Error error in errors)
                    {
                        ErrorList.Items.Add(error.Description);
                    }
                }
            }
        }

        /// <summary>
        /// El mensaje de error.
        /// </summary>
        public string Message
        {
            get { return (string)Label.Text; }
            set { Label.Text = value; }
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="message">Mensaje de error.</param>
        /// <param name="errors">Lista de errores para mostrar como detalle.</param>
        public ErrorDialog(string message, Collection<Error> errors)
        {
            this.InitializeComponent();
            isShow = true;
            Errors = errors;
            Label.Text = message;
            Keyboard.Focus(OkButton);
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="message">Mensaje de error.</param>
        public ErrorDialog(string message)
            : this(message, null)
        {

        }

        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnShowHideClicked(object sender, RoutedEventArgs e)
        {
            if (isShow)
            {
                ShowHideButton.Content = UtnEmall.ServerManager.Properties.Resources.HideContent;
                ErrorList.Visibility = Visibility.Visible;
                ErrorList.Visibility = Visibility.Visible;
                Height += ErrorList.MinHeight - 20;
            }
            else
            {
                ShowHideButton.Content = UtnEmall.ServerManager.Properties.Resources.ShowContent;
                ErrorList.Visibility = Visibility.Collapsed;
                ErrorList.Visibility = Visibility.Collapsed;
                Height -= ErrorList.MinHeight - 20;
            }

            isShow = !isShow;
        }
    }
}