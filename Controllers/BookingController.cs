using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FullStackRestaurantMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApiService _apiService;

        public BookingController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new BookingViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var startDateTime = model.Date.Date + model.Time;

            var bookingData = new
            {
                tableId = model.TableId,
                customerId = model.CustomerId,
                start = startDateTime.ToUniversalTime().ToString("o"), // ISO 8601 format
                guests = model.Guests
            };

            await _apiService.PostAsync<object>("api/Bookings", bookingData);

            return RedirectToAction("Index", "Home");
        }
    }
}
