using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_MEMBER.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
