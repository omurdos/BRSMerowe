using AutoMapper;
using Core.Entities;
using Core.Models;
using Dashboard.ViewModel;
using Emgu.CV.Ocl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    [Authorize(policy: "StudentProfilesClaims")]
    public class StudentsProfilesController : Controller
    {

        private readonly ILogger<PushNotificationsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly FirebaseService _pushNotificationService;
        private readonly ImageUploadService _imageUploadService;
        private readonly ImageProcessingService _imageProcessingService;
        private readonly FacultyClaimsService _facultyClaimsService;

        private readonly UserManager<APIUser> _userManager;
        private readonly IWebHostEnvironment _env;
        // List<string> facultyNumbers = new List<string>();
        // List<Claim> facultiesClaims = new List<Claim>();


        public StudentsProfilesController(ILogger<PushNotificationsController> logger,

        TSTDBContext dbContext, IMapper mapper, FirebaseService pushNotificationService, ImageUploadService imageUploadService, ImageProcessingService imageProcessingService, UserManager<APIUser> userManager,
        FacultyClaimsService facultyClaimsService, IWebHostEnvironment env)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
            _imageUploadService = imageUploadService;
            _imageProcessingService = imageProcessingService;
            _userManager = userManager;
            _facultyClaimsService = facultyClaimsService;
            _env = env; 
        }


        public async Task<IActionResult> Index(int? page, string? facultyNumber, string? departmentNumber, int? batchId, int? programId, string studentNumber, string query)
        {
            try
            {
                List<Core.Entities.Student> students = [];


                _facultyClaimsService.LoadFaculties(this.User);




                //For single student
                if (query != null && facultyNumber == null || facultyNumber == "")
                {



                    // if (_facultyClaimsService.facultyNumbers.Contains(facultyNumber))
                    // {

                    students = await _dbContext.Students.Include(s => s.Department).ThenInclude(d => d.Faculty)
                                          .Where(s =>
                                          (s.StudentNameA.Contains(query) || s.StudentNameE.Contains(query) || s.StudentNumber.Contains(query) || s.Phone.Contains(query)) && s.IsActive == true && _facultyClaimsService.facultyNumbers.Contains(s.FacultyNumber))
                                          .ToListAsync();
                    await LoadLookups(facultyNumber, departmentNumber, batchId, programId);

                    // }



                    TempData["Title"] = "Students Profiles";
                    return View(students.ToPagedList(page ?? 1, 10));

                }


                //For All Students

                students = await _dbContext.Students.Include(s => s.Department).ThenInclude(d => d.Faculty)
             .Where(s => s.IsActive == true &&

               (batchId == null || batchId == 0 || s.BatchId == batchId) &&
                  (facultyNumber == null || facultyNumber == "" || s.FacultyNumber == facultyNumber) &&
                  _facultyClaimsService.facultyNumbers.Contains(s.FacultyNumber) &&
                  (programId == null || programId == 0 || s.ProgramId == programId) &&
                  (departmentNumber == null || departmentNumber == "" || s.DepartmentNumber == departmentNumber) &&
                  (studentNumber == null || studentNumber == "" || s.StudentNumber == studentNumber)


             )
    .OrderBy(s => s.StudentId) // 👈 Add this

             .ToListAsync();

                await LoadLookups(facultyNumber, departmentNumber, batchId, programId);

                TempData["Title"] = "Students Profiles";
                Console.Write(students);
                var pagedStudents = students.ToPagedList(page ?? 1, 10);
                return View(pagedStudents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching students profiles: {Message}", ex.Message);
                throw ex;
            }

        }

        public async Task<IActionResult> StudentResults(int? page)
        {
            try
            {
                _facultyClaimsService.LoadFaculties(this.User);

                var students = await _dbContext.Students.Include(s => s.Department).ThenInclude(d => d.Faculty)
                    .Where(s => s.Phone == "920101807").ToListAsync();
                TempData["Title"] = "Students Profiles";
                return View(students.ToPagedList(page ?? 1, 10));
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] string studentNumber, [FromQuery] string decision)
        {
            try
            {
                _facultyClaimsService.LoadFaculties(this.User);

                var student = await _dbContext.Students.Include(s => s.Department)
                    .ThenInclude(d => d.Faculty)
                    .Include(s => s.Batch)
                    .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);


                if (student == null)
                {
                    TempData["Title"] = "Students Profiles";
                    TempData["Message"] = "Student not found";
                    return RedirectToAction("Index", "StudentsProfiles");

                }

                if (!_facultyClaimsService.facultyNumbers.Contains(student.FacultyNumber))
                {

                    return RedirectToAction("Index", "StudentsProfiles");
                }


                if (decision == "approve")
                {
                    student.IsPersonalPhotoApproved = true;
                    student.CanEditPersonalPhoto = false;
                    _dbContext.Students.Update(student);
                    await _dbContext.SaveChangesAsync();
                    var editStudentViewModel = new EditStudentViewModel
                    {
                        StudentNumber = student.StudentNumber,
                        StudentName = student.StudentNameA,
                        Department = student.Department.DepartmentNameA,
                        Faculty = student.Department.Faculty.FacultyNameA,
                        PersonalPhoto = student.PersonalPhoto,
                        IsStudentCardBlocked = student.IsStudentCardBlocked,
                        Phone = student.Phone
                    };

                    var user = await _userManager.Users.Include(u => u.Student).Include(u => u.Devices).FirstOrDefaultAsync(u => u.Student.StudentNumber == student.StudentNumber);

                    var notificationSendResult = await _pushNotificationService.SendMulticastNotificationAsync(user.Devices.Select(d => d.FCMToken).ToList(), "الموافقة على الصورة الشخصية", "تم الموافقة على الصورة الشخصية الخاصة بك", "notification", _env.IsDevelopment());
                    var arabicNotificationSendResult = await _pushNotificationService.SendMulticastNotificationAsync(user.Devices.Select(d => d.FCMToken).ToList(), "Profile picture approved", "Your profile picture has been approved", "notification", _env.IsDevelopment());

                    _logger.LogInformation("Notification sent to user {UserId} with result: {Result}", user.Id, notificationSendResult.SuccessCount);

                    TempData["Title"] = "Students Profiles";
                    TempData["Message"] = "Profile picture approved successfully";
                    return View(editStudentViewModel);

                }


                if (decision == "reject")
                {
                    student.IsPersonalPhotoApproved = false;
                    student.CanEditPersonalPhoto = true;
                    _dbContext.Students.Update(student);
                    await _dbContext.SaveChangesAsync();
                    var editStudentViewModel = new EditStudentViewModel
                    {
                        StudentNumber = student.StudentNumber,
                        StudentName = student.StudentNameA,
                        Department = student.Department.DepartmentNameA,
                        Faculty = student.Department.Faculty.FacultyNameA,
                        PersonalPhoto = student.PersonalPhoto,
                        IsStudentCardBlocked = student.IsStudentCardBlocked,
                        Phone = student.Phone
                    };
                    var user = await _userManager.Users.Include(u => u.Student).Include(u => u.Devices).FirstOrDefaultAsync(u => u.Student.StudentNumber == student.StudentNumber);

                    var notificationSendResult = await _pushNotificationService.SendMulticastNotificationAsync(user.Devices.Select(d => d.FCMToken).ToList(), "Profile picture rejected", "Your profile picture has been rejected, please upload a valid photo", "notification", _env.IsDevelopment());
                    var arabicNotificationSendResult = await _pushNotificationService.SendMulticastNotificationAsync(user.Devices.Select(d => d.FCMToken).ToList(), "الصورة الشخصية مرفوضة", "تم رفض الصورة الشخصية المرفقة الرجاء ارفاق صورة مطابقة للمواصفات", "notification", _env.IsDevelopment());

                    _logger.LogInformation("Notification sent to user {UserId} with result: {Result}", user.Id, notificationSendResult.SuccessCount);

                    TempData["Title"] = "Students Profiles";
                    TempData["Message"] = "Profile picture rejected successfully";
                    return View(editStudentViewModel);

                }
                await LoadLookups(student.FacultyNumber, student.DepartmentNumber, null, null);

                if (student != null)
                {

                    var editStudentViewModel = new EditStudentViewModel
                    {
                        StudentNumber = student.StudentNumber,
                        StudentName = student.StudentNameA,
                        Department = student.Department.DepartmentNameA,
                        DepartmentId = student.Department.DepartmentNumber,
                        FacultyId = student.Department.Faculty.FacultyNumber,
                        Faculty = student.Department.Faculty.FacultyNameA,
                        BatchId = student.BatchId,
                        Batch = student.Batch.BatchDescription,
                        PersonalPhoto = student.PersonalPhoto,
                        Phone = student.Phone,
                        IsStudentCardBlocked = student.IsStudentCardBlocked,
                        IsActive = student.IsActive,
                    };

                    //await LoadLookups(null, null, null, null);







                    TempData["Title"] = "Students Profiles";
                    return View(editStudentViewModel);
                }

                return RedirectToAction("Index", "StudentsProfiles");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student profile for editing: {Message}", ex.Message);
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStudentViewModel editStudentViewModel)
        {
            try
            {
                _facultyClaimsService.LoadFaculties(this.User);


                if (ModelState.IsValid)
                {
                    var student = await _dbContext.Students.Include(s => s.Department)
                        .ThenInclude(d => d.Faculty)
                        .Include(s => s.Batch)
                        .FirstOrDefaultAsync(s => s.StudentNumber == editStudentViewModel.StudentNumber);


                    if (student == null)
                    {
                        TempData["Title"] = "Students Profiles";
                        TempData["Message"] = "Student not found";
                        return RedirectToAction("Index", "StudentsProfiles");

                    }

                    if (!_facultyClaimsService.facultyNumbers.Contains(student.FacultyNumber))
                    {

                        return RedirectToAction("Index", "StudentsProfiles");
                    }
                    await LoadLookups(student.FacultyNumber, student.DepartmentNumber, null, null);


                    if (student != null)
                    {


                        if (editStudentViewModel.PersonalPhotoFile != null)
                        {
                            var uploadedImage = await _imageUploadService.ConvertToBytes(editStudentViewModel.PersonalPhotoFile);
                            //Check if the file is meeting the requirements
                            if (!_imageProcessingService.IsValidImage(uploadedImage))
                            {
                                TempData["Title"] = "Students Profiles";
                                TempData["Message"] = "Invalid image";

                                return View(editStudentViewModel);

                            }

                            var fileUploadResult = await _imageUploadService.Upload(Convert.ToBase64String(uploadedImage), "profile");

                            if (fileUploadResult.Succeed)
                            {
                                var personalPhoto = fileUploadResult.FileName;
                                student.PersonalPhoto = personalPhoto;


                            }
                            else if (!fileUploadResult.Succeed)
                            {
                                TempData["Title"] = "Students Profiles";
                                TempData["Message"] = fileUploadResult.Message;

                                return View(editStudentViewModel);
                            }

                        }

                        student.IsStudentCardBlocked = editStudentViewModel.IsStudentCardBlocked;
                        student.FacultyNumber = editStudentViewModel.FacultyId;
                        student.DepartmentNumber = editStudentViewModel.DepartmentId;
                        student.BatchId = editStudentViewModel.BatchId;
                        student.IsActive = editStudentViewModel.IsActive;
                        if (editStudentViewModel.Phone != null) {
                            if (!editStudentViewModel.Phone.Equals(student.Phone))
                            {
                                student.Phone = editStudentViewModel.Phone;
                                var user = await _userManager.Users.Include(u => u.Student).FirstOrDefaultAsync(u => u.Student.StudentNumber == student.StudentNumber);
                                if (user != null)
                                {
                                    user.UserName = editStudentViewModel.Phone;
                                    user.NormalizedUserName = editStudentViewModel.Phone.ToUpper();
                                    user.PhoneNumber = editStudentViewModel.Phone;
                                    await _userManager.UpdateAsync(user);
                                }
                            }
                        }
                        
                        _dbContext.Students.Update(student);
                        await _dbContext.SaveChangesAsync();
                        TempData["Title"] = "Students Profiles";
                        return RedirectToAction("Index", "StudentsProfiles");

                    }

                    return View(editStudentViewModel);
                }
                TempData["Title"] = "Students Profiles";
                TempData["Message"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View(editStudentViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while editing student profile: {Message}", ex.Message);
                throw ex;
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteAPIUser([FromQuery] string studentNumber)
        {
            try
            {
                _facultyClaimsService.LoadFaculties(this.User);

                var user = await _userManager.Users.Include(u => u.Student)
                    .FirstOrDefaultAsync(u => u.Student.StudentNumber == studentNumber);

                if (user == null)
                {
                    TempData["Title"] = "Students Profiles";
                    TempData["Message"] = "Student not found";
                    return RedirectToAction("Index", "StudentsProfiles");

                }

                if (!_facultyClaimsService.facultyNumbers.Contains(user.Student.FacultyNumber))
                {

                    return RedirectToAction("Index", "StudentsProfiles");
                }



                //Handle user does not exist
                if (user == null)
                {
                    TempData["Title"] = "Students Profiles";
                    TempData["Message"] = "User does not exist";
                    return RedirectToAction("Index", "StudentsProfiles");
                }
                var userDevices = await _dbContext.Devices.Where(d => d.APIUser.Id == user.Id).ToListAsync();
                if (userDevices != null)
                {
                    _dbContext.Devices.RemoveRange(userDevices);
                }
                var student = await _dbContext.Students
                    .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
                student.IsERegistrationComplete = false;
                student.Phone = null;

                _dbContext.Update(student);
                var userAccountToDelete = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                await _userManager.DeleteAsync(userAccountToDelete);

                TempData["Title"] = "Students Profiles";
                TempData["Message"] = "User deleted successfully";
                return RedirectToAction("Index", "StudentsProfiles");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student profile for editing: {Message}", ex.Message);
                throw;
            }
        }

        private async Task LoadLookups(string? facultyNumber, string? departmentNumber, int? batchId, int? programId)
        {
            _facultyClaimsService.LoadFaculties(this.User);


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

            //var batches = await _dbContext.Batches.Where(b => batchId == null || batchId == 0 ? b. != batchId : b.BatchId == batchId).ToListAsync();
            //var programs = await _dbContext.Programs.Where(p => programId == null || programId == 0 ? p.ProgramId != programId : p.ProgramId == programId).ToListAsync();

            ViewBag.Faculties = faculties;
            ViewBag.Departments = departments;
            ViewBag.Batches = batches;
            ViewBag.Programs = programs;

        }

    }
}
