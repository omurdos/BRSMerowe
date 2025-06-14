using Core.Entities;

namespace Dashboard.ViewModel
{
    public class StudentSubjectViewModel
    {
        public decimal Id { get; set; }
        public decimal SubjectCodeId { get; set; }
        public string SubjectGrade { get; set; }
        public long Semester { get; set; }


        public string SubjectCode { get; set; }
        public Subject SubjectCodeNavigation { get; set; }
        public int ViewYesNO { get; set; }
        public List<Faculty> Faculties { get; set; }
        public string FacultyNumber { get; set; }
        public string DepartmentNumber { get; set; }
        public int BatchId { get; set; }
        public int ProgramId { get; set; }

    }
}
