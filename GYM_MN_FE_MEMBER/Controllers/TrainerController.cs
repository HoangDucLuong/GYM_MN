using GYM_MN_FE_MEMBER.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace GYM_MN_FE_MEMBER.Controllers
{
    public class TrainerController : Controller
    {
        private readonly HttpClient _httpClient;

        public TrainerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7178/api/");
        }

        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
            List<TrainerViewModel> trainers = new List<TrainerViewModel>();

            HttpResponseMessage response = await _httpClient.GetAsync("Trainers/GetTrainers");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                trainers = JsonConvert.DeserializeObject<List<TrainerViewModel>>(data);
            }

            return View(trainers);
        }

        [HttpPost]
        public IActionResult SelectTrainer(int trainerId)
        {
            ViewData["IsLoggedIn"] = true;
            HttpContext.Session.SetInt32("SelectedTrainerId", trainerId);
            return RedirectToAction("Create", "Booking");
        }
    }

}
