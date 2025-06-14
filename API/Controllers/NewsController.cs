using API.DTOs;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly TSTDBContext _context;
        private readonly IMapper _mapper;
        public NewsController(TSTDBContext context, IMapper mapper)
        {
            _context= context;
            _mapper= mapper;
        }


        // GET: api/<NewsController>
        [HttpGet]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {

                var news = await _context.News.OrderBy(n => n.CreatedAt)
                                 .Skip(page ?? 0 * 10)
                                 .Take(10)
                                 .ToListAsync();


                return Ok(news);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _context.News.FindAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<NewsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateNewsDTO dto)
        {
            try
            {
                if (ModelState.IsValid) {

                    var news = await _context.News
                        .Where(n => n.Title == dto.Title)
                        .FirstOrDefaultAsync();
                    if (news != null) {
                        return Conflict(new { message = "A news with the same title already exists" });
                    }

                    news = _mapper.Map<News>(dto);

                    await _context.News.AddAsync(news);
                    var createResult = await _context.SaveChangesAsync();
                    if (createResult > 0) {
                        return Created("",news);
                    }
                    return BadRequest(new { messge = "Failed to save your message"});

                }
                return BadRequest(ModelState.ValidationState.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateNewsDTO dto)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var news = await _context.News
                        .FindAsync(id);

                    if (news == null)
                    {
                        return NotFound(new { message = $"NO Record with the id {dto.Id}" });
                    }

                    // = _mapper.Map<News>(dto);
                    _mapper.Map(dto, news);
                    _context.News.Update(news);
                    var updateResult = await _context.SaveChangesAsync();
                    if (updateResult > 0)
                    {
                        return Ok(news);
                    }
                    return BadRequest(new { messge = "Failed to save your message" });

                }
                return BadRequest(ModelState.ValidationState.ToString());



            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
