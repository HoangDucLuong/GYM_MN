using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? MemberId { get; set; }

    public int? TrainerId { get; set; }

    public DateTime? BookingDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }

    public int? MembershipTypeId { get; set; }

    public virtual Member? Member { get; set; }

    public virtual MembershipType? MembershipType { get; set; }

    public virtual Trainer? Trainer { get; set; }
}
