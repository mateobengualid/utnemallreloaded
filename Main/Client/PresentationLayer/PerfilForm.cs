using System;
using System.Windows.Forms;
using System.Collections;
using Janus.Client.EntityModel;
using Janus.Client.BusinessLogic;
using System.Drawing;

namespace Janus.Client.PresentationLayer
{
    public partial class PerfilForm : Form
    {
        private CustomerEntity customer;
        public PerfilForm()
        {
            InitializeComponent();
        }

 
        private void menuItemChange_Click(object sender, EventArgs e)
        {
            if (txtPreferences.Focused)
            {
                PreferenceForm preferenceForm = new PreferenceForm(customer);
                preferenceForm.Visible = true;
            }
        }

        public CustomerEntity llenarPerfil()
        {

            Customer customerBusiness = new Customer();
            
            CustomerEntity customer = new CustomerEntity();

            PreferenceEntity preference = new PreferenceEntity();
            CategoryEntity category = new CategoryEntity();
            category.Description = "Sport";
            preference.Category = category;
            preference.Active = true;

            PreferenceEntity preference2 = new PreferenceEntity();
            CategoryEntity category2 = new CategoryEntity();
            category2.Description = "Food";
            preference2.Category = category2;
            preference2.Active = true;

            customer.Preferences = new ArrayList();
            customer.Preferences.Add(preference);
            customer.Preferences.Add(preference2);

            DeviceProfileEntity device = new DeviceProfileEntity();
            
            device.DeviceType = "Smartphone";
            device.DeviceModel = "5.5"; 
            device.MacAddress = "8F-6A-88-F0-AA-10"; 
            device.WindowsMobileVersion = "6.0";
            
            customer.Id = 1;
            customer.Name = "Javier";
            customer.Surname = "Dall' Amore";
            customer.Phonenumber = "4871419";
            customer.Address = "Facundo Quiroga 2525";
            customer.UserName = "javierdallamore";
            customer.Password = "nonricordo";
            customer.IdMall = 1;

            customer.DeviceProfile = device;

            customerBusiness.Delete(customer, "");
            customerBusiness.Save(customer, "");
            return customer;
        }

        private void menuItemCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void PerfilForm_Load(object sender, EventArgs e)
        {
            Customer customerBusiness = new Customer();
            ArrayList customers = customerBusiness.GetCustomerWhere("idcustomer", "1", "");

            if (customers.Count == 0)
            {
                customer = llenarPerfil();
            } else
            {
                customer = (CustomerEntity)customers[0];
            }

            txtName.Text = customer.Name;
            txtSurname.Text = customer.Surname;
            txtAddress.Text = customer.Address;
            txtPhoneNumber.Text = customer.Phonenumber;

            Cursor.Current = Cursors.Default;
        }

        private void PerfilForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.F1))
            {
                // Soft Key 1
                // No capturada.
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F2))
            {
                // Soft Key 2
                // No capturada.
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D1))
            {
                // 1
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D2))
            {
                // 2
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D3))
            {
                // 3
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D4))
            {
                // 4
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D5))
            {
                // 5
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D6))
            {
                // 6
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D7))
            {
                // 7
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D8))
            {
                // 8
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D9))
            {
                // 9
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F8))
            {
                // *
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D0))
            {
                // 0
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F9))
            {
                // #
            }

        }


        private void TextBoxGotFocus(TextBox txt)
        {
            txt.BackColor = Color.Beige;
        }
        private void TextBoxLostFocus(TextBox txt)
        {
            txt.BackColor = Color.White;
        }

        private void txtName_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        private void txtName_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        private void txtSurname_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        private void txtSurname_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        private void txtPhoneNumber_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        private void txtPhoneNumber_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        private void txtAddress_GotFocus(object sender, EventArgs e)
        {
            TextBoxGotFocus((TextBox)sender);
        }

        private void txtAddress_LostFocus(object sender, EventArgs e)
        {
            TextBoxLostFocus((TextBox)sender);
        }

        
    }
}