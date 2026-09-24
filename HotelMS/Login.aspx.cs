using System;
using HotelMS.DAL;
using HotelMS.Models;

namespace HotelMS
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && SessionManager.IsLoggedIn())
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter username and password.");
                return;
            }

            User user = db.Login(username, password);
            if (user == null)
            {
                ShowError("Invalid username or password.");
                return;
            }

            SessionManager.CreateSession(user.Id, user.FullName);
            Response.Redirect("~/Dashboard.aspx");
        }

        private void ShowError(string message)
        {
            pnlMessage.Visible = true;
            litMessage.Text = message;
        }
    }
}
