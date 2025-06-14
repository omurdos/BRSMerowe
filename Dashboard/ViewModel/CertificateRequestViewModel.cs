using Core.Entities;
using Core.Enums;

namespace Dashboard.ViewModel
{

    public class CertificateRequestViewModel
    {
        public string Id { get; set; }
        public string FullNameAR { get; set; }
        public string FullNameEN { get; set; }
        public CertificateLanguage Language { get; set; }
        public string ReceiptPhoto { get; set; }
        public Service Service { get; set; }
        public RequestStatus Status { get; set; }
        public Student Student { get; set; }
        public Payment Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

       public class EditCertificateRequestViewModel
    {
        public string Id { get; set; }
        public string FullNameAR { get; set; }
        public string FullNameEN { get; set; }
        public CertificateLanguage Language { get; set; }
        public string ReceiptPhoto { get; set; }
        public Service Service { get; set; }
        public RequestStatus Status { get; set; }
        public Payment Payment { get; set; }
        public Student Student { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
