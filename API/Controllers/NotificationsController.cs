using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly SMSService _smsService;
        private readonly UserManager<APIUser> _userManager;
        private readonly ILogger<NotificationsController> logger;
        private readonly TSTDBContext _context;
        public NotificationsController(ILogger<NotificationsController> logger, SMSService smsService, UserManager<APIUser> userManager, TSTDBContext context)
        {
            this.logger=logger;
            this._smsService=smsService;
            _userManager = userManager;
            _context = context;
        }

        // GET: api/<NotificationsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NotificationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<NotificationsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationDTO dto)
        {
            try
            {
                int successCount = 0;
                int failureCount = 0;
                logger.LogInformation("Sending SMS notification to All registered users");
                var sentNumbers = await _context.OTPCodes.Where(o => EF.Functions.Like(o.Code, "%تحديث%")).Select(o => o.PhoneNumber).ToListAsync();
                var confirmedUsers = await _userManager.Users
                    .Where(u => u.PhoneNumberConfirmed == true ).ToListAsync();
                var ls = confirmedUsers.Where(u => !sentNumbers.Contains(u.PhoneNumber)).ToList();

                var users = ls;
                //var phoneNumbers = new List<string> { "920101807" , "10861008",  };
                //foreach (var phoneNumber in phoneNumbers)
                //{
                //    var result = await _smsService.Message(phoneNumber, dto.Message, "API");
                //    if (result)
                //    {
                //        successCount++;
                //    }
                //    else { failureCount++; }
                //}
                foreach (var user in users)
                {
                    var result = await _smsService.Message(user.PhoneNumber, dto.Message, "API");
                    if (result)
                    {
                        successCount++;
                    }
                    else { failureCount++; }
                }

                return Ok(new { successCount, failureCount });
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<NotificationsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NotificationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
