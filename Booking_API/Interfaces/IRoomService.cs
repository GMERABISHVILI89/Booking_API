using Booking_API.Models.Rooms;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;

namespace Booking_API.Interfaces
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<Room>>> GetRoomsByHotelId(int hotelId);
        Task<ServiceResponse<Room>> GetRoomById(int roomId);
        Task<ServiceResponse<Room>> AddRoom(int hotelId, RoomDTO roomDTO);
        Task<ServiceResponse<Room>> UpdateRoom(int roomId, RoomDTO roomDTO);
        Task<ServiceResponse<bool>> DeleteRoom(int roomId);
    }
}
