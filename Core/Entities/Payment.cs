
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Entities
{
    public class Payment
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Semester { get; set; }
        public double Amount { get; set; }
        public Student Student { get; set; }
        public PaymentStatus Status { get; set; }
        public string CardRequestId { get; set; }
        public CardRequest CardRequest { get; set; }
        public string EnrollmentRequestId { get; set; }
        public EnrollmentRequest EnrollmentRequest { get; set; }
        public string CertificateRequestId { get; set; }
        public CertificateRequest CertificateRequest { get; set; }
        public string TranscriptRequestId { get; set; }
        public TranscriptRequest TranscriptRequest { get; set; }
        public string ReceiptPhoto { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
