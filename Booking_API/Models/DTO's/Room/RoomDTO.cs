namespace Booking_API.Models.DTO_s.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }  // Room ID
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public bool Available { get; set; }
        public int MaximumGuests { get; set; }
        public int RoomTypeId { get; set; }

        public List<string> Images { get; set; } = new List<string>(); // List of image URLs
    }
}
