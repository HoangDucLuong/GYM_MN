namespace GYM_MN_FE_MEMBER.Models
{
    public class BookingViewModel
    {
        public int? MemberId { get; set; }
        public int? TrainerId { get; set; }
        public DateTime? BookingDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Waitting";
        public int? MembershipTypeId { get; set; }
        public string? MemberTypeName { get; set; }
        public string? FullName { get; set; }
    }
}
