using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private UserManager<APIUser> _userManager { get; set; }
        private readonly IJWTService _jWTGenerator;
        private readonly IMapper _mapper;
        private readonly TSTDBContext _context;
        private readonly SMSService _SMSService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<APIUser> userManager,
            IJWTService jWTService, IMapper mapper, TSTDBContext context, SMSService sMSService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jWTGenerator = jWTService;
            _mapper = mapper;
            _context = context;
            _SMSService = sMSService;
            _logger = logger;
        }

        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDTO dto)
        {
            try
            {
                _logger.LogInformation("user trying to login with credentials {@dto}", dto);
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(dto.UserName);
                    if (user == null)
                    {
                        return NotFound(
                            new
                            {
                                Message = "Incorrect phone number or password"
                            });
                    }

                    var result =
                        _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                    if (result == PasswordVerificationResult.Success)
                    {
                        var token = await _jWTGenerator.GenerateToken(user, DateTime.Now.AddDays(20));
                        _logger.LogInformation("user details {@user} with token {@token}", user, token);
                        return Ok(new { user, token });
                    }

                    return NotFound(
                        new
                        {
                            Message = "Incorrect phone number or password"
                        });
                }

                return BadRequest(new
                {
                    Message = "Please provide both phone number and password"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SigninDTO dto)
        {
            try
            {
                _logger.LogInformation("user trying to login with credentials {@dto}", dto);
                if (ModelState.IsValid)
                {
                    APIUser user;
                    PasswordVerificationResult result;
                    var tempUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == dto.PhoneNumber);

                    var userRoles = await _userManager.GetRolesAsync(tempUser);

                    if (!userRoles.Contains("Student"))
                    {
                        _logger.LogInformation("user trying to login with credentials {@dto}", dto);
                        if (ModelState.IsValid)
                        {
                            user = await _userManager.FindByNameAsync(dto.PhoneNumber);
                            if (user == null)
                            {
                                return NotFound(
                                    new
                                    {
                                        Message = "Incorrect phone number or password"
                                    });
                            }

                            result =
                               _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                            if (result == PasswordVerificationResult.Success)
                            {
                                var token = await _jWTGenerator.GenerateToken(user, DateTime.Now.AddDays(20));
                                _logger.LogInformation("user details {@user} with token {@token}", user, token);
                                return Ok(new { user, token });
                            }

                            return NotFound(
                                new
                                {
                                    Message = "Incorrect phone number or password"
                                });
                        }

                        return BadRequest(new
                        {
                            Message = "Please provide both phone number and password"
                        });
                    }


                    user = await _userManager
                       .Users
                       .Include(u => u.Student)
                       .ThenInclude(u => u.Batch)
                       .Include(u => u.Student)
                       .ThenInclude(u => u.Department)
                       .ThenInclude(u => u.Faculty)
                       .Include(u => u.Student)
                       .ThenInclude(u => u.Guardian)
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

                    if (user == null)
                    {
                        return NotFound(
                            new
                            {
                                Message = "Incorrect phone number or password"
                            });
                    }

                    result =
                       _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                    if (result == PasswordVerificationResult.Success)
                    {
                        _logger.LogInformation("user using {@device}", dto.Device);
                        if (dto.Device != null)
                        {
                            await saveDevice(user.Id, dto.Device);
                        }
                        //Generate token and build successful user response
                        var token = await _jWTGenerator.GenerateToken(user);
                        _logger.LogInformation("user details {@user} with token {@token}", user, token);
                        return Ok(new { user, token });
                    }

                    return NotFound(
                        new
                        {
                            Message = "Incorrect phone number or password"
                        });
                }

                return BadRequest(new
                {
                    Message = "Please provide both phone number and password"
                });
            }
            catch (Exception ex)
            {
                throw;
                //return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        private async Task<Device> saveDevice(string userId, CreateDeviceDTO dto) {
            try
            {
                _logger.LogInformation("Checking if user device exists {dto}", dto);

                var device = await _context.Devices.FirstOrDefaultAsync(d => d.APIUserId == userId);
                if (device != null)
                {
                    _logger.LogInformation("Device  exists", dto);

                    _mapper.Map(dto, device);
                    device.ModifiedAt = DateTime.Now;

                    _context.Devices.Update(device);

                    var updateResult = await _context.SaveChangesAsync();
                    if (updateResult > 0)
                    {
                        _logger.LogInformation("Device details upated");

                        return device;
                    }
                    return null;
                }
                else
                {
                    device = _mapper.Map<Device>(dto);
                    device.APIUserId = userId;
                    device.CreatedAt = DateTime.Now;
                    await _context.Devices.AddAsync(device);
                    var insertResult = await _context.SaveChangesAsync();
                    if (insertResult > 0)
                    {
                        return device;
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register([FromBody] SignupDTO dto)
        {
            try
            {
                _logger.LogInformation("----------------new SIGNUP REQUEST-----------------");
                _logger.LogInformation("User details {@dto}", dto);

                if (ModelState.IsValid)
                {
                    _logger.LogInformation("User details are valid");
                    _logger.LogInformation("Fetching user from DB");
                    var ExistingUser =
                        await _userManager.Users.Include(u => u.Student).FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber ||   u.Student.StudentNumber == dto.StudentNumberOrFormAddmission ||
                            u.Student.AddmissionFormNo == dto.StudentNumberOrFormAddmission);
                    _logger.LogInformation("Fetching student from DB");

                    var ExistingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Phone == dto.PhoneNumber);

                   

                    if (ExistingUser != null || ExistingStudent != null)
                    {
                        _logger.LogInformation("User already Exists returning 409 status to client");
                        return Conflict(new
                        {
                            Message = "User already exists, please register with another phone number"
                        });
                    }
                    _logger.LogInformation("User doesn't Exists continuing with signing up the user");
                    _logger.LogInformation("Fetching student details");

                    Core.Entities.Student student = await _context.Students
                        .Include(u => u.Batch)
                        .Include(u => u.Department)
                        .ThenInclude(u => u.Faculty)
                        //.Include(u => u.Guardian)
                        .FirstOrDefaultAsync(s =>
                            s.StudentNumber == dto.StudentNumberOrFormAddmission ||
                            s.AddmissionFormNo == dto.StudentNumberOrFormAddmission);

                    if (student == null)
                    {
                        return NotFound(new
                            { message = $"No Student with student number {dto.StudentNumberOrFormAddmission} found" });
                    }
                    _logger.LogInformation("Found student with details {@student}", student);
                    if (student != null)
                    {
                        _logger.LogInformation("Is Student allowed to signup ? {isActive}", student.IsActive);

                        if (!student.IsActive)
                        {
                            return BadRequest(new { message = "You're not allowed not register yet." });
                        }
                    }

                    _logger.LogInformation("Preparing user model");

                    var user = _mapper.Map<APIUser>(dto);
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = dto.PhoneNumber;
                    user.Student = student;
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    _logger.LogInformation("Attemting to create the users");

                    var result = await _userManager.CreateAsync(user, dto.Password);
                    _logger.LogInformation("Create command sent");

                    if (result == IdentityResult.Success)
                    {
                        _logger.LogInformation("User created successfully");
                        _logger.LogInformation("Adding user to role");

                        var IsAddedToRole = await _userManager.AddToRoleAsync(user, "Student");
                        if (IsAddedToRole == IdentityResult.Success)
                        {
                            _logger.LogInformation("User added successfully to student role");
                            _logger.LogInformation("Generating token");

                            var token = await _jWTGenerator.GenerateToken(user);
                            _logger.LogInformation("Token generated successfully");
                            _logger.LogInformation("Generating OTP Code");

                            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                            _logger.LogInformation("OTP Code generated successfully");
                            _logger.LogInformation("Sending SMS to Student phone number");

                            var isSent = await _SMSService.SendSMS(user.PhoneNumber,code, "API");
                            _logger.LogInformation("Middleware response {isSent}", isSent);
                            _logger.LogInformation("User device details {@Device}", dto.Device);

                            if (dto.Device != null)
                            {
                                await saveDevice(user.Id, dto.Device);
                            }
                            return Created($"/users/{user.Id}", new { token, user });

       
                        }

                        await _userManager.DeleteAsync(user);
                        return BadRequest(new { Message = "Can't complete your request for now" });
                    }

                    return BadRequest(result.Errors);
                }

                return BadRequest($"invalid Model { ModelState.ValidationState}");
            }
            catch (Exception ex)
            {
                _logger.LogError("exception occured signing up the user: {ex}", ex);

                throw;
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                _logger.LogInformation("Initiating phone number verification");
                if (!ModelState.IsValid) {
                    _logger.LogInformation("Invalid model state submitted");

                    return BadRequest();
                }
                _logger.LogInformation("Model state is valid");

                var user = _userManager.Users.Include(user => user.Student).FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);
                if (user == null) {

                    _logger.LogInformation("User not found");

                    return NotFound(); }
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                _logger.LogInformation($"{code} generated successfully for {user.PhoneNumber}");

                var isSent = await _SMSService.SendSMS(user.PhoneNumber, code, "API");
                if (isSent)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    return Ok(new { token, phoneNumber = user.PhoneNumber });
                }

                return BadRequest("Failed to send otp through whatsapp thats why you are seeing this.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPasswrod([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
                if (user == null) return NotFound();
                IdentityResult result;
                result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = result.Succeeded });
                }
                else
                {
                    return Ok(new { message = result.Succeeded, error = result.Errors });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ChangePasswrod([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
                if (user == null) return NotFound();
                IdentityResult result;
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                result = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = result.Succeeded });
                }
                else
                {
                    return Ok(new { message = result.Succeeded, error = result.Errors });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUserById( string id)
        {
            try
            {
                _logger.LogInformation("Fetching user with id {id}", id);
                var user = await _userManager
                        .Users
                        .Include(u => u.Student)
                        .ThenInclude(u => u.Batch)
                        .Include(u => u.Student)
                        .ThenInclude(u => u.Department)
                        .ThenInclude(u => u.Faculty)
                        .Include(u => u.Student)
                        .ThenInclude(u => u.Guardian)
                        .FirstOrDefaultAsync(u => u.Id == id);

                    if (user == null)
                    {
                        _logger.LogInformation("User not found with id {id}", id);
                        return NotFound(
                            new
                            {
                                Message = "User not found"
                            });
                    }
                    _logger.LogInformation("User found with id {id}", id);
                    return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while fetching user with id {id} : {ex}", id, ex);
                return BadRequest(ex);
            }
        }



    }
}