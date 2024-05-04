using GYM_MN_FE_ADMIN.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GYM_MN_FE_ADMIN.Controllers
{
    public class MemberController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7178/api");
        private readonly HttpClient _httpClient;

        public MemberController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
            List<MemberViewModel> memberList = new List<MemberViewModel>();

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members/GetMembers").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                memberList = JsonConvert.DeserializeObject<List<MemberViewModel>>(data);
            }
            return View(memberList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IsLoggedIn"] = true;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MemberViewModel member)
        {
            var json = JsonConvert.SerializeObject(member);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Members/PostMember", content);
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["IsLoggedIn"] = true;
            try
            {
                MemberViewModel model = new MemberViewModel();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members/GetMember/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<MemberViewModel>(data);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, MemberViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về View với model nếu có lỗi
                    return View(model);
                }

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync($"{_httpClient.BaseAddress}/Members/PutMember", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Nhân viên đã được cập nhật.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Nếu không thành công, thêm thông báo lỗi vào TempData
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi cập nhật nhân viên.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi trong quá trình xử lý, thêm thông báo lỗi vào TempData
                TempData["errorMessage"] = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["IsLoggedIn"] = true;
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members/GetMember/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                MemberViewModel member = JsonConvert.DeserializeObject<MemberViewModel>(data);
                return View(member);
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to retrieve member information.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Members/DeleteMember/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Member deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete member.";
            }

            return RedirectToAction("Index");
        }
    }
}