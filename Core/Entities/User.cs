using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class User
    {
        public decimal UserTypeId { get; set; }
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string FacultyNumber { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
