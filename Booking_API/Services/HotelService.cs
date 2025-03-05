using Booking_API.Interfaces;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Services
{
    public class HotelService : IHotelService
    {

        private readonly ApplicationDbContext _context;


        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Hotel>>> GetAll()
        {
            try
            {
                List<Hotel> hotelList = await _context.Hotels.ToListAsync();

                return new ServiceResponse<List<Hotel>>
                {
                    Data = hotelList,
                    Success = true,
                    Message = "Hotels retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Hotel>>
                {
                    Data = null,
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public Task<ServiceResponse<Hotel>> GetHotel(HotelsDTO hotelsDTO)
        {
            throw new NotImplementedException();
        }
    }
}
