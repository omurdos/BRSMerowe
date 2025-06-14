﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Country 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public List<State> States { get; set; }
    }
}
