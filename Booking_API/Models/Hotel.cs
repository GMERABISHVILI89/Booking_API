using Booking_API.Models.Rooms;
using System.Text.Json.Serialization;

namespace Booking_API.Models
{
    public class Hotel
    {
        public int Id { get; set; } // Primary Key

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string hotelImage { get; set; } = string.Empty; // Store image URL or path

  
        public List<Room> Rooms { get; set; } = new List<Room>(); // Navigation property
    }
}
