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
    public partial class Home : System.Web.UI.Page
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exceptions, this is a controler event.")]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                
                // Recupera el usuario de la sesión
                UserEntity userActive = (UserEntity)Session[SessionConstants.User];

                if (userActive != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + userActive.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";
                }
                else
                {
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
        /// Muestra un mensaje de información.
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no.</param>
        private void ShowMessage(bool state)
        {
            InformationImage.Visible = state;
            LabelMessage.Visible = state;
        }
    }
}
