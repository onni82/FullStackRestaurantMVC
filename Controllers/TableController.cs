using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    public class TableController : Controller
    {
        private readonly ApiService _apiService;

        public TableController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables");
            return View(tables ?? new List<Table>());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Table table)
        {
            var created = await _apiService.PostAsync<Table>("api/Tables", table);
            return created == null ? View(table) : RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var table = await _apiService.GetAsync<Table>($"api/Tables/{id}");
            return table == null ? NotFound() : View(table);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Table table)
        {
            var ok = await _apiService.PutAsync($"api/Tables/{id}", table);
            return ok ? RedirectToAction(nameof(Index)) : View(table);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"api/Tables/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
