using System;
using System.Globalization;
using HotelMS.DAL;
using HotelMS.Models;

namespace HotelMS
{
    public partial class Booking : System.Web.UI.Page
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
                LoadRoomTypes();
                Recalculate(sender, e);
            }
        }

        private void LoadRoomTypes()
        {
            var roomTypes = db.GetAllRoomTypes();
            ddlRoomType.Items.Clear();

            if (roomTypes.Count == 0)
            {
                ShowError("Please add a room type first from Room Pricing.");
                ddlRoomType.Items.Add(new System.Web.UI.WebControls.ListItem("No room types available", ""));
                return;
            }

            foreach (var r in roomTypes)
            {
                string label = string.Format("{0} - ₹{1:N0}/night", r.Name, r.PricePerNight);
                ddlRoomType.Items.Add(new System.Web.UI.WebControls.ListItem(label, r.Id.ToString()));
            }
        }

        protected void Recalculate(object sender, EventArgs e)
        {
            RoomType selected = GetSelectedRoom();
            decimal rate = selected != null ? selected.PricePerNight : 0;
            litRate.Text = string.Format("₹{0:N2}", rate);

            int nights = ComputeNights();
            litNights.Text = nights.ToString();

            decimal total = nights * rate;
            litEstimatedTotal.Text = string.Format("₹{0:N2}", total);
        }

        private int ComputeNights()
        {
            DateTime checkIn, checkOut;
            if (DateTime.TryParse(txtCheckIn.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out checkIn) &&
                DateTime.TryParse(txtCheckOut.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out checkOut))
            {
                int nights = (int)(checkOut.Date - checkIn.Date).TotalDays;
                return nights < 0 ? 0 : nights;
            }
            return 0;
        }

        private RoomType GetSelectedRoom()
        {
            if (ddlRoomType.Items.Count == 0 || string.IsNullOrEmpty(ddlRoomType.SelectedValue)) return null;
            int id = Convert.ToInt32(ddlRoomType.SelectedValue);
            return db.GetRoomType(id);
        }

        protected void btnConfirmBooking_Click(object sender, EventArgs e)
        {
            string guestName = txtGuestName.Text.Trim();
            string guestPhone = txtGuestPhone.Text.Trim();
            string guestCountStr = txtGuestCount.Text.Trim();

            if (string.IsNullOrEmpty(guestName))
            {
                ShowError("Please enter guest name.");
                return;
            }

            RoomType room = GetSelectedRoom();
            if (room == null)
            {
                ShowError("No room type selected.");
                return;
            }

            DateTime checkIn, checkOut;
            bool checkInOk = DateTime.TryParse(txtCheckIn.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out checkIn);
            bool checkOutOk = DateTime.TryParse(txtCheckOut.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out checkOut);

            if (!checkInOk || !checkOutOk)
            {
                ShowError("Please select check-in and check-out dates.");
                return;
            }

            int nights = (int)(checkOut.Date - checkIn.Date).TotalDays;
            if (nights <= 0)
            {
                ShowError("Check-out date must be after check-in date.");
                return;
            }

            int guestCount = string.IsNullOrEmpty(guestCountStr) ? 1 : Convert.ToInt32(guestCountStr);
            decimal total = nights * room.PricePerNight;

            var booking = new Models.Booking
            {
                GuestName = guestName,
                GuestPhone = guestPhone,
                RoomTypeId = room.Id,
                RoomTypeName = room.Name,
                RoomPrice = room.PricePerNight,
                CheckIn = checkIn.Date,
                CheckOut = checkOut.Date,
                Nights = nights,
                Guests = guestCount,
                TotalAmount = total
            };

            db.CreateBooking(booking);

            Response.Redirect("~/BookingList.aspx");
        }

        private void ShowError(string message)
        {
            pnlMessage.Visible = true;
            litMessage.Text = message;
        }
    }
}
