using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            StudentSubjects = new HashSet<StudentSubject>();
            TblSections = new HashSet<TblSection>();
        }

        public decimal SubjectCodeId { get; set; }
        public string SubjectCode { get; set; }
        public string DepartmentNumber { get; set; }
        public string FacultyNumber { get; set; }
        public string SubjectNameA { get; set; }
        public string SubjectNameE { get; set; }
        public short? SubjectHour { get; set; }
        public int? ProgramId { get; set; }
        //public decimal? OrederInResult { get; set; }
        //public int Required { get; set; }
        //public decimal SubjectCodeId { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
        public virtual ICollection<TblSection> TblSections { get; set; }
    }
}
