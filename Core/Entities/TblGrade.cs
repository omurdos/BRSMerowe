using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class TblGrade
    {
        public string Grade { get; set; }
        public string GradeA { get; set; }
        public decimal? Weight { get; set; }
        public decimal? XMin { get; set; }
        public decimal? XMax { get; set; }
        public long Id { get; set; }
        public string Note { get; set; }
    }
}
