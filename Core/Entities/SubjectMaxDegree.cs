using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class SubjectMaxDegree
    {
        public string SubjectCode { get; set; }
        public decimal Semester { get; set; }
        public string BatchDescription { get; set; }
        public decimal ProgramId { get; set; }
        public decimal? MaxDegree { get; set; }
    }
}
