using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WebApplication
{
    public static class PagesConstants
    {
        // Aqui estan definidas las constantes que representas cadenas URL a las páginas.
        public const string LogOnPage = "~/LogOn.aspx";
        public const string VisualDesignersPage = "~/VisualDesigners.aspx";
        public const string ServicePage = "~/Service.aspx";
        public const string Homepage = "~/Home.aspx";
        public const string ListServicesPage = "~/ListServices.aspx";
        public const string ChangePasswordPage = "~/ChangePassword.aspx";
        public const string StoreProfilePage = "~/StoreProfile.aspx";
        public const string UserProfilePage = "~/UserProfile.aspx";
        public const string StoreStatisticsViewerPage = "~/StoreStatisticsViewer.aspx";
        public const string ServiceStatisticsViewerPage = "~/ServiceStatisticsViewer.aspx";
        public const string ErrorPage = "~/ErrorPage.aspx";
    }
}
