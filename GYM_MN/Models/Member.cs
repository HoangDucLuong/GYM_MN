using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public int? MembershipTypeId { get; set; }

    public DateTime? JoinDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual MembershipType? MembershipType { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
