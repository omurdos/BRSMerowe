using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class APIUser : IdentityUser
    {
           public Student Student { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsProfileComplete { get; set; } = false;
        public Collection<Device> Devices { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [NotMapped]
        public List<string> RoleNames { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
