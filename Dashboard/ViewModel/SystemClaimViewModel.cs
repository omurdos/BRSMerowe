using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Dashboard.ViewModel
{
   public class SystemClaimViewModel {
           public int Id { get; set; }
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSelected { get; set; }
   }
  
}

