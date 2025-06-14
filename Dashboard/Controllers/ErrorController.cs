using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            return View(statusCode.ToString());
        }

        [Route("Error/500")]
        public IActionResult HandleException()
        {
            return View("500");
        }
    }
}
