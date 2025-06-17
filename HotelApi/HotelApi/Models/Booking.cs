using System;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    [Required]
    public int? RoomId { get; set; }

    [Required]
    public DateOnly? CheckInDate { get; set; }

    [Required]
    public DateOnly? CheckOutDate { get; set; }

    
    [Required]
    public int ClientId { get; set; }
    public virtual Client? Client { get; set; }

    public virtual Room? Room { get; set; }
}
