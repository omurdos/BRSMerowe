using API.DTOs;
using Core.Entities;
using Core.Enums;
using FirebaseAdmin.Messaging;
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

    public class PaymentsController : ControllerBase
    {
        private readonly TSTDBContext _context;
        ILogger<GeneralCertificatesController> _logger;
        private readonly FileUploadService _fileUploadService;
        private readonly StudentDetailsService _studentDetailsService;

        public PaymentsController(TSTDBContext context, ILogger<GeneralCertificatesController> logger, FileUploadService fileUploadService, StudentDetailsService studentDetailsService)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
            _context = context;
            _studentDetailsService = studentDetailsService;
        }
        // GET: api/<PaymentsController>
        // GET: api/<PaymentsController>
        [HttpGet("student")]
        public async Task<IActionResult> Get([FromQuery] string studentNumber)
        {
            try
            {
                var student = await _context.Students
                 .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);

                if (student == null)
                {
                    return NotFound(new { status = "Failed", message = "Student not found" });
                }

                var payments = await _context.Payments
                    .Include(p => p.Student)
                     // .Include(p => p.CardRequest)
                     // .ThenInclude(cr => cr.Status)
                     // .Include(p => p.CardRequest)
                     // .ThenInclude(cr => cr.Service)

                     .Include(p => p.CertificateRequest)
                    .ThenInclude(cr => cr.Status)
                    .Include(p => p.CertificateRequest)
                    .ThenInclude(cr => cr.Service)

                    //  .Include(p => p.EnrollmentRequest)
                    // .ThenInclude(cr => cr.Status)
                    // .Include(p => p.EnrollmentRequest)
                    // .ThenInclude(cr => cr.Service)

                    // .Include(p => p.TranscriptRequest)
                    // .ThenInclude(cr => cr.Status)
                    // .Include(p => p.TranscriptRequest)
                    // .ThenInclude(cr => cr.Service)

                    .Where(p => p.Student.StudentNumber == studentNumber)
                    .ToListAsync();
                _logger.LogInformation("Payments retrieved for student: {StudentNumber}", studentNumber);
                _logger.LogInformation("Payments: {Payments}", payments);


                var studentPayments = await _studentDetailsService.GetStudentDetailsFromApi(studentNumber);
                if (studentPayments != null)
                {
                    if (studentPayments.Any())
                    {

                        foreach (var dto in studentPayments)
                        {
                            var payment = new Payment
                            {
                                Id = dto.Id.ToString(),
                                ReferenceNumber = dto.PaymentReference,
                                Semester = dto.SemesterId.ToString(),
                                Amount = double.TryParse(dto.PaymentAmount, out var parsedAmount) ? parsedAmount : 0,
                                Student = student,
                                Status = dto.IsPaid == 0 ? PaymentStatus.PENDING : PaymentStatus.SUCCESS,
                                CreatedAt = dto.CreatedAt.DateTime
                            };
                            payments.Add(payment);
                        }
                    }

                }


                return Ok(payments);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        // GET api/<PaymentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PaymentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PaymentsController>/5
        [HttpPost("study-fees")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StudyFeesPayment([FromBody] StudyFeeDTO dto)
        {
            try
            {
                var student = await _context
                    .Students
                    .FirstOrDefaultAsync(s => s.StudentNumber == dto.StudentNumber);
                var payment = new Payment
                {
                    Amount = dto.Amount,
                    Student = student,
                    CreatedAt = dto.PaidAt,
                    Semester = dto.Semester,
                    Status = PaymentStatus.SUCCESS,
                    ReferenceNumber = dto.ReferenceNumber
                };

                await _context.Payments.AddAsync(payment);
                var createResult = await _context.SaveChangesAsync();
                if (createResult > 0)
                {

                    var Invoice = new Invoice
                    {
                        PaymentId = payment.Id,
                        CreatedAt = DateTime.Now,
                        ReferenceNumber = new Random().Next().ToString(),
                    };
                    await _context.Invoices.AddAsync(Invoice);
                    var createInvoiceResult = await _context.SaveChangesAsync();
                    if (createInvoiceResult > 0)
                    {
                        return Ok(new { status = "Success", message = "Payment & invoice generated" });

                    }

                }
                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }


        // PUT api/<PaymentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdatePaymentDTO dto)
        {
            try
            {
                var payment = await _context.Payments.Include(p => p.CertificateRequest).ThenInclude(cr => cr.Status).FirstOrDefaultAsync(p => p.Id == id);
                if (payment == null)
                {
                    _logger.LogWarning("Payment with ID: {Id} not found", id);
                    return NotFound(new { status = "Failed", Message = "Payment not found" });
                }
                else if (payment.Status == PaymentStatus.SUCCESS)
                {
                    _logger.LogWarning("Payment with ID: {Id} is already successful", id);
                    return BadRequest(new {status = "Failed", Message = "Payment is already successful" });
                }
                else if (payment.CertificateRequest.Status.Name != "Pending Payment")
                {
                    _logger.LogWarning("Payment with ID: {Id} is not in a valid state for update", id);
                    return BadRequest(new { status = "Failed", Message = "Payment is not in a valid state for update" });
                }
                if (dto.Status != null)
                {
                    payment.Status = dto.Status ?? PaymentStatus.PENDING;
                }

                if (dto.ReceiptPhoto != null)
                {
                    var fileUploadResult = await _fileUploadService.Upload(dto.ReceiptPhoto, AttachmentType.RECIEPT);
                    if (fileUploadResult == null || fileUploadResult.Succeed == false)
                    {
                        _logger.LogError("File upload failed for payment with ID: {Id}", id);
                        return BadRequest(new { status = "Failed", Message = "File upload failed" });
                    }
                    var status = await _context.RequestStatuses.FirstOrDefaultAsync(s => s.Name == "Pending Verification");
                    payment.CertificateRequest.Status = status;
                    payment.CertificateRequest.RequestStatusId = status.Id.ToString();
                    payment.ReceiptPhoto = fileUploadResult.FileName;
                    payment.Status = PaymentStatus.PENDING_VERIFICATION;
                }
                _context.Payments.Update(payment);
                var updateResult = await _context.SaveChangesAsync();
                if (updateResult > 0)
                {

                    if (dto.Status == PaymentStatus.SUCCESS)
                    {
                        var Invoice = new Invoice
                        {

                            PaymentId = payment.Id,
                            CreatedAt = DateTime.Now,
                            ReferenceNumber = new Random().Next().ToString(),

                        };
                        await _context.Invoices.AddAsync(Invoice);
                        var createInvoiceResult = await _context.SaveChangesAsync();
                        return Ok(new { status = "Success", message = "Payment updated successfully and invoice generated" });
                    }
                    return Ok(new { status = "Success", message = "Payment updated successfully" });
                }
                return BadRequest(new { status = "Failed", message = "Payment update failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating payment with ID: {Id}", id);
                throw ex;
            }
        }

        // DELETE api/<PaymentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
