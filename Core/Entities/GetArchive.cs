using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class GetArchive
    {
        public decimal StudentId { get; set; }
        public string StudentNumber { get; set; }
        public string StudentNameA { get; set; }
        public string StudentNameE { get; set; }
        public DateTime? AddmissionDate { get; set; }
        public string Addmissiontype { get; set; }
        public DateTime? GraduationDate { get; set; }
        public string CertificateType { get; set; }
        public string AddmissionFormNo { get; set; }
        public string Address { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string NationalityE { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string HomeLanguage { get; set; }
        public decimal? StudyFees { get; set; }
        public decimal? RegistrationFees { get; set; }
        public string Note { get; set; }
        public decimal? BatchId { get; set; }
        public string BatchDescription { get; set; }
        public decimal? ProgramId { get; set; }
        public string ProgramNameA { get; set; }
        public string ProgramNameE { get; set; }
        public string DepartmentNumber { get; set; }
        public string DepartmentNameA { get; set; }
        public string DepartmentNameE { get; set; }
        public string FacultyNumber { get; set; }
        public string FacultyNameA { get; set; }
        public string FacultyNameE { get; set; }
        public long Semester { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectNameA { get; set; }
        public string SubjectNameE { get; set; }
        public long? SubjectHour { get; set; }
        public float? Degree { get; set; }
        public string SubjectGrade { get; set; }
        public string SubjectGradeA { get; set; }
        public string ReGrade { get; set; }
        public string ReGradeA { get; set; }
        public string ReReGrade { get; set; }
        public string ReReGradeA { get; set; }
        public bool? Register { get; set; }
        public decimal? Weight { get; set; }
        public short? SHours { get; set; }
        public float? SPoints { get; set; }
        public float? Gpa { get; set; }
        public short? CHours { get; set; }
        public float? CPoints { get; set; }
        public float? Cgpa { get; set; }
        public string Status { get; set; }
        public decimal? BatchGradeId { get; set; }
        public decimal? BatchSubjectId { get; set; }
    }
}
