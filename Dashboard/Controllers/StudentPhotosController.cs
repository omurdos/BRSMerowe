using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class StudentPhotosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
