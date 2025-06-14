using API.DTOs;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.IO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementsController : ControllerBase
    {

        private readonly TSTDBContext _context;
        private readonly ILogger<AnnouncementsController> _logger;
        public AnnouncementsController(TSTDBContext context,  ILogger<AnnouncementsController> logger)
        {
            _context=context;
            _logger=logger;
        }

        // GET: api/<AnnouncementsController>
        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery]string studentNumber)
        {
            try
            {
                _logger.LogInformation("fetching all announcements requests for {@studentNumber}", studentNumber);
                if (string.IsNullOrEmpty(studentNumber))
                {
                    return BadRequest(new { message = "Student number is required" });
                }
                // Fetch the student to check if it exists
               var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
                if (student == null)
                {
                    _logger.LogWarning("Student not found: {StudentNumber}", studentNumber);
                    return NotFound(new { message = "Student not found" });
                }
                var result = await _context.Announcement
                    .Include(s => s.Faculty)
                    .Include(s => s.Department)
                    .Include(s => s.Batch)
                    .Include(s => s.Program)
                    .Where(s => (s.FacultyNumber == student.FacultyNumber || s.FacultyNumber == null)
                     && (s.DepartmentNumber == student.DepartmentNumber || s.DepartmentNumber == null) 
                     && (s.BatchId == student.BatchId || s.BatchId == null) 
                     && (s.ProgramId == student.ProgramId || s.ProgramId == null))
                     .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync();
                _logger.LogInformation("{@studentNumber} announcements requests are: {@requests}", studentNumber, result);
                return Ok(result);
            }
            catch (Exception ex){
               _logger.LogError("{@ex}", ex);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // GET api/<AnnouncementsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnnouncement(string id)
        {
            try
            {
                _logger.LogInformation("fetching announcement with id {@id}", id);
                var announcement = await _context.Announcement
                    .Include(s => s.Faculty)
                    .Include(s => s.Department)
                    .Include(s => s.Batch)
                    .Include(s => s.Program)
                    .FirstOrDefaultAsync(a => a.Id == id);
                if (announcement == null)
                {
                    _logger.LogWarning("Announcement not found: {Id}", id);
                    return NotFound(new { message = "Announcement not found" });
                }
                return Ok(announcement);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex} Failed with fetching announcment", ex);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
      
    }
}
