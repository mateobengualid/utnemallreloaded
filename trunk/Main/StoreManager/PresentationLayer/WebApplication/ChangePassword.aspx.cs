using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.Globalization;
using UtnEmall.Server.BusinessLogic;
using System.Diagnostics;

namespace WebApplication
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private UserEntity userEntity;
        
        #region Instance Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exceptions, this is a controler event.")]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                // Obtiene el usuario activo.
                userEntity = (UserEntity)Session[SessionConstants.User];

                if (userEntity != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + userEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";
                }
                else
                {
                    // Si el usiario no esta logeado es redireccionado a la página de Log.
                    Server.Transfer(PagesConstants.LogOnPage);
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);           
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification=" Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonAccept_Click(object sender, EventArgs e)
        {
            // Encripta la contraseña del usuario para mayor seguridad.
            string currentPassword = Utilities.CalculateHashString(TextBoxPassword.Text.Trim());

            try
            {
                if (string.CompareOrdinal(userEntity.Password, currentPassword) == 0
                    && ConfirmPassword() && ServicesClients.User.Validate(userEntity))
                {
                    userEntity.Password = Utilities.CalculateHashString(TextBoxNewPassword.Text.Trim());
                    userEntity.Changed = true;
                    userEntity.IsNew = false;

                    if (ServicesClients.User.Save(userEntity, (string)Session[SessionConstants.UserSession]) == null)
                    {
                        InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageSuccess").ToString();
                    }
                }
                else
                {
                    InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageProblems").ToString();
                }

                ShowMessage(true);
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        /// <summary>
        /// Verifica la confirmación de contraseña.
        /// </summary>
        /// <returns>Retorna true si la confirmación fue exitosa caso contrario retorna false</returns>
        private bool ConfirmPassword()
        {            
            string password = Utilities.CalculateHashString(TextBoxNewPassword.Text.Trim());
            string confirm = Utilities.CalculateHashString(TextBoxNewPasswordConfirm.Text.Trim());

            if (string.CompareOrdinal(password, confirm) == 0)
            {
                return true;
            }
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Navega atrás hacia la página principal.
            Server.Transfer(PagesConstants.Homepage);
        }

        /// <summary>
        /// Muestra un mensaje de información.
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no.</param>
        private void ShowMessage(bool state)
        {           
            InformationImage.Visible = state;
            LabelMessage.Visible = state;                     
        }

        #endregion
    }
}
