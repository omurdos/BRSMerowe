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
    public class CountriesController : ControllerBase
    {

        private readonly TSTDBContext _context;

        public CountriesController(TSTDBContext context)
        {
            _context = context;
        }

        // GET: api/<CountriesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var countries = await _context.Countries
                    .OrderBy(c => c.NameAr)
                    .ToListAsync();
                return Ok(countries);
            }
            catch (Exception) {
                throw;
            }
        }

        // GET api/<CountriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CountriesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CountriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CountriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
