using System;
using System.Diagnostics;
using System.Globalization;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.EntityModel;

namespace WebApplication
{
    public partial class StatisticsViewer : System.Web.UI.Page
    {
        private UserEntity userEntity;

        #region Instance Methods

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

        protected void ButtonServiceStatistics_Click(object sender, EventArgs e)
        {
            // Navega a la página que muestra las estadísticas para cada servicio individual.
            Server.Transfer(PagesConstants.ServiceStatisticsViewerPage);
        }

        protected void ButtonGlobalStatistics_Click(object sender, EventArgs e)
        {
            // Navega a la página que muetra ls estadísticas globales de úso.
            Server.Transfer(PagesConstants.StoreStatisticsViewerPage);
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
