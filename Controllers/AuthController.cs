using FullStackRestaurantMVC.Models;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace FullStackRestaurantMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var res = await _apiService.PostAsync<JsonElement>("api/Auth/login", new { model.Username, model.Password });

            if (res.ValueKind == JsonValueKind.Undefined)
            {
                ViewBag.Error = "Felaktiga inloggningsuppgifter";
                return View();
            }

            var token = res.GetProperty("token").GetString();
            _apiService.SetToken(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtObject = handler.ReadJwtToken(token);

            // Spara token i session för framtida anrop
            //HttpContext.Session.SetString("jwtToken", token);
            HttpContext.Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = jwtObject.ValidTo
            });

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _apiService.SetToken(null);
            //HttpContext.Session.Remove("jwtToken");
            HttpContext.Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
