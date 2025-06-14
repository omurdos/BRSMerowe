using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Signature
    {
        public string RegNameA { get; set; }
        public string RegNameE { get; set; }
        public string DeanFacultyA { get; set; }
        public string DeanFacultyE { get; set; }
        public string DeanAffairsA { get; set; }
        public string DeanAffairsE { get; set; }
        public string ExamOfficerA { get; set; }
        public string ExamOfficerE { get; set; }
        public decimal? FacultyNumber { get; set; }
    }
}
