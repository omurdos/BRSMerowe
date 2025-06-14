using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Batch
    {
        public Batch()
        {
            Students = new HashSet<Student>();
            Announcements = new HashSet<Announcement>();
        }

        public string BatchDescription { get; set; }
        public decimal BatchId { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
