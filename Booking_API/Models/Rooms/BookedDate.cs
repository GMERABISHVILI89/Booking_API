using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.Rooms
{
    public class BookedDate
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public DateTime Date { get; set; } // Booked Date

        public int RoomId { get; set; } // Foreign Key for Room
        public Room Room { get; set; } // Navigation Property
    }
}
