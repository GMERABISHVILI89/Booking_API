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

        [HttpGet("GetAll")] 
        public async Task<ActionResult<ServiceResponse<List<Hotel>>>> GetAll() //Or [FromBody] if post
        {
            var response = await _hotelService.GetAll();

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response); // Or StatusCode(500, response) for internal server errors
            }
        }

        [HttpGet("GetHotelById")]
        public async Task<ActionResult<ServiceResponse<List<Hotel>>>> GetHotelById(int hoteId) //Or [FromBody] if post
        {
            var response = await _hotelService.GetHotel(hoteId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response); 
            }
        }


    }
}
