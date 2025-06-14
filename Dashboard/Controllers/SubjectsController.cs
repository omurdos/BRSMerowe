using System.Security.Claims;
using AutoMapper;
using Core.Entities;
using Core.Models;
using Dashboard.ViewModel;
using Emgu.CV.Ocl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    [Authorize(policy: "SubjectManagementClaims")]
    public class SubjectsController : Controller
    {
        private readonly ILogger<SubjectsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        List<string> facultyNumbers = new List<string>();
        List<Claim> facultiesClaims = new List<Claim>();


        public SubjectsController(ILogger<SubjectsController> logger, TSTDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            
        }


        public async Task<IActionResult> Index(int? page, string? facultyNumber, string? departmentNumber, int? batchId, int? programId, string studentNumber)
        {
            try
            {
                facultiesClaims = User.Claims.Where(c => c.Type == "Faculty").ToList();

            if (facultiesClaims.Count > 0)
            {
                foreach (var claim in facultiesClaims)
                {
                    facultyNumbers.Add(claim.Value.Split("-")[0]);
                }
            }

                List<StudentSubject> result = [];
                if (facultyNumber != null && facultyNumber != "")
                {

                    result = await (from st in _dbContext.Students
                                    join s in _dbContext.StudentSubjects on st.StudentNumber equals s.StudentNumber
                                    join sub in _dbContext.Subjects on s.SubjectCodeId equals sub.SubjectCodeId
                                    where
                         (batchId == null || batchId == 0 || s.BatchId == batchId) &&
                         (sub.FacultyNumber == facultyNumber) && facultyNumbers.Contains(facultyNumber) &&
                         (programId == null || programId == 0 || sub.ProgramId == programId) &&
                         (departmentNumber == null || departmentNumber == "" || sub.DepartmentNumber == departmentNumber) &&
                         (studentNumber == null || studentNumber == "" || s.StudentNumber == studentNumber)

                                    select new StudentSubject
                                    {
                                        SubjectCodeId = sub.SubjectCodeId,
                                        SubjectCode = s.SubjectCode,
                                        Semester = s.Semester,
                                        BatchId = st.BatchId,
                                        SubjectCodeNavigation = sub,
                                        ViewYesNO = s.ViewYesNO,
                                    })
                       .Distinct()
                                                                  .OrderBy(s => s.Semester)
                                                                  .ThenBy(s => s.SubjectCodeNavigation.SubjectNameE)

                       .ToListAsync();
                }

                if (studentNumber != null && studentNumber != "")
                {

                    var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);

                    result = await (from st in _dbContext.Students
                                    join s in _dbContext.StudentSubjects on st.StudentNumber equals s.StudentNumber
                                    join sub in _dbContext.Subjects on s.SubjectCodeId equals sub.SubjectCodeId
                                    where
                                        (sub.FacultyNumber == student.FacultyNumber) &&
                                        (s.StudentNumber == studentNumber) && facultyNumbers.Contains(sub.FacultyNumber)
                                    select new StudentSubject
                                    {
                                        SubjectCodeId = sub.SubjectCodeId,
                                        SubjectCode = s.SubjectCode,
                                        SubjectGrade = s.SubjectGrade,
                                        Semester = s.Semester,
                                        BatchId = st.BatchId,
                                        SubjectCodeNavigation = sub,
                                        ViewYesNO = s.ViewYesNO,
                                    })
                  .Distinct()
                  .OrderBy(s => s.Semester)
                  .ThenBy(s => s.SubjectCodeNavigation.SubjectNameE)
                  .ToListAsync();

                }


                var faculties = await _dbContext.Faculties.ToListAsync();
                var departments = await _dbContext.Departments.Where(d => facultyNumber == null ? d.FacultyNumber != facultyNumber : d.FacultyNumber == facultyNumber).ToListAsync();

                var batches = await _dbContext.Students
                .Where(s => s.FacultyNumber == facultyNumber && s.DepartmentNumber == departmentNumber)
                .Join(_dbContext.Batches,
                    student => student.BatchId,
                    batch => batch.BatchId,
                    (student, batch) => new Core.Entities.Batch
                    {
                        BatchId = batch.BatchId,
                        BatchDescription = batch.BatchDescription,
                    })
                .Distinct()
                .ToListAsync();
                var programs = await _dbContext.Students
               .Where(s => s.FacultyNumber == facultyNumber && s.DepartmentNumber == departmentNumber && s.BatchId == batchId)
               .Join(_dbContext.Programs,
                   student => student.ProgramId,
                   program => program.ProgramId,
                   (student, program) => new Core.Entities.Program
                   {
                       ProgramId = program.ProgramId,
                       ProgramNameA = program.ProgramNameA,
                       ProgramNameE = program.ProgramNameE,
                   })
               .Distinct()
               .ToListAsync();

                ViewBag.Faculties = faculties;
                ViewBag.Departments = departments;
                ViewBag.Batches = batches;
                ViewBag.Programs = programs;


                var subjects = _mapper.Map<List<StudentSubjectViewModel>>(result);

                return View(subjects.ToPagedList(page ?? 1, 10));

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while retrieving subjects.");
                throw;

            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? page, string facultyNumber, string departmentNumber, int? batchId, int? programId, int? enabled, string studentNumber)
        {
            try
            {



                if (id == 0)
                {
                    return NotFound();
                }
                var query = "";
                var facultyNumberList = string.Join(",", facultyNumbers.Select(fn => fn.ToString()));

                if (!string.IsNullOrEmpty(studentNumber) && string.IsNullOrEmpty(facultyNumber))
                {
                    query = $"UPDATE s SET s.ViewYesNO = {enabled} FROM Students st INNER JOIN StudentSubjects s ON st.StudentNumber = s.StudentNumber INNER JOIN Subjects sub ON s.SubjectCode = sub.SubjectCode WHERE s.SubjectCodeid = {id} AND st.studentNumber = '{studentNumber}' and st.facultyNumber in ({facultyNumberList});";


                }
                else
                {
                    query = $"UPDATE s SET s.ViewYesNO = {enabled} FROM Students st INNER JOIN StudentSubjects s ON st.StudentNumber = s.StudentNumber INNER JOIN Subjects sub ON s.SubjectCode = sub.SubjectCode WHERE  st.BatchID = {batchId} AND  st.FacultyNumber = {facultyNumber} AND  st.ProgramID = {programId} AND  st.DepartmentNumber = {departmentNumber} AND s.SubjectCodeid = {id} and st.facultyNumber in ({facultyNumberList});";

                }


                var result = await _dbContext.Database.ExecuteSqlRawAsync(query);

                if (result > 0)
                {
                    TempData["Success"] = "Subject updated successfully.";
                    Console.WriteLine("Update successful!");
                }
                else
                {
                    TempData["Error"] = "Failed to update subject.";

                    Console.WriteLine("No rows matched the criteria.");
                }

                return RedirectToAction("Index", new { page, facultyNumber, departmentNumber, batchId, programId, studentNumber });


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while retrieving subjects.");
                throw;

            }
        }

    }
}
