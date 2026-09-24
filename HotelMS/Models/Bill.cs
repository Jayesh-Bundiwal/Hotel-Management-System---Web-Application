using System;

namespace HotelMS.Models
{
    /// <summary>
    /// The published invoice for a booking (subtotal + tax + grand total).
    /// </summary>
    public class Bill
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string GeneratedAt { get; set; }
        public bool Published { get; set; }
    }
}
