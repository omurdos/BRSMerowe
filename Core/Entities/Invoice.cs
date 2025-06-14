using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Invoice
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string PaymentId { get; set; }
        public Payment Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
