using GYM_MN.Models;
using GYM_MN.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly GymMnContext _context;

        public FeedbackController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Trainer)
                .Include(f => f.Member)
                .Select(f => new FeedbackDto
                
                {
                    FeedbackId = f.FeedbackId,
                    TrainerName = f.Trainer.FullName,
                    MemberName = f.Member.FullName,
                    MemberId = f.MemberId,
                    TrainerId = f.TrainerId,
                    FeedbackDate = f.FeedbackDate,
                    Rating = f.Rating,
                    Comment = f.Comment
                })
                .ToListAsync();

            return feedbacks;
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            var feedbackDto = new FeedbackDto
            {
                FeedbackId = feedback.FeedbackId,
                MemberId = feedback.MemberId,
                TrainerId = feedback.TrainerId,
                FeedbackDate = feedback.FeedbackDate,
                Rating = feedback.Rating,
                Comment = feedback.Comment
            };

            return feedbackDto;
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackDto feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = new Feedback

            {
                MemberId = feedbackDto.MemberId,
                TrainerId = feedbackDto.TrainerId,
                FeedbackDate = feedbackDto.FeedbackDate,
                Rating = feedbackDto.Rating,
                Comment = feedbackDto.Comment
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.FeedbackId }, feedback);
        }

        // PUT: api/Feedback/5
        // PUT: api/Feedback
        [HttpPut]
        public async Task<IActionResult> PutFeedback(FeedbackDto feedbackDto)
        {
            if (feedbackDto == null || feedbackDto.FeedbackId == null)
            {
                return BadRequest("Invalid feedback data or feedback ID is missing");
            }

            var feedback = await _context.Feedbacks.FindAsync(feedbackDto.FeedbackId);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.MemberId = feedbackDto.MemberId;
            feedback.TrainerId = feedbackDto.TrainerId;
            feedback.FeedbackDate = feedbackDto.FeedbackDate;
            feedback.Rating = feedbackDto.Rating;
            feedback.Comment = feedbackDto.Comment;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(feedbackDto.FeedbackId))
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


        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
