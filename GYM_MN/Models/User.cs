using System;
using System.Collections.Generic;

namespace GYM_MN.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}
