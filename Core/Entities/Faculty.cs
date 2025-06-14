using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Faculty
    {
        public Faculty()
        {
            Departments = new HashSet<Department>();
            Announcements = new HashSet<Announcement>();
        }

        public string FacultyNumber { get; set; }
        public string FacultyNameE { get; set; }
        public string FacultyNameA { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
