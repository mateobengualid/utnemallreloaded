using System.Windows.Forms;
using UtnEmall.Client.EntityModel;
using System.Collections.Generic;
using System;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// This class defines the visual component to edit the customer's device info.
    /// </summary>
    public partial class DeviceProfileForm : Form
    {
        private DeviceProfileEntity deviceProfileCustomer;

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="deviceProfile">
        /// the customer' device to edit
        /// </param>
        public DeviceProfileForm(DeviceProfileEntity deviceProfile)
        {
            if (deviceProfile == null)
            {
                this.Dispose();
                return;
            }
            else
            {
                deviceProfileCustomer = deviceProfile;
            }
            InitializeComponent();
        }

        /// <summary>
        /// method called to validate the customer's device info.
        /// </summary>
        /// <returns>true if the content is valid, false otherwise</returns>
        private bool ValidateFields()
        {
            if (String.IsNullOrEmpty(textBoxDeviceModel.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.DeviceProfileFormModelError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }

            if (String.IsNullOrEmpty(textBoxDeviceType.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.DeviceProfileFormTypeError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }

            if (String.IsNullOrEmpty(textBoxMacAddress.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.DeviceProfileFormMACError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }

            if (String.IsNullOrEmpty(textBoxWindowsMobileVersion.Text))
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.DeviceProfileFormVersionError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
                return false;
            }
            return true;
        }

        /// <summary>
        /// method called when "Save" is selected on the menu
        /// This method update the customer' device info.
        /// </summary>
        /// <param name="sender">
        /// the object that generates the event
        /// </param>
        /// <param name="e">
        /// an object that contains information about the event
        /// </param>
        private void menuItemSave_Click(object sender, System.EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            deviceProfileCustomer.DeviceModel = textBoxDeviceModel.Text;
            deviceProfileCustomer.DeviceType = textBoxDeviceType.Text;
            deviceProfileCustomer.MacAddress = textBoxMacAddress.Text;
            deviceProfileCustomer.WindowsMobileVersion = textBoxWindowsMobileVersion.Text;
            this.Dispose();
        }

        /// <summary>
        /// method called when device profile is loaded.
        /// This method set the info of the customer's device in the textbox corresponding.
        /// </summary>
        /// <param name="sender">
        /// the object that generates the event
        /// </param>
        /// <param name="e">
        /// an object that contains information about the event
        /// </param>
        private void DeviceProfile_Load(object sender, System.EventArgs e)
        {
            textBoxDeviceModel.Text = deviceProfileCustomer.DeviceModel;
            textBoxDeviceType.Text = deviceProfileCustomer.DeviceType;
            textBoxMacAddress.Text = deviceProfileCustomer.MacAddress;
            textBoxWindowsMobileVersion.Text = deviceProfileCustomer.WindowsMobileVersion;
        }

        /// <summary>
        /// method called when "Cancel" is selected.
        /// Close the form and clean up any resources being used.
        /// </summary>
        /// <param name="sender">
        /// the object that generates the event
        /// </param>
        /// <param name="e">
        /// an object that contains information about the event
        /// </param>
        private void menuItemCancel_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }
    }
}