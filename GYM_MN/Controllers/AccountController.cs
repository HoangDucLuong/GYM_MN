using GYM_MN.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly GymMnContext _context;

        public AccountController(IConfiguration config, GymMnContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordByUsername([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                // Authenticate user
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == changePasswordDto.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.Password))
                {
                    return Unauthorized("Invalid old password"); // Unauthorized if user not found or old password is incorrect
                }

                // Hash new password using bcrypt
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                user.Password = hashedPassword;

                await _context.SaveChangesAsync();

                // Generate new JWT token after changing password
                var tokenString = GenerateJWTToken(user);

                return Ok(new { Token = tokenString }); // Password changed successfully
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal server error
            }
        }

        private string GenerateJWTToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()), // Add claim for username
                    new Claim("userId", user.UserId.ToString()) // Add claim for userId
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
