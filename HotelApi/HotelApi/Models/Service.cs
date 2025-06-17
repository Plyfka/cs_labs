using System;
using System.Collections.Generic;

namespace HotelApi.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
}
