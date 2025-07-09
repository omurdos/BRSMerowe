using AutoMapper;
using Core.Entities;
using X.PagedList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.Reflection.Metadata.Ecma335;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Identity;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Authorization;
using Emgu.CV.Ocl;

namespace Dashboard.Controllers
{
    [Authorize(policy: "AnnouncementsClaims")]
    public class AnnouncementsController : Controller
    {

        private readonly ILogger<AnnouncementsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly FirebaseService _pushNotificationService;
        private readonly ImageUploadService _imageUploadService;
        private readonly ImageProcessingService _imageProcessingService;

        private readonly UserManager<APIUser> _userManager;
        private readonly IWebHostEnvironment _env;


        public AnnouncementsController(ILogger<AnnouncementsController> logger,

        TSTDBContext dbContext, IMapper mapper, FirebaseService pushNotificationService, ImageUploadService imageUploadService, ImageProcessingService imageProcessingService, UserManager<APIUser> userManager, IWebHostEnvironment env)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
            _imageUploadService = imageUploadService;
            _imageProcessingService = imageProcessingService;
            _userManager = userManager;
            _env = env;

        }

        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                var announcements = await _dbContext.Announcement.Include(a => a.Faculty).Include(a => a.Department).Include(a => a.Batch).Include(a => a.Program)
                .Where(a => !a.IsDeleted)
                   .ToListAsync();
                TempData["Title"] = "Announcements";
                return View(announcements.ToPagedList(page ?? 1, 10));
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                //Faculty,  department,  batch and programme at last
                var faculties = await _dbContext.Faculties.ToListAsync();
                var departments = await _dbContext.Departments.ToListAsync();

