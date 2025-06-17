using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HotelApi.Models;

namespace HotelApi.Models
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelApi.Models.Booking> Bookings { get; set; } = default!;
    }
}
