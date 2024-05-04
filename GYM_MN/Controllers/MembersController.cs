using GYM_MN.Models;
using GYM_MN.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly GymMnContext _context;

        public MembersController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            var members = await _context.Members
                .Select(m => new MemberDto
                {
                    UserId = m.UserId,
                    MemberId = m.MemberId,
                    FullName = m.FullName,
                    Email = m.Email,
                    Phone = m.Phone,
                    Gender = m.Gender,
                    Dob = m.Dob,
                    Address = m.Address,
                    MembershipTypeId = m.MembershipTypeId,
                    JoinDate = m.JoinDate
                })
                .ToListAsync();

            return members;
        }
        [HttpGet("memberid/{userId}")]
        public async Task<ActionResult<MemberDto>> GetMemberIdFromUserId(int userId)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy thành viên với UserID tương ứng
            }

            var memberDto = new MemberDto
            {
                UserId = member.UserId,
                MemberId = member.MemberId,           
            };

            return memberDto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            var memberDto = new MemberDto
            {
                UserId  = member.UserId,
                MemberId = member.MemberId,
                FullName = member.FullName,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender,
                Dob = member.Dob,
                Address = member.Address,
                MembershipTypeId = member.MembershipTypeId,
                JoinDate = member.JoinDate
            };

            return memberDto;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<MemberDto>> GetMemberByUserId(int userId)
        {
            var member = await _context.Members
        .Include(m => m.MembershipType)
        .FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                return NotFound();
            }

            var memberDto = new MemberDto
            {
                UserId = member.UserId,
                MemberId = member.MemberId,
                FullName = member.FullName,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender,
                Dob = member.Dob,
                Address = member.Address,
                MembershipTypeId = member.MembershipTypeId,
                MemberTypeName = member.MembershipType?.TypeName,
                JoinDate = member.JoinDate
            };

            return memberDto;
        }

        // POST: api/Members
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(MemberDto memberDto)
        {
            var member = new Member
            {
                UserId = memberDto.UserId,
                FullName = memberDto.FullName,
                Email = memberDto.Email,
                Phone = memberDto.Phone,
                Gender = memberDto.Gender,
                Dob = memberDto.Dob,
                Address = memberDto.Address,
                MembershipTypeId = memberDto.MembershipTypeId,
                JoinDate = memberDto.JoinDate
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.MemberId }, member);
        }
        // PUT: api/Members/PutMemberByUserId/5
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutMemberByUserId(int userId, MemberDto memberDto)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin thành viên
            member.FullName = memberDto.FullName;
            member.Email = memberDto.Email;
            member.Phone = memberDto.Phone;
            member.Gender = memberDto.Gender;
            member.Dob = memberDto.Dob;
            member.Address = memberDto.Address;
            member.MembershipTypeId = memberDto.MembershipTypeId;
            member.JoinDate = memberDto.JoinDate;

            // Lấy thông tin về MembershipType từ cơ sở dữ liệu
            var membershipType = await _context.MembershipTypes.FindAsync(memberDto.MembershipTypeId);
            if (membershipType != null)
            {
                // Cập nhật MemberTypeName
                memberDto.MemberTypeName = membershipType.TypeName;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(member.MemberId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Trả về NoContent với thông tin về thành viên sau khi cập nhật
            return NoContent();
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
