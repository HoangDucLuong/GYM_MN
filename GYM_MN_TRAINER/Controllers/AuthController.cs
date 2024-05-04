using GYM_MN_TRAINER.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_TRAINER.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7178/api/Auth/");
        }

        public IActionResult Login()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("Login/login", content);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenViewModel>(data);

                // Lưu token vào session hoặc cookie
                HttpContext.Session.SetString("Token", token.Token);

                // Cập nhật trạng thái đăng nhập sau khi đăng nhập thành công
                ViewData["IsLoggedIn"] = true;

                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chính sau khi đăng nhập thành công
            }
            else
            {
                ModelState.AddModelError("Password", "Invalid password."); // Thêm thông báo lỗi vào ModelState cho trường Password
                return View(login);
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Xóa token từ session hoặc cookie
            // Ví dụ: HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");
        }
    }
}
