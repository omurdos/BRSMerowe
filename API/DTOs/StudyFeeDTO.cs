namespace API.DTOs
{
    public class StudyFeeDTO
    {
        public string StudentNumber { get; set; }
        public string Semester { get; set; }
        public double Amount { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime PaidAt { get; set; }

    }
}
