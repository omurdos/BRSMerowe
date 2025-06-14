using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Subject
    {
        public string SubjectCode { get; set; }
        public string DepartmentNumber { get; set; }
        public string FacultyNumber { get; set; }
        public string SubjectNameA { get; set; }
        public string SubjectNameE { get; set; }
        public int SubjectHours { get; set; }
        public int ProgramID { get; set; }
        public decimal OrderInResult { get; set; }
        public decimal SubjectCodeID { get; set; }
        public decimal SubjectCodeRight { get; set; }
    }
}
