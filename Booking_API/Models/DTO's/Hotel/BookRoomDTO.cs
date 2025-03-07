namespace Booking_API.Models.DTO_s.Hotel
{
    public class BookRoomDTO
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; } // Check-in date
        public DateTime EndDate { get; set; }   // Check-out date
    }
}
