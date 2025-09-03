using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
