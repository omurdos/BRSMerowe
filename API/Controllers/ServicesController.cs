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
    public class ServicesController : ControllerBase
    {

        private readonly TSTDBContext _context;

        public ServicesController(TSTDBContext context)
        {
            _context=context;
        }



        // GET: api/<ServicessController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ServicessController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var service = await _context.Services.FirstOrDefaultAsync(s => s.Name == name);
                if (service == null) {
                    return NotFound();
                }
                return Ok(service);
            }
            catch (Exception)
            {

                throw;
            }
        }



        // POST api/<ServicessController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServicessController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServicessController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
