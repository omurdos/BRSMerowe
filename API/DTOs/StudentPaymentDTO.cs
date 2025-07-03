using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class StudentPaymentDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        //[JsonPropertyName("student_id")]
        //public string StudentId { get; set; }

        //[JsonPropertyName("student_name")]
        //public string StudentName { get; set; }

        [JsonPropertyName("payment_reference")]
        public string PaymentReference { get; set; }

        //[JsonPropertyName("start")]
        //public DateTime Start { get; set; }

        //[JsonPropertyName("end")]
        //public DateTime End { get; set; }

        [JsonPropertyName("is_paid")]
        public int IsPaid { get; set; }

        //[JsonPropertyName("fee_type_id")]
        //public int? FeeTypeId { get; set; }

        //[JsonPropertyName("study_fees")]
        //public decimal StudyFees { get; set; }

        //[JsonPropertyName("registration_fees")]
        //public decimal RegistrationFees { get; set; }

        //[JsonPropertyName("insurance_fees")]
        //public decimal InsuranceFees { get; set; }

        //[JsonPropertyName("discount")]
        //public decimal Discount { get; set; }

        [JsonPropertyName("semester_id")]
        public int SemesterId { get; set; }

        [JsonPropertyName("payment_amount")]
        public string PaymentAmount { get; set; }

        //[JsonPropertyName("due_amount")]
        //public decimal DueAmount { get; set; }

        //[JsonPropertyName("user_id")]
        //public int UserId { get; set; }

        //[JsonPropertyName("stamp")]
        //public decimal Stamp { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        ////[JsonPropertyName("updated_at")]
        ////public DateTime UpdatedAt { get; set; }

        ////[JsonPropertyName("deleted_at")]
        ////public DateTime? DeletedAt { get; set; }

        //[JsonPropertyName("payment_currency")]
        //public string PaymentCurrency { get; set; }

        //[JsonPropertyName("penalty")]
        //public decimal Penalty { get; set; }
    }

}