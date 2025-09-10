using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FullStackRestaurantMVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _jwtToken;

        public ApiService(HttpClient httpClient, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config["ApiBaseUrl"] ?? "https://localhost:7232");
            _httpContextAccessor = httpContextAccessor;
        }

        public void EnsureToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var token = httpContext.Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        public void SetToken(string token)
        {
            _jwtToken = token;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            EnsureToken();
            var res = await _httpClient.GetAsync(endpoint);
            if (!res.IsSuccessStatusCode) return default;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            EnsureToken();
            var json = JsonSerializer.Serialize(data);
            var res = await _httpClient.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
            if (!res.IsSuccessStatusCode) return default;
            var body = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> PutAsync(string endpoint, object data)
        {
            EnsureToken();
            var json = JsonSerializer.Serialize(data);
            var res = await _httpClient.PutAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            EnsureToken();
            var res = await _httpClient.DeleteAsync(endpoint);
            return res.IsSuccessStatusCode;
        }
    }
}
