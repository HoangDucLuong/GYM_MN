using GYM_MN_FE_MEMBER.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_MEMBER.Controllers
{
    public class MemberController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemberController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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
                HttpResponseMessage memberResponse = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Members/GetMemberByUserId/{userId}");

                if (memberResponse.IsSuccessStatusCode)
                {
                    string memberData = await memberResponse.Content.ReadAsStringAsync();
                    var member = JsonConvert.DeserializeObject<MemberViewModel>(memberData); // Sử dụng MemberDto thay vì MemberViewModel

                    return View(member);
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
                HttpResponseMessage memberResponse = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Members/GetMemberByUserId/{userId}");

                if (memberResponse.IsSuccessStatusCode)
                {
                    string memberData = await memberResponse.Content.ReadAsStringAsync();
                    var member = JsonConvert.DeserializeObject<MemberViewModel>(memberData); // Sử dụng MemberDto thay vì MemberViewModel

                    return View(member);
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
        public async Task<IActionResult> Edit(MemberViewModel member)
        {
            ViewData["IsLoggedIn"] = true;
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            try
            {
                var userId = GetIdFromToken();
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var jsonMember = JsonConvert.SerializeObject(member);
                var content = new StringContent(jsonMember, Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT đến endpoint API để chỉnh sửa thông tin thành viên
                HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Members/PutMemberByUserId/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["errorMessage"] = "Không thể cập nhật thông tin thành viên.";
                    return View(member);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Error");
            }
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
