using Booking_API.Models.DTO_s.Auth;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;

namespace Booking_API.Interfaces
{
    public interface IHotelService
    {

        Task<ServiceResponse<Hotel>> AddHotel(CreateHotelDTO hotelDTO, IFormFile hotelImage);
        Task<ServiceResponse<List<Hotel>>> GetAllHotels();
        Task<ServiceResponse<Hotel>> GetHotelById(int hotelId);
        Task<ServiceResponse<Hotel>> UpdateHotel(int hotelId, UpdateHotelDTO hotelDTO, IFormFile hotelImage);
        Task<ServiceResponse<bool>> DeleteHotel(int hotelId);

    }
}
