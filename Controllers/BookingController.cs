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
            var vm = new CreateBookingViewModel
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // reload tables if form is invalid
                var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables/available");
                vm.AvailableTables = tables?.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"Table {t.TableNumber} ({t.Seats} seats)"
                }).ToList() ?? new List<SelectListItem>();
                return View(vm);
            }

            // 1. Create customer
            var customer = await _apiService.PostAsync<JsonElement>("api/Customers", new
            {
                Name = vm.CustomerName,
                PhoneNumber = vm.PhoneNumber
            });

            if (customer.ValueKind == JsonValueKind.Undefined)
            {
                ModelState.AddModelError("", "Could not create customer.");
                return View(vm);
            }

            var customerId = customer.GetProperty("id").GetInt32();

            // 2. Create booking
            var booking = await _apiService.PostAsync<JsonElement>("api/Bookings", new
            {
                TableId = vm.TableId,
                CustomerId = customerId,
                Start = vm.Start,
                Guests = vm.Guests
            });

            if (booking.ValueKind == JsonValueKind.Undefined)
            {
                ModelState.AddModelError("", "Could not create booking.");
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
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
