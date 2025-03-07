using AutoMapper;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Booking;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.DTO_s.Room;
using Booking_API.Models.Rooms;

namespace Booking_API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ImageDTO, Image>();

            CreateMap<Room, RoomDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Source).ToList()));

            CreateMap<Booking, BookingDTO>();

        }
    }
}
