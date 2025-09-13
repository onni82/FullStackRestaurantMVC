using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Models.ViewModels;
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
            //var token = HttpContext.Request.Cookies["jwtToken"];
            //if (string.IsNullOrEmpty(token))
            //{
            //    return RedirectToAction("Login", "Auth");
            //}
            // The main dashboard page
            return View();
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

		[HttpGet]
		public async Task<IActionResult> CreateBooking()
		{
			var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers") ?? new List<Customer>();
			var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables") ?? new List<Table>();

			var vm = new BookingViewModel
			{
				Start = DateTime.Now,
				Guests = 1, // default
				Customers = customers.Select(c => new CustomerViewModel
				{
					Id = c.Id,
					Name = $"{c.Name} ({c.PhoneNumber})",
					PhoneNumber = c.PhoneNumber
				}),
				Tables = tables.Where(t => t.Capacity >= 1).Select(t => new TableViewModel
				{
					Id = t.Id,
					TableNumber = t.TableNumber,
					Capacity = t.Capacity
				})
			};

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> CreateBooking(BookingViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers") ?? new List<Customer>();
				var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables") ?? new List<Table>();

				vm.Customers = customers.Select(c => new CustomerViewModel
				{
					Id = c.Id,
					Name = $"{c.Name} ({c.PhoneNumber})",
					PhoneNumber = c.PhoneNumber
				});

				// filter by entered guest count
				vm.Tables = tables.Where(t => t.Capacity >= vm.Guests).Select(t => new TableViewModel
				{
					Id = t.Id,
					TableNumber = t.TableNumber,
					Capacity = t.Capacity
				});

				return View(vm);
			}

			var dto = new { vm.Start, vm.Guests, vm.CustomerId, vm.TableId };
			var created = await _apiService.PostAsync<Booking>("api/Bookings", dto);

			return created == null ? View(vm) : RedirectToAction(nameof(Bookings));
		}

		[HttpGet]
		public async Task<IActionResult> EditBooking(int id)
		{
			var booking = await _apiService.GetAsync<Booking>($"api/Bookings/{id}");
			if (booking == null) return NotFound();

			var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers") ?? new List<Customer>();
			var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables") ?? new List<Table>();

			var vm = new BookingViewModel
			{
				Id = booking.Id,
				Start = booking.Start,
				Guests = booking.Guests,
				CustomerId = booking.CustomerId,
				TableId = booking.TableId,
				Customers = customers.Select(c => new CustomerViewModel
				{
					Id = c.Id,
					Name = $"{c.Name} ({c.PhoneNumber})",
					PhoneNumber = c.PhoneNumber
				}),
				Tables = tables.Select(t => new TableViewModel
				{
					Id = t.Id,
					TableNumber = t.TableNumber,
					Capacity = t.Capacity
				})
			};

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> EditBooking(int id, BookingViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers") ?? new List<Customer>();
				var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables") ?? new List<Table>();

				vm.Customers = customers.Select(c => new CustomerViewModel
				{
					Id = c.Id,
					Name = $"{c.Name} ({c.PhoneNumber})",
					PhoneNumber = c.PhoneNumber
				});

				vm.Tables = tables.Select(t => new TableViewModel
				{
					Id = t.Id,
					TableNumber = t.TableNumber,
					Capacity = t.Capacity
				});

				return View(vm);
			}

			var dto = new
			{
				vm.Start,
				vm.Guests,
				vm.CustomerId,
				vm.TableId
			};

			try
			{
				var ok = await _apiService.PutAsync($"api/Bookings/{id}", dto);
				return RedirectToAction(nameof(Bookings));
			}
			catch (HttpRequestException ex)
			{
				ModelState.AddModelError(string.Empty, $"Kunde inte uppdatera bokningen: {ex.Message}");
				return View(vm);
			}
		}

		[HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _apiService.DeleteAsync($"api/Bookings/{id}");
            return RedirectToAction(nameof(Bookings));
        }

        // -------- CUSTOMER MANAGEMENT --------

        public async Task<IActionResult> Customers()
        {
            var customers = await _apiService.GetAsync<IEnumerable<Customer>>("api/Customers")
                            ?? new List<Customer>();

            var viewModels = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber
            });

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult CreateCustomer() => View(new CustomerViewModel());

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new
            {
                vm.Name,
                vm.PhoneNumber
            };

            var created = await _apiService.PostAsync<Customer>("api/Customers", dto);
            return created == null ? View(vm) : RedirectToAction(nameof(Customers));
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var customer = await _apiService.GetAsync<Customer>($"api/Customers/{id}");
            if (customer == null) return NotFound();

            var vm = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(int id, CustomerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new
            {
                vm.Name,
                vm.PhoneNumber
            };

            var ok = await _apiService.PutAsync($"api/Customers/{id}", dto);
            return ok ? RedirectToAction(nameof(Customers)) : View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _apiService.DeleteAsync($"api/Customers/{id}");
            return RedirectToAction(nameof(Customers));
        }

        // -------- TABLE MANAGEMENT --------

        public async Task<IActionResult> Tables()
        {
            var tables = await _apiService.GetAsync<IEnumerable<Table>>("api/Tables") ?? new List<Table>();

            var vms = tables.Select(t => new TableViewModel
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity
            });

            return View(vms);
        }

        [HttpGet]
        public IActionResult CreateTable() => View(new TableViewModel());

        [HttpPost]
        public async Task<IActionResult> CreateTable(TableViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new { vm.TableNumber, vm.Capacity };

            var created = await _apiService.PostAsync<Table>("api/Tables", dto);
            return created == null ? View(vm) : RedirectToAction(nameof(Tables));
        }

        [HttpGet]
        public async Task<IActionResult> EditTable(int id)
        {
            var table = await _apiService.GetAsync<Table>($"api/Tables/{id}");
            if (table == null) return NotFound();

            var vm = new TableViewModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTable(int id, TableViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new { vm.TableNumber, vm.Capacity };

            var ok = await _apiService.PutAsync($"api/Tables/{id}", dto);
            return ok ? RedirectToAction(nameof(Tables)) : View(vm);
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
        public async Task<IActionResult> CreateMenu(MenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new
            {
                model.Name,
                model.Price,
                model.Description,
                model.IsPopular,
                model.ImageUrl
            };

            var created = await _apiService.PostAsync<MenuItem>("api/MenuItems", dto);
            return created == null ? View(model) : RedirectToAction(nameof(Menu));
        }

        [HttpGet]
        public async Task<IActionResult> EditMenu(int id)
        {
            var item = await _apiService.GetAsync<MenuItem>($"api/MenuItems/{id}");
            if (item == null) return NotFound();

            var vm = new MenuItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                IsPopular = item.IsPopular,
                ImageUrl = item.ImageUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenu(int id, MenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new
            {
                model.Name,
                model.Price,
                model.Description,
                model.IsPopular,
                model.ImageUrl
            };

            var ok = await _apiService.PutAsync($"api/MenuItems/{id}", dto);
            return ok ? RedirectToAction(nameof(Menu)) : View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            await _apiService.DeleteAsync($"api/MenuItems/{id}");
            return RedirectToAction(nameof(Menu));
        }
    }
}
