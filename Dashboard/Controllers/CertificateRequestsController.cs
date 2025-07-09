using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    public class CertificateRequestsController : Controller
    {
        private readonly ILogger<CertificateRequestsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly FirebaseService _firebaseService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CertificateRequestsController(ILogger<CertificateRequestsController> logger,
         TSTDBContext dbContext,
          IMapper mapper,
          FirebaseService firebaseService,
          IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _firebaseService = firebaseService;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> Index(int? page)
        {
            var result = await _dbContext.CertificateRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status)
            .Include(c => c.Payment)
            .ToListAsync();
            var certificateRequests = _mapper.Map<List<CertificateRequestViewModel>>(result);

            TempData["Title"] = "Certificate Requests";
            return View(certificateRequests.ToPagedList(page ?? 1, 10));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                _logger.LogInformation("Edit method of ServicesController called with id: {id}", id);
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Service ID cannot be null or empty.");
                }
                var certificateRequest = await _dbContext.CertificateRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status)
            .Include(c => c.Payment)
            .FirstOrDefaultAsync(s => s.Id == id);
                if (certificateRequest == null)
                {
                    return NotFound();
                }
                var editCertificateRequestViewModel = _mapper.Map<EditCertificateRequestViewModel>(certificateRequest);
                return View(editCertificateRequestViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of ServicesController");
                return RedirectToAction("Index", "CertificateReq");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Decision(string id, string decision)
        {
            try
            {
                _logger.LogInformation("Edit method of Certificate Request Controller called with id: {id} and decision: {decision}", id, decision);
                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(decision))
                {
                    return BadRequest("Certificate Request ID cannot be null or empty");
                }
                var certificateRequest = await _dbContext.CertificateRequests.Include(c => c.Student)
            .Include(c => c.Service)
            .Include(c => c.Status)
            .Include(c => c.Payment)

            .FirstOrDefaultAsync(s => s.Id == id);
                if (certificateRequest == null)
                {
                    return NotFound();
                }
                RequestStatus status = null;
                Payment payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.CertificateRequestId == id);
                int updateResult = 0;
                string title = string.Empty;
                string body = string.Empty;
                string titleAr = string.Empty;
                string bodyAr = string.Empty;
                List<Device> devices= new List<Device>();
                switch (decision)
                {

                    case "Rejected":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Pending Payment");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        payment.Status = Core.Enums.PaymentStatus.PENDING;
                        _dbContext.Payments.Update(payment);
                        _dbContext.CertificateRequests.Update(certificateRequest);
                        updateResult = await _dbContext.SaveChangesAsync();
                        if (updateResult > 0) {
                             devices = await _dbContext.Devices.Include(d => d.APIUser).ThenInclude(a => a.Student).Where(d => d.APIUser.Student.StudentNumber == certificateRequest.Student.StudentNumber).ToListAsync();
                            if (devices.Any())
                            {
                                 title = "Payment Receipt Rejected";
                                 body = $"Your payment receipt for {certificateRequest.Service.Name} has been rejected. Please upload the correct receipt";
                                await _firebaseService.SendMulticastNotificationAsync(devices.Select(d => d.FCMToken).ToList(), title, body, "", _webHostEnvironment.IsDevelopment());
                                 titleAr = "تم رفض إيصال الدفع";
                                 bodyAr = $"تم رفض إيصال الدفع الخاص بك لخدمة {certificateRequest.Service.NameAr}. يرجى رفع الإيصال الصحيح.";

                                await _firebaseService.SendMulticastNotificationAsync(devices.Select(d => d.FCMToken).ToList(), titleAr, bodyAr, "", _webHostEnvironment.IsDevelopment());
                            }
                        }

                        break;
                    case "Approved":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Approved");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        payment.Status = Core.Enums.PaymentStatus.SUCCESS;
                        _dbContext.Payments.Update(payment);
                        _dbContext.CertificateRequests.Update(certificateRequest);
                         updateResult = await _dbContext.SaveChangesAsync();
                         devices = await _dbContext.Devices.Include(d => d.APIUser).ThenInclude(a => a.Student).Where(d => d.APIUser.Student.StudentNumber == certificateRequest.Student.StudentNumber).ToListAsync();
                        if (devices.Any())
                        {
                             title = "Payment Receipt Approved";
                             body = $"Your payment receipt for {certificateRequest.Service.Name} has been approved.";

                             titleAr = "تمت الموافقة على إيصال الدفع";
                             bodyAr = $"تمت الموافقة على إيصال الدفع الخاص بك لخدمة {certificateRequest.Service.NameAr}.";


                            await _firebaseService.SendMulticastNotificationAsync(devices.Select(d => d.FCMToken).ToList(), titleAr, bodyAr, "", _webHostEnvironment.IsDevelopment());
                        }
                        break;
                    case "Printed":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Printed");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.CertificateRequests.Update(certificateRequest);
                        updateResult = await _dbContext.SaveChangesAsync();
                         devices = await _dbContext.Devices.Include(d => d.APIUser).ThenInclude(a => a.Student).Where(d => d.APIUser.Student.StudentNumber == certificateRequest.Student.StudentNumber).ToListAsync();
                        if (devices.Any())
                        {
                             title = "Certificate Printed";
                             body = $"Your certificate for {certificateRequest.Service.Name} has been printed.";

                             titleAr = "تم طباعة الشهادة";
                             bodyAr = $"تم طباعة الشهادة الخاصة بك لخدمة {certificateRequest.Service.NameAr}.";



                            await _firebaseService.SendMulticastNotificationAsync(devices.Select(d => d.FCMToken).ToList(), titleAr, bodyAr, "", _webHostEnvironment.IsDevelopment());
                        }
                        break;
                    case "Recieved":
                        status = await _dbContext.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Recieved");
                        certificateRequest.Status = status;
                        certificateRequest.RequestStatusId = status.Id.ToString();
                        _dbContext.CertificateRequests.Update(certificateRequest);
                        updateResult = await _dbContext.SaveChangesAsync();
                        devices = await _dbContext.Devices.Include(d => d.APIUser).ThenInclude(a => a.Student).Where(d => d.APIUser.Student.StudentNumber == certificateRequest.Student.StudentNumber).ToListAsync();
                        if (devices.Any())
                        {
                             title = "Certificate Received";
                             body = $"You have received your certificate for {certificateRequest.Service.Name}.";

                             titleAr = "تم استلام الشهادة";
                             bodyAr = $"تم استلام الشهادة الخاصة بك لخدمة {certificateRequest.Service.NameAr}.";

                            await _firebaseService.SendMulticastNotificationAsync(devices.Select(d => d.FCMToken).ToList(), titleAr, bodyAr, "", _webHostEnvironment.IsDevelopment());
                        }
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
