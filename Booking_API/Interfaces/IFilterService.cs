using Booking_API.Models.DTO_s.Room;
using Booking_API.Models;

namespace Booking_API.Interfaces
{
    public interface IFilterService
    {
        Task<ServiceResponse<List<FilteredRoomDTO>>> GetFilteredRooms(FilterDTO filter);
        Task<ServiceResponse<List<RoomDTO>>> GetAvailableRooms(DateTime startDate, DateTime endDate);
        Task<ServiceResponse<List<RoomTypeDTO>>> GetRoomTypes();
    }
}
