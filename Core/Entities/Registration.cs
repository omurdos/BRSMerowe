using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Registration
    {
        public string StudentNumber { get; set; }
        public decimal Semester { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegistrationStatus { get; set; }
        public decimal? RegistrationFees { get; set; }
        public decimal? StudyFees { get; set; }
        public decimal? RegCourses { get; set; }
        public string SlipNumber { get; set; }
        public decimal? RegistrationTypeId { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PayedStudyFees { get; set; }
        public string Note { get; set; }
        public decimal? CardFees { get; set; }
        public decimal? Feed { get; set; }
        public decimal StudentId { get; set; }
        public decimal SlipId { get; set; }

        public virtual Student StudentNumberNavigation { get; set; }
    }
}
