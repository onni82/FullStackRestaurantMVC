using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApiService _apiService;

        public MenuController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _apiService.GetAsync<IEnumerable<MenuItem>>("api/MenuItems");
            return View(items ?? new List<MenuItem>());
        }
    }
}
