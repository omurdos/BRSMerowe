using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Program
    {
        public Program()
        {
            Students = new HashSet<Student>();
            Announcements = new HashSet<Announcement>();
        }

        public string ProgramNameA { get; set; }
        public string ProgramNameE { get; set; }
        public decimal ProgramId { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
