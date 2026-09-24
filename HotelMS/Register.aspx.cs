using System;
using HotelMS.DAL;
using HotelMS.Models;

namespace HotelMS
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && SessionManager.IsLoggedIn())
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Full name, username and password are required.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match.");
                return;
            }

            if (db.IsUsernameTaken(username))
            {
                ShowError("Username already taken, choose another.");
                return;
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Username = username,
                Password = password
            };

            int id = db.RegisterUser(user);

            SessionManager.CreateSession(id, fullName);
            Response.Redirect("~/Dashboard.aspx");
        }

        private void ShowError(string message)
        {
            pnlMessage.Visible = true;
            litMessage.Text = message;
        }
    }
}
