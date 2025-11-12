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
    public class InvoicesController : ControllerBase
    {
        private readonly TSTDBContext _context;
        private readonly StudentDetailsService _studentDetailsService;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(TSTDBContext context, ILogger<InvoicesController> logger, StudentDetailsService studentDetailsService)
        {
            _context=context;
            _logger =logger;
            _studentDetailsService = studentDetailsService;
        }

        // GET: api/<InvoicesController>
        [HttpGet("students")]
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

                var invoices = await _context.Invoices
                .Include(i => i.Payment)
                .ThenInclude(i => i.Student)
                .Include(i => i.Payment)
                .ThenInclude(i => i.CardRequest)
                .ThenInclude(cr => cr.Service)
                .Include(i => i.Payment)
                .ThenInclude(i => i.CardRequest)
                .ThenInclude(cr => cr.Status)

                .Include(i => i.Payment)
                .ThenInclude(i => i.EnrollmentRequest)
                .ThenInclude(cr => cr.Service)
                .Include(i => i.Payment)
                .ThenInclude(i => i.EnrollmentRequest)
                .ThenInclude(cr => cr.Status)

                .Include(i => i.Payment)
                .ThenInclude(i => i.CertificateRequest)
                .ThenInclude(cr => cr.Service)
                .Include(i => i.Payment)
                .ThenInclude(i => i.CertificateRequest)
                .ThenInclude(cr => cr.Status)

                .Include(i => i.Payment)
                .ThenInclude(i => i.TranscriptRequest)
                .ThenInclude(cr => cr.Service)
                .Include(i => i.Payment)
                .ThenInclude(i => i.TranscriptRequest)
                .ThenInclude(cr => cr.Status)

                .Where(i => i.Payment.Student.StudentNumber == studentNumber)
                .ToListAsync();


                var studentInvoices = await _studentDetailsService.GetStudentInvoices(studentNumber);


                if (studentInvoices != null) {
                    if (studentInvoices.Any())
                    {

                        foreach (var studentInvoice in studentInvoices)
                        {

                            var invoice = new Invoice
                            {

                                Id = studentInvoice.Id.ToString(),
                                ReferenceNumber = studentInvoice.PaymentReference,
                                CreatedAt = studentInvoice.CreatedAt.DateTime,
                                PaymentId = studentInvoice.PaymentReference,
                                Payment = new Payment
                                {
                                    Id = studentInvoice.PaymentReference.ToString(),
                                    Amount = double.TryParse(studentInvoice.PaymentAmount, out var parsedAmount) ? parsedAmount : 0,
                                    Semester = studentInvoice.SemesterId.ToString(),
                                    ReferenceNumber = studentInvoice.PaymentReference,
                                    Student = student,

                                    Status = PaymentStatus.SUCCESS,


                                }


                            };

                            invoices.Add(invoice);

                        }

                    }
                }



                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error retrieving invoices for student: {StudentNumber}, Error: {Error}", studentNumber, ex.Message);
                throw;
            }
        }


        // GET api/<InvoicesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InvoicesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InvoicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InvoicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
