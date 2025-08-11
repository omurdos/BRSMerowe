using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Dashboard.ViewModel
{
   public class StudentViewModel {
    public string StudentNumber { get; set; }
    public string StudentName { get; set; }
        public string DepartmentId { get; set; }
        public string Department { get; set; }
        public string FacultyId { get; set; }
        public string Faculty { get; set; }
        public decimal BatchId { get; set; }
        public string Batch { get; set; }
        public string PersonalPhoto { get; set; }
    public bool IsStudentCardBlocked { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
   }
   public class EditStudentViewModel : StudentViewModel
    {
        public IFormFile PersonalPhotoFile { get; set; }
    }
}

