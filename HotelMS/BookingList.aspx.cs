using System;
using HotelMS.DAL;
using HotelMS.Models;

namespace HotelMS
{
    public partial class BookingList : System.Web.UI.Page
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadBookings();
            }
        }

        private void LoadBookings()
        {
            gvBookings.DataSource = db.GetAllBookings();
            gvBookings.DataBind();
        }

        protected void gvBookings_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewBill")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Bill.aspx?id=" + id);
            }
        }

        protected string GetPillClass(string status)
        {
            switch (status)
            {
                case Booking.STATUS_BILLED: return "pill-billed";
                case Booking.STATUS_CANCELLED: return "pill-cancelled";
                default: return "pill-booked";
            }
        }
    }
}
