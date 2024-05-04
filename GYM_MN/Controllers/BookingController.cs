using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GYM_MN.Models;
using GYM_MN.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly GymMnContext _context;

        public BookingController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            var bookings = await _context.Bookings
                .Select(b => new BookingDto
                {
                    MemberId = b.MemberId,
                    MembershipTypeId = b.MembershipTypeId,
                    TrainerId = b.TrainerId,
                    BookingDate = b.BookingDate,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,  
                    Status = b.Status
                })
                .ToListAsync();

            return bookings;
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            var bookingDto = new BookingDto
            {
                MemberId = booking.MemberId,
                TrainerId = booking.TrainerId,
                MembershipTypeId= booking.MembershipTypeId,
                BookingDate = booking.BookingDate,
                StartDate = booking.StartDate, 
                EndDate = booking.EndDate,    
                Status = booking.Status
            };

            return bookingDto;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> PostBooking(BookingDto bookingDto)
        {
            var booking = new Booking
            {
                MemberId = bookingDto.MemberId,
                TrainerId = bookingDto.TrainerId,
                MembershipTypeId = bookingDto.MembershipTypeId,
                BookingDate = bookingDto.BookingDate,
                StartDate = bookingDto.StartDate, 
                EndDate = bookingDto.EndDate,     
                Status = bookingDto.Status
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }


        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
