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


        public async Task<ServiceResponse<Hotel>> AddHotel(HotelsDTO hotelDto)
        {
            var response = new ServiceResponse<Hotel>();

            try
            {
                var hotel = new Hotel
                {
                    Name = hotelDto.name,
                    Address = hotelDto.address,
                    City = hotelDto.city,
                    FeaturedImage = hotelDto.featuredImage
                };
              
                 _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();

                response.Data = hotel;
                response.Success = true;
                response.Message = "Hotel added successfully.";
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = $"An error occurred while adding the hotel: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteHotel(int hotelId)  // Return bool here
        {
            var response = new ServiceResponse<bool>();

            try
            {
              
                var hotel = await _context.Hotels.Include(h => h.Rooms)
                                                 .ThenInclude(r => r.Images)
                                                 .FirstOrDefaultAsync(h => h.Id == hotelId);

                if (hotel == null)
                {
                    response.Success = false;
                    response.Message = "Hotel not found.";
                    response.Data = false; // Hotel not found, return false
                }
                else
                {
                    // Optionally delete associated rooms and images (if needed)
                    _context.Rooms.RemoveRange(hotel.Rooms); // Remove associated rooms
                    await _context.SaveChangesAsync();

                    _context.Hotels.Remove(hotel); 
                    await _context.SaveChangesAsync();
                    response.Data = true; 
                    response.Message = "Hotel deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while deleting the hotel: {ex.Message}";
                response.Data = false; 
            }

            return response;
        }


        public async Task<ServiceResponse<Hotel>> UpdateHotel(int hotelId, HotelsDTO hotelDTO)
        {
            var response = new ServiceResponse<Hotel>();

            try
            {
                // Find the hotel by ID
                var hotel = await _context.Hotels.Include(h => h.Rooms)
                                                 .ThenInclude(r => r.Images)
                                                 .FirstOrDefaultAsync(h => h.Id == hotelId);

                if (hotel == null)
                {
                    response.Success = false;
                    response.Message = "Hotel not found.";
                    response.Data = null;
                }
                else
                {
  
                    hotel.Name = hotelDTO.name;
                    hotel.Address = hotelDTO.address;
                    hotel.City = hotelDTO.city;
                    hotel.FeaturedImage = hotelDTO.featuredImage;

   
                    await _context.SaveChangesAsync();

                    response.Data = hotel;
                    response.Message = "Hotel updated successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while updating the hotel: {ex.Message}";
                response.Data = null;
            }

            return response;
        }

    }
}
