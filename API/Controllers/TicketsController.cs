using API.DTOs;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {

        private readonly TSTDBContext _context;
        private readonly ILogger<TicketsController> _logger;
        public TicketsController(TSTDBContext context,  ILogger<TicketsController> logger)
        {
            _context=context;
            _logger=logger;
        }

        [HttpGet("user/{UserId}")]
        public async Task<IActionResult> Get(string UserId)
        {
            try
            {

                _logger.LogInformation("Fetching all tickets for user {@UserId}", UserId);
                var tickets = await _context.Tickets
                    .Include(t => t.Owner)
                    .Where(t => t.Owner.Id == UserId)
                    .ToListAsync();
                return Ok(tickets);
                
            }
            catch (Exception ex){
               _logger.LogError("{@ex}", ex);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostTicket(CreateTicketDTO dto)
        {
            try
            {
               _logger.LogInformation("Creating ticket {@dto}", dto);
                var ticket = new Ticket
                {
                    TicketId = Guid.NewGuid().ToString(),
                    TicketTitle = dto.TicketTitle,
                    TicketDescription = dto.TicketDescription,
                    TicketStatus = TicketStatus.Open,
                    APIUserId = dto.userId
                };
                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex} Failed with fetching announcment", ex);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(string id)
        {
            try
            {
              _logger.LogInformation("Fetching ticket {@id}", id);
                var ticket = await _context.Tickets
                    .Include(t => t.Owner)
                    .FirstOrDefaultAsync(t => t.TicketId == id);
                if (ticket == null)
                {
                    return NotFound(new { message = "Ticket not found" });
                }
                return Ok(ticket);
            
            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex} Failed with fetching announcment", ex);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
      
    }
}
