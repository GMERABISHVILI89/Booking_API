using Booking_API.Models.Rooms;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.DTO_s.Room;

namespace Booking_API.Interfaces
{
    public interface IRoomService
    {

        Task<ServiceResponse<RoomDTO>> AddRoom(CreateRoomDTO roomDTO, List<IFormFile> roomImages);

        // Update an existing room
        Task<ServiceResponse<RoomDTO>> UpdateRoom(int roomId, CreateRoomDTO roomDTO, List<IFormFile> roomImages);

        // Get all rooms
        Task<ServiceResponse<List<RoomDTO>>> GetAllRooms();

        // Get a room by its ID
        Task<ServiceResponse<RoomDTO>> GetRoomById(int roomId);

        // Get rooms by hotel ID
        Task<ServiceResponse<List<RoomDTO>>> GetRoomsByHotelId(int hotelId);

        // Delete a room by its ID
        Task<ServiceResponse<bool>> DeleteRoom(int roomId);

    }
}
