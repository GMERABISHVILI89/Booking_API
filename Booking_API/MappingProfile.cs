using AutoMapper;
using Booking_API.Models;
using Booking_API.Models.DTO_s.Booking;
using Booking_API.Models.DTO_s.Hotel;
using Booking_API.Models.DTO_s.Room;
using Booking_API.Models.DTO_s.RoomType;
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

            CreateMap<Booking, BookingWithImageDTO>()
           .ForMember(dest => dest.RoomImage, opt => opt.Ignore());
            CreateMap<ImageDTO, Image>();
            CreateMap<RoomType, RoomTypeGetDTO>(); // For single item retrieval
            CreateMap<RoomType, RoomTypeGetAllDTO>(); // For list retrieval
            CreateMap<CreateRoomTypeDTO, RoomType>(); // For creating a RoomType
            CreateMap<UpdateRoomTypeDTO, RoomType>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // for updating a roomType.
            CreateMap<Booking, BookingDTO>();



        }
    }
}
