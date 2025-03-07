using Booking_API.Models.DTO_s.Hotel;

namespace Booking_API.Models.DTO_s.Room
{
    public class UpdateRoomDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int MaximumGuests { get; set; }
        public int RoomTypeId { get; set; }

        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}
