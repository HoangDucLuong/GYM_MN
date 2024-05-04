namespace GYM_MN.Dtos
{
    public class TrainerDto
    {
        public int? UserId { get; set; }
        public int TrainerId { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Specialization { get; set; }
        public TimeOnly? WorkStartTime { get; set; }

        public TimeOnly? WorkEndTime { get; set; }
    }
}
