﻿using Booking_API.Models.Rooms;

namespace Booking_API.Models.DTO_s.Hotel
{
    public class CreateHotelDTO
    {
        public string name { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public IFormFile? hotelImage { get; set; }  // Accept file upload

    }
}
