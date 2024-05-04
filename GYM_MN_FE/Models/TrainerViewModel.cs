using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_ADMIN.Models
{
    public class TrainerViewModel
    {
        public int? UserId { get; set; }
        public int? TrainerId { get; set; }
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Specialization { get; set; }
        [Display(Name = "Work Start Time")]
        public TimeOnly? WorkStartTime { get; set; }

        [Display(Name = "Work End Time")]
        public TimeOnly? WorkEndTime { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }
    }
}
