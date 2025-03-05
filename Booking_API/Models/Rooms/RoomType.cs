using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.Rooms
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g., "Standard", "Deluxe", "Suite"

        public string? Description { get; set; } // Optional description
    }
}
