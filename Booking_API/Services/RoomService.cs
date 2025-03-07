using Booking_API.Interfaces;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.Rooms;
using Booking_API.Models;
using Microsoft.EntityFrameworkCore;
using Booking_API.Models.DTO_s.Room;
using AutoMapper;

namespace Booking_API.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoomService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<RoomDTO>>> GetRoomsByHotelId(int hotelId)
        {
            var response = new ServiceResponse<List<RoomDTO>>();
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
                    response.Data = _mapper.Map<List<RoomDTO>>(rooms); 
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
        public async Task<ServiceResponse<List<CreateRoomDTO>>> GetAllRooms()
        {
            var response = new ServiceResponse<List<CreateRoomDTO>>();

            try
            {
                var rooms = await _context.Rooms
                 //  .Include(r => r.BookedDates)  // Include booked dates
                    .Include(r => r.Images)       // Include images
                    .ToListAsync();

                response.Data = rooms.Select(r => new CreateRoomDTO
                {
                    Name = r.Name,
                    HotelId = r.HotelId,
                    PricePerNight = r.PricePerNight,
                    MaximumGuests = r.MaximumGuests,
                    RoomTypeId = r.RoomTypeId,
                    Images = r.Images.Select(i => new ImageDTO
                    {
                        Source = i.Source
                    }).ToList()
                }).ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error fetching rooms: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<RoomDTO>> GetRoomById(int roomId)
        {
            var response = new ServiceResponse<RoomDTO>();
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
                    // Map the Room entity to RoomDTO using AutoMapper
                    response.Data = _mapper.Map<RoomDTO>(room);
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


        public async Task<ServiceResponse<RoomDTO>> AddRoom(CreateRoomDTO roomDto)
        {
            var response = new ServiceResponse<RoomDTO>();

            try
            {
                // 1️⃣ Create Room entity
                var newRoom = new Room
                {
                    Name = roomDto.Name,
                    HotelId = roomDto.HotelId,
                    PricePerNight = roomDto.PricePerNight,
                    Available = true, // Defaulting Available to true
                    MaximumGuests = roomDto.MaximumGuests,
                    RoomTypeId = roomDto.RoomTypeId
                };

                // 2️⃣ Save Room first (to generate an ID)
                _context.Rooms.Add(newRoom);
                await _context.SaveChangesAsync();

                // 3️⃣ Assign RoomId to Images and save them
                if (roomDto.Images != null && roomDto.Images.Any())
                {
                    var images = roomDto.Images.Select(i => new Image
                    {
                        Source = i.Source,
                        RoomId = newRoom.Id // Assign RoomId after room is created
                    }).ToList();

                    _context.Images.AddRange(images);
                    await _context.SaveChangesAsync();

                    // Attach images to room after saving
                    newRoom.Images = images;
                }

                response.Data = _mapper.Map<RoomDTO>(newRoom); ;
                response.Message = "Room added successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error adding room: {ex.Message}";
            }

            return response;
        }



        public async Task<ServiceResponse<RoomDTO>> UpdateRoom(int roomId, UpdateRoomDTO roomDTO)
        {
            var response = new ServiceResponse<RoomDTO>();
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
                room.MaximumGuests = roomDTO.MaximumGuests;
                room.RoomTypeId = roomDTO.RoomTypeId;

                if (roomDTO.Images != null)
                {
                    room.Images.Clear();
                    foreach (var imgUrl in roomDTO.Images)
                    {
                        room.Images = _mapper.Map<List<Image>>(roomDTO.Images);
                    }
                }

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<RoomDTO>(room);
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



       public async Task<ServiceResponse<bool>> BookRoom(BookRoomDTO bookingDto)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var room = await _context.Rooms
                                         .Include(r => r.BookedDates)
                                         .FirstOrDefaultAsync(r => r.Id == bookingDto.RoomId);

                if (room == null)
                {
                    response.Success = false;
                    response.Message = "Room not found.";
                    return response;
                }

                // Check if room is already booked for the given period
                bool isBooked = room.BookedDates.Any(b =>
                    (bookingDto.StartDate >= b.StartDate && bookingDto.StartDate <= b.EndDate) || // Overlaps start
                    (bookingDto.EndDate >= b.StartDate && bookingDto.EndDate <= b.EndDate) || // Overlaps end
                    (bookingDto.StartDate <= b.StartDate && bookingDto.EndDate >= b.EndDate) // Fully covers existing booking
                );

                if (isBooked)
                {
                    response.Success = false;
                    response.Message = "Room is already booked for the selected dates.";
                    return response;
                }

                // Create new booking entry
                var newBooking = new BookedDate
                {
                    RoomId = bookingDto.RoomId,
                    StartDate = bookingDto.StartDate,
                    EndDate = bookingDto.EndDate
                };

                _context.BookedDates.Add(newBooking);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Room booked successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while booking the room: " + ex.Message;
            }

            return response;
        }


      

    }

}
