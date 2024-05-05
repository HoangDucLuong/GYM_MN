using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_MEMBER.Models
{
    public class FeedBackViewModel
    {
        public int FeedbackId { get; set; }

        public int? MemberId { get; set; }

        public int? TrainerId { get; set; }

        public DateTime? FeedbackDate { get; set; } = DateTime.Now;
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        public string? Comment { get; set; }
    }
}
