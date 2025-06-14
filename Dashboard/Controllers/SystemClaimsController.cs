using AutoMapper;
using Core.Entities;
using Core.Enums;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    public class SystemClaimsController : Controller
    {
        private readonly ILogger<SystemClaimsController> _logger;
        private readonly TSTDBContext _dbContext;
        private readonly FirebaseService _pushNotificationService;


        public SystemClaimsController(ILogger<SystemClaimsController> logger, TSTDBContext dbContext, FirebaseService pushNotificationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _pushNotificationService = pushNotificationService;


        }

        // GET: Claims  
        public IActionResult Index(int? page)
        {

            var claims = _dbContext.SystemClaims.OrderBy(sc => sc.ClaimValue).ToList();
            return View(claims.ToPagedList(page ?? 1, 10));

        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SystemClaim systemClaim)
        {
            if (ModelState.IsValid)
            {
              _dbContext.SystemClaims.Add(systemClaim);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(systemClaim);
        }

        // GET: Claims/Edit/{id}
        public IActionResult Edit(int id)
        {
            var claim = _dbContext.SystemClaims.FirstOrDefault(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }
          
            return View(claim);
        }

        // POST: Claims/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,  SystemClaim updatedClaim)
        {
            if (id != updatedClaim.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingClaim = _dbContext.SystemClaims.FirstOrDefault(c => c.Id == id);
                if (existingClaim != null)
                {
                    existingClaim.ClaimType = updatedClaim.ClaimType;
                    existingClaim.ClaimValue = updatedClaim.ClaimValue;
                    existingClaim.Description = updatedClaim.Description;


                    _dbContext.SystemClaims.Update(existingClaim);
                    _dbContext.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedClaim);
        }













    }
}
