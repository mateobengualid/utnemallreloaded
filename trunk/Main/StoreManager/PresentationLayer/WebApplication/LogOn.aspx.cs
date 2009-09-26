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
using UtnEmall.Server.Base;
using System.ServiceModel;
using UtnEmall.Server.EntityModel;
using System.Collections.Generic;
using System.Diagnostics;
using UtnEmall.Server.BusinessLogic;

namespace WebApplication
{
    public partial class LogOn : System.Web.UI.Page
    {
        private UserEntity userEntity;
        public UserEntity UserEntity 
        {
            get { return userEntity; }
            set { userEntity = value; }
        }

        #region Instance Methods

        private string session;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();                
                ShowMessage(false);
                SetFocus(TextBoxUserName);
            }
            catch (ArgumentNullException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("ErrorImage").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageAplicationProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(error.Message);
            }
        }

        protected void ButtonLogOn_Click(object sender, EventArgs e)
        {
            try
            {
                string username = TextBoxUserName.Text;
                // Contraseña encriptada.
                string password = Utilities.CalculateHashString(TextBoxPassword.Text.Trim());

                if (string.IsNullOrEmpty(username))
                {
                    LabelMessage.Visible = true;
                    InformationImage.Visible = true;
                    return;
                }
                else if (string.IsNullOrEmpty(password))
                {
                    LabelMessage.Visible = true;
                    InformationImage.Visible = true;
                    return;
                }

                // Valida el usuario y obtiene la sesión para ese usuario.
                session = ServicesClients.Session.ValidateUser(username, password);

                if ((!string.IsNullOrEmpty(session)) && (UserWS(username)))
                {
                    // Añade el identificador de sesión a la sesión de la aplicación.
                    Session.Add(SessionConstants.UserSession, session);
                    FormsAuthentication.RedirectFromLoginPage(username, true);
                }
                else
                {
                    InformationImage.ImageUrl = GetLocalResourceObject("ErrorImage").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageProblems").ToString();
                    ShowMessage(true);
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("ErrorImage").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();
                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
            catch (CommunicationException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("ErrorImage").ToString();
                LabelMessage.Text = GetLocalResourceObject("CommunicationException").ToString();
                ShowMessage(true);

                Debug.WriteLine(error.Message);
            }
        }

        /// <summary>
        /// Obtiene la entidad del usuario y verifica que el usuario posé una tienda asociada.
        /// </summary>
        private bool UserWS(string username)
        {
            // Obtiene la enttidad del usuario.
            List<UserEntity> userList = ServicesClients.User.GetUserWhereEqual(UserEntity.DBUserName, username, true, session);

            if ((userList.Count > 0) && (userList[0].IdStore > 0))
            {
                UserEntity = userList[0];
                // Obtiene la entidad de la tienda.
                StoreEntity storeEntity = ServicesClients.Store.GetStore(UserEntity.IdStore, true, session);

                if (storeEntity != null)
                {
                    UserEntity.Store = storeEntity;
                }
                
                // Añade el usuario a la sesión, ya que es un usuario válido.
                Session.Add(SessionConstants.User, UserEntity);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Mustra un mensaje de información
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