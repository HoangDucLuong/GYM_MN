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

            // Cập nhật thông tin thành viên sau khi tạo booking
            var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == bookingDto.MemberId);
            if (member != null)
            {
                member.MembershipTypeId = bookingDto.MembershipTypeId; // Cập nhật MembershipTypeId của thành viên
                await _context.SaveChangesAsync();
            }
            else
            {
                // Xử lý trường hợp không tìm thấy thành viên
                return NotFound("Member not found");
            }

            return Ok(bookingDto);
        }


        // GET: api/Booking/GetBookingByMemberId/5
        [HttpGet("GetBookingByMemberId/{memberId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingByMemberId(int memberId)
        {
            var bookings = await _context.Bookings
                .Include(m => m.MembershipType)
                .Include(m => m.Trainer)
                .Where(b => b.MemberId == memberId)
                .Select(b => new BookingDto
                {
                    MemberId = b.MemberId,
                    TrainerId = b.TrainerId,
                    FullName = b.Trainer.FullName,
                    MembershipTypeId = b.MembershipTypeId,
                    MemberTypeName = b.MembershipType.TypeName,
                    BookingDate = b.BookingDate,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status
                })
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound();
            }

            return bookings;
        }

        // GET: api/Booking/GetBookingByTrainerId/5
        [HttpGet("GetBookingByTrainerId/{trainerId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingByTrainerId(int trainerId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.TrainerId == trainerId)
                .Include(m => m.Member)
                .Include(m => m.MembershipType)
                .Select(b => new BookingDto
                {
                    MemberId = b.MemberId,
                    FullName = b.Member.FullName,
                    TrainerId = b.TrainerId,
                    MembershipTypeId = b.MembershipTypeId,
                    MemberTypeName = b.MembershipType.TypeName,
                    BookingDate = b.BookingDate,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status
                })
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound();
            }

            return bookings;
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
