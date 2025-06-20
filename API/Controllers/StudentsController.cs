using API.DTOs;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly TSTDBContext _context;
        private readonly IMapper _mapper;
        private readonly SMSService _smsService;
        private readonly UserManager<APIUser> _userManager;
        private readonly ILogger<StudentsController> _logger;
        private readonly FacultyClaimsService _facultyClaimsService;

        public StudentsController(TSTDBContext context, IMapper mapper, SMSService sMSService, UserManager<APIUser> userManager, ILogger<StudentsController> logger, FacultyClaimsService facultyClaimsService)
        {
            _context= context;
            _mapper= mapper;
            _smsService= sMSService;
            _userManager= userManager;
            _logger = logger;
            _facultyClaimsService = facultyClaimsService;

        }


        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents(
            string? facultyNumber,
            string? departmentNumber,
            int? batchId,
            int? programId,
            string? studentNumber,
            string? query,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                _facultyClaimsService.LoadFaculties(HttpContext.User);

                var studentsQuery = _context.Students
                    .Include(s => s.Department)
                        .ThenInclude(d => d.Faculty)
                    .Where(s => s.IsActive == true &&
                        (batchId == null || batchId == 0 || s.BatchId == batchId) &&
                        (facultyNumber == null || facultyNumber == "" || s.FacultyNumber == facultyNumber) &&
                        (programId == null || programId == 0 || s.ProgramId == programId) &&
                        (departmentNumber == null || departmentNumber == "" || s.DepartmentNumber == departmentNumber) &&
                        (studentNumber == null || studentNumber == "" || s.StudentNumber == studentNumber) &&
                        _facultyClaimsService.facultyNumbers.Contains(s.FacultyNumber)
                    );

                // Special case for search query
                if (!string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(facultyNumber))
                {
                    studentsQuery = studentsQuery.Where(s =>
                        s.StudentNameA.Contains(query) ||
                        s.StudentNameE.Contains(query) ||
                        s.StudentNumber.Contains(query) ||
                        s.Phone.Contains(query));
                }

                var totalCount = await studentsQuery.CountAsync();

                var students = await studentsQuery
                    .OrderBy(s => s.StudentNameA) // or any default ordering
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = new
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Students = students
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching students profiles: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while retrieving students.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentByFormNo([FromQuery] string AdmissionFormNoOrStudentNumber)
        {
            try
            {
                var student = await _context
                    .Students
                    .Include(s => s.Department)
                    .ThenInclude(d => d.Faculty)
                    .FirstOrDefaultAsync(s => s.AddmissionFormNo == AdmissionFormNoOrStudentNumber || s.StudentNumber == AdmissionFormNoOrStudentNumber);
                if (student != null)
                {
                    return Ok(student);
                }
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("Medical")]
        public async Task<IActionResult> GetStudentMedicalReport([FromQuery] string AdmissionFormNoOrStudentNumber)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => 
                    s.AddmissionFormNo == AdmissionFormNoOrStudentNumber 
                    || s.StudentNumber == AdmissionFormNoOrStudentNumber);
                if (student != null)
                {
                   
                        if ((student.IsMedicallyFit ?? false) && student.IsERegistrationComplete)
                        {
                            return Ok(new { student.IsMedicallyFit });
                        }

                        return Ok(new { IsMedicallyFit = false});




                }
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("profile-photo")]
        public async Task<IActionResult> GetProfilePicture([FromQuery]string studentNumber)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.Student)
                    .FirstOrDefaultAsync(u => u.Student.StudentNumber == studentNumber);

                if (user != null)
                {

                    if (user.ProfilePicture != null) {
                        
                        return Ok(new { resourceAt = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/images/profile/{user.ProfilePicture}" });
                    }

                    return Ok( new { resourceAt = "" });



                }
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("is-owed")]
        public async Task<IActionResult> GetIsOwed([FromQuery] string studentNumber)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(u => u.StudentNumber == studentNumber);

                if (student != null)
                {
                    return Ok(new { student.IsOwed });
                }

                else {

                    return BadRequest($"No student with number {studentNumber} was found");


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("id-photo")]
        public async Task<IActionResult> GetIdPicture([FromQuery] string studentNumber)
        {
            try
            {
                var cardRequest = await _context.CardRequests
                    .Include(c => c.Student)
                    .FirstOrDefaultAsync(c => c.Student.StudentNumber  == studentNumber);

                if (cardRequest != null)
                {

                    if (cardRequest.Photo != null)
                    {

                        return Ok(new { resourceAt = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/images/ids/{cardRequest.Photo}" });
                    }

                    return Ok(new { resourceAt = "" });

                }
                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }




        [Authorize(Roles = "Admin")]
        [HttpPut("Medical/reset")]
        public async Task<IActionResult> ResetStudentMedicalFitness([FromQuery] string AdmissionFormNoOrStudentNumber)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(
                    s => 
                    s.AddmissionFormNo == AdmissionFormNoOrStudentNumber 
                    || s.AddmissionFormNo == AdmissionFormNoOrStudentNumber
                    );
                if (student == null) return NotFound();
                student.IsMedicallyFit = null;
                _context.Students.Update(student);
                var affectedRows = await _context.SaveChangesAsync();
                if (affectedRows > 0) {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPut("Medical")]
        public async Task<IActionResult> PutStudentMedicalReport([FromQuery] string AdmissionFormNoOrStudentNumber, StudentMedicalDTO dto)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s =>
                        s.AddmissionFormNo == AdmissionFormNoOrStudentNumber
                        || s.StudentNumber == AdmissionFormNoOrStudentNumber
                        );
                if (student == null) return NotFound();
                student.IsMedicallyFit = IsMedicallyFit(dto.Ticket.ToUpper());
                _context.Students.Update(student);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    if (student.IsMedicallyFit != null)
                    {
                        if (student.IsMedicallyFit ?? false)
                        {
                            string studyFeesMessage = $"الرقم الجامعي {student.StudentNumber}"
      + "\n"
      + $"رسوم التسجيل {student.RegistrationFees}"
       + "\n"
      + $"رسوم التامين الصحي {1200}"
       + "\n"
      + $"رسوم الدراسة للسمستر  {(student.StudyFees/2)}"
      + "\n"
      + $"المجموع  {(student.StudyFees/2) + 1200 + student.RegistrationFees}"
      ;

                            var banksOutlets = "الرجاء استخدام رقمك الجامعي"
                                + $"{student.StudentNumber}"
                                + "\n"
                                + "لسداد الرسوم الدراسية في جميع فروع بنك النيل وبنك المزارع التجاري";

                           var isSent = await _smsService.SendFeesSMS(student.Phone, studyFeesMessage, "API");
                           



                            //_smsService.SendSMS(student.Phone, banksOutlets, "API");
                            //_smsService.SendFeesSMS(student.Phone, studyFeesMessage, "API");
                            //_smsService.SendFeesSMS(student.Phone, banksOutlets, "API");
                        }
                    }

                    return Ok(new { student.IsMedicallyFit });

                }

                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private readonly char[] _validTickets = { 'A', 'B', 'C', 'D', 'E', 'F' };
        private bool IsMedicallyFit(string ticket)
        {

            var ticketChars = ticket.ToCharArray();
            if (_validTickets.Contains(ticketChars[0]) && _validTickets.Contains(ticketChars[2]))
            {
                return true;
            }
            return false;
        }




        // POST api/<StudentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StudentsController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] string StudentNumber, [FromBody] StudentProfileDTO dto)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == StudentNumber);
                    if (student == null)
                    {
                        return NotFound();
                    }
                    _mapper.Map(dto, student);
                    //if (dto.Attachments.PersonalPhoto != null)
                    //{
                    //    var result = await _fileUploadService.Upload(dto.Attachments.PersonalPhoto, AttachmentType.PERSONAL_PICTURE);
                    //    if (result.Succeed)
                    //    {
                    //        student.Attachment.PersonalPhoto = result.FileName;
                    //    }
                    //}
                    //if (dto.Attachments.IdentityProof != null)
                    //{
                    //    var result = await _fileUploadService.Upload(dto.Attachments.IdentityProof, AttachmentType.IDENTITY);
                    //    if (result.Succeed)
                    //    {
                    //        student.Attachment.IdentityProof = result.FileName;
                    //    }
                    //}
                    //if (dto.Attachments.MedicalReport != null)
                    //{
                    //    var result = await _fileUploadService.Upload(dto.Attachments.PersonalPhoto, AttachmentType.MEDICAL_REPORT);
                    //    if (result.Succeed)
                    //    {
                    //        student.Attachment.MedicalReport = result.FileName;
                    //    }
                    //}

                    var guardian = await _context.Guardians.FirstOrDefaultAsync(g => g.StudentNumber == student.StudentNumber);
                    if (guardian == null)
                    {
                        guardian = dto.Guardian;
                        await _context.AddAsync(guardian);
                    }

                    _context.Students.Update(student);
                    var saveResult = await _context.SaveChangesAsync();
                    if (saveResult > 0)
                    {
                        return Ok(student);
                    }
                    return BadRequest();

                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
