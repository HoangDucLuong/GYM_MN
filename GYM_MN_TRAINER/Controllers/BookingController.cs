using GYM_MN_FE_TRAINER.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_TRAINER.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7178/api");
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> GetBookingsByTrainerId()
        {
            ViewData["IsLoggedIn"] = true;
            
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Lấy MemberId từ UserId
            var trainerId = await GetTrainerIdFromUserId(userId.Value);
            if (trainerId == null)
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

            try
            {
                // Gửi yêu cầu GET đến API để lấy danh sách đặt phòng của thành viên với MemberId tương ứng
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Booking/GetBookingByTrainerId/GetBookingByTrainerId/{trainerId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var bookings = JsonConvert.DeserializeObject<List<BookingViewModel>>(jsonString);

                    // Hiển thị danh sách đặt phòng của thành viên
                    return View(bookings);
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to retrieve bookings.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["IsLoggedIn"] = true;

            // Lấy UserID từ token
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Lấy MemberId từ UserId
            var memberId = await GetTrainerIdFromUserId(userId.Value);
            if (memberId == null)
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

            // Lấy membershipTypeId và trainerId từ session
            var membershipTypeId = HttpContext.Session.GetInt32("SelectedMembershipTypeId");
            var trainerId = HttpContext.Session.GetInt32("SelectedTrainerId");

            if (membershipTypeId == null || trainerId == null)
            {
                // Nếu không tìm thấy membershipTypeId hoặc trainerId trong session, chuyển hướng người dùng đến trang tương ứng để chọn
                return RedirectToAction("Index", "MembershipTypes");
            }

            var bookingViewModel = new BookingViewModel
            {
                MemberId = memberId,
                MembershipTypeId = membershipTypeId.Value,
                TrainerId = trainerId.Value
            };

            return View(bookingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel bookingViewModel)
        {
            ViewData["IsLoggedIn"] = true;

            // Lấy UserID từ token
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Gán MemberId từ UserId
            var trainerId = await GetTrainerIdFromUserId(userId.Value);
            if (trainerId == null)
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

            bookingViewModel.TrainerId = trainerId.Value; // Gán MemberId bằng giá trị lấy được từ UserId

            try
            {
                var json = JsonConvert.SerializeObject(bookingViewModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST đến API để tạo mới booking
                var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Booking/PostBooking", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Booking created successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create booking.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View("Error");
            }

            return RedirectToAction("Index", "MembershipTypes");

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

        private async Task<int?> GetTrainerIdFromUserId(int userId)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Trainers/GetTrainerIdFromUserId/trainerid/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonString);

                // Trích xuất memberId từ đối tượng JSON
                var trainerId = (int?)jsonObject["trainerId"];

                return trainerId;
            }
            else
            {
                return null;
            }
        }


    }
}
