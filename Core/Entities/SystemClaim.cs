using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class SystemClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

}
