using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Department
    {
        public Department()
        {
            Students = new HashSet<Student>();
            Announcements = new HashSet<Announcement>();
        }

        public string FacultyNumber { get; set; }
        public string DepartmentNumber { get; set; }
        public string DepartmentNameA { get; set; }
        public string DepartmentNameE { get; set; }
        public string DegreeOfferE { get; set; }
        public string DegreeOfferA { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
