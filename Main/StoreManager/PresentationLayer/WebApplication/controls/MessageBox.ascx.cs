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

namespace WebApplication.controls
{
    public enum IconType
    {
        Error = 0,
        Information = 1,
        Question = 2
    }

    public partial class MessageBox : System.Web.UI.UserControl
    {
        #region Instance Variables and Properties

        private const string errorImage = "~/img/msgs/error.png";
        private const string informationImage = "~/img/msgs/information.png";
        private const string questionImage = "~/img/msgs/question.png";

        private IconType icon;
        public IconType Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;

                // Selecciona la imagen correcta.
                string img = errorImage;
                switch (icon)
                {
                    case IconType.Information:
                        img = informationImage;
                        break;
                    case IconType.Question:
                        img = questionImage;
                        break;
                }

                MsgBoxImage.ImageUrl = img;
            }
        }

        public string Title
        {
            get
            {
                return MsgBoxTitle.Text;
            }
            set
            {
                MsgBoxTitle.Text = value;
            }
        }

        public string Message
        {
            get
            {
                return MsgBoxMessage.Text;
            }
            set
            {
                MsgBoxMessage.Text = value;
            }
        }

        #endregion

        #region Instance Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO Why do I feel so empty?
        }

        #endregion
    }
}