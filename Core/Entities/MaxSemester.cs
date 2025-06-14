using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class MaxSemester
    {
        public decimal SemesterNo { get; set; }
        public string FacultyNumber { get; set; }
        public decimal ProgramId { get; set; }
        public decimal? TotalHours { get; set; }
    }
}
