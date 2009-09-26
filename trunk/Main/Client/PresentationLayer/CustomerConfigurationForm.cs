using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UtnEmall.Client.BusinessLogic;
using System.ServiceModel;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.ServiceAccessLayer;
using System.Reflection;
using Microsoft.Tools.ServiceModel;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario para configurar el usuario
    /// </summary>
    public partial class CustomerConfigurationForm : Form
    {
        
        public CustomerConfigurationForm()
        {
            InitializeComponent();

            int height, width, headerHeight;

            height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);
            
            this.Height = height;
            this.Width = width;

            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;

            labelUsername.Top = headerHeight + 5;
            labelUsername.Left = 5;
            textBoxUserName.Top = headerHeight + 35;
            textBoxUserName.Left = 5;
            textBoxUserName.Width = width - 10;

            labelPassword.Top = headerHeight + 80;
            labelPassword.Left = 5;
            textBoxPassword.Top = headerHeight + 110;
            textBoxPassword.Left = 5;
            textBoxPassword.Width = width - 10;
        }

        /// <summary>
        /// Metodo llamado cuando se presiona "Guardar" en el ménu.
        /// Valida y guarda el usuario a la base de datos local.
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
            string username = textBoxUserName.Text;
            string password = textBoxPassword.Text;

            try
            {
                // Valida y obtiene la entidad del cliente desde el servidor
                CustomerEntity customer = UtnEmallClientApplication.ValidateAndGetCustomer(username, password);
                // guarda el cliente a la base de datos local
                UtnEmallClientApplication.SaveCustomerToLocalDatabase(customer);
            }
            catch (TargetInvocationException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.TargetInvocationExceptionMessage,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            catch (CFFaultException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.LoginPermissionError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            catch (CommunicationException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.CommunicationException,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            catch (ArgumentException ex)
            {
                BaseForm.ShowErrorMessage(ex.Message,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            catch (UtnEmallBusinessLogicException utnEmallBusinessLogicException)
            {
                BaseForm.ShowErrorMessage(utnEmallBusinessLogicException.Message,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            
            BaseForm.ShowMessage(
                global::PresentationLayer.GeneralResources.ConnectionStateSuccess, 
                global::PresentationLayer.GeneralResources.SuccessTitle);

            this.Close();
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