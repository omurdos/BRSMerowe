using System.Security.Claims;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using X.PagedList;
using X.PagedList.Extensions;



namespace Dashboard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly TSTDBContext _context;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<Role> roleManager, TSTDBContext context, IMapper mapper)
        {
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }


        // GET: Roles

        public async Task<IActionResult> Index(int? page)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            TempData["Title"] = "Roles";
            return View(roles.ToPagedList(page ?? 1, 10));
        }

        public IActionResult Create()
        {
            var SystemClaims = _context.SystemClaims.ToList();
            ViewBag.SystemClaims = SystemClaims;
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new Role { Id = Guid.NewGuid().ToString(), Name = role.Name, NormalizedName = role.Name.ToUpper() });
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var SystemClaims = _context.SystemClaims.ToList();
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var systemClaimsViewModel = _mapper.Map<List<SystemClaimViewModel>>(SystemClaims);
            foreach (var claim in systemClaimsViewModel)
            {
                claim.IsSelected = roleClaims.Any(c => c.Type == claim.ClaimType && c.Value == claim.ClaimValue);
            }

            ViewBag.Claims = systemClaimsViewModel;

            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Role role, List<SystemClaimViewModel> claims)
        {
            if (id != role.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingRole = await _roleManager.FindByIdAsync(id);
                if (existingRole == null)
                    return NotFound();

                existingRole.Name = role.Name;
                existingRole.NormalizedName = role.Name.ToUpper();

                var SelectedClaims = claims.Where(r => r.IsSelected)

                                               .ToList();

                foreach (var claim in SelectedClaims)
                {
                    var systemClaim = await _context.SystemClaims.FirstOrDefaultAsync(c => c.Id == claim.Id);
                    if (systemClaim != null)
                    {
                        var newClaim = new Claim(systemClaim.ClaimType, systemClaim.ClaimValue);
                        var addClaimsResult = await _roleManager.AddClaimAsync(existingRole, newClaim);
                        Console.WriteLine($"Claim added: {systemClaim.ClaimType} - {systemClaim.ClaimValue}");
                        if (!addClaimsResult.Succeeded)
                        {
                            foreach (var error in addClaimsResult.Errors)
                                ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                // Remove claims that are not selected
                var existingClaims = await _roleManager.GetClaimsAsync(existingRole);
                // Find claims to remove
                var claimsToRemove = existingClaims
                    .Where(c => !SelectedClaims.Any(s => s.ClaimType == c.Type && s.ClaimValue == c.Value))
                    .ToList();

                // Remove each claim
                foreach (var claim in claimsToRemove)
                {
                    var removeResult = await _roleManager.RemoveClaimAsync(existingRole, claim);
                    if (!removeResult.Succeeded)
                    {
                        foreach (var error in removeResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }


                var result = await _roleManager.UpdateAsync(existingRole);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(role);
        }

        // GET: Roles/Create



    }
}
