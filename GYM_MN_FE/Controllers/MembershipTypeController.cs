using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GYM_MN_FE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GYM_MN_FE.Controllers
{
    public class MembershipTypeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7178/api");
        private readonly HttpClient _httpClient;

        public MembershipTypeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/MembershipTypes/GetMembershipTypes");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var membershipTypes = JsonConvert.DeserializeObject<List<MembershipTypeViewModel>>(data);
                return View(membershipTypes);
            }
            else
            {
                // Xử lý lỗi nếu có
                return View(new List<MembershipTypeViewModel>());
            }
        }

        // Thêm các action khác tại đây nếu cần
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IsLoggedIn"] = true;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MembershipTypeViewModel member)
        {
            var json = JsonConvert.SerializeObject(member);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/MembershipTypes/PostMembershipType", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Member created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create member.";
            }

            return RedirectToAction("Index");
        }
    }
}
