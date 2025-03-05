using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddHotel([FromBody] HotelsDTO hotelDto)
        {
            var response = await _hotelService.AddHotel(hotelDto);

            if (response.Success)
            {
                return Ok(response);  
            }
            else
            {
                return BadRequest(response); 
            }
        }


        [HttpGet("GetAllHotels")] 
        public async Task<ActionResult<ServiceResponse<List<Hotel>>>> GetAll() 
        {
            var response = await _hotelService.GetAll();

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response); 
            }
        }

        [HttpGet("{hotelId}")]
        public async Task<ActionResult<ServiceResponse<List<Hotel>>>> GetHotelById(int hotelId) 
        {
            var response = await _hotelService.GetHotel(hotelId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response); 
            }
        }

        [HttpDelete("{hotelId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteHotel(int hotelId)  // Return bool here
        {
            var response = await _hotelService.DeleteHotel(hotelId);
            if (!response.Data)
            {
                return NotFound(response); 
            }
            return Ok(response); 
        }


        [HttpPut("update/{hotelId}")]
        public async Task<ActionResult<ServiceResponse<Hotel>>> UpdateHotel(int hotelId, HotelsDTO hotelDTO)
        {
            var response = await _hotelService.UpdateHotel(hotelId, hotelDTO);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
