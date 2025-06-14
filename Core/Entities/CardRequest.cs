using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CardRequest
    {
        public string Id { get; set; }
        public string Photo { get; set; }
        public string ServiceId { get; set; }
        public Service Service { get; set; }
        public string RequestStatusId { get; set; }
        public RequestStatus Status { get; set; }
        public Student Student { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
