using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.Rooms;
using Booking_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Room>>> GetRoomsByHotelId(int hotelId)
        {
            var response = new ServiceResponse<List<Room>>();
            try
            {
                var rooms = await _context.Rooms
                                           .Where(r => r.HotelId == hotelId)
                                           .Include(r => r.Images)
                                           .Include(r => r.BookedDates)
                                           .ToListAsync();

                if (rooms == null || rooms.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No rooms found for this hotel.";
                }
                else
                {
                    response.Data = rooms;
                    response.Message = "Rooms retrieved successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving rooms: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<ServiceResponse<Room>> GetRoomById(int roomId)
        {
            var response = new ServiceResponse<Room>();
            try
            {
                var room = await _context.Rooms
                                          .Include(r => r.Images)
                                          .Include(r => r.BookedDates)
                                          .FirstOrDefaultAsync(r => r.Id == roomId);

                if (room == null)
                {
                    response.Success = false;
                    response.Message = "Room not found.";
                    response.Data = null;
                }
                else
                {
                    response.Data = room;
                    response.Message = "Room retrieved successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving the room: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<ServiceResponse<Room>> AddRoom(int hotelId, RoomDTO roomDTO)
        {
            var response = new ServiceResponse<Room>();
            try
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
                if (hotel == null)
                {
                    response.Success = false;
                    response.Message = "Hotel not found.";
                    response.Data = null;
                    return response;
                }

                var room = new Room
                {
                    HotelId = hotelId,
                    Name = roomDTO.Name,
                    PricePerNight = roomDTO.PricePerNight,
                    Available = roomDTO.Available,
                    MaximumGuests = roomDTO.MaximumGuests,
                    RoomTypeId = roomDTO.RoomTypeId
                };

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                response.Data = room;
                response.Message = "Room added successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while adding the room: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<ServiceResponse<Room>> UpdateRoom(int roomId, RoomDTO roomDTO)
        {
            var response = new ServiceResponse<Room>();
            try
            {
                var room = await _context.Rooms
                                          .Include(r => r.Images)
                                          .Include(r => r.BookedDates)
                                          .FirstOrDefaultAsync(r => r.Id == roomId);

                if (room == null)
                {
                    response.Success = false;
                    response.Message = "Room not found.";
                    response.Data = null;
                    return response;
                }

                room.Name = roomDTO.Name;
                room.PricePerNight = roomDTO.PricePerNight;
                room.Available = roomDTO.Available;
                room.MaximumGuests = roomDTO.MaximumGuests;
                room.RoomTypeId = roomDTO.RoomTypeId;

                await _context.SaveChangesAsync();

                response.Data = room;
                response.Message = "Room updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while updating the room: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteRoom(int roomId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);

                if (room == null)
                {
                    response.Success = false;
                    response.Message = "Room not found.";
                    response.Data = false;
                    return response;
                }

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = "Room deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while deleting the room: {ex.Message}";
                response.Data = false;
            }
            return response;
        }
    }

}
