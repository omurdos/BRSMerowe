using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Studentstemp
    {
        public decimal Serial { get; set; }
        public string Facultynumber { get; set; }
        public decimal? BatchId { get; set; }
        public decimal? ProgramId { get; set; }
        public string DepartmentNumber { get; set; }
        public string StudentNumber { get; set; }
        public string StudentNameA { get; set; }
        public string ApplicationNumber { get; set; }
        public decimal? StudyFees { get; set; }
        public decimal? Proiority { get; set; }
    }
}
