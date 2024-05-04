using System;

namespace GYM_MN.Dtos
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }

        public int? MemberId { get; set; }

        public int? TrainerId { get; set; }

        public DateTime? FeedbackDate { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }
    }
}
