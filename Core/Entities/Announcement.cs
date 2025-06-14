using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Announcement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDisplayed { get; set; }
        public string FacultyNumber { get; set; }
        public Faculty Faculty { get; set; }
        public string DepartmentNumber { get; set; }
        public Department Department { get; set; }
        public decimal? BatchId { get; set; }
        public Batch Batch { get; set; }
        public decimal? ProgramId { get; set; }
        public Program Program { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
