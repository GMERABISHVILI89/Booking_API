using Booking_API.Models.Rooms;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.DTO_s.Room;

namespace Booking_API.Interfaces
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<RoomDTO>>> GetRoomsByHotelId(int hotelId);
        Task<ServiceResponse<List<CreateRoomDTO>>> GetAllRooms();

        Task<ServiceResponse<RoomDTO>> GetRoomById(int roomId);
        Task<ServiceResponse<RoomDTO>> AddRoom(CreateRoomDTO roomDTO);
        Task<ServiceResponse<RoomDTO>> UpdateRoom(int roomId, CreateRoomDTO roomDTO);
        Task<ServiceResponse<bool>> DeleteRoom(int roomId);

   


        Task<ServiceResponse<List<FilteredRoomDTO>>> GetFilteredRooms(FilterDTO filter);
    }
}
