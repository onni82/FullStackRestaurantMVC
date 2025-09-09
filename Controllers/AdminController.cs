using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullStackRestaurantMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            // The main dashboard page
            return View();
        }

        // -------- CUSTOMER MANAGEMENT --------

        public async Task<IActionResult> Customers()
        {
            var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers")
                            ?? new List<Customer>();

            return View(customers);
        }

        [HttpGet]
        public IActionResult CreateCustomer() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            var created = await _apiService.PostAsync<Customer>("api/Customers", customer);
            return created == null ? View(customer) : RedirectToAction(nameof(Customers));
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var customer = await _apiService.GetAsync<Customer>($"api/Customers/{id}");
            return customer == null ? NotFound() : View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(int id, Customer customer)
        {
            var ok = await _apiService.PutAsync($"api/Customers/{id}", customer);
            return ok ? RedirectToAction(nameof(Customers)) : View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _apiService.DeleteAsync($"api/Customers/{id}");
            return RedirectToAction(nameof(Customers));
        }

        // -------- BOOKING MANAGEMENT --------

        public async Task<IActionResult> Bookings()
        {
            var bookings = await _apiService.GetAsync<IEnumerable<Booking>>("api/Bookings")
                           ?? new List<Booking>();

            var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers")
                            ?? new List<Customer>();

            var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables")
                         ?? new List<Table>();

            var viewModel = from b in bookings
                            join c in customers on b.CustomerId equals c.Id
                            join t in tables on b.TableId equals t.Id
                            select new BookingWithDetailsVM
                            {
                                Id = b.Id,
                                Start = b.Start,
                                Guests = b.Guests,
                                CustomerName = c.Name,
                                CustomerPhone = c.PhoneNumber,
                                TableNumber = t.TableNumber
                            };

            return View(viewModel);
        }

        // -------- TABLE MANAGEMENT --------

        public async Task<IActionResult> Tables()
        {
            var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables")
                         ?? new List<Table>();
            return View(tables);
        }

        [HttpGet]
        public IActionResult CreateTable() => View();

        [HttpPost]
        public async Task<IActionResult> CreateTable(Table table)
        {
            var created = await _apiService.PostAsync<Table>("api/Tables", table);
            return created == null ? View(table) : RedirectToAction(nameof(Tables));
        }

        [HttpGet]
        public async Task<IActionResult> EditTable(int id)
        {
            var table = await _apiService.GetAsync<Table>($"api/Tables/{id}");
            return table == null ? NotFound() : View(table);
        }

        [HttpPost]
        public async Task<IActionResult> EditTable(int id, Table table)
        {
            var ok = await _apiService.PutAsync($"api/Tables/{id}", table);
            return ok ? RedirectToAction(nameof(Tables)) : View(table);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTable(int id)
        {
            await _apiService.DeleteAsync($"api/Tables/{id}");
            return RedirectToAction(nameof(Tables));
        }

        // -------- MENU MANAGEMENT --------

        public async Task<IActionResult> Menu()
        {
            var items = await _apiService.GetAsync<IEnumerable<MenuItem>>("api/MenuItems")
                        ?? new List<MenuItem>();
            return View(items);
        }

        [HttpGet]
        public IActionResult CreateMenu() => View();

        [HttpPost]
        public async Task<IActionResult> CreateMenu(MenuItem item)
        {
            var created = await _apiService.PostAsync<MenuItem>("api/MenuItems", item);
            return created == null ? View(item) : RedirectToAction(nameof(Menu));
        }

        [HttpGet]
        public async Task<IActionResult> EditMenu(int id)
        {
            var item = await _apiService.GetAsync<MenuItem>($"api/MenuItems/{id}");
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenu(int id, MenuItem item)
        {
            var ok = await _apiService.PutAsync($"api/MenuItems/{id}", item);
            return ok ? RedirectToAction(nameof(Menu)) : View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            await _apiService.DeleteAsync($"api/MenuItems/{id}");
            return RedirectToAction(nameof(Menu));
        }
    }
}
