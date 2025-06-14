using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    [Authorize(Policy = "UserManagementClaims")]
    public class UsersController : Controller
    {
        private readonly ILogger<PushNotificationsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly UserManager<APIUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly FirebaseService _pushNotificationService;
        private readonly FacultyClaimsService _facultyClaimsService;
        public UsersController(ILogger<PushNotificationsController> logger, TSTDBContext dbContext, IMapper mapper, FirebaseService pushNotificationService, UserManager<APIUser> userManager, RoleManager<Role> roleManager, FacultyClaimsService facultyClaimsService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
            _userManager = userManager;
            _roleManager = roleManager;
            _facultyClaimsService = facultyClaimsService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            var result = await _userManager.Users.ToListAsync();
            foreach (var user in result)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = roles.ToList();
            }
            var users = _mapper.Map<List<UserViewModel>>(result.Where(r => !r.RoleNames.Contains("Student")).ToList());

            TempData["Title"] = "Users";
            return View(users.ToPagedList(page ?? 1, 10));
        }
        private async Task<List<APIUser>> GetUsersAsync(string? query, string? facultyNumber, string? departmentNumber, int? batchId, int? programId)
        {

           _facultyClaimsService.LoadFaculties(User);

            var usersQuery = _userManager.Users
                .Include(u => u.Student)
                    .ThenInclude(s => s.Department)
                        .ThenInclude(d => d.Faculty)
                .Include(u => u.Devices)
                .Where( u => _facultyClaimsService.facultyNumbers.Contains(u.Student.FacultyNumber))
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                usersQuery = usersQuery.Where(u =>
                    (u.Student.StudentNumber.Contains(query) ||
                    u.PhoneNumber.Contains(query) ) && _facultyClaimsService.facultyNumbers.Contains(u.Student.FacultyNumber));
            }

            if (facultyNumber != null && facultyNumber != "")
            {
                usersQuery = usersQuery.Where(u =>

                  (batchId == null || batchId == 0 || u.Student.BatchId == batchId) &&
                         (_facultyClaimsService.facultyNumbers.Contains(u.Student.FacultyNumber) || u.Student.FacultyNumber == facultyNumber) && 
                         (programId == null || programId == 0 || u.Student.ProgramId == programId) &&
                         (departmentNumber == null || departmentNumber == "" || u.Student.DepartmentNumber == departmentNumber));
            }

            var result = await usersQuery.ToListAsync();

            foreach (var user in result)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = roles.ToList();
            }

            return result.Where(u => u.RoleNames.Contains("Student")).ToList();
        }
        [Authorize(policy: "StudentsMobilesClaims")]
        public async Task<IActionResult> StudentsUsers(string query, int? page, string? facultyNumber, string? departmentNumber, int? batchId, int? programId)
        {
            var users = await GetUsersAsync(query, facultyNumber, departmentNumber, batchId, programId);

            await LoadLookups(facultyNumber, departmentNumber, batchId, programId);
            TempData["Title"] = "Users";
            return View(users.ToPagedList(page ?? 1, 10));
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.UserRole = userRole;
            ViewBag.Roles = roles;
            EditUserViewModel viewModel =_mapper.Map<EditUserViewModel>(user);
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(viewModel.Id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    _mapper.Map(viewModel, user);
                    
                    IdentityResult addToRoleResult = null;
                    var userRoles = _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles.Result)
                    {
                        if (role == viewModel.Role.ToUpper())
                        {
                        }
                        else {
                            await _userManager.RemoveFromRoleAsync(user, role);
                            addToRoleResult = await _userManager.AddToRoleAsync(user, viewModel.Role.ToUpper());

                        }


                    }

                    if (!string.IsNullOrEmpty(viewModel.Password) && !string.IsNullOrEmpty(viewModel.ConfirmPassword)) {

                        if (viewModel.Password.Equals(viewModel.ConfirmPassword)) {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, viewModel.Password);
                            if (!resetPasswordResult.Succeeded)
                            {
                                foreach (var error in resetPasswordResult.Errors)
                                {
                                    _logger.LogInformation(error.Description);
                                }
                            }
                            else { 
                            return RedirectToAction("Edit", "Users", new { Id = user.Id });

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                        }

                    }

                    var createUserResult = await _userManager.UpdateAsync(user);

                    if (createUserResult.Succeeded && addToRoleResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        foreach (var error in createUserResult.Errors)
                        {
                            _logger.LogInformation(error.Description);
                        }
                        return RedirectToAction("Create", "Users");
                    }
                }
                else
                {

                    var roles = await _roleManager.Roles.ToListAsync();
                    ViewBag.Roles = roles;
                    return View();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel viewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<APIUser>(viewModel);
                    var createUserResult = await _userManager.CreateAsync(user, viewModel.Password);
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, viewModel.Role.ToUpper());

                    if (createUserResult.Succeeded && addToRoleResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        foreach (var error in createUserResult.Errors)
                        {
                            _logger.LogInformation(error.Description);
                        }
                        return RedirectToAction("Create", "Users");
                    }
                }
                else
                {

                    var roles = await _roleManager.Roles.ToListAsync();
                    ViewBag.Roles = roles;
                    return View();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task LoadLookups(string? facultyNumber, string? departmentNumber, int? batchId, int? programId)
        {

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
