using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Booking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // Add Hotel
        [HttpPost("AddHotel")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Hotel>>> AddHotel([FromForm] CreateHotelDTO hotelDTO, [FromForm] IFormFile hotelImage)
        {
            if (hotelDTO == null)
                return BadRequest(new ServiceResponse<Hotel> { Success = false, Message = "Invalid data." });

            var response = await _hotelService.AddHotel(hotelDTO, hotelImage);

            if (!response.Success)
                return Conflict(response); // Hotel already exists

            return CreatedAtAction(nameof(GetHotelById), new { hotelId = response.Data.Id }, response);
        }

        // ✅ Get All Hotels
        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<Hotel>>>> GetAllHotels()
        {
            var response = await _hotelService.GetAllHotels();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // ✅ Get Hotel By ID
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<ServiceResponse<Hotel>>> GetHotelById(int hotelId)
        {
            var response = await _hotelService.GetHotelById(hotelId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{hotelId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteHotel(int hotelId)
        {
            var response = await _hotelService.DeleteHotel(hotelId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [Authorize(Roles = "Admin")]
        // ✅ Update Hotel
        [HttpPut("update/{hotelId}")]
        public async Task<ActionResult<ServiceResponse<Hotel>>> UpdateHotel(int hotelId,
                                                                                                                                [FromForm] string name,
                                                                                                                                [FromForm] string address,
                                                                                                                                [FromForm] string city,
                                                                                                                                [FromForm] IFormFile hotelImage)
        {
            var hotelDTO = new UpdateHotelDTO
            {
                name = name,
                address = address,
                city = city
            };
            if (hotelDTO == null)
                return BadRequest(new ServiceResponse<Hotel> { Success = false, Message = "Invalid data." });

            var response = await _hotelService.UpdateHotel(hotelId, hotelDTO, hotelImage);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
