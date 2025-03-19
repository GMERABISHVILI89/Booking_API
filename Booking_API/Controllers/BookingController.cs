using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Booking_API.Services;
using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Booking;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Booking_API.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]

    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        // Constructor injection of IBookingService
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("addBooking")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<ServiceResponse<BookingDTO>>> CreateBooking(BookingDTO bookingDto)
        {
            var response = await _bookingService.CreateBooking(bookingDto);

            if (response.Success)
            {
                // Use the Id from the response.Data, now that it's included in the DTO
                return CreatedAtAction(nameof(GetBookingById), new { bookingId = response.Data.Id }, response);
            }

            return Ok(response);  // Return a BadRequest if there was an error
        }


        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<BookingDTO>>>> GetUserBookings()
        {
            // Get the user's ID from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ServiceResponse<List<BookingDTO>> { Success = false, Message = "User ID not found." });
            }

            var response = await _bookingService.GetBookings(userId);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // GET: api/Booking/{id}   need to change to get particular user booking's ! 
        [HttpGet("{bookingId}")]
        public async Task<ActionResult<ServiceResponse<BookingDTO>>> GetBookingById(int bookingId)
        {
            var response = await _bookingService.GetBookingById(bookingId);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // DELETE: api/Booking/{id}
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id}")] // Add route parameter for booking ID
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteBooking(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "User ID not found." });
            }

            var response = await _bookingService.DeleteBooking(id, userId);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}
