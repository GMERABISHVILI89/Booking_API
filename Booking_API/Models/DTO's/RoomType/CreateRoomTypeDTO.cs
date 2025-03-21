using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.DTO_s.RoomType
{
    public class CreateRoomTypeDTO
    {
        [Required(ErrorMessage = "TypeName is required.")]
        public string TypeName { get; set; } = string.Empty;
    }
}
