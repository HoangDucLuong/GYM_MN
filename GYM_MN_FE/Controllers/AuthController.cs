using GYM_MN_FE_ADMIN.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_ADMIN.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7178/api/Auth/");
        }

        // Hiển thị form đăng nhập
        public IActionResult Login()
        {
            ViewData["IsLoggedIn"] = false;
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
                // Ví dụ: HttpContext.Session.SetString("Token", token.Token);

                ViewData["IsLoggedIn"] = true; // Cập nhật trạng thái đăng nhập sau khi đăng nhập thành công

                return RedirectToAction("Index", "Member"); // Chuyển hướng đến trang chính sau khi đăng nhập thành công
            }
            else
            {
                ModelState.AddModelError("Password", "Invalid password."); // Thêm thông báo lỗi vào ModelState cho trường Password
                return View(login);
            }
        }

        // Hiển thị form đăng nhập
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            foreach (var sessionKey in HttpContext.Session.Keys)
            {
                HttpContext.Session.Remove(sessionKey);
            }

            return RedirectToAction("Login");
        }

    }
}