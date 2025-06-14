using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Models;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {

        private readonly UserManager<APIUser> _userManager;
        private readonly ImageUploadService _imageUploadService;
        private readonly TSTDBContext _context;
        private readonly IMapper _mapper;
        private readonly SMSService _smsService;
        private readonly ImageProcessingService _imageProcessingService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<APIUser> userManager, ImageUploadService imageUploadService, TSTDBContext context, IMapper mapper, SMSService smsService, ImageProcessingService imageProcessingService, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _imageUploadService = imageUploadService;
            _context = context;
            _mapper = mapper;
            _smsService = smsService;
            _imageProcessingService = imageProcessingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var MedicalView = new APIUser
                {
                    UserName = "912345678",
                    NormalizedUserName = "912345678",
                };
                var MedicalReset = new APIUser
                {
                    UserName = "912345671",
                    NormalizedUserName = "912345671",
                };

                var createResult = await _userManager.CreateAsync(MedicalView, "684514");
                var createMedicalReset = await _userManager.CreateAsync(MedicalReset, "985468");
                if (createResult.Succeeded && createMedicalReset.Succeeded)
                {
                    await _userManager.AddToRoleAsync(MedicalView, "Admin");
                    await _userManager.AddToRoleAsync(MedicalReset, "Admin");
                }
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }



        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                return Ok(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}/profile")]
        [Authorize]
        public async Task<IActionResult> PutProfilePicture([FromBody] ProfilePictureDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == dto.StudentNumber);
                    if (student == null)
                    {
                        return NotFound(new { message = $"No student with number {student.StudentNumber}" });
                    }
                    else
                    {

                        if (_imageProcessingService.IsValidImage(Convert.FromBase64String(dto.Image)))
                        {

                            var result = await _imageUploadService.Upload(dto.Image, "profile");
                            if (result.Succeed)
                            {
                                student.PersonalPhoto = result.FileName;
                                _context.Update(student);
                                var saveChangesResult = await _context.SaveChangesAsync();
                                if (saveChangesResult > 0)
                                {
                                    return Ok(new { ImageValidationPassed = true, result.FileName, message = "File uploaded successfully" });
                                }
                                else
                                {
                                    return BadRequest("Failed to update students Personal Photo");
                                }
                            }
                            else
                            {
                                return BadRequest(new { message = "Failed to upload your profile picture" });
                            }

                        }
                        else
                        {

                            return BadRequest(new { ImageValidationPassed = false, message = "The uploaded image doesn't meet our requirements" });

                        }


                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(string id, [FromBody] StudentProfileDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsFirstUpdate = true;

                    var user = await _userManager.Users
                        .Include(u => u.Student)
                        .ThenInclude(s => s.Guardian).FirstOrDefaultAsync(u => u.Id == id);

                    if (user == null)
                    {
                        return NotFound(new { message = "User not found" });
                    }

                    else
                    {
                        if (user.Student.IsERegistrationComplete)
                        {
                            IsFirstUpdate = false;
                        }
                        _mapper.Map(dto, user.Student);
                        user.Student.Phone = user.PhoneNumber;
                        user.Student.IsERegistrationComplete = true;
                        user.Student.Address = "";
                        _context.Students.Update(user.Student);
                        var updateResult = await _context.SaveChangesAsync();
                        if (updateResult > 0)
                        {
                            user.IsProfileComplete = true;
                            var userupdate = await _userManager.UpdateAsync(user);
                            if (userupdate.Succeeded)
                            {
                                if (IsFirstUpdate && user.Student.FirstSemster == 1)
                                {
                                    var message = "تم إكمال بياناتك بنجاح الرجاء التوجه لاكمال اجراءات الفحص الطبي";
                                    var isSent = await _smsService.Message(user.PhoneNumber, message, "API");

                                }
                                else if (IsFirstUpdate && user.Student.FirstSemster != 1 && user.Student.SendFeesSMS)
                                {
                                    var student = user.Student;
                                    string studyFeesMessage = $"الرقم الجامعي {student.StudentNumber}"
                                    + "\n"
      + $"رسوم التسجيل {student.RegistrationFees}"
       + "\n"
                                    + $"رسوم التامين الصحي {1200}"
                                    + "\n"
                                    + $"رسوم الدراسة للسمستر  {(student.StudyFees / 2)}"
                                    + "\n"
      + $"المجموع  {(student.StudyFees / 2) + 1200 + student.RegistrationFees}"
      ;

                                    var isSent = await _smsService.SendFeesSMS(student.Phone, studyFeesMessage, "API");
                                }

                                var updatedUser = await _userManager
                        .Users
                        .Include(u => u.Student)
                        .ThenInclude(u => u.Batch)
                         .Include(u => u.Student)
                        .ThenInclude(u => u.Department)
                         .ThenInclude(u => u.Faculty)
                        .Include(u => u.Student)
                        .ThenInclude(u => u.Guardian)
                        .FirstOrDefaultAsync(u => u.Id == id);
                                return Ok(updatedUser);
                            }
                            return BadRequest();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error updating user profile: {Message}", ex.Message);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

    }
}
