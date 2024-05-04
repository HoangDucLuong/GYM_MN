using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? Specialization { get; set; }

    public int? UserId { get; set; }

    public TimeOnly? WorkStartTime { get; set; }

    public TimeOnly? WorkEndTime { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User? User { get; set; }
}
