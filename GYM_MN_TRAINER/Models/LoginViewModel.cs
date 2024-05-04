using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GYM_MN_TRAINER.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
