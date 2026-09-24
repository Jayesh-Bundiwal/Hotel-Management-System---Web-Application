using System;

namespace HotelMS.Models
{
    /// <summary>
    /// A guest reservation for a room type over a date range.
    /// </summary>
    public class Booking
    {
        public const string STATUS_BOOKED = "BOOKED";
        public const string STATUS_BILLED = "BILLED";
        public const string STATUS_CANCELLED = "CANCELLED";

        public int Id { get; set; }
        public string GuestName { get; set; }
        public string GuestPhone { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public decimal RoomPrice { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Nights { get; set; }
        public int Guests { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
    }
}
