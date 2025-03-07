﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Booking_API.Models.DTO_s.Booking
{
    public class BookingDTO
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsConfirmed { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerPhone { get; set; }
    }
}
