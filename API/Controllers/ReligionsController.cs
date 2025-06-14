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
    public class ReligionsController : ControllerBase
    {

        private readonly TSTDBContext _context;
        public ReligionsController(TSTDBContext context)
        {
            _context=context;
        }


        // GET: api/<ReligionsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var religioins = await _context.Religions.ToListAsync();
                return Ok(religioins);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET api/<ReligionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReligionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReligionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReligionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
