using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.Rooms;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Booking_API.Models.DTO_s.Room;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Booking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // POST: api/room/add
        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<RoomDTO>>> AddRoom([FromForm] CreateRoomDTO roomDTO, [FromForm] List<IFormFile> roomImages)
        {
            var response = await _roomService.AddRoom(roomDTO, roomImages);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // PUT: api/room/update/{roomId}
        [HttpPut("update/{roomId}")]
        public async Task<ActionResult<ServiceResponse<RoomDTO>>> UpdateRoom(int roomId, [FromForm] CreateRoomDTO roomDTO, [FromForm] List<IFormFile> roomImages)
        {
            var response = await _roomService.UpdateRoom(roomId, roomDTO, roomImages);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // GET: api/room/{roomId}
        [HttpGet("{roomId}")]
        public async Task<ActionResult<ServiceResponse<RoomDTO>>> GetRoomById(int roomId)
        {
            var response = await _roomService.GetRoomById(roomId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // GET: api/room/hotel/{hotelId}
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<ServiceResponse<List<RoomDTO>>>> GetRoomsByHotelId(int hotelId)
        {
            var response = await _roomService.GetRoomsByHotelId(hotelId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // DELETE: api/room/delete/{roomId}
        [HttpDelete("delete/{roomId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteRoom(int roomId)
        {
            var response = await _roomService.DeleteRoom(roomId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // GET: api/room/all
        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<RoomDTO>>>> GetAllRooms()
        {
            var response = await _roomService.GetAllRooms();
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
