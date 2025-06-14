using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    public class TranscriptCertificatesController : Controller
    {
        private readonly ILogger<TranscriptCertificatesController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        public TranscriptCertificatesController(ILogger<TranscriptCertificatesController> logger,
         TSTDBContext dbContext,
          IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(int? page)
        {
            var result = await _dbContext.TranscriptRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status)
            .ToListAsync();
            var certificateRequests = _mapper.Map<List<CertificateRequestViewModel>>(result);

            TempData["Title"] = "Transcript Certificate Requests";
            return View(certificateRequests.ToPagedList(page ?? 1, 10));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                _logger.LogInformation("Edit method of Transcript Request Controller called with id: {id}", id);
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Transcript Request ID cannot be null or empty.");
                }
                var certificateRequest = await _dbContext.TranscriptRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status).FirstOrDefaultAsync(s => s.Id == id);
                if (certificateRequest == null)
                {
                    return NotFound();
                }
                var editCertificateRequestViewModel = _mapper.Map<EditCertificateRequestViewModel>(certificateRequest);
                return View(editCertificateRequestViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of Transcript Request Controller");
                return RedirectToAction("Index", "TranscriptCertificates");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Decision(string id, string decision)
        {
            try
            {
                _logger.LogInformation("Edit method of Transcript Request Controller called with id: {id} and decision: {decision}", id, decision);
                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(decision))
                {
                    return BadRequest("Certificate Request ID cannot be null or empty");
                }
                var certificateRequest = await _dbContext.TranscriptRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status).FirstOrDefaultAsync(s => s.Id == id);
                if (certificateRequest == null)
                {
                    return NotFound();
                }
                RequestStatus status = null;
                switch (decision)
                {
                    case "Approved":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Approved");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.TranscriptRequests.Update(certificateRequest);
                        await _dbContext.SaveChangesAsync();

                        break;
                    case "Printed":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Printed");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.TranscriptRequests.Update(certificateRequest);
                        await _dbContext.SaveChangesAsync();
                        break;
                    case "Recieved":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Recieved");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.TranscriptRequests.Update(certificateRequest);
                        await _dbContext.SaveChangesAsync();
                        break;
                    default:
                        return BadRequest("Invalid decision");
                }


                var editCertificateRequestViewModel = _mapper.Map<EditCertificateRequestViewModel>(certificateRequest);
                return View("Edit", editCertificateRequestViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of ServicesController");
                return RedirectToAction("Index", "CertificateReq");
            }
        }



    }
}
