using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.Rooms
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;  // E.g., "Single", "Double", etc.
    }
}
