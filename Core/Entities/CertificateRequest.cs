using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CertificateRequest
    {
        public string Id { get; set; }
        public string FullNameAR { get; set; }
        public string FullNameEN { get; set; }
        public CertificateLanguage Language { get; set; }
        public string ReceiptPhoto { get; set; }
        public string ServiceId { get; set; }
        public Service Service { get; set; }
        public string RequestStatusId { get; set; }
        public RequestStatus Status { get; set; }
        public Student Student { get; set; }
        public Payment Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
