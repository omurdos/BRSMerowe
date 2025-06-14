using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class StudentSubjects
    {
        public string StudentNumber { get; set; }
        public string SubjectCode { get; set; }
        public int Semester { get; set; }
        public int SubjectHours { get; set; }
        public decimal Weight { get; set; }
        public int Degree { get; set; }
        public string SubjectGrade { get; set; }
        public string SubjectGradeA { get; set; }
        public string ReGrade { get; set; }
        public string ReGradeA { get; set; }
        public string ReReGrade { get; set; }
        public string ReReGradeA { get; set; }
        public decimal BatchID { get; set; }
        public decimal GradeCounter { get; set; }
        public bool Register { get; set; }
        public decimal ID { get; set; }
        public decimal GradeID { get; set; }
        public decimal SubjectCodeID { get; set; }
        public decimal SubjectCodeRight { get; set; }
        public decimal ViewYesNO { get; set; }
    }
}
