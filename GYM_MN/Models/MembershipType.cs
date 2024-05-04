using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class MembershipType
{
    public int MembershipTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
