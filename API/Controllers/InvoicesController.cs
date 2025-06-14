using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly TSTDBContext _context;

        public InvoicesController(TSTDBContext context)
        {
            _context=context;
        }

        // GET: api/<InvoicesController>
        [HttpGet("students")]
        public async Task<IActionResult> Get([FromQuery]string studentNumber)
        {
            try
            {
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
                return Ok(invoices);
            }
            catch (Exception)
            {

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
