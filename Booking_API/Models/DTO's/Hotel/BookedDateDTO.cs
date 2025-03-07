using Booking_API.Models.Rooms;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Booking_API.Models.DTO_s.Hotel
{
    public class BookedDateDTO
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; } // Link to the room

        public Booking_API.Models.Rooms.Room Room { get; set; }  // Navigation property

        public DateTime StartDate { get; set; } // Booking start date
        public DateTime EndDate { get; set; }   // Booking end date
    }
}
