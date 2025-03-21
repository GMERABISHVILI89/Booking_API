using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.DTO_s.Room;
using Booking_API.Models;
using Booking_API.Models.DTO_s.RoomType;

namespace Booking_API.Interfaces
{
    public interface IRoomTypeService
    {
        Task<ServiceResponse<RoomTypeGetDTO>> AddRoomType(CreateRoomTypeDTO roomType);
        Task<ServiceResponse<RoomTypeGetDTO>> EditRoomType(int id, UpdateRoomTypeDTO roomType);
        Task<ServiceResponse<List<RoomTypeGetAllDTO>>> GetAllRoomTypes();
        Task<ServiceResponse<bool>> DeleteRoomType(int id);

    }
}
