using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class GetStdGrade
    {
        public string StudentNumber { get; set; }
        public float? Degree { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectGrade { get; set; }
        public string SubjectGradeA { get; set; }
        public bool? Register { get; set; }
        public string Grade { get; set; }
        public string GradeA { get; set; }
    }
}
