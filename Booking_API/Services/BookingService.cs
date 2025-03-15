using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.Rooms;
using Booking_API.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Booking_API.Models.DTO_s.Booking;
using System.Security.Claims;
using Booking_API.Models.Entities;
using Booking_API.Migrations;

namespace Booking_API.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }   

        public async Task<ServiceResponse<BookingDTO>> CreateBooking(BookingDTO bookingDto)
        {
            var response = new ServiceResponse<BookingDTO>();

            try
            {


                // Get the logged-in user's customerId (assuming the user is authenticated via JWT)
                var loggedInUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loggedInUserName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;


                if (loggedInUserId == null)
                {
                    response.Success = false;
                    response.Message = "You are not authorized to make this booking.";
                    return response;
                }


                // Check if the room is available for the requested dates
                var room = await _context.Rooms
                                          .Include(r => r.BookedDates)
                                          .FirstOrDefaultAsync(r => r.Id == bookingDto.RoomId);

                if (room == null)
                {
                    response.Success = false;
                    response.Message = "Room not found.";
                    return response;
                }

                // Check if the room is available during the requested dates
                var conflictingBooking = room.BookedDates
                                              .Any(b => (b.StartDate < bookingDto.CheckOutDate && b.EndDate > bookingDto.CheckInDate));

                if (conflictingBooking)
                {
                    response.Success = false;
                    response.Message = "The room is already booked for the selected dates.";
                    return response;
                }

                // Proceed with booking the room
                var booking = new Booking
                {
                    RoomId = bookingDto.RoomId,
                    CheckInDate = bookingDto.CheckInDate,
                    CheckOutDate = bookingDto.CheckOutDate,
                    TotalPrice = bookingDto.TotalPrice,
                    CustomerName = loggedInUserName!,
                    CustomerId = loggedInUserId,
                    CustomerPhone = bookingDto.CustomerPhone
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Update the Room's BookedDates
                var bookedDate = new BookedDate
                {
                    RoomId = bookingDto.RoomId,
                    StartDate = bookingDto.CheckInDate,
                    EndDate = bookingDto.CheckOutDate
                };

                // Add the new booked date to the Room
                room.BookedDates.Add(bookedDate);
                await _context.SaveChangesAsync(); // Save the changes to the Room's BookedDates


                // Map the newly created Booking entity to the DTO, including Id
                response.Data = _mapper.Map<BookingDTO>(booking);

                response.Message = "Booking created successfully.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating booking: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<List<BookingDTO>>> GetBookings(string userId)
        {
            var response = new ServiceResponse<List<BookingDTO>>();
            try
            {
                var bookings = await _context.Bookings
                    .Where(b => b.CustomerId ==userId) // Filter bookings by user ID
                    .ToListAsync();

                response.Data = _mapper.Map<List<BookingDTO>>(bookings);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving user bookings: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<BookingDTO>> GetBookingById(int bookingId)
        {
            var response = new ServiceResponse<BookingDTO>();
            try
            {
                var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
                if (booking == null)
                {
                    response.Success = false;
                    response.Message = "Booking not found.";
                }
                else
                {
                    response.Data = _mapper.Map<BookingDTO>(booking);
                    response.Message = "Booking retrieved successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving booking: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<BookingDTO>> UpdateBooking(int bookingId, BookingDTO bookingDto)
        {
            var response = new ServiceResponse<BookingDTO>();
            try
            {
                var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
                if (booking == null)
                {
                    response.Success = false;
                    response.Message = "Booking not found.";
                }
                else
                {
                    _mapper.Map(bookingDto, booking);
                    _context.Bookings.Update(booking);
                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<BookingDTO>(booking);
                    response.Message = "Booking updated successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error updating booking: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteBooking(int id, string userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var booking = await _context.Bookings.FindAsync(id);

                if (booking == null)
                {
                    response.Success = false;
                    response.Message = "Booking not found.";
                    return response;
                }

                if (booking.Id != id)
                {
                    response.Success = false;
                    response.Message = "You are not authorized to delete this booking.";
                    return response;
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true; // Indicate successful deletion
                response.Message = "Booking deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error deleting booking: {ex.Message}";
            }

            return response;
        }
    }

}
