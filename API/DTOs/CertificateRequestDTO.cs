using Core.Enums;

namespace API.DTOs
{

    public class CertificateRequestDTO { 
    }

    public class CreateCertificateRequestDTO
    {
        public string StudentNumber { get; set; }
        public string FullNameArabic { get; set; }
        public string FullNameEnglish { get; set; }
        public CertificateLanguage Language { get; set; }
        public string ServiceName { get; set; }
    }
    public class UpdateCertificateRequestDTO
    {
        public string Id { get; set; }
        public string StudentNumber { get; set; }
        public string FullNameArabic { get; set; }
        public string FullNameEnglish { get; set; }
        public CertificateLanguage Language { get; set; }
                public string ServiceName { get; set; }

        public string ReceiptPhoto { get; set; }
}

}
