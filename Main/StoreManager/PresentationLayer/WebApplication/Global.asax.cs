using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Diagnostics;

namespace WebApplication
{
    public class Global : System.Web.HttpApplication
    {
        #region Instance Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification="It is required by ASP.NET")]
        protected void Application_Start(object sender, EventArgs e)
        {
            ServicesClients.Server(new WebApplication.Properties.Settings().ServerName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "It is required by ASP.NET")]
        protected void Application_End(object sender, EventArgs e)
        {
            new WebApplication.Properties.Settings().Save();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "It is required by ASP.NET")]
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {               
                Exception error = Server.GetLastError().GetBaseException();                
                              
                Server.ClearError();

                Application.Add(SessionConstants.Error, error);
            }
             finally
            {
                Response.Redirect(PagesConstants.ErrorPage);
            }
        }

        #endregion
    }
}