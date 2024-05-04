using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? MemberId { get; set; }

    public int? TrainerId { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Trainer? Trainer { get; set; }
}
