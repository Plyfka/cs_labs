using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HotelApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HotelApi.Services
{
    public class AvailabilityRecalculationService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public AvailabilityRecalculationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Запуск сразу при старте
            _ = RecalculateAvailabilityAsync();

            // Планируем запуск каждый день в 2:00 ночи
            var now = DateTime.Now;
            var nextRun = DateTime.Today.AddDays(1).AddHours(2);
            var delay = nextRun - now;

            _timer = new Timer(async _ => await RecalculateAvailabilityAsync(), null, delay, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async Task RecalculateAvailabilityAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HotelContext>();

            var today = DateOnly.FromDateTime(DateTime.Today);
            var rooms = await db.Rooms.ToListAsync();

            foreach (var room in rooms)
            {
                var bookings = await db.Bookings
                    .Where(b => b.RoomId == room.RoomId && b.CheckOutDate >= today)
                    .OrderBy(b => b.CheckInDate)
                    .ToListAsync();

                if (bookings.Count == 0)
                {
                    // Нет бронирований — комната свободна
                    if (room.IsAvailable != true)
                    {
                        room.IsAvailable = true;
                        db.Rooms.Update(room);
                    }
                    continue;
                }

                DateOnly cursor = today;
                bool isFullyBooked = true;

                foreach (var booking in bookings)
                {
                    var checkIn = booking.CheckInDate.GetValueOrDefault();
                    var checkOut = booking.CheckOutDate.GetValueOrDefault();

                    if (cursor < checkIn)
                    {
                        // Нашли разрыв (свободный промежуток)
                        isFullyBooked = false;
                        break;
                    }

                    if (cursor < checkOut)
                    {
                        cursor = checkOut;
                    }
                }

                // Если курсор ушёл дальше или равен сегодняшней дате — значит после бронирований свободных дат нет
                // Иначе есть свободное место после последней брони
                if (cursor > today)
                {
                    isFullyBooked = false;
                }

                bool newAvailability = !isFullyBooked;

                if (room.IsAvailable != newAvailability)
                {
                    room.IsAvailable = newAvailability;
                    db.Rooms.Update(room);
                }
            }

            await db.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
