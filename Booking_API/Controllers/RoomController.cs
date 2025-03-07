using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.Rooms;
using Booking_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Booking_API.Models.DTO_s.Room;

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

        // Get all rooms for a specific hotel
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<ServiceResponse<List<Room>>>> GetRoomsByHotelId(int hotelId)
        {
            var response = await _roomService.GetRoomsByHotelId(hotelId);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        // Get room by ID
        [HttpGet("{roomId}")]
        public async Task<ActionResult<ServiceResponse<Room>>> GetRoomById(int roomId)
        {
            var response = await _roomService.GetRoomById(roomId);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        // Add a new room to a hotel
        [HttpPost("AddRoom")]
        public async Task<ActionResult<ServiceResponse<Room>>> AddRoom(CreateRoomDTO roomDTO)
        {
            var response = await _roomService.AddRoom(roomDTO);
            if (response.Success)
                return CreatedAtAction(nameof(GetRoomById), new { roomId = response.Data.Id }, response);
            else
                return BadRequest(response);
        }

        // Update an existing room
        [HttpPut("{roomId}")]
        public async Task<ActionResult<ServiceResponse<Room>>> UpdateRoom(int roomId, CreateRoomDTO roomDTO)
        {
            var response = await _roomService.UpdateRoom(roomId, roomDTO);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        // Delete a room
        [HttpDelete("{roomId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteRoom(int roomId)
        {
            var response = await _roomService.DeleteRoom(roomId);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }


 

        [HttpGet("get-all")]
        public async Task<ActionResult<ServiceResponse<List<CreateRoomDTO>>>> GetAllRooms()
        {
            var response = await _roomService.GetAllRooms();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("filter")]
        public async Task<ActionResult<ServiceResponse<List<CreateRoomDTO>>>> FilterRooms([FromBody] FilterDTO filter)
        {
            var response = await _roomService.GetFilteredRooms(filter);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }

}
