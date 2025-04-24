using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RestaurantMenu.Helper;
using RestaurantMenu.Models;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text;

namespace RestaurantMenu.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IDatabase _redisCache;
        public IConfiguration _configuration { get; }

        public HomeController(HttpClient httpClient, IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _redisCache = redis.GetDatabase();
        }

        public async Task<IActionResult> Index()
        {
            string decryptedUserIdStr;
            try
            {
                var user = _configuration["UserId"].ToString();
                decryptedUserIdStr = EncryptionEngine.Decrypt(user);

            }
            catch
            {
                return BadRequest("Decryption failed.");
            }
             
            if (!int.TryParse(decryptedUserIdStr, out int userId))
            {
                return BadRequest("Invalid User ID format");
            }

            var cachedData = await _redisCache.StringGetAsync($"UserCategories:{userId}");
            if (!cachedData.IsNull)
            {
                var cachedResult = JsonConvert.DeserializeObject<UserResponseModel>(cachedData);
                return View(cachedResult);
            }

            string apiUrl = $"{_configuration["GetCategoryAndItemsEndpoint"]}{userId}";
            UserResponseModel userData = new UserResponseModel();

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                userData = JsonConvert.DeserializeObject<UserResponseModel>(jsonString);

                await _redisCache.StringSetAsync($"UserCategories:{userId}", JsonConvert.SerializeObject(userData));
            }

            return View(userData);
        }


        public async Task<IActionResult> Index2()
        {
            string decryptedUserIdStr;
            try
            {
                var user = _configuration["UserId"].ToString();
                decryptedUserIdStr = EncryptionEngine.Decrypt(user);

            }
            catch
            {
                return BadRequest("Decryption failed.");
            }

            if (!int.TryParse(decryptedUserIdStr, out int userId))
                return BadRequest("Invalid User ID format");

            var cachedData = await _redisCache.StringGetAsync($"UserCategories:{userId}");
            if (!cachedData.IsNull)
            {
                var cachedResult = JsonConvert.DeserializeObject<UserResponseModel>(cachedData);
                return View(cachedResult);
            }

            string apiUrl = $"{_configuration["GetCategoryAndItemsEndpoint"]}{userId}";
            UserResponseModel userData = new UserResponseModel();

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                userData = JsonConvert.DeserializeObject<UserResponseModel>(jsonString);

                await _redisCache.StringSetAsync($"UserCategories:{userId}", JsonConvert.SerializeObject(userData));
            }

            return View(userData);
        }

        public async Task<IActionResult> TheHive()
        {

            string decryptedUserIdStr;
            try
            {
                decryptedUserIdStr = _configuration["UserId"].ToString();
            }
            catch
            {
                return BadRequest("Decryption failed.");
            }

            if (!int.TryParse(decryptedUserIdStr, out int userId))
            {
                return BadRequest("Invalid User ID format");
            }

            var cachedData = await _redisCache.StringGetAsync($"UserCategories:{userId}");
            if (!cachedData.IsNull)
            {
                var cachedResult = JsonConvert.DeserializeObject<UserResponseModel>(cachedData);
                return View(cachedResult);
            }

            string apiUrl = $"{_configuration["GetCategoryAndItemsEndpoint"]}{userId}";
            UserResponseModel userData = new UserResponseModel();

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                userData = JsonConvert.DeserializeObject<UserResponseModel>(jsonString);

                await _redisCache.StringSetAsync($"UserCategories:{userId}", JsonConvert.SerializeObject(userData));
            }

            return View(userData);
        }


        public async Task<IActionResult> Index3()
        {

            string decryptedUserIdStr;
            try
            {
                var user = _configuration["UserId"].ToString();
                decryptedUserIdStr = EncryptionEngine.Decrypt(user);

            }
            catch
            {
                return BadRequest("Decryption failed.");
            }
            
            if (!int.TryParse(decryptedUserIdStr, out int userId))
            {
                return BadRequest("Invalid User ID format");
            }

            var cachedData = await _redisCache.StringGetAsync($"UserCategories:{userId}");
            if (!cachedData.IsNull)
            {
                var cachedResult = JsonConvert.DeserializeObject<UserResponseModel>(cachedData);
                return View(cachedResult);
            }

            string apiUrl = $"{_configuration["GetCategoryAndItemsEndpoint"]}{userId}";
            UserResponseModel userData = new UserResponseModel();

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                userData = JsonConvert.DeserializeObject<UserResponseModel>(jsonString);

                await _redisCache.StringSetAsync($"UserCategories:{userId}", JsonConvert.SerializeObject(userData));
            }

            return View(userData);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] Messages message)
        {
            if (message == null)
            {
                return BadRequest("Invalid message data.");
            }

            string apiUrl = _configuration["MessageApiUrl"]; // Ensure this is set in appsettings.json

            var jsonContent = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true, message = "Message sent successfully!" });
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { success = false, message = errorResponse });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
