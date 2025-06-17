using System;
using System.Collections.Generic;

namespace HotelApi.Models;

public partial class ClientService
{
    public int ClientServiceId { get; set; }

    public int? ClientId { get; set; }

    public int? ServiceId { get; set; }

    public DateOnly? DateUsed { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Service? Service { get; set; }
}
