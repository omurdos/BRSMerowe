using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class CardFee
    {
        public decimal CardFeesId { get; set; }
        public decimal? CardFees { get; set; }
        public decimal BatchId { get; set; }
        public decimal Semester { get; set; }
        public string FacultyNumber { get; set; }
        public decimal? ProgramId { get; set; }
    }
}
