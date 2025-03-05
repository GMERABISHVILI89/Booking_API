namespace Booking_API.Models.DTO_s.Hotel
{
    public class RoomDTO
    {
        public string Name { get; set; }
        public int HotelId { get; set; }
        public decimal PricePerNight { get; set; }
        public bool Available { get; set; }
        public int MaximumGuests { get; set; }
        public int RoomTypeId { get; set; }
        // Optionally, include collections of related entities like BookedDates and Images
        public List<int> BookedDates { get; set; } = new List<int>(); // You could just pass IDs, or handle full objects if needed
        public List<int> Images { get; set; } = new List<int>(); // Again, either IDs or image objects

    }
}
