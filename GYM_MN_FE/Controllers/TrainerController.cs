using GYM_MN_FE_ADMIN.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_ADMIN.Controllers
{
    public class TrainerController : Controller
    {
        private readonly HttpClient _httpClient;

        public TrainerController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7178/api");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IsLoggedIn"] = true;
            List<TrainerViewModel> trainerList = new List<TrainerViewModel>();

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Trainers/GetTrainers");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                trainerList = JsonConvert.DeserializeObject<List<TrainerViewModel>>(data);
            }

            return View(trainerList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IsLoggedIn"] = true;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel registerTrainer)
        {
            ViewData["IsLoggedIn"] = true;
            if (!ModelState.IsValid)
            {
                return View(registerTrainer);
            }

            // Tạo một đối tượng mới với dữ liệu từ RegisterViewModel
            var newTrainer = new RegisterViewModel
            {
                Username = registerTrainer.Username,
                Password = registerTrainer.Password,
                FullName = registerTrainer.FullName,
                Email = registerTrainer.Email,
                Phone = registerTrainer.Phone,
                Gender = registerTrainer.Gender,
                Dob = registerTrainer.Dob,
                Specialization = registerTrainer.Specialization,
                WorkStartTime = registerTrainer.WorkStartTime,
                WorkEndTime = registerTrainer.WorkEndTime,  

            };

            var json = JsonConvert.SerializeObject(newTrainer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/RegisterTrainer/register-trainer", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successful.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to register.";
                return View(registerTrainer);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["IsLoggedIn"] = true;
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Trainers/GetTrainer/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var trainer = JsonConvert.DeserializeObject<TrainerViewModel>(data);
                return View(trainer);
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to retrieve Trainer details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerViewModel trainer)
        {
            ViewData["IsLoggedIn"] = true;
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }

            var json = JsonConvert.SerializeObject(trainer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Trainers/PutTrainer/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Trainer.";
                return View(trainer);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["IsLoggedIn"] = true;
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Trainers/GetTrainer/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                TrainerViewModel trainer = JsonConvert.DeserializeObject<TrainerViewModel>(data);
                return View(trainer);
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
            ViewData["IsLoggedIn"] = true;
            // Gửi yêu cầu DELETE đến API để xóa huấn luyện viên có id tương ứng
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Trainers/DeleteTrainer/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Trainer.";
            }

            // Sau khi xóa, chuyển hướng người dùng đến trang Index
            return RedirectToAction("Index");
        }

    }
}
