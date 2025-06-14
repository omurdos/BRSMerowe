using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    [Authorize(Policy = "ServicesClaims")]
    public class ServicesController : Controller
    {
        private readonly ILogger<PushNotificationsController> _logger;
        private readonly TSTDBContext _dbContext;

        private readonly IMapper _mapper;
        public ServicesController(ILogger<PushNotificationsController> logger, TSTDBContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(int? page)
        {
            var result = await _dbContext.Services.ToListAsync();
            var Services = _mapper.Map<List<ServiceViewModel>>(result);

            TempData["Title"] = "Services";
            return View(Services.ToPagedList(page ?? 1, 10));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                _logger.LogInformation("Edit method of ServicesController called with id: {id}", id);
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Service ID cannot be null or empty.");
                }
                var service = await _dbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
                if (service == null)
                {
                    return NotFound();
                }
                var serviceViewModel = _mapper.Map<ServiceViewModel>(service);
                return View(serviceViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of ServicesController");
                return RedirectToAction("Index", "Services");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Edit method of ServicesController called with id: {id}", viewModel.Id);
                    if (string.IsNullOrEmpty(viewModel.Id))
                    {
                        return BadRequest("Service ID cannot be null or empty.");
                    }
                    var service = await _dbContext.Services.FirstOrDefaultAsync(s => s.Id == viewModel.Id);
                    if (service == null)
                    {
                        return NotFound();
                    }


                    _mapper.Map(viewModel, service);
                    _dbContext.Services.Update(service);
                    var saveChangesResult = await _dbContext.SaveChangesAsync();
                    if (saveChangesResult > 0)
                    {
                        return RedirectToAction("Index", "Services");

                    }
                    return View(viewModel);
                }
                //add Modelstate errors to a view bag
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit method of ServicesController");
                return RedirectToAction("Index", "Services");
            }
        }




    }
}
