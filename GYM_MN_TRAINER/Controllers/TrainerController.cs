using GYM_MN_FE_TRAINER.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_TRAINER.Controllers
{
    public class TrainerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrainerController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7178/api");
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            ViewData["IsLoggedIn"] = true;

            var userId = GetIdFromToken();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpResponseMessage trainerResponse = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Trainers/GetTrainerByUserId/{userId}");

                if (trainerResponse.IsSuccessStatusCode)
                {
                    string trainerData = await trainerResponse.Content.ReadAsStringAsync();
                    var trainer = JsonConvert.DeserializeObject<TrainerViewModel>(trainerData); // Sử dụng MemberDto thay vì MemberViewModel

                    return View(trainer);
                }
                else
                {
                    TempData["errorMessage"] = "Không thể tìm thấy thông tin thành viên.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            ViewData["IsLoggedIn"] = true;
            var userId = GetIdFromToken();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                HttpResponseMessage trainerResponse = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Trainers/GetTrainerByUserId/{userId}");

                if (trainerResponse.IsSuccessStatusCode)
                {
                    string trainerData = await trainerResponse.Content.ReadAsStringAsync();
                    var trainer = JsonConvert.DeserializeObject<TrainerViewModel>(trainerData); 

                    return View(trainer);
                }
                else
                {
                    TempData["errorMessage"] = "Không thể tìm thấy thông tin thành viên.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TrainerViewModel trainer)
        {
            ViewData["IsLoggedIn"] = true;
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }

            try
            {
                var userId = GetIdFromToken();
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var jsonTrainer = JsonConvert.SerializeObject(trainer);
                var content = new StringContent(jsonTrainer, Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT đến endpoint API để chỉnh sửa thông tin thành viên
                HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Trainers/PutTrainerByUserId/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["errorMessage"] = "Không thể cập nhật thông tin thành viên.";
                    return View(trainer);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Error");
            }
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
        private string GetIdFromToken()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return null;
        }

        private string GetUsernameFromToken()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (usernameClaim != null)
                {
                    return usernameClaim.Value;
                }
            }

            return null;
        }

    }
}
