using System;

namespace HotelMS.Models
{
    /// <summary>
    /// Represents a staff/admin account. Mirrors the original Android User model.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
