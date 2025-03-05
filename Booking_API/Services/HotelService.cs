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

        public async Task<ServiceResponse<Hotel>> GetHotel(int hotelid)
        {
            try
            {
                var hotel = await _context.Hotels
                                                             .Include(h => h.Rooms) // Include Rooms
                                                            .ThenInclude(r => r.Images) // Include Images inside Rooms
                                                            .Include(h => h.Rooms) // Include Rooms again
                                                            .ThenInclude(r => r.BookedDates) // Include BookedDates inside Rooms
                                                            .FirstOrDefaultAsync(h => h.Id == hotelid);
                if (hotel == null) {
                    return new ServiceResponse<Hotel>
                    {
                        Data = null!,
                        Success = false,
                        Message = "Hotel Not Found !"
                    };
                }
                return new ServiceResponse<Hotel> {
                    Data = hotel!,
                    Success = true,
                    Message = "Success"
                };
            }
            catch (Exception exe)
            {

                return new ServiceResponse<Hotel>
                {
                    Data = null,
                    Success = false,
                    Message = $"An error occurred: {exe.Message}"
                };
            }
        }
    }
}
