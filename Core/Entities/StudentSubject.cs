using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class StudentSubject
    {
        public string StudentNumber { get; set; }
        public string SubjectCode { get; set; }
        public long Semester { get; set; }
        public long? SubjectHour { get; set; }
        public decimal? Weight { get; set; }
        public float? Degree { get; set; }
        public string SubjectGrade { get; set; }
        public string SubjectGradeA { get; set; }
        public string ReGrade { get; set; }
        public string ReGradeA { get; set; }
        public string ReReGrade { get; set; }
        public string ReReGradeA { get; set; }
        public decimal? BatchId { get; set; }
        public decimal? GradeCounter { get; set; }
        public bool? Register { get; set; }
        public decimal Id { get; set; }
        public decimal? GradeId { get; set; }
        public int ViewYesNO { get; set; }
        public decimal SubjectCodeId { get; set; }

        //public decimal SubjectCodeId { get; set; }

        public virtual Student StudentNumberNavigation { get; set; }
        public virtual Subject SubjectCodeNavigation { get; set; }
    }
}
