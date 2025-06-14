using API.DTOs;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.IO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {

        private readonly TSTDBContext _context;
        private readonly ImageUploadService _imageUploadService;
        private readonly ILogger<CardsController> _logger;
        public CardsController(TSTDBContext context, ImageUploadService imageUploadService, ILogger<CardsController> logger)
        {
            _context=context;
            _imageUploadService=imageUploadService;
            _logger=logger;
        }

        // GET: api/<CardsController>
        [HttpGet()]
        public async Task<IActionResult> Get(int pageNumber)
        {
            try
            {
                _logger.LogInformation("fetching all cards requests ");
                if (pageNumber != 0)
                {
                    var result = await _context.CardRequests
             .Include(s => s.Service)
             .Include(s => s.Student)
             .Include(s => s.Status)
             .Skip((pageNumber - 1) * 10)
               .Take(10)
             .ToListAsync();
                    _logger.LogInformation("cards requests are: {@requests}", result);
                    return Ok(result);
                }
                else
                {
                    var result = await _context.CardRequests
              .Include(s => s.Service)
              .Include(s => s.Student)
              .Include(s => s.Status)
              .ToListAsync();
                    _logger.LogInformation("cards requests are: {@requests}", result);
                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex}", ex);
                throw;
            }
        }

        // GET: api/<CardsController>
        [HttpGet("student/{studentNumber}")]
        public async Task<IActionResult> Get(string studentNumber)
        {
            try
            {
                _logger.LogInformation("fetching all cards requests for {@studentNumber}", studentNumber);
                var result = await _context.CardRequests
                    .Include(s => s.Service)
                    .Include(s => s.Student)
                    .Include(s => s.Status)
                    .Where(cr => cr.Student.StudentNumber == studentNumber)
                    .ToListAsync();
                _logger.LogInformation("{@studentNumber} cards requests are: {@requests}", studentNumber, result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex}", ex);
                throw;
            }
        }

        // GET api/<CardsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardRequest(string id)
        {
            try
            {
                //Fetch Card request details....
                var CardRequest = await _context.CardRequests
                    .Include(c => c.Service)
                    .Include(c => c.Student)
                    .Include(c => c.Status)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (CardRequest == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(CardRequest);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<CardsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCardRequestDTO dto)
        {
            try
            {

                if (ModelState.IsValid)
                {


                    var cardRequest = await _context.CardRequests.FirstOrDefaultAsync(cr => cr.Student.StudentNumber == dto.StudentNumber && cr.Status.Id != 4);
                    if (cardRequest != null)
                    {
                        return Conflict(new { message = "You already have a req" });
                    }
                    var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == dto.StudentNumber);

                    if (student == null)
                    {
                        return NotFound();
                    }

                    var uploadResult = await _imageUploadService.Upload(dto.Photo, "IDs");
                    if (uploadResult.Succeed)
                    {


                        var status = await _context.RequestStatuses.FirstOrDefaultAsync(s => s.Id == 1);
                        var service = await _context.Services.FirstOrDefaultAsync(s => s.Name == "New ID Card");


                        cardRequest = new CardRequest
                        {
                            Student = student,
                            Status = status,
                            Service = service,
                            Photo = uploadResult.FileName
                        };
                        await _context.CardRequests.AddAsync(cardRequest);
                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            var Payment = new Payment
                            {
                                Student = student,
                                Status = PaymentStatus.PENDING,
                                CardRequestId = cardRequest.Id,
                                CardRequest = cardRequest,
                                Amount = service.Fee,
                                ReferenceNumber = new Random().Next().ToString(),
                                CreatedAt = DateTime.Now,
                            };
                            await _context.Payments.AddAsync(Payment);
                            _context.CardRequests.Update(cardRequest);
                            var paymentResult = await _context.SaveChangesAsync();
                            if (paymentResult > 0)
                            {
                                return Created(uri: "", cardRequest);
                            }
                        }
                        else
                        {
                            return BadRequest(result);
                        }

                    }

                    return BadRequest();

                }
                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<CardsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateCardRequestDTO dto)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var cardRequest = await _context.CardRequests
                        .Include(cr => cr.Student).Include(cr => cr.Service)
                        .FirstOrDefaultAsync(cr => cr.Id == id);
                    if (cardRequest == null)
                    {
                        return NotFound();
                    }


                    if (dto.Photo != null)
                    {
                        if (cardRequest.Photo != null)
                        {
                            var filePath = Path.Combine("D:", "Images", "IDs", cardRequest.Photo);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);

                                var uploadResult = await _imageUploadService.Upload(dto.Photo, "IDs");

                                if (uploadResult.Succeed)
                                {
                                    cardRequest.Photo = uploadResult.FileName;
                                    _context.CardRequests.Update(cardRequest);
                                    var isupdated = await _context.SaveChangesAsync();
                                    if (isupdated > 0)
                                    {
                                        return Ok(uploadResult.Succeed);
                                    }
                                    return BadRequest();

                                }


                            }
                        }
                    }
                    return BadRequest();


                }
                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE api/<CardsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
