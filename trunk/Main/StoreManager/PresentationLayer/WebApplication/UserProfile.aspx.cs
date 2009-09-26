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
using System.Globalization;
using UtnEmall.Server.BusinessLogic;
using System.Diagnostics;

namespace WebApplication
{
    public partial class UserProfile : System.Web.UI.Page
    {
        #region Constants, Variables and Properties

        private UserEntity userEntity;
        public UserEntity UserEntity
        {
            get{ return userEntity; }
            set{ userEntity = value; }
        }

        #endregion

        #region Instance Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                // Obtiene el usuario activo.
                UserEntity = (UserEntity)Session[SessionConstants.User];

                if (UserEntity != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + UserEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";

                    if (!Page.IsPostBack)
                    {                        
                        LoadUserData();
                    }   
                }
                else
                {
                    // Si el usuario no esta logeado es direccionado a la página de log.
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

        /// <summary>
        /// Carga los datos del usuario en el control.
        /// </summary>
        private void LoadUserData()
        {
            TextBoxUserName.Text = UserEntity.UserName;
            TextBoxName.Text = UserEntity.Name;
            TextBoxSurname.Text = UserEntity.Surname;
            TextBoxPhoneNumber.Text = UserEntity.PhoneNumber;
            TextBoxCharge.Text = UserEntity.Charge;
        }

        protected void ButtonAccept_Click(object sender, EventArgs e)
        {
            UserEntity.UserName = TextBoxUserName.Text;
            UserEntity.Name = TextBoxName.Text;
            UserEntity.Surname = TextBoxSurname.Text;
            UserEntity.PhoneNumber = TextBoxPhoneNumber.Text;
            UserEntity.Charge = TextBoxCharge.Text;

            if (ServicesClients.User.Validate(userEntity))
            {
                UserEntity.Changed = true;
                UserEntity.IsNew = false;

                if (ServicesClients.User.Save(UserEntity, (string)Session[SessionConstants.UserSession]) == null)
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

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Navega hacia la página principal.
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
