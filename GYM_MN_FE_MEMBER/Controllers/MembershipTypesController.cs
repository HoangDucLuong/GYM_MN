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

        public MembershipTypesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7178/api/");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
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
    }
}
