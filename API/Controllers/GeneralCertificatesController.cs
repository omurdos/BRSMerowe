using System.Threading.Tasks;
using API.DTOs;
using BRSSinnar.Dashboard.Helpers;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralCertificatesController : ControllerBase
    {
        TSTDBContext _context;
        ILogger<GeneralCertificatesController> _logger;
        private readonly FileUploadService _fileUploadService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public GeneralCertificatesController(TSTDBContext context, ILogger<GeneralCertificatesController> logger, FileUploadService fileUploadService, IHubContext<NotificationHub> hubContext)
        {
            _fileUploadService = fileUploadService;
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
            _logger.LogInformation("GeneralCertificatesController initialized");
        }


        // GET: api/<CertificateRequestsController>
        [HttpGet("student/{studentNumber}")]
        public async Task<IActionResult> GetStudentRequests(string studentNumber)
        {
            try
            {
                _logger.LogInformation("Fetching certificate requests for student number: {StudentNumber}", studentNumber);
                var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
                if (student == null)
                {
                    _logger.LogWarning("Student not found: {StudentNumber}", studentNumber);
                    return NotFound(new { Message = "Student not found" });
                }
                var result = await _context.CertificateRequests
                    .Include(s => s.Service)
                    .Include(s => s.Student)
                    .Include(s => s.Status)
                    .Where(cr => cr.Student.StudentNumber == studentNumber)
                    .ToListAsync();
                _logger.LogInformation("Fetched {Count} certificate requests for student number: {StudentNumber}", result.Count, studentNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching certificate requests for student number: {StudentNumber}", studentNumber);
                return StatusCode(500, new { Message = "Internal server error" });
            }

        }

        // GET api/<CertificateRequestsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                _logger.LogInformation("Fetching certificate request with ID: {Id}", id);
                var certificateRequest = await _context.CertificateRequests
                    .Include(s => s.Service)
                    .Include(s => s.Student)
                    .Include(s => s.Status)
                    .FirstOrDefaultAsync(cr => cr.Id == id);
                if (certificateRequest == null)
                {
                    _logger.LogWarning("Certificate request not found: {Id}", id);
                    return NotFound(new { Message = "Certificate request not found" });
                }
                _logger.LogInformation("Fetched certificate request with ID: {Id}", id);
                return Ok(certificateRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching certificate request with ID: {Id}", id);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }
        [HttpPost]
        // POST api/<CertificateRequestsController>
        public async Task<IActionResult> Post([FromBody] CreateCertificateRequestDTO dto)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == dto.StudentNumber);
                var status = await _context.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Pending Payment");
                var service = await _context.Services.FirstOrDefaultAsync(s => s.Name == dto.ServiceName);
                if (service == null)
                {
                    _logger.LogWarning("Service not found: {ServiceName}", dto.ServiceName);
                    return NotFound(new { Message = "Service not found" });
                }
                if (student != null)
                {
                    var certificateRequest = new CertificateRequest
                    {
                        Student = student,
                        Status = status,
                        Service = service,
                        FullNameAR = dto.FullNameArabic,
                        FullNameEN = dto.FullNameEnglish,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        Language = dto.Language,
                    };
                    await _context.CertificateRequests.AddAsync(certificateRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        var Payment = new Payment
                        {
                            Student = student,
                            Status = PaymentStatus.PENDING,
                            CertificateRequestId = certificateRequest.Id,
                            CertificateRequest = certificateRequest,
                            CreatedAt = DateTime.Now,
                            Amount = service.Fee,
                            ReferenceNumber = new Random().Next().ToString(),


                        };
                        await _context.Payments.AddAsync(Payment);
                        //_context.CardRequests.Update(CardRequest);
                        var paymentResult = await _context.SaveChangesAsync();
                        if (paymentResult > 0)
                        {
                            // Notify admins
                            await _hubContext.Clients.Group("Admins")
                                .SendAsync("ReceiveNotification",
                                new
                                {
                                    Title = "New Request",
                                    Message = "A new certificate was requested"
                                });
                            return Created(uri: "", certificateRequest);
                        }
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                return BadRequest();

            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<CertificateRequestsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateCertificateRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Updating certificate request with ID: {Id}", id);
                var certificateRequest = _context.CertificateRequests.Include(c => c.Status).FirstOrDefault(cr => cr.Id == id);
                if (certificateRequest != null)
                {
                    var pendingVerificationStatus = await _context.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Pending Verification");
                    certificateRequest.FullNameAR = dto.FullNameArabic;
                    certificateRequest.FullNameEN = dto.FullNameEnglish;
                    certificateRequest.Language = dto.Language;

                    certificateRequest.Status = pendingVerificationStatus;
                    certificateRequest.ModifiedAt = DateTime.Now;
                    if (dto.ReceiptPhoto != null)
                    {
                        var fileUploadResult = await _fileUploadService.Upload(dto.ReceiptPhoto, AttachmentType.RECIEPT);
                        if (fileUploadResult == null || fileUploadResult.Succeed == false)
                        {
                            _logger.LogError("File upload failed for certificate request with ID: {Id}", id);
                            return BadRequest(new { Message = "File upload failed" });
                        }

                        certificateRequest.ReceiptPhoto = fileUploadResult.FileName;
                    }
                    _context.CertificateRequests.Update(certificateRequest);
                    var saveChangesResult = await _context.SaveChangesAsync();
                    if (saveChangesResult > 0)
                    {
                        _logger.LogInformation("Successfully updated certificate request with ID: {Id}", id);
                        return Ok(certificateRequest);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to update certificate request with ID: {Id}", id);
                        return BadRequest(new { Message = "Failed to update certificate request" });
                    }
                }
                return NotFound(new { Message = "Certificate request not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating certificate request with ID: {Id}", id);
                return StatusCode(500, new { Message = "Internal server error" });
            }

        }

        // DELETE api/<CertificateRequestsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
