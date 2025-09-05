using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApiService _apiService;

        public BookingController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _apiService.GetAsync<IEnumerable<Booking>>("api/Bookings");
            return View(bookings ?? new List<Booking>());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            var created = await _apiService.PostAsync<Booking>("api/Bookings", booking);
            if (created == null)
            {
                ViewBag.Error = "Kunde inte skapa bokning (bordet kan vara upptaget).";
                return View(booking);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _apiService.DeleteAsync($"api/Bookings/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
