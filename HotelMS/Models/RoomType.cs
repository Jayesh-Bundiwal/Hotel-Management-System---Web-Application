using System;

namespace HotelMS.Models
{
    /// <summary>
    /// A room category with its nightly price and inventory count.
    /// </summary>
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int TotalRooms { get; set; }
    }
}
