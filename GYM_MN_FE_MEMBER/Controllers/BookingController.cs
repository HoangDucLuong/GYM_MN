using GYM_MN_FE_MEMBER.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GYM_MN_FE_MEMBER.Controllers
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
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Booking/GetBookings");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var bookings = await response.Content.ReadFromJsonAsync<BookingViewModel[]>();
            return View(bookings);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Booking/GetBooking/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var booking = await response.Content.ReadFromJsonAsync<BookingViewModel>();
            return View(booking);
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
            var memberId = await GetMemberIdFromUserId(userId.Value);
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
            var memberId = await GetMemberIdFromUserId(userId.Value);
            if (memberId == null)
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

            bookingViewModel.MemberId = memberId.Value; // Gán MemberId bằng giá trị lấy được từ UserId

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

            return RedirectToAction(nameof(Index));
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

        private async Task<int?> GetMemberIdFromUserId(int userId)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Members/GetMemberIdFromUserId/memberid/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonString);

                // Trích xuất memberId từ đối tượng JSON
                var memberId = (int?)jsonObject["memberId"];

                return memberId;
            }
            else
            {
                return null;
            }
        }


    }
}
