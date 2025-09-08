using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;
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

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _apiService.GetAsync<JsonElement[]>("api/Bookings");

            var model = bookings.Select(b => new BookingViewModel
            {
                Id = b.GetProperty("id").GetInt32(),
                TableId = b.GetProperty("tableId").GetInt32(),
                TableName = b.GetProperty("tableName").GetString() ?? "Unknown", // adjust if API doesn't return name
                CustomerId = b.GetProperty("customerId").GetInt32(),
                CustomerName = b.GetProperty("customerName").GetString() ?? "Guest", // adjust if API doesn't return name
                Start = b.GetProperty("start").GetDateTime(),
                Guests = b.GetProperty("guests").GetInt32()
            }).ToList();

            return View(model);
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create()
        {
            // Load available tables for dropdown
            var tables = await _apiService.GetAsync<JsonElement[]>("api/Tables");
            ViewBag.Tables = tables.Select(t => new { Id = t.GetProperty("id").GetInt32(), Name = t.GetProperty("name").GetString() });

            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        public async Task<IActionResult> Create(int tableId, int guests, DateTime date, TimeSpan time)
        {
            var booking = new
            {
                tableId,
                customerId = 0, // if not logged in
                start = date.Date + time,
                guests
            };

            await _apiService.PostAsync("api/Bookings", booking);
            return RedirectToAction(nameof(Index));
        }
    }
}
