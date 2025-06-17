using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Models;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly HotelContext _context;

        public BookingsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            return await _context.Bookings.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDto bookingDto)
        {
            var room = await _context.Rooms.FindAsync(bookingDto.RoomId);
            if (room == null)
            {
                return BadRequest("Кімната не знайдена.");
            }

            if (bookingDto.CheckOutDate < bookingDto.CheckInDate)
            {
                return BadRequest("Дата виїзду повинна бути пізніше за дату заїзду.");
            }

            if (bookingDto.CheckOutDate == bookingDto.CheckInDate)
            {
                return BadRequest("Мінімальне бронювання номеру - доба.");
            }

            bool isRoomAvailable = !_context.Bookings.Any(b =>
                b.RoomId == bookingDto.RoomId &&
                bookingDto.CheckInDate < b.CheckOutDate &&
                bookingDto.CheckOutDate > b.CheckInDate
            );

            if (!isRoomAvailable)
            {
                return BadRequest("Кімната зайнята на обрані дати.");
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c =>
                c.Email == bookingDto.Email || c.Phone == bookingDto.Phone);

            if (client == null)
            {
                client = new Client
                {
                    FullName = bookingDto.FullName,
                    Email = bookingDto.Email,
                    Phone = bookingDto.Phone
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var booking = new Booking
            {
                RoomId = bookingDto.RoomId,
                CheckInDate = bookingDto.CheckInDate,
                CheckOutDate = bookingDto.CheckOutDate,
                ClientId = client.ClientId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Перевірка: чи всі дати у кімнати тепер зайняті
            var futureDates = await _context.Bookings
                .Where(b => b.RoomId == bookingDto.RoomId && b.CheckOutDate >= DateOnly.FromDateTime(DateTime.Today))
                .ToListAsync();

            DateOnly cursor = DateOnly.FromDateTime(DateTime.Today);
            bool hasFreeDate = true;

            futureDates = futureDates.OrderBy(b => b.CheckInDate).ToList();

            foreach (var b in futureDates)
            {
                if (cursor < b.CheckInDate.GetValueOrDefault())
                {
                    hasFreeDate = true;
                    break;
                }

                if (cursor < b.CheckOutDate.GetValueOrDefault())
                {
                    cursor = b.CheckOutDate.GetValueOrDefault();
                    hasFreeDate = false;
                }
            }

            if (!hasFreeDate)
            {
                room.IsAvailable = false;
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        [HttpGet("room-dates/{roomId}")]
        public async Task<IActionResult> GetFutureBookingsForRoom(int roomId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var bookings = await _context.Bookings
                .Where(b => b.RoomId == roomId && b.CheckOutDate >= today)
                .Select(b => new
                {
                    b.CheckInDate,
                    b.CheckOutDate
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            int? roomId = booking.RoomId;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            if (roomId.HasValue)
            {
                var cursor = DateOnly.FromDateTime(DateTime.Today);

                var futureDates = await _context.Bookings
                    .Where(b => b.RoomId == roomId && b.CheckOutDate >= cursor)
                    .OrderBy(b => b.CheckInDate)
                    .ToListAsync();

                bool hasFreeDate = true;

                foreach (var b in futureDates)
                {
                    if (cursor < b.CheckInDate.GetValueOrDefault())
                    {
                        hasFreeDate = true;
                        break;
                    }

                    if (cursor < b.CheckOutDate.GetValueOrDefault())
                    {
                        cursor = b.CheckOutDate.GetValueOrDefault();
                        hasFreeDate = false;
                    }
                }

                if (hasFreeDate)
                {
                    var room = await _context.Rooms.FindAsync(roomId.Value);
                    if (room != null)
                    {
                        room.IsAvailable = true;
                        _context.Rooms.Update(room);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
