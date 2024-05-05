using GYM_MN.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GYM_MN.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using GYM_MN.DTOs;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly GymMnContext _context;

        public AuthController(IConfiguration config, GymMnContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDTO)
        {
            // Authenticate user
            var user = _context.Users.SingleOrDefault(tk => tk.Username == loginDTO.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return Unauthorized(); // Unauthorized if user not found or invalid credentials
            }

            // Generate JWT token
            var tokenString = GenerateJWTToken(user);

            // Return token
            return Ok(new { Token = tokenString });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDTO)
        {
            // Check if username is already taken
            if (await _context.Users.AnyAsync(tk => tk.Username == registerDTO.Username))
            {
                return BadRequest("Username is already taken");
            }

            // Hash password using bcrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            // Create new user with hashed password and default role set to 3
            var newUser = new User
            {
                Username = registerDTO.Username,
                Password = hashedPassword,
                RoleId = 3
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Create new member
            var newMember = new Member
            {
                UserId = newUser.UserId, // Use the UserId of the newly created user
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                Phone = registerDTO.Phone,
                Gender = registerDTO.Gender,
                Dob = registerDTO.Dob,
                Address = registerDTO.Address,   
                JoinDate = DateTime.Now
            };

            _context.Members.Add(newMember);
            await _context.SaveChangesAsync();

            return StatusCode(201); // Created
        }
        [HttpPost("register-trainer")]
        public async Task<IActionResult> RegisterTrainer(RegisterTrainerDto registerTrainerDTO)
        {
            // Check if username is already taken
            if (await _context.Users.AnyAsync(tk => tk.Username == registerTrainerDTO.Username))
            {
                return BadRequest("Username is already taken");
            }

            // Hash password using bcrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerTrainerDTO.Password);

            // Create new user with hashed password and default role set to 3
            var newUser = new User
            {
                Username = registerTrainerDTO.Username,
                Password = hashedPassword,
                RoleId = 2
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Create new member
            var newTrainer = new Trainer
            {
                UserId = newUser.UserId, // Use the UserId of the newly created user
                FullName = registerTrainerDTO.FullName,
                Email = registerTrainerDTO.Email,
                Phone = registerTrainerDTO.Phone,
                Gender = registerTrainerDTO.Gender,
                Dob = registerTrainerDTO.Dob,
                Specialization = registerTrainerDTO.Specialization,
                WorkStartTime = registerTrainerDTO.WorkStartTime,
                WorkEndTime = registerTrainerDTO.WorkEndTime,
            };

            _context.Trainers.Add(newTrainer);
            await _context.SaveChangesAsync();

            return StatusCode(201); // Created
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xóa token từ session hoặc cookie
            HttpContext.Session.Remove("Token"); // hoặc HttpContext.Response.Cookies.Delete("Token")

            // Trả về mã trạng thái thành công
            return Ok();
        }


        private string GenerateJWTToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Username.ToString()),
            new Claim("userId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()) 
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}