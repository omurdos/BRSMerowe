using API.DTOs;
using AutoMapper;
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
    public class DevicesController : ControllerBase
    {
        private readonly TSTDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DevicesController> _logger;
        public DevicesController(TSTDBContext context, IMapper mapper, ILogger<DevicesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;   
        }

        //// GET: api/<DevicesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<DevicesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<DevicesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateDeviceDTO dto)
        {
            try
            {
                _logger.LogInformation("Checking if user device exists {dto}", dto);

                var device = await _context.Devices.FirstOrDefaultAsync(d => d.APIUserId == dto.UserId);
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

                        return Ok(device);
                    }
                    return null;
                }
                else
                {
                    device = _mapper.Map<Device>(dto);
                    device.APIUserId = dto.UserId;
                    device.CreatedAt = DateTime.Now;
                    await _context.Devices.AddAsync(device);
                    var insertResult = await _context.SaveChangesAsync();
                    if (insertResult > 0)
                    {
                        return Ok(device);
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //// PUT api/<DevicesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DevicesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
