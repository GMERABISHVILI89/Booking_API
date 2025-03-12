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

            CreateMap<Room, RoomDTO>()
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.TypeName))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.roomImage)))
            .ForMember(dest => dest.BookedDates, opt => opt.MapFrom(src => src.BookedDates
            .Select(bd => new BookedDateDTO
            {
            StartDate = bd.StartDate,
            EndDate = bd.EndDate
             }).ToList()));


            CreateMap<ImageDTO, Image>();
            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<Booking, BookingDTO>();


        }
    }
}
