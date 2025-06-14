using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.Data;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    [Authorize( Policy = "NotificationsClaims")]
    public class PushNotificationsController : Controller
    {
        private readonly ILogger<PushNotificationsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly FirebaseService _pushNotificationService;
        public PushNotificationsController(ILogger<PushNotificationsController> logger, TSTDBContext dbContext, IMapper mapper, FirebaseService pushNotificationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
        }
        // GET: PushNotificationsController
        public async Task<ActionResult> Index(int? page)
        {

            try
            {
                
                var result = await _dbContext.PushNotifications.Where(p => !p.IsDeleted).ToListAsync();
                var pushNotifications = _mapper.Map<List<PushNotificationViewModel>>(result);
                TempData["Title"] = "Notifications";
                return View(pushNotifications.ToPagedList(page ?? 1, 10));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching push notifications");
                return View(new List<PushNotificationViewModel>()); // Return an empty list or handle the error as needed
            }
        }
        

        // GET: PushNotificationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PushNotificationsController/Create
        public async Task<ActionResult> Create()
        {
            TempData["Title"] = "Send notification";
            var faculties = await _dbContext.Faculties.ToListAsync();
            var viewModel = new CreatePushNotificationViewModel()
            {
                Faculties = faculties
            };

            return View(viewModel);
        }
        public ViewResult Individual()
        {
            return View();
        }


        // POST: PushNotificationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Individual(CreateIndividualPushNotificationViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var pushNotification = _mapper.Map<PushNotification>(viewModel);
                    pushNotification.CreatedAt = DateTime.Now;
                    await _dbContext.PushNotifications.AddAsync(pushNotification);
                    var result = await _dbContext.SaveChangesAsync();
                    if (result > 0)
                    {


                        var device = await _dbContext.Devices.Include(d => d.APIUser).ThenInclude(u => u.Student).ThenInclude(s => s.Department).ThenInclude(d => d.Faculty)
                    .Where(u => (u.APIUser.Student.Email == viewModel.StudentIdentifier || u.APIUser.Student.Phone == viewModel.StudentIdentifier || u.APIUser.Student.StudentNumber == viewModel.StudentIdentifier))
                    .OrderByDescending(d => d.CreatedAt)
                    .FirstOrDefaultAsync();

                        //Send notifications
                        if (device.FCMToken != "" && device.FCMToken != null)
                        {
                            var sendResult = await _pushNotificationService
                              .SendNotificationAsync(device.FCMToken, viewModel.Title, viewModel.Message);
                            if (sendResult == "Success")
                            {
                                TempData["SuccessMessage"] = $"<strong>Success!</strong>, Notification was sent to {sendResult} Students successfully.";
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                return RedirectToAction(nameof(Index));
                            }

                        }

                    }
                    else
                    {
                        return View();

                    }


                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        // POST: PushNotificationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePushNotificationViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var pushNotification = _mapper.Map<PushNotification>(viewModel);
                    pushNotification.CreatedAt = DateTime.Now;
                    await _dbContext.PushNotifications.AddAsync(pushNotification);
                    var result = await _dbContext.SaveChangesAsync();
                    if (result > 0)
                    {


                        var tokens = await GetTokens(viewModel.SelectedDepartmentNumber, viewModel.SelectedFacultyNumber, 0, 0);

                        //Send notifications
                        if (tokens.Count > 0)
                        {
                            var sendResult = await _pushNotificationService
                              .SendMulticastNotificationAsync(tokens, viewModel.Title, viewModel.Message, "notifications");
                            if (sendResult.SuccessCount > 0)
                            {
                                TempData["SuccessMessage"] = $"<strong>Success!</strong>, Notification was sent to {sendResult} Students successfully.";
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                return RedirectToAction(nameof(Index));
                            }

                        }

                    }
                    else
                    {
                        return View();

                    }


                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        // GET: PushNotificationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PushNotificationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting notification with ID: {id}");
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogWarning("Notification ID is null or empty.");
                    TempData["Title"] = "Delete Notification";
                    TempData["Message"] = "Notification ID cannot be null or empty.";
                    return RedirectToAction("Index", "PushNotifications");
                }
                // Logic to delete the announcement by ID
                var notification = _dbContext.PushNotifications.Find(id);
                if (notification != null)
                {
                    notification.IsDeleted = true; // Soft delete
                    notification.ModifiedAt = DateTime.UtcNow;
                    _dbContext.PushNotifications.Update(notification);
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

                // // Get tokens for the students
                // var tokens = await _dbContext.Devices
                //     .Where(d => d.APIUser != null && studentIds.Contains(d.APIUser.Student.StudentNumber))
                //     .Select(d => d.FCMToken) // Assuming the token is stored in a "Token" column
                //     .ToListAsync();

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

                throw ex;
            }
        }

    }
}
