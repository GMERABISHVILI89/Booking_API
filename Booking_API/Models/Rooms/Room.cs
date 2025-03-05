using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Booking_API.Models.Rooms
{
    public class Room
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public string Name { get; set; } = string.Empty;

        public int HotelId { get; set; } // Foreign Key for Hotel
        public Hotel Hotel { get; set; } // Navigation Property

        public decimal PricePerNight { get; set; }

        public bool Available { get; set; } = true;

        public int MaximumGuests { get; set; }

        public int RoomTypeId { get; set; } // Foreign Key for Room Type

        public List<BookedDate> BookedDates { get; set; } = new List<BookedDate>(); // List of Booked Dates

        public List<Image> Images { get; set; } = new List<Image>(); // List of Room Images
    }
}
