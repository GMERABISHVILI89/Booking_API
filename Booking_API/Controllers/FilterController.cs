using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Room;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Services;

namespace Booking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;

        public FilterController(IFilterService filterService)
        {
            _filterService = filterService;
        }

        // GET: api/filter/availableRooms?startDate=2025-03-07&endDate=2025-03-10
        [HttpGet("availableRooms")]
        public async Task<ActionResult<ServiceResponse<List<FilteredRoomDTO>>>> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _filterService.GetAvailableRooms(startDate, endDate);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // GET: api/filter/roomTypes
        [HttpGet("roomTypes")]
        public async Task<ActionResult<ServiceResponse<List<RoomTypeDTO>>>> GetRoomTypes()
        {
            var response = await _filterService.GetRoomTypes();

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // es gadasatania filter shi ..
        [HttpPost("filter")]
        public async Task<ActionResult<ServiceResponse<List<FilteredRoomDTO>>>> FilterRooms([FromBody] FilterDTO filter)
        {
       
            var response = await _filterService.GetFilteredRooms(filter);
            if (response.Success)
            {
                // Construct the base URL dynamically
                var baseUrl = $"{Request.Scheme}://{Request.Host}/";

                foreach (var room in response.Data)
                {
                    if (room.imageUrls != null && room.imageUrls.Any())
                    {
                        room.imageUrls = room.imageUrls.Select(imageDto => new ImageDTO
                        {
                            roomImage = baseUrl + imageDto.roomImage
                        }).ToList();
                    }
                }
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
