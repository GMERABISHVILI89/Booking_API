using Booking_API.Models.DTO_s.Room;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;

namespace Booking_API.Interfaces
{
    public interface IFilterService
    {
        Task<ServiceResponse<List<FilteredRoomDTO>>> GetFilteredRooms(FilterDTO filter);
        Task<ServiceResponse<List<FilteredRoomDTO>>> GetAvailableRooms(DateTime startDate, DateTime endDate);
        Task<ServiceResponse<List<RoomTypeDTO>>> GetRoomTypes();

        Task<ServiceResponse<List<FilterByCityDTO>>> GetHotelsByCity(string city);

    }
}
