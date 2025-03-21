using System.Text.Json.Serialization;

namespace Booking_API.Models.DTO_s.Room
{
    public class RoomTypeDTO
    {
        public  int  Id { get; set; }
        public string TypeName { get; set; }    = string.Empty; 
    }
}
