using System;
using HotelMS.DAL;

namespace HotelMS
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(SessionManager.IsLoggedIn() ? "~/Dashboard.aspx" : "~/Login.aspx");
        }
    }
}
