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
using System.Diagnostics;

namespace WebApplication
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        private const string errorIn = "Error in: ";
        private const string errorMessage = "\nError Message:";
        private const string stackTrace = "\nStack Trace:";

        Exception errorHandler;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Is the last level to catch exception")]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(true);
                // Obtiene el error.
                errorHandler = (Exception)Application[SessionConstants.Error];
            }
            catch (Exception error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageApplicationProblems").ToString();

                errorHandler = error;

                ShowMessage(true);

                Debug.WriteLine(errorHandler.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification="Is the last level to catch exception")]
        protected void ButtonShowErrorDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (!textBoxErrorDetails.Visible)
                {
                    ShowDetails(true);

                    ButtonShowErrorDetails.Text = "Hide Details";
                }
                else
                {
                    ShowDetails(false);

                    ButtonShowErrorDetails.Text = "Show Details";
                }
            }
            catch (Exception error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageApplicationProblems").ToString();

                errorHandler = error;

                ShowMessage(true);

                Debug.WriteLine(errorHandler.Message);
            }
        }

        /// <summary>
        /// Visualiza los detalles del error.
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no</param>
        private void ShowDetails(bool state)
        {
            textBoxErrorDetails.Visible = state;
            textBoxErrorDetails.Text = errorIn + Request.Url.ToString() +
                                       errorMessage + errorHandler.Message.ToString() +
                                       stackTrace + errorHandler.StackTrace.ToString();
        }

        /// <summary>
        /// Visualiza un mensaje de información
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no.</param>
        private void ShowMessage(bool state)
        {
            InformationImage.Visible = state;
            LabelMessage.Visible = state;
        }
    }
}
