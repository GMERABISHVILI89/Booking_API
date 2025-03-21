﻿using AutoMapper;
using Booking_API.Interfaces;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Booking;
using Booking_API.Models.DTO_s.Room;
using Booking_API.Models.DTO_s.RoomType;
using Booking_API.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public RoomTypeService(ApplicationDbContext context,IMapper mapper)
        {
           _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<RoomTypeGetDTO>> AddRoomType(CreateRoomTypeDTO roomTypeDTO)
        {
            var response = new ServiceResponse<RoomTypeGetDTO>();
            try
            {
                var roomType = _mapper.Map<RoomType>(roomTypeDTO);
                _context.RoomTypes.Add(roomType);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<RoomTypeGetDTO>(roomType);
                response.Success = true;
                response.Message = "RoomType added successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error adding RoomType: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<RoomTypeGetDTO>> EditRoomType(int id, UpdateRoomTypeDTO roomTypeDTO)
        {
            var response = new ServiceResponse<RoomTypeGetDTO>();
            try
            {
                var roomType = await _context.RoomTypes.FirstOrDefaultAsync(t => t.Id == id);
                if (roomType == null)
                {
                    response.Success = false;
                    response.Message = "RoomType not found.";
                    return response;
                }
                _mapper.Map(roomTypeDTO, roomType);
                _context.RoomTypes.Update(roomType);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<RoomTypeGetDTO>(roomType);
                response.Success = true;
                response.Message = "RoomType edited successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error editing RoomType: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<List<RoomTypeGetAllDTO>>> GetAllRoomTypes()
        {
            var response = new ServiceResponse<List<RoomTypeGetAllDTO>>();
            try
            {
                var roomTypes = await _context.RoomTypes.ToListAsync();
                response.Data = _mapper.Map<List<RoomTypeGetAllDTO>>(roomTypes);
                response.Success = true;
                response.Message = "RoomTypes retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving RoomTypes: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteRoomType(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var roomType = await _context.RoomTypes.FirstOrDefaultAsync(t => t.Id == id);
                if (roomType == null)
                {
                    response.Success = false;
                    response.Message = "RoomType not found.";
                    return response;
                }
                _context.RoomTypes.Remove(roomType);
                await _context.SaveChangesAsync();
                response.Data = true;
                response.Success = true;
                response.Message = "RoomType deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error deleting RoomType: {ex.Message}";
            }
            return response;
        }
    }
}
