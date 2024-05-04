using System.ComponentModel.DataAnnotations;

namespace GYM_MN_FE_MEMBER.Models
{
    public class TimeSelectionViewModel
    {
        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        public List<string> SelectedTimeSlots { get; set; }
    }
}
