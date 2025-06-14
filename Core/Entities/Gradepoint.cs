using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Gradepoint
    {
        public string StudentNumber { get; set; }
        public long? Semester { get; set; }
        //public strin BatchId { get; set; }
        public float? SPoints { get; set; }
        public short? SHours { get; set; }
        public float? Gpa { get; set; }
        public float? CPoints { get; set; }
        public short? CHours { get; set; }
        public float Cgpa { get; set; }
       // public decimal Pcgpa { get; set; }
        //public float? Ppcgpa { get; set; }
        public string Status { get; set; }
    }
}
