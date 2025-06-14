using Core.Enums;

namespace API.DTOs
{
    public class PaymentDTO
    {
    }
    public class UpdatePaymentDTO
    {
        public string Id { get; set; }
        public PaymentStatus? Status { get; set; }
        public string ReceiptPhoto { get; set; }
    }
}
