using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly TSTDBContext _context;

        public SubjectsController(TSTDBContext context)
        {
            _context = context;
        }
        // GET: api/<SubjectsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return Ok(subjects);
        }

        [HttpGet("Students")]
        public async Task<IActionResult> StudentsSubjects()
        {
            try
            {
                //var students = await _context.Students.Include(s => s.Batch)
                //    .Include(s => s.StudentSubjects)
                //    .ThenInclude(ss => ss.SubjectCodeNavigation)
                //    .Include(s => s.Department)
                //    .Where(s => s.StudentNumber == "HI3-11-14-922186")
                //    .FirstOrDefaultAsync();

                var results = await _context.StudentSubjects
                   .Include(s => s.SubjectCodeNavigation)
                  .Where(s => s.StudentNumber == "HI3-11-14-922186")
                  .OrderByDescending(s => s.Semester)
                  .ToListAsync();



                var cpga = await _context.Gradepoints.FirstOrDefaultAsync(g => g.StudentNumber == "HI3-11-14-922186" && g.Semester == 1);
                return Ok(new { results, cpga });
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        [HttpGet("Result")]
        public async Task<IActionResult> Result()
        {
            try
            {
                var student = await _context.StudentSubjects
                   
                   // .Include(sb => sb.SubjectCodeNavigation)
                   .Where(s => s.StudentNumber == "HI3-11-14-922186")
                   .OrderByDescending(s => s.Semester)
                   .ToListAsync();


                var grades = await _context.Gradepoints
                    .FirstOrDefaultAsync(g => g.StudentNumber == "HI3-11-14-922186" && g.Semester == 1);
                return Ok(new { student, grades });
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // GET api/<SubjectsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SubjectsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SubjectsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SubjectsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
