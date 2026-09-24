using System;
using HotelMS.DAL;

namespace HotelMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            litUserName.Text = System.Web.HttpUtility.HtmlEncode(SessionManager.GetUserName());
        }
    }
}
