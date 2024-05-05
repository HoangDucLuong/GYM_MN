using GYM_MN_FE_MEMBER.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace GYM_MN_FE_MEMBER.Controllers
{
    public class TrainerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrainerController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7178/api/");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Tiếp tục với xử lý bình thường
            ViewData["IsLoggedIn"] = true;
            List<TrainerViewModel> trainers = new List<TrainerViewModel>();

            HttpResponseMessage response = await _httpClient.GetAsync("Trainers/GetTrainers");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                trainers = JsonConvert.DeserializeObject<List<TrainerViewModel>>(data);
            }

            return View(trainers);
        }


        [HttpPost]
        public IActionResult SelectTrainer(int trainerId)
        {
            ViewData["IsLoggedIn"] = true;
            HttpContext.Session.SetInt32("SelectedTrainerId", trainerId);
            return RedirectToAction("Create", "Booking");
        }
        private int? GetUserIdFromToken()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            return null;
        }
    }

}
