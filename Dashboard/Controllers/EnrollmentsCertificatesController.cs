using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    public class EnrollmentsCertificatesController : Controller
    {
        private readonly ILogger<EnrollmentsCertificatesController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        public EnrollmentsCertificatesController(ILogger<EnrollmentsCertificatesController> logger,
         TSTDBContext dbContext,
          IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(int? page)
        {
            var result = await _dbContext.EnrollmentRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status)
            .ToListAsync();
            var enrollmentCertificateRequests = _mapper.Map<List<CertificateRequestViewModel>>(result);

            TempData["Title"] = "Enrollment Certificates Requests";
            return View(enrollmentCertificateRequests.ToPagedList(page ?? 1, 10));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            try
            {
                _logger.LogInformation("Edit method of Enrollment Certificate called with id: {id}", Id);
                if (string.IsNullOrEmpty(Id))
                {
                    return BadRequest("Enrollment Certificate Id cannot be null or empty.");
                }
                var enrollmentCertificateRequests = await _dbContext.EnrollmentRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status).FirstOrDefaultAsync(s => s.Id == Id);
                if (enrollmentCertificateRequests == null)
                {
                    return NotFound();
                }
                var editCertificateRequestViewModel = _mapper.Map<EditCertificateRequestViewModel>(enrollmentCertificateRequests);
                return View(editCertificateRequestViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of Enrollment Certificate");
                return RedirectToAction("Index", "EnrollmentsCertificates");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Decision(string id, string decision)
        {
            try
            {
                _logger.LogInformation("Edit method of Enrollment Certificate Request Controller called with id: {id} and decision: {decision}", id, decision);
                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(decision))
                {
                    return BadRequest("Enrollment Certificate Request ID cannot be null or empty");
                }
                var enrollmentRequest = await _dbContext.EnrollmentRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status).FirstOrDefaultAsync(s => s.Id == id);
                if (enrollmentRequest == null)
                {
                    return NotFound();
                }
                RequestStatus status = null;
                switch (decision)
                {
                    case "Approved":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Approved");
                        enrollmentRequest.Status = status;
                        enrollmentRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.EnrollmentRequests.Update(enrollmentRequest);
                        await _dbContext.SaveChangesAsync();

                        break;
                    case "Printed":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Printed");
                        enrollmentRequest.Status = status;
                        enrollmentRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.EnrollmentRequests.Update(enrollmentRequest);
                        await _dbContext.SaveChangesAsync();
                        break;
                    case "Recieved":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Recieved");
                        enrollmentRequest.Status = status;
                        enrollmentRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.EnrollmentRequests.Update(enrollmentRequest);
                        await _dbContext.SaveChangesAsync();
                        break;
                    default:
                        return BadRequest("Invalid decision");
                }


                var editCertificateRequestViewModel = _mapper.Map<EditCertificateRequestViewModel>(enrollmentRequest);
                return View("Edit", editCertificateRequestViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of EnrollmentsCertificates");
                return RedirectToAction("Index", "EnrollmentsCertificates");
            }
        }


    }
}
