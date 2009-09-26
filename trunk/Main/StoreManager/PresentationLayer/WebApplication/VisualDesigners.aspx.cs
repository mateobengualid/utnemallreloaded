using System;
using System.Web.UI;
using UtnEmall.Server.EntityModel;
using System.Globalization;
using UtnEmall.Server.BusinessLogic;
using System.Diagnostics;

namespace WebApplication
{
    public partial class VisualDesigners : System.Web.UI.Page
    {
        private const string equalSign = "=";
        private const string separator = ",";
        private int idToUse;
        private int designerMode;

        private UserEntity userEntity;
        public UserEntity UserEntity 
        {
            get { return userEntity; }
            set { userEntity = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Obtiene el usuario activo.
            UserEntity = (UserEntity)Session[SessionConstants.User];

            if (UserEntity != null)
            {
                if (!Page.IsPostBack)
                {
                    // Obtiene el ID del servicio personalizado y el modo de diseño para cargar el diseñador visual.
                    idToUse = Convert.ToInt32(Session[SessionConstants.IdCustomerServiceDataToUse], CultureInfo.InvariantCulture);
                    designerMode = Convert.ToInt32(Session[SessionConstants.DesignerMode], CultureInfo.InvariantCulture);
                    // Asigna los parámetros iniciales del Objeto Silverlight(XAP).
                    this.Silverlight.InitParameters = SessionConstants.IdCustomerServiceDataToUse + equalSign + idToUse + separator + SessionConstants.DesignerMode + equalSign + designerMode;
                }   
            }
            else
            {
                // Si el usuario no esta logeado es direccionado a la página de log.
                Server.Transfer(PagesConstants.LogOnPage);
            }
        }
    }
}
