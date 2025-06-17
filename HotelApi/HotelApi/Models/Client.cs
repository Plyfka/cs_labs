using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models;

public partial class Client
{
    public int ClientId { get; set; }

    [Required(ErrorMessage = "ПІБ обов’язкове")]
    [StringLength(100)]
    public string? FullName { get; set; }

    [Phone(ErrorMessage = "Невірний номер телефону")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Невірний email")]
    [StringLength(100)]
    public string? Email { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
}
