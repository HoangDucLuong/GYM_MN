using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_ADMIN.Models
{
    public class MemberViewModel
    {
        public int UserId { get; set; }
        public int? MemberId { get; set; }
        [DisplayName("Member Name")]
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

