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
    public class UsersController : ControllerBase   
    {
        private readonly GymMnContext _context;

        public UsersController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Password = u.Password,
                    RoleId = u.RoleId
                })
                .ToListAsync();

            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Password = user.Password,
                RoleId = user.RoleId
            };

            return userDto;
        }
        // GET: api/Users/GetUserByUsername/username
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Password = user.Password,
                RoleId = user.RoleId
            };

            return userDto;
        }
        // GET: api/Users/GetPasswordByUserId/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<string>> GetPasswordByUserId(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user.Password;
        }


        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            // Kiểm tra xem userDto có hợp lệ không
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tạo mới đối tượng User từ UserDto
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                RoleId = userDto.RoleId
            };

            // Thêm đối tượng User vào DbSet trong DbContext
            _context.Users.Add(user);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về mã 201 Created và thông tin của đối tượng đã tạo
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }


        // PUT: api/Users
        [HttpPut]
        public async Task<IActionResult> PutUser(UserDto userDto)
        {
            var id = userDto.UserId;
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.Password = userDto.Password;
            user.RoleId = userDto.RoleId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
