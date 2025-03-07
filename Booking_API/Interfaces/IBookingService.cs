using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Booking;

namespace Booking_API.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceResponse<BookingDTO>> CreateBooking(BookingDTO bookingDto);
        Task<ServiceResponse<List<BookingDTO>>> GetBookings();
        Task<ServiceResponse<BookingDTO>> GetBookingById(int bookingId);
        Task<ServiceResponse<BookingDTO>> UpdateBooking(int bookingId, BookingDTO bookingDto);
        Task<ServiceResponse<bool>> DeleteBooking(int bookingId);
    }
}