                var announcementViewModel = new AnnouncementViewModel
                {
                    Faculties = faculties,
                    Departments = departments,
                };
                TempData["Title"] = "Create Announcement";
                return View(announcementViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating announcement view model.");
                throw;
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var announcement = _mapper.Map<Announcement>(viewModel);
                    announcement.CreatedAt = DateTime.UtcNow;
                    announcement.ModifiedAt = DateTime.UtcNow;
                    announcement.CreatedBy = User.Identity.Name;
                    announcement.ModifiedBy = User.Identity.Name;
                    if (viewModel.IsDisplayed)
                    {
                        var tokens = await GetTokens(viewModel.DepartmentNumber, viewModel.FacultyNumber, viewModel.BatchId ?? 0, viewModel.ProgramId ?? 0);
                        await _pushNotificationService.SendMulticastNotificationAsync(tokens, announcement.Title, announcement.Description, "announcements", _env.IsDevelopment() );
                    }
                    await _dbContext.Announcement.AddAsync(announcement);
                    await _dbContext.SaveChangesAsync();
                    TempData["Title"] = "Create Announcement";
                    TempData["Message"] = "Announcement created successfully";
                    return RedirectToAction("Index", "Announcements");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating announcement view model.");
                    throw;

                }
            }
            return RedirectToAction("Create", "Announcements");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var announcement = await _dbContext.Announcement.Include(a => a.Faculty).Include(a => a.Department).Include(a => a.Batch).Include(a => a.Program)
                    .FirstOrDefaultAsync(a => a.Id == id);
                if (announcement == null)
                {
                    TempData["Title"] = "Edit Announcement";
                    TempData["Message"] = "Announcement not found";
                    return RedirectToAction("Index", "Announcements");
                }
                var faculties = await _dbContext.Faculties.ToListAsync();
                var departments = await _dbContext.Departments.Where(d => d.FacultyNumber == announcement.FacultyNumber).ToListAsync();
                var batches = await _dbContext.Students
                .Where(s => s.FacultyNumber == announcement.FacultyNumber && s.DepartmentNumber == announcement.DepartmentNumber)
                .Join(_dbContext.Batches,
                    student => student.BatchId,
                    batch => batch.BatchId,
                    (student, batch) => new Batch
                    {
                        BatchId = batch.BatchId,
                        BatchDescription = batch.BatchDescription,
                    })
                .Distinct()
                .ToListAsync();
                var programs = await _dbContext.Students
                .Where(s => s.FacultyNumber == announcement.FacultyNumber && s.DepartmentNumber == announcement.DepartmentNumber && s.BatchId == announcement.BatchId)
                .Join(_dbContext.Programs,
                    student => student.ProgramId,
                    program => program.ProgramId,
                    (student, program) => new Core.Entities.Program()
                    {
                        ProgramId = program.ProgramId,
                        ProgramNameA = program.ProgramNameA,
                        ProgramNameE = program.ProgramNameE,
                    })
                .Distinct()
                .ToListAsync();
                var announcementViewModel = _mapper.Map<EditAnnouncementViewModel>(announcement);
                announcementViewModel.Faculties = faculties;
                announcementViewModel.Departments = departments;
                announcementViewModel.Batches = batches;
                announcementViewModel.Programs = programs;
                TempData["Title"] = "Edit Announcement";
                return View(announcementViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating announcement view model.");
                throw;
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAnnouncementViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var announcement = _mapper.Map<Announcement>(viewModel);
                    announcement.CreatedAt = DateTime.UtcNow;
                    announcement.ModifiedAt = DateTime.UtcNow;
                    announcement.CreatedBy = User.Identity.Name;
                    announcement.ModifiedBy = User.Identity.Name;
                    if (viewModel.IsDisplayed)
                    {
                        var tokens = await GetTokens(viewModel.DepartmentNumber, viewModel.FacultyNumber, viewModel.BatchId ?? 0, viewModel.ProgramId ?? 0);
                        await _pushNotificationService.SendMulticastNotificationAsync(tokens, announcement.Title, announcement.Description, "announcements", _env.IsDevelopment());
                    }

                    _dbContext.Announcement.Update(announcement);
                    await _dbContext.SaveChangesAsync();
                    TempData["Title"] = "Announcements";
                    TempData["Message"] = "Announcement updated successfully";
                    return RedirectToAction("Index", "Announcements");

                }
                return RedirectToAction("Edit", "Announcements", new { id = viewModel.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating announcement view model.");
                throw;
            }
        }


        private async Task<List<string>> GetTokens(string SelectedDepartmentNumber, string SelectedFacultyNumber, decimal selectedBatchId, decimal selectedProgramId)
        {
            try
            {
                var query = _dbContext.Students
                                    .Include(s => s.Department)
                                    .ThenInclude(d => d.Faculty)
                                    .AsQueryable();

                // Apply filters only when needed
                if (!string.IsNullOrEmpty(SelectedDepartmentNumber))
                {
                    query = query.Where(s => s.DepartmentNumber == SelectedDepartmentNumber);
                }

                if (!string.IsNullOrEmpty(SelectedFacultyNumber))
                {
                    query = query.Where(s => s.FacultyNumber == SelectedFacultyNumber);
                }

                if (selectedBatchId != 0)
                {
                    query = query.Where(s => s.BatchId == selectedBatchId);
                }
                if (selectedProgramId != 0)
                {
                    query = query.Where(s => s.ProgramId == selectedProgramId);
                }
                var students = await query.ToListAsync();


                // Extract Student IDs
                var studentIds = students.Select(s => s.StudentNumber).ToList();

                var tokens = await _dbContext.Devices
                    .Include(d => d.APIUser)
                    .ThenInclude(u => u.Student)
                    .Where(d => d.APIUser != null && studentIds.Contains(d.APIUser.Student.StudentNumber))
                    .Select(d => d.FCMToken)
                    .Distinct()  // Add this if you might have duplicate tokens
                    .ToListAsync();
                return tokens;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tokens for announcements.");
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting announcement with ID: {id}");
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogWarning("Announcement ID is null or empty.");
                    TempData["Title"] = "Delete Announcement";
                    TempData["Message"] = "Announcement ID cannot be null or empty.";
                    return RedirectToAction("Index", "Announcements");
                }
                // Logic to delete the announcement by ID
                var announcement = _dbContext.Announcement.Find(id);
                if (announcement != null)
                {
                    announcement.IsDeleted = true; // Soft delete
                    announcement.ModifiedAt = DateTime.UtcNow;
                    announcement.ModifiedBy = User.Identity.Name;
                    _dbContext.Announcement.Update(announcement);
                    _dbContext.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting announcement.");
                TempData["Title"] = "Delete Announcement";
                TempData["Message"] = "Error occurred while deleting announcement.";
                return RedirectToAction("Index", "Announcements");
            }
        }


    }
}
