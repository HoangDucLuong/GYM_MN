using GYM_MN_FE_MEMBER.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_MEMBER.Controllers
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
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Feedback/GetFeedback");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var feedbacks = JsonConvert.DeserializeObject<List<FeedBackViewModel>>(data);
                return View(feedbacks);
            }
            return View("Error");
        }
        [HttpGet]
        public IActionResult Create(int memberId, int trainerId)
        {
            var feedbackViewModel = new FeedBackViewModel
            {
                MemberId = memberId,
                TrainerId = trainerId
            };
            return View("Create", feedbackViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(FeedBackViewModel feedback )
        {
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Feedback/PostFeedback", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Feedback sent successfully.";
                return RedirectToAction("Index", "MembershipTypes");
            }
            return View("Error");
        }
    }
}
