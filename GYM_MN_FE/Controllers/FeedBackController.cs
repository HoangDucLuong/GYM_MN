using GYM_MN_FE_ADMIN.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_ADMIN.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly HttpClient _httpClient;

        public FeedbackController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7178/api");
        }

        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Feedback/GetFeedbacks");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var feedbacks = JsonConvert.DeserializeObject<List<FeedBackViewModel>>(data);
                return View(feedbacks);
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IsLoggedIn"] = true;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedBackViewModel feedback)
        {
            ViewData["IsLoggedIn"] = true;
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/PostFeedback", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["IsLoggedIn"] = true;
            var response = await _httpClient.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var feedback = JsonConvert.DeserializeObject<FeedBackViewModel>(data);
                return View(feedback);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FeedBackViewModel feedback)
        {
            ViewData["IsLoggedIn"] = true;
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["IsLoggedIn"] = true;
            var response = await _httpClient.DeleteAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }
    }
}
