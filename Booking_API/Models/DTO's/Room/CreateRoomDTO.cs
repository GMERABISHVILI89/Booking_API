using System.Text.Json.Serialization;

namespace Booking_API.Models.DTO_s.Hotel
{
    public class CreateRoomDTO
    {
        public string Name { get; set; } = string.Empty;
        public int HotelId { get; set; }  // Foreign Key for Hotel
        public decimal PricePerNight { get; set; }
        public int MaximumGuests { get; set; }
        public int RoomTypeId { get; set; }

        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}
