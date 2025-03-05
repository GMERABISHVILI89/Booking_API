using Booking_API.Models.DTO_s.Auth;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;

namespace Booking_API.Interfaces
{
    public interface IHotelService
    {
        Task<ServiceResponse<List<Hotel>>> GetAll();
        Task<ServiceResponse<Hotel>> GetHotel(int hotelId);
        Task<ServiceResponse<Hotel>> AddHotel(HotelsDTO hotel);
        Task<ServiceResponse<bool>> DeleteHotel(int hotelId);
        Task<ServiceResponse<Hotel>> UpdateHotel(int hotelId, HotelsDTO hotelDTO);

    }
}
