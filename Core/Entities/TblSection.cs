using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class TblSection
    {
        public string SubjectCode { get; set; }
        public decimal ProgramId { get; set; }
        public long Semester { get; set; }
        public decimal SectionId { get; set; }
        public string DepartmentNumber { get; set; }
        public string FacultyNumber { get; set; }
        public decimal SubjectCodeId { get; set; }

        public virtual Subject SubjectCodeNavigation { get; set; }
    }
}
