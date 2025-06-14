using Core.Entities;
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class TicketDTO
    {
        public string TicketId { get; set; }
        public string? TicketTitle { get; set; }
        public string? TicketDescription { get; set; }
        public string? TicketResolution { get; set; }
        public int TicketStatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string APIUserId { get; set; }
        public APIUser Owner { get; set; }

    }
    public class CreateTicketDTO
    {
        [Required]
        public string TicketTitle { get; set; }
        [Required]
        public string TicketDescription { get; set; }
        [Required]
        public string userId { get; set; }
        
    }
}
