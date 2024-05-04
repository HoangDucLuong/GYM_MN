namespace GYM_MN.DTOs
{
    public class RegisterTrainerDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
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
