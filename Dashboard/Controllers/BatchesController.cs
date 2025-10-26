using AutoMapper;
using Core.Entities;
using X.PagedList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.Reflection.Metadata.Ecma335;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Controllers
{

    public class BatchesController : Controller
    {

        private readonly ILogger<BatchesController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly IMapper _mapper;



        public BatchesController(ILogger<BatchesController> logger,

        TSTDBContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;

        }
        // API Method - Returns JSON
        [HttpGet]
        public async Task<IActionResult> GetBatches([FromQuery] string FacultyNumber, [FromQuery] string DepartmentNumber)
        {
            try
            {
                _logger.LogInformation("Fetching batches for FacultyNumber: {FacultyNumber}, DepartmentNumber: {DepartmentNumber}", FacultyNumber, DepartmentNumber);

                if (FacultyNumber == null && DepartmentNumber == null ) {
                    
                    var allBatches = await _dbContext.Batches.Select(b => new
                    {
                        BatchId = b.BatchId,
                        BatchDescription = b.BatchDescription,
                    }).Distinct().ToListAsync();

                    return Json(allBatches);
                }

                var batches = await _dbContext.Students
                .Where(s => s.FacultyNumber == FacultyNumber && s.DepartmentNumber == DepartmentNumber)
                .Join(_dbContext.Batches,
                    student => student.BatchId,
                    batch => batch.BatchId,
                    (student, batch) => new
                    {
                        BatchId = batch.BatchId,
                        BatchDescription = batch.BatchDescription,
                    })
                .Distinct()
                .ToListAsync();
                _logger.LogInformation("Fetched {Count} batches", batches.Count);
                return Json(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batches");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
