using FullStackRestaurantMVC.Models.ViewModels;
using FullStackRestaurantMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

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

            var claims = jwtObject.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = jwtObject.ValidTo
                });

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

        public async Task<IActionResult> Logout()
        {
            _apiService.SetToken(null);
            //HttpContext.Session.Remove("jwtToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
