using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
