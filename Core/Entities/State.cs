using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class State
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string CountryId { get; set; }
        public Country Country { get; set; }
    }
}
