using AutoMapper;
using Booking_API.Interfaces;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.DTO_s.Room;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Services
{
    public class FilterService : IFilterService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilterService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<FilteredRoomDTO>>> GetFilteredRooms(FilterDTO filter)
        {
            var response = new ServiceResponse<List<FilteredRoomDTO>>();

            try
            {
                var query = _context.Rooms
                    .Include(r => r.BookedDates)
                    .Include(r => r.Images)
                     .AsNoTracking()
                    .AsQueryable();

                // Filter by Room Type
                if (filter.RoomTypeId.HasValue && filter.RoomTypeId.Value > 0)
                {
                    query = query.Where(r => r.RoomTypeId == filter.RoomTypeId);
                }

                // Filter by Price Range
                if (filter.PriceFrom.HasValue)
                {
                    query = query.Where(r => r.PricePerNight >= filter.PriceFrom);
                }
                if (filter.PriceTo.HasValue && filter.PriceTo.Value > 0)
                {
                    query = query.Where(r => r.PricePerNight <= filter.PriceTo);
                }

                // Filter by Maximum Guests
                if (filter.MaximumGuests.HasValue && filter.MaximumGuests.Value > 0)
                {
                    query = query.Where(r => r.MaximumGuests >= filter.MaximumGuests);
                }

                // Filter by Availability (Check-in & Check-out)
                if (filter.CheckIn.HasValue && filter.CheckOut.HasValue)
                {
                    var checkIn = filter.CheckIn.Value.Date;
                    var checkOut = filter.CheckOut.Value.Date;

                    query = query.Where(r => !r.BookedDates
                        .Any(b => b.StartDate >= checkIn && b.EndDate < checkOut));
                }

                var rooms = await query.ToListAsync();

                response.Data = rooms.Select(r => new FilteredRoomDTO
                {
                    Name = r.Name,
                    HotelId = r.HotelId,
                    PricePerNight = r.PricePerNight,
                    Available = r.Available,
                    MaximumGuests = r.MaximumGuests,
                    RoomTypeId = r.RoomTypeId,
                    BookedDates = r.BookedDates.Select(b => new BookedDateDTO
                    {
                        Id = b.Id,
                        StartDate = b.StartDate,
                        EndDate = b.EndDate,
                        RoomId = b.RoomId
                    }).ToList(),
                    Images = r.Images.Select(i => new ImageDTO
                    {
                        Source = i.Source
                    }).ToList()
                }).ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error fetching filtered rooms: " + ex.Message;
            }

            return response;
        }
        // Get available rooms based on the start and end dates
        public async Task<ServiceResponse<List<RoomDTO>>> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            var response = new ServiceResponse<List<RoomDTO>>();

            try
            {
                // Get rooms that are not booked during the selected dates
                var availableRooms = await _context.Rooms
                    .Where(r => !r.BookedDates.Any(b => (b.StartDate < endDate && b.EndDate > startDate)))
                    .Include(r => r.RoomType) // Include the RoomType
                    .ToListAsync();

                response.Data = _mapper.Map<List<RoomDTO>>(availableRooms);
                response.Message = "Available rooms retrieved successfully.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving available rooms: {ex.Message}";
            }

            return response;
        }

        // Get all room types
        public async Task<ServiceResponse<List<RoomTypeDTO>>> GetRoomTypes()
        {
            var response = new ServiceResponse<List<RoomTypeDTO>>();

            try
            {
                var roomTypes = await _context.RoomTypes.ToListAsync();
                response.Data = _mapper.Map<List<RoomTypeDTO>>(roomTypes);
                response.Message = "Room types retrieved successfully.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving room types: {ex.Message}";
            }

            return response;
        }
    }
}
