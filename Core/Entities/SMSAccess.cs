using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SMSAccess 
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public int SendCount { get; set; }
        public bool IsBlocked { get; set; }
        public int BlockCounts { get; set; }
        public DateTime LockedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
