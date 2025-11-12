using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class StudentInvoiceDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("student_id")]
        public string StudentId { get; set; }

        [JsonPropertyName("student_name")]
        public string StudentName { get; set; }

        [JsonPropertyName("payment_reference")]
        public string PaymentReference { get; set; }

        [JsonPropertyName("semester_id")]
        public int SemesterId { get; set; }

        [JsonPropertyName("payment_amount")]
        public string PaymentAmount { get; set; }

        [JsonPropertyName("transaction_ref")]
        public string TransactionRef { get; set; }

        [JsonPropertyName("bank_id")]
        public int BankId { get; set; }

        [JsonPropertyName("fee_type_id")]
        public int FeeTypeId { get; set; }

        [JsonPropertyName("deleted_at")]
        public DateTimeOffset? DeletedAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonPropertyName("study_fees")]
        public string StudyFees { get; set; }

        [JsonPropertyName("registration_fees")]
        public string RegistrationFees { get; set; }

        [JsonPropertyName("insurance_fees")]
        public string InsuranceFees { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("penalty")]
        public string Penalty { get; set; }

        [JsonPropertyName("stamp")]
        public string Stamp { get; set; }

        [JsonPropertyName("payment_currency")]
        public string PaymentCurrency { get; set; }

        [JsonPropertyName("constraint_id")]
        public int? ConstraintId { get; set; }
    }


}