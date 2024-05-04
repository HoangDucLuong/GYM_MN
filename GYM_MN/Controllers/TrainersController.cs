using GYM_MN.Models;
using GYM_MN.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GYM_MN.DTOs;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly GymMnContext _context;

        public TrainersController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetTrainers()
        {
            var trainers = await _context.Trainers
                .Select(t => new TrainerDto
                {
                    UserId = t.UserId,
                    TrainerId = t.TrainerId,
                    FullName = t.FullName,
                    Dob = t.Dob,
                    Email = t.Email,
                    Phone = t.Phone,
                    Gender = t.Gender,
                    Specialization = t.Specialization,
                    WorkStartTime = t.WorkStartTime,
                    WorkEndTime = t.WorkEndTime,
                })
                .ToListAsync();

            return trainers;
        }

        // GET: api/Trainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainerDto>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
            {
                return NotFound();
            }

            var trainerDto = new TrainerDto
            {
                UserId = trainer.UserId,
                TrainerId = trainer.TrainerId,
                FullName = trainer.FullName,
                Dob = trainer.Dob,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Gender = trainer.Gender,
                Specialization = trainer.Specialization,
                WorkStartTime = trainer.WorkStartTime,
                WorkEndTime = trainer.WorkEndTime,
            };

            return trainerDto;
        }

        // POST: api/Trainers
        [HttpPost]
        public async Task<ActionResult<Trainer>> PostTrainer(TrainerDto trainerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trainer = new Trainer
            {
                UserId = trainerDto.UserId,
                FullName = trainerDto.FullName,
                Dob = trainerDto.Dob,
                Email = trainerDto.Email,
                Phone = trainerDto.Phone,
                Gender = trainerDto.Gender,
                Specialization = trainerDto.Specialization
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainer), new { id = trainer.TrainerId }, trainer);
        }

        // PUT: api/Trainers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainer(int id, TrainerDto trainerDto)
        {
            if (id != trainerDto.TrainerId)
            {
                return BadRequest();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            trainer.UserId = trainerDto.UserId;
            trainer.FullName = trainerDto.FullName;
            trainer.Dob = trainerDto.Dob;
            trainer.Email = trainerDto.Email;
            trainer.Phone = trainerDto.Phone;
            trainer.Gender = trainerDto.Gender;
            trainer.Specialization = trainerDto.Specialization;
            trainer.WorkStartTime = trainerDto.WorkStartTime;
            trainer.WorkEndTime = trainerDto.WorkEndTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(id))
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
        public async Task<ActionResult<Trainer>> CreateTrainerWithUser(TrainerUserDto trainerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tạo User mới từ thông tin được nhận
            var user = new User
            {
                Username = trainerUserDto.Username,
                Password = trainerUserDto.Password,
                RoleId = 2 // Mặc định RoleId là 2 (đã được sửa)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Sau khi tạo User thành công, sử dụng UserId của User mới tạo để gán cho Trainer
            var trainer = new Trainer
            {
                UserId = user.UserId,
                FullName = trainerUserDto.FullName,
                Dob = trainerUserDto.Dob,
                Email = trainerUserDto.Email,
                Phone = trainerUserDto.Phone,
                Gender = trainerUserDto.Gender,
                Specialization = trainerUserDto.Specialization
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainer), new { id = trainer.TrainerId }, trainer);
        }

       

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(trainer.UserId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Trainers.Remove(trainer);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.TrainerId == id);
        }
    }
}
