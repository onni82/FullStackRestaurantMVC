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

        [HttpGet]
        public IActionResult Create() => View(new BookingCreateViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Fyll i alla fält korrekt.";
                return View(model);
            }

            try
            {
                // Combine date and time into a single DateTime
                var start = model.Date.Date + model.Time;

                // Call API to get available tables
                var tables = await _apiService.GetAsync<JsonElement[]>(
                    $"api/Tables/available?start={start:O}&guests={model.Guests}");

                if (tables.Length == 0)
                {
                    ViewBag.Error = "Tyvärr, inga bord är lediga vid denna tid.";
                    return View(model);
                }

                var tableId = tables[0].GetProperty("id").GetInt32();

                // Create booking
                var bookingPayload = new
                {
                    tableId,
                    customerId = 0, // customers don't log in
                    start = start.ToString("O"),
                    guests = model.Guests,
                    name = model.Name,
                    phone = model.Phone
                };

                var result = await _apiService.PostAsync<JsonElement>("api/Bookings", bookingPayload);

                ViewBag.Message = "Bokningen är registrerad! Vi ser fram emot att träffa dig.";
                return View(new BookingCreateViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ett fel uppstod vid bokningen: " + ex.Message;
                return View(model);
            }
        }
    }
}
