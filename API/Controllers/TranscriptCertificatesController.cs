using API.DTOs;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriptCertificatesController : ControllerBase
    {
        TSTDBContext _context;
        ILogger<GeneralCertificatesController> _logger;
        private readonly FileUploadService _fileUploadService;
        public TranscriptCertificatesController(TSTDBContext context, ILogger<GeneralCertificatesController> logger, FileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
            _context = context;
        }

        // GET: api/<CertificateRequestsController>
        [HttpGet("student/{studentNumber}")]
        public async Task<IActionResult> Get(string studentNumber)
        {
            try
            {
                var result = await _context.TranscriptRequests
                    .Include(s => s.Service)
                    .Include(s => s.Student)
                    .Include(s => s.Status)
                    .Where(cr => cr.Student.StudentNumber == studentNumber)
                    .ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }


        // GET api/<TranscriptCertificatesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        // POST api/<CertificateRequestsController>
        public async Task<IActionResult> Post([FromBody] CreateCertificateRequestDTO dto)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == dto.StudentNumber);
                var status = await _context.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Pending Payment");
                var service = await _context.Services.FirstOrDefaultAsync(s => s.Name == "Transcript Certificate");
                if (student != null)
                {
                    var certificateRequest = new TranscriptRequest
                    {
                        Student = student,
                        Status = status,
                        Service = service,
                        FullNameAR = dto.FullNameArabic,
                        FullNameEN = dto.FullNameEnglish,
                        CreatedAt = DateTime.Now,
                        ModifiedAt =   DateTime.Now,
                        Language = dto.Language,
                    };
                    await _context.TranscriptRequests.AddAsync(certificateRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        var Payment = new Payment
                        {
                            Student = student,
                            Status = PaymentStatus.PENDING,
                            TranscriptRequestId = certificateRequest.Id,
                            TranscriptRequest = certificateRequest,
                            CreatedAt = DateTime.Now,
                            Amount = service.Fee,
                            ReferenceNumber = new Random().Next().ToString(),

                        };
                        await _context.Payments.AddAsync(Payment);
                        var paymentResult = await _context.SaveChangesAsync();
                        if (paymentResult > 0)
                        {
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
                var certificateRequest = _context.TranscriptRequests.Include(c => c.Status).FirstOrDefault(cr => cr.Id == id);
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
                        if(fileUploadResult == null || fileUploadResult.Succeed == false)
                        {
                            _logger.LogError("File upload failed for certificate request with ID: {Id}", id);
                            return BadRequest(new { Message = "File upload failed" });
                        }
                       
                        certificateRequest.ReceiptPhoto = fileUploadResult.FileName;
                    }
                    _context.TranscriptRequests.Update(certificateRequest);
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

    }
}
