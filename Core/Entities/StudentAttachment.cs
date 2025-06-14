using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class StudentAttachment
    {
        public string Id { get; set; }
        public string PersonalPhoto { get; set; }
        public string MedicalReport { get; set; }
        public string IdentityProof { get; set; }
        public string StudentNumber { get; set; }
        public Student Student { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
