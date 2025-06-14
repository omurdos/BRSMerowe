using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Student
    {
        public Student()
        {
            Registrations = new HashSet<Registration>();
            StudentSubjects = new HashSet<StudentSubject>();
        }

        public decimal StudentId { get; set; }
        public string StudentNumber { get; set; }
        public string StudentNameA { get; set; }
        public string StudentNameE { get; set; }
        public decimal BatchId { get; set; }
        public string FacultyNumber { get; set; }
        public string DepartmentNumber { get; set; }
        public decimal? ProgramId { get; set; }
        public string Addmissiontype { get; set; }
        public DateTime? AddmissionDate { get; set; }
        public string AddmissionFormNo { get; set; }
        public byte? FirstSemster { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string NationalityE { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string CertificateType { get; set; }
        //public byte[] StdPicture { get; set; }
        public string Specialization { get; set; }
        public DateTime? GraduationDate { get; set; }
        public string SpecializationE { get; set; }
        public int? Studentstatus { get; set; }
        public decimal? StudyFees { get; set; }
        public decimal? RegistrationFees { get; set; }
        public DateTime? Birthdate { get; set; }
        public string HomeLanguage { get; set; }
        public string Note { get; set; }
        public int? LastOfLists { get; set; }
        public decimal? OrederInResult { get; set; }
        public int? CurrencyNo { get; set; }
        public decimal? StudentPercent { get; set; }
        // public int? RegistrationType { get; set; }
        //Student's profile info
        public string ParentPhone { get; set; }
        public string HighSchoolName { get; set; }
        public string NationalNumber { get; set; }
        public bool? IsMedicallyFit { get; set; }
        public string Accomodation { get; set; }
        public string CountryId { get; set; }
        public Country Country { get; set; }
        public string StateId { get; set; }
        public State State { get; set; }
        public bool? IsPersonalPhotoApproved { get; set; }
        public bool? CanEditPersonalPhoto { get; set; }
        public bool IsStudentCardBlocked { get; set; }
        public string AdmissionTypeId { get; set; }
        public AdmissionType AdmissionType { get; set; }
        public Guardian Guardian { get; set; }
        public StudentAttachment Attachment { get; set; }
        public bool IsERegistrationComplete { get; set; }
        public bool IsActive { get; set; }
        public bool SendFeesSMS { get; set; }
        public string ReligionId { get; set; }
        public Religion Religion { get; set; }
        public string PersonalPhoto { get; set; }
        public bool IsOwed { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Department Department { get; set; }
        public virtual Program Program { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }

    }
}
