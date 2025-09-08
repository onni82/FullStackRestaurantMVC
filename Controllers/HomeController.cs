using System.Diagnostics;
using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Hämtar meny från API (null om error)
                var menuItems = await _apiService.GetAsync<List<MenuItem>>("api/MenuItems");

                // Visar topp 3 som populära val
                var popularItems = menuItems?
                    .Where(x => x.IsPopular)
                    .Take(3)
                    .ToList() ?? new List<MenuItem>();

                return View(popularItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load menu items for Home/Index.");
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
