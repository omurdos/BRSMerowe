using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private TSTDBContext _context;
        private UserManager<APIUser> _userManager { get; set; }

        public StudentsController(TSTDBContext context, UserManager<APIUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }


  
        [HttpGet("search")]
        public async Task<IActionResult> SearchStudent([FromQuery] string studentIdentifier)
        {
            try
            {


                var device = await _context.Devices.Include(d => d.APIUser).ThenInclude(u => u.Student).ThenInclude(s => s.Department).ThenInclude(d => d.Faculty)
                    .Where(u => (u.APIUser.Student.Email == studentIdentifier ||u.APIUser.Student.Phone == studentIdentifier || u.APIUser.Student.StudentNumber == studentIdentifier))
                    .OrderByDescending(d  => d.CreatedAt)
                    .FirstOrDefaultAsync();

                //var student = await _userManager.Users.Include(u => u.Student)
                //    .Where(u => (u.Student.Email == studentIdentifier || u.Student.Phone == studentIdentifier || u.Student.StudentNumber == studentIdentifier) )
                //    .FirstOrDefaultAsync();
                return Ok(new { device.FCMToken, device.APIUser.Student  });
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
