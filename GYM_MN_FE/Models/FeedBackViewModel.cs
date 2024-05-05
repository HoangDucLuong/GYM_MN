namespace GYM_MN_FE_ADMIN.Models
{
    public class FeedBackViewModel
    {
        public int FeedbackId { get; set; }

        public int? MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? TrainerName { get; set; }
        public int? TrainerId { get; set; }

        public DateTime? FeedbackDate { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }
    }
}
