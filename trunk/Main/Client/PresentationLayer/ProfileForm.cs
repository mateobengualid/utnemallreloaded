using System;
using System.Windows.Forms;
using System.Collections;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.BusinessLogic;
using System.Drawing;
using System.Collections.Generic;
using System.ServiceModel;
using System.Reflection;
using UtnEmall.Client.SmartClientLayer;
using UtnEmall.Client.ServiceAccessLayer;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario para editar el perfil del cliente
    /// </summary>
    public partial class ProfileForm : Form
    {
        CustomerEntity customer;
        public ProfileForm()
        {
            InitializeComponent();
            
            int height, width, headerHeight, step, stepBase;

            height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);


            stepBase = (int)(height / 8.0);
            step = headerHeight;

            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;

            label1.Left = 5;
            label1.Top = step;
            step += stepBase;
            txtName.Left = 5;
            txtName.Top = step;
            txtName.Width = width - 10;

            step += stepBase;

            label2.Left = 5;
            label2.Top = step;
            step += stepBase;
            txtSurname.Left = 5;
            txtSurname.Top = step;
            txtSurname.Width = width - 10;

            step += stepBase;

            label3.Left = 5;
            label3.Top = step;
            step += stepBase;
            txtAddress.Left = 5;
            txtAddress.Top = step;
            txtAddress.Width = width - 10;

            step += stepBase;

            label4.Left = 5;
            label4.Top = step;
            step += stepBase;
            txtPhoneNumber.Left = 5;
            txtPhoneNumber.Top = step;
            txtPhoneNumber.Width = width - 10;
        }

        /// <summary>
        /// Metodo llamado para validar la información del cliente.
        /// </summary>
        /// <returns>true si el contenido es valido y correcto</returns>
        private bool ValidateFields()
        {
            
            if(String.IsNullOrEmpty(txtSurname.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.ProfileFormSurnameError, 
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }
            if(String.IsNullOrEmpty(txtName.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.ProfileFormFirstNameError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }
                
            if(String.IsNullOrEmpty(txtAddress.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.ProfileFormAddressError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }
            if (String.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.ProfileFormPhoneError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Metodo llamado cuando se presiona "Guardar" 
        /// Actualiza la información del cliente y las preferencias en la base de datos
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            
            // Establecer la información del cliente
            customer.Name = txtName.Text;
            customer.Surname = txtSurname.Text;
            customer.Address = txtAddress.Text;
            customer.PhoneNumber = txtPhoneNumber.Text;

            CustomerEntity returnCustomer;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                returnCustomer = UtnEmallClientApplication.UpdateCustomerProfile(customer);
            }
            catch (TargetInvocationException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.TargetInvocationExceptionMessage,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                this.Close();
                return;
            }
            catch (CommunicationException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.CommunicationException,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                this.Close();
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            if (returnCustomer == null)
            {
                Utilities.ShowWarning(
                    global::PresentationLayer.GeneralResources.SuccessTitle,
                    global::PresentationLayer.GeneralResources.SaveCustomerSuccess);
                this.Close();
            }
            else
            {
                // Mostrar los errores si es necesario
                string messageError = "";
                foreach (Error error in returnCustomer.Errors)
                {
                    messageError += error.Description + "\n";
                }
                Utilities.ShowError(global::PresentationLayer.GeneralResources.ErrorTitle,
                    messageError);
            }
        }

        /// <summary>
        /// Metodo llamado cuando se carga el formulario
        /// Obtiene y establece la información del perfil del cliente
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void ProfileForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (UtnEmallClientApplication.IsOnline)
            {
                try
                {
                    // Actualizar la información del cliente desde el servidor si se esta en línea
                    UtnEmallClientApplication.Instance.ReloadCustomer();
                }
                catch (TargetInvocationException)
                {
                    BaseForm.ShowErrorMessage(
                        global::PresentationLayer.GeneralResources.TargetInvocationExceptionMessage,
                        global::PresentationLayer.GeneralResources.ErrorTitle);
                    this.Close();
                    return;
                }
                catch (CommunicationException)
                {
                    BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.CommunicationException,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                    this.Close();
                    return;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            Cursor.Current = Cursors.Default;
            
            // Establecer el cliente
            customer = UtnEmallClientApplication.Instance.Customer;
           
            // Actualizar los valores en los controles
            txtName.Text = customer.Name;
            txtSurname.Text = customer.Surname;
            txtAddress.Text = customer.Address;
            txtPhoneNumber.Text = customer.PhoneNumber;
        }


        private static void TextBoxGotFocus(TextBox txt)
        {
            txt.BackColor = Color.Beige;
        }

        private static void TextBoxLostFocus(TextBox txt)
        {
            txt.BackColor = Color.White;
        }

        /// <summary>
        /// Método llamado cuando "txtName" obtiene el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtName_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        /// <summary>
        /// método llamado cuando "txtName" obtiene el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtName_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        /// <summary>
        /// Metodo llamado cuando "txtSurname" obtiene el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtSurname_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        /// <summary>
        /// método llamado cuando "txtName" pierde el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtSurname_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        /// <summary>
        /// metodo llamado cuando "Phone number" obtiene el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtPhoneNumber_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        /// <summary>
        /// metodo llamado cuando "Phone number" pierde el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtPhoneNumber_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        /// <summary>
        /// metodo llamado cuando "txtAddress" obtiene el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtAddress_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        /// <summary>
        /// metodo llamado cuando "txtAddress" pierde el foco
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void txtAddress_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        /// <summary>
        /// Metodo llamado cuando "menuItemPreferences" es seleccionado desde el menú
        /// Muestra el formulario para editar preferencias
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemPreferences_Click(object sender, EventArgs e)
        {
            PreferenceForm preferenceForm = new PreferenceForm(customer);
            preferenceForm.Owner = this;
            preferenceForm.ShowDialog();
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