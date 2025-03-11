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

            CreateMap<CreateHotelDTO, Hotel>()
           .ForMember(dest => dest.hotelImage, opt => opt.MapFrom(src => src.hotelImage));

            CreateMap<Hotel, CreateHotelDTO>()
                .ForMember(dest => dest.hotelImage, opt => opt.MapFrom(src => src.hotelImage));



            CreateMap<ImageDTO, Image>();
            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<Booking, BookingDTO>();
            CreateMap<Room, RoomDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Source).ToList()));

        }
    }
}
