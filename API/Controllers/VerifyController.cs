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
    public class VerifyController : ControllerBase
    {

        private readonly TSTDBContext _context;
        private readonly UserManager<APIUser> _userManager;
        private readonly SMSService _SMSService;

        public VerifyController(TSTDBContext context, UserManager<APIUser> userManager, SMSService SMSService)
        {
            _context=context;
            _userManager = userManager;
            _SMSService=SMSService;

        }

        [HttpGet]
        [Route("send-otp")]
        public async Task<IActionResult> SendOTP(string phoneNumber)
        {
            try
            {

                var user = _userManager.Users.Include(u => u.Student).FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                if (user == null)
                {
                    return BadRequest();
                }
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                var isSent = await _SMSService.SendSMS(user.PhoneNumber, code, "API");
                if (isSent) {
                    return Ok(new { token = code, User = user });
                }
                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("resend-code")]
        public async Task<IActionResult> ResendCode(string PhoneNumber)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var user = _userManager.Users.Include( u => u.Student).FirstOrDefault(u => u.PhoneNumber == PhoneNumber);
                if (user == null) return NoContent();
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                var isSent = await _SMSService.SendSMS(user.PhoneNumber, code, "API");
                if (isSent)
                {
                    return Ok(new { token = code, User = user });
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        // GET: api/<VerifyController>
        [HttpPost]
        [Route("phone")]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneNumberDTO dto)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);
                if (user == null)
                {
                    return BadRequest();
                }
                var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, dto.Code, user.PhoneNumber);
                if (result)
                {
                    var changePhoneResult = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, dto.Code);
                    if (changePhoneResult.Succeeded)
                    {
                        return Ok(new { success = true });

                    }
                    return Ok(new { success = false });

                }
                return BadRequest(new { message = "Invalid OTP" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [Route("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyPhoneNumberDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);
                if (user == null) return NotFound();
                var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, dto.Code, user.PhoneNumber);
                return Ok(new { message = result });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
