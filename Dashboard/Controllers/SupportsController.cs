using AutoMapper;
using Core.Entities;
using Core.Enums;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    [Authorize(Policy = "TicketManagementClaims")]
    public class SupportsController : Controller
    {
        private readonly ILogger<SupportsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly FirebaseService _pushNotificationService;


        public SupportsController(ILogger<SupportsController> logger, TSTDBContext dbContext, FirebaseService pushNotificationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _pushNotificationService = pushNotificationService;


        }
        public async Task<IActionResult> Index(int? page, string studentNumber)
        {
            try
            {
                List<Ticket> tickets = [];
                if (!string.IsNullOrEmpty(studentNumber))
                {
                    tickets = await _dbContext.Tickets.Include(t => t.Owner).ThenInclude(t => t.Student)
                   .Where(t => t.Owner.Student.StudentNumber == studentNumber).ToListAsync();
                }
                else
                {

                    tickets = await _dbContext.Tickets.Include(t => t.Owner).ThenInclude(t => t.Student)
                       .ToListAsync();
                }



                return View(tickets.ToPagedList(page ?? 1, 10));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var ticket = await _dbContext.Tickets.Include(t => t.Owner).ThenInclude(t => t.Student).FirstOrDefaultAsync(t => t.TicketId == id);
                if (ticket == null)
                {
                    return NotFound();
                }
                return View(ticket);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Ticket model)
        {
            try
            {
                var ticket = await _dbContext.Tickets.Include(t => t.Owner).ThenInclude(t => t.Student).FirstOrDefaultAsync(t => t.TicketId == model.TicketId);
                if (ticket == null)
                {
                    return NotFound();
                }
                ticket.TicketResolution = model.TicketResolution;
                ticket.TicketStatus = TicketStatus.Resolved;
                ticket.ResolvedOn = DateTime.Now;
                ticket.ResolvedBy = User.Identity.Name;
                _dbContext.Tickets.Update(ticket);
                _dbContext.SaveChanges();


                var device = await _dbContext.Devices.Where(d => d.APIUserId == ticket.APIUserId).FirstOrDefaultAsync();


                //Send notifications
                if (device.FCMToken != "" && device.FCMToken != null)
                {
                    var sendResult = await _pushNotificationService
                      .SendNotificationAsync(device.FCMToken,  $"Ticket Resolved"  , $"Your ticket {ticket.TicketTitle} has been resolved");
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





                return View(ticket);
            }
            catch (Exception ex)
            {

                throw ex;
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
