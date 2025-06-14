using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        private TSTDBContext _context;
        public FacultiesController(TSTDBContext context)
        {
            _context = context;
        }


        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartmentsByFacultyNumber([FromQuery] string facultyNumber) {

            try
            {
                var departments = await _context.Departments.Where(d => d.FacultyNumber == facultyNumber).ToListAsync();
                return Ok(departments);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
