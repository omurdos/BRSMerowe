using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class OTPCode
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
