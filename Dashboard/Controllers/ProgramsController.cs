using AutoMapper;
using Core.Entities;
using X.PagedList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{

    public class ProgramsController : Controller
    {

        private readonly ILogger<ProgramsController> _logger;
        private readonly TSTDBContext _dbContext;

        public ProgramsController(ILogger<ProgramsController> logger,

        TSTDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }
        // API Method - Returns JSON
        [HttpGet]
        public async Task<IActionResult> GetPrograms([FromQuery] string FacultyNumber, [FromQuery] string DepartmentNumber, [FromQuery] int BatchId)
        {
            try
            {
                var programs = await _dbContext.Students
                .Where(s => s.FacultyNumber == FacultyNumber && s.DepartmentNumber == DepartmentNumber && s.BatchId == BatchId)
                .Join(_dbContext.Programs,
                    student => student.ProgramId,
                    program => program.ProgramId,
                    (student, program) => new
                    {
                        ProgramId = program.ProgramId,
                        ProgramNameA = program.ProgramNameA,
                        ProgramNameE = program.ProgramNameE,
                    })
                .Distinct()
                .ToListAsync();
                return Json(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching programs");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
