using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace FullStackRestaurantMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApiService _apiService;

        public BookingController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _apiService.GetAsync<IEnumerable<Booking>>("api/Bookings");
            return View(bookings ?? new List<Booking>());
        }

        // GET: /Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _apiService.GetAsync<Booking>($"api/Bookings/{id}");
            if (booking == null) return NotFound();
            return View(booking);
        }

        // GET: /Booking/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables/available");
            var vm = new BookingViewModel
            {
                Start = DateTime.Now.AddHours(1),
                Guests = 2,
                AvailableTables = tables?.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"Table {t.TableNumber} ({t.Seats} seats)"
                }).ToList() ?? new List<SelectListItem>()
            };
            return View(vm);
        }

        // POST: /Booking/Create
        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Combine date + time into one DateTime
            var bookingDateTime = model.Date.Date + model.Time;

            var booking = new
            {
                Date = bookingDateTime,
                Guests = model.Guests,
                Name = model.Name,
                Phone = model.Phone
            };

            await _apiService.PostAsync<object>("api/Booking", booking);

            return RedirectToAction("Index");
        }

        // GET: /Booking/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _apiService.GetAsync<Booking>($"api/Bookings/{id}");
            if (booking == null) return NotFound();
            return View(booking);
        }

        // POST: /Booking/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _apiService.DeleteAsync($"api/Bookings/{id}");
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
