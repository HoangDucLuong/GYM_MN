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
    public class MembershipTypesController : ControllerBase
    {
        private readonly GymMnContext _context;

        public MembershipTypesController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/MembershipTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipTypeDto>>> GetMembershipTypes()
        {
            var membershipTypes = await _context.MembershipTypes
                .Select(mt => new MembershipTypeDto
                {
                    MembershipTypeId = mt.MembershipTypeId,
                    TypeName = mt.TypeName,
                    Description = mt.Description,
                    Price = mt.Price
                })
                .ToListAsync();

            return membershipTypes;
        }

        // GET: api/MembershipTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipTypeDto>> GetMembershipType(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);

            if (membershipType == null)
            {
                return NotFound();
            }

            var membershipTypeDto = new MembershipTypeDto
            {
                MembershipTypeId = membershipType.MembershipTypeId,
                TypeName = membershipType.TypeName,
                Description = membershipType.Description,
                Price = membershipType.Price
            };

            return membershipTypeDto;
        }

        // POST: api/MembershipTypes
        [HttpPost]
        public async Task<ActionResult<MembershipType>> PostMembershipType(MembershipTypeDto membershipTypeDto)
        {
            // Tạo mới đối tượng MembershipType từ MembershipTypeDto
            var membershipType = new MembershipType
            {
                TypeName = membershipTypeDto.TypeName,
                Description = membershipTypeDto.Description,
                Price = membershipTypeDto.Price
            };

            _context.MembershipTypes.Add(membershipType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembershipType), new { id = membershipType.MembershipTypeId }, membershipType);
        }

        // PUT: api/MembershipTypes
        [HttpPut]
        public async Task<IActionResult> PutMembershipType(MembershipTypeDto membershipTypeDto)
        {
            var id = membershipTypeDto.MembershipTypeId;
            var membershipType = await _context.MembershipTypes.FindAsync(id);

            if (membershipType == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin của đối tượng MembershipType từ MembershipTypeDto
            membershipType.TypeName = membershipTypeDto.TypeName;
            membershipType.Description = membershipTypeDto.Description;
            membershipType.Price = membershipTypeDto.Price;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipTypeExists(id))
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

        // DELETE: api/MembershipTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembershipType(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            _context.MembershipTypes.Remove(membershipType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MembershipTypeExists(int id)
        {
            return _context.MembershipTypes.Any(e => e.MembershipTypeId == id);
        }
    }
}
