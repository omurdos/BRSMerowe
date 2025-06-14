using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly TSTDBContext _context;

        public ResultsController(TSTDBContext context)
        {
            _context = context;
        }

        // GET: api/<ResultsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ResultsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // GET api/<ResultsController>/5
        [HttpGet("Student")]
        public async Task<IActionResult> Get([FromQuery] string studentNumber, [FromQuery] long semester)
        {
            try
            {


                    var results = await (from st in _context.Students
                                    join s in _context.StudentSubjects on st.StudentNumber equals s.StudentNumber
                                    join sub in _context.Subjects on s.SubjectCodeId equals sub.SubjectCodeId
                                    where
                       
                         (studentNumber == null || studentNumber == "" || s.StudentNumber == studentNumber) && s.Semester == semester && sub.FacultyNumber == st.FacultyNumber && s.ViewYesNO == 1

                                    select new StudentSubject
                                    {
                                                          StudentNumber = st.StudentNumber,
                            SubjectCode = s.SubjectCode,
                            Semester = s.Semester,
                            SubjectHour = s.SubjectHour,
                            Weight = s.Weight,
                            Degree = s.Degree,
                            SubjectGrade = s.SubjectGrade,
                            SubjectGradeA = s.SubjectGradeA,
                            ReGrade = s.ReGrade,
                            ReReGrade = s.ReGradeA,
                            SubjectCodeNavigation = sub,
                            ViewYesNO = s.ViewYesNO,
                                    })
                                           .Distinct()
                                           .OrderBy(s => s.Semester)
                                                                                                             .ThenBy(s => s.SubjectCodeNavigation.SubjectNameE)

                                           .ToListAsync();


                





                var grades = await _context.Gradepoints
                    .FirstOrDefaultAsync(g => g.StudentNumber == studentNumber && g.Semester == semester);

                grades.Cgpa = (float)Math.Round(grades.Cgpa, 2);
                grades.Gpa = grades.Gpa.HasValue ? (float?)Math.Round((decimal)grades.Gpa.Value, 2) : null;
                return Ok(new { results, grades });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET api/<ResultsController>/5
        [HttpGet("Semesters")]
        public async Task<IActionResult> Get([FromQuery] string studentNumber)
        {
            try
            {
                var semesters = await _context.StudentSubjects
                    .Where(sb => sb.StudentNumber == studentNumber)
                   .Select(sb => sb.Semester).Distinct().ToListAsync();
                return Ok(semesters);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // POST api/<ResultsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ResultsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ResultsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
