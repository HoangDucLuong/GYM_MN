using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_MEMBER.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public int? MembershipTypeId { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
