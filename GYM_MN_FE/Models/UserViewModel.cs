using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_ADMIN.Models
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public RoleViewModel Role { get; set; }
    }

    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
