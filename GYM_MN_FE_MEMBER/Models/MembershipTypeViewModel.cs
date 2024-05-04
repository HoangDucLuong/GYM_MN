using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_MEMBER.Models
{
    public class MembershipTypeViewModel
    {
        public int? MembershipTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
