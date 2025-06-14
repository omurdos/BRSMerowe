using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public decimal UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
