using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace Dashboard.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<APIUser> _userManager;
        private readonly SignInManager<APIUser> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(UserManager<APIUser> userManager, SignInManager<APIUser> signInManager, RoleManager<Role> roleManager, ILogger<LoginController> logger)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _roleManager=roleManager;
            _logger=logger;
        }


        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }


        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid) {
                    _logger.LogInformation("Login attempt for user: {UserName}", viewModel.UserName);
                    var user = await _userManager.FindByNameAsync(viewModel.UserName);
                    var loginAttempt = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, false);
                    if (loginAttempt.Succeeded)
                    {
                        _logger.LogInformation("Login successful for user: {UserName}", viewModel.UserName);
                        var signinResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);
                        if (signinResult.Succeeded) {
                            _logger.LogInformation("Password sign in successful for user: {UserName}", viewModel.UserName);
                            var roles = await _userManager.GetRolesAsync(user);
                            string roleslist = string.Join(",", roles);
                            _logger.LogInformation("Roles: {Roles}", roleslist);
                            HttpContext.Session.SetString("Roles", roleslist);

                            foreach (var roleName in roles)
                            {
                                var role = await _roleManager.FindByNameAsync(roleName);
                                var claims = await _roleManager.GetClaimsAsync(role);

                                foreach (var claim in claims)
                                {
                                    if (!User.HasClaim(claim.Type, claim.Value))
                                       User.Claims.ToList().Add(new Claim(claim.Type, claim.Value));
                                }
                            }

                            _logger.LogInformation("Redirecting to dashboard");

                            return RedirectToAction(controllerName: "Dashboard", actionName: nameof(DashboardController.Index));
                        }
                    }
                    _logger.LogInformation("Login failed for user: {UserName}", viewModel.UserName);
                    return View();
                }
                return View(ModelState);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Login");
                throw ex;
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    
    }
}
