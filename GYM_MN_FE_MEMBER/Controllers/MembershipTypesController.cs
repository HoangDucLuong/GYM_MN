using GYM_MN_FE_MEMBER.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_ADMIN.Controllers
{
    public class MembershipTypesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MembershipTypesController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7178/api/");
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<MembershipTypeViewModel> membershipTypeList = new List<MembershipTypeViewModel>();

            HttpResponseMessage response = await _httpClient.GetAsync("MembershipTypes/GetMembershipTypes");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                membershipTypeList = JsonConvert.DeserializeObject<List<MembershipTypeViewModel>>(data);
            }
            return View(membershipTypeList);
        }

        [HttpPost]
        public IActionResult SelectMembershipType(int membershipTypeId)
        {
            ViewData["IsLoggedIn"] = true;
            HttpContext.Session.SetInt32("SelectedMembershipTypeId", membershipTypeId);
            return RedirectToAction("Index", "Trainer");
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
