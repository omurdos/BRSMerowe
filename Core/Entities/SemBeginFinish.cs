using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class SemBeginFinish
    {
        public string FacultyNumber { get; set; }
        public string DepartmentNumber { get; set; }
        public decimal? ProgramId { get; set; }
        public decimal? BatchId { get; set; }
        public string Semester { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
