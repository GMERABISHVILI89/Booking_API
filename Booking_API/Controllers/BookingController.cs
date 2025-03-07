using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Booking_API.Services;
using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Booking;

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

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<BookingDTO>>> CreateBooking(BookingDTO bookingDto)
        {
            var response = await _bookingService.CreateBooking(bookingDto);

            if (response.Success)
            {
                // Use the Id from the response.Data, now that it's included in the DTO
                return CreatedAtAction(nameof(GetBookingById), new { bookingId = response.Data.Id }, response);
            }

            return BadRequest(response);  // Return a BadRequest if there was an error
        }


        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<BookingDTO>>>> GetBookings()
        {
            var response = await _bookingService.GetBookings();

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // GET: api/Booking/{id}
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

        // PUT: api/Booking/{id}
        [HttpPut("{bookingId}")]
        public async Task<ActionResult<ServiceResponse<BookingDTO>>> UpdateBooking(int bookingId, BookingDTO bookingDto)
        {
            var response = await _bookingService.UpdateBooking(bookingId, bookingDto);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // DELETE: api/Booking/{id}
        [HttpDelete("{bookingId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteBooking(int bookingId)
        {
            var response = await _bookingService.DeleteBooking(bookingId);

            if (response.Data)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}
