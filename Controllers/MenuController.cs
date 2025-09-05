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

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(MenuItem item)
        {
            var created = await _apiService.PostAsync<MenuItem>("api/MenuItems", item);
            if (created == null) return View(item);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _apiService.GetAsync<MenuItem>($"api/MenuItems/{id}");
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MenuItem item)
        {
            var ok = await _apiService.PutAsync($"api/MenuItem/{id}", item);
            return ok ? RedirectToAction(nameof(Index)) : View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"api/MenuItems/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
