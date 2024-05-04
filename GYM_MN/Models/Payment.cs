using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? MemberId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public int? MembershipTypeId { get; set; }

    public virtual Member? Member { get; set; }

    public virtual MembershipType? MembershipType { get; set; }
}
