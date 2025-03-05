using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.Rooms
{
    public class Image
    {
        [Key]
        public int Id { get; set; } // Primary Key
        public string Source { get; set; } = string.Empty; // Image URL or Path

        public int RoomId { get; set; } // Foreign Key for Room
        public Room Room { get; set; } // Navigation Property
    }
}
