using System;
using HotelMS.DAL;

namespace HotelMS
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionManager.IsLoggedIn())
            {
                pnlNav.Visible = true;
                litUserName.Text = System.Web.HttpUtility.HtmlEncode(SessionManager.GetUserName());
            }
            else
            {
                pnlNav.Visible = false;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.Logout();
            Response.Redirect("~/Login.aspx");
        }
    }
}
