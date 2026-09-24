using System;
using System.Configuration;
using HotelMS.DAL;
using HotelMS.Models;

namespace HotelMS
{
    public partial class Bill : System.Web.UI.Page
    {
        private readonly DatabaseHelper db = new DatabaseHelper();
        private static readonly decimal TaxRatePercent = GetTaxRate();
        private int bookingId;

        private static decimal GetTaxRate()
        {
            decimal rate;
            string setting = ConfigurationManager.AppSettings["TaxRatePercent"];
            return decimal.TryParse(setting, out rate) ? rate : 12.0m;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            litTaxLabel.Text = string.Format("Tax ({0:0}%)", TaxRatePercent);

            if (!int.TryParse(Request.QueryString["id"], out bookingId))
            {
                ShowNotFound();
                return;
            }

            if (!IsPostBack)
            {
                LoadBooking();
            }
        }

        private void LoadBooking()
        {
            Models.Booking booking = db.GetBooking(bookingId);
            if (booking == null)
            {
                ShowNotFound();
                return;
            }

            litGuest.Text = System.Web.HttpUtility.HtmlEncode(booking.GuestName);
            litPhone.Text = string.IsNullOrEmpty(booking.GuestPhone) ? "—" : System.Web.HttpUtility.HtmlEncode(booking.GuestPhone);
            litRoom.Text = string.Format("{0} (₹{1:N2}/night)", booking.RoomTypeName, booking.RoomPrice);
            litDates.Text = string.Format("{0:dd MMM yyyy} → {1:dd MMM yyyy}", booking.CheckIn, booking.CheckOut);
            litNights.Text = booking.Nights.ToString();

            Models.Bill existingBill = db.GetBillForBooking(bookingId);
            if (existingBill != null)
            {
                RenderBill(existingBill);
            }
            else
            {
                // Live estimate before publishing
                decimal subtotal = booking.TotalAmount;
                decimal tax = subtotal * (TaxRatePercent / 100.0m);
                litSubtotal.Text = string.Format("₹{0:N2}", subtotal);
                litTax.Text = string.Format("₹{0:N2}", tax);
                litGrandTotal.Text = string.Format("₹{0:N2}", subtotal + tax);
                litGeneratedAt.Text = "Not yet published";
                btnPublish.Visible = true;
                btnPrint.Visible = false;
            }
        }

        private void RenderBill(Models.Bill bill)
        {
            litSubtotal.Text = string.Format("₹{0:N2}", bill.Subtotal);
            litTax.Text = string.Format("₹{0:N2}", bill.TaxAmount);
            litGrandTotal.Text = string.Format("₹{0:N2}", bill.GrandTotal);
            litGeneratedAt.Text = "Published on " + bill.GeneratedAt;
            btnPublish.Visible = false;
            btnPrint.Visible = true;
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            Models.Bill bill = db.PublishBill(bookingId, TaxRatePercent);
            if (bill == null)
            {
                pnlMessage.Visible = true;
                litMessage.Text = "Could not publish bill.";
                return;
            }
            RenderBill(bill);
        }

        private void ShowNotFound()
        {
            pnlBill.Visible = false;
            pnlMessage.Visible = true;
            litMessage.Text = "Booking not found.";
        }
    }
}
