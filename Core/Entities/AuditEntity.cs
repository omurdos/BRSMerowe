using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class AuditEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
  
}
