using System;
using System.Collections.Generic;
using Core.Enums;

namespace Core.Entities
{
    public class Ticket : AuditEntity
    {
        public string TicketId { get; set; }
        public string? TicketTitle { get; set; }
        public string? TicketDescription { get; set; }
        public string? TicketResolution { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string APIUserId { get; set; }
        public APIUser Owner { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string? ResolvedBy { get; set; }

    }
}
