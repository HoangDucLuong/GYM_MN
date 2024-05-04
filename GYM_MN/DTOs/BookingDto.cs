namespace GYM_MN.Dtos
{
    public class BookingDto
    { 
        public int? MemberId { get; set; }
        public int? TrainerId { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public int? MembershipTypeId { get; set; }

    }
}
