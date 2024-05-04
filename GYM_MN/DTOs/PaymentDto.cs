namespace GYM_MN.Dtos
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int? MemberId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public int? MembershipTypeId { get; set; }
    }
}
