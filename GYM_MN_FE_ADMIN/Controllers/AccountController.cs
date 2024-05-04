// AccountController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var client = _httpClientFactory.CreateClient();
        var loginUrl = "https://localhost:7178/api/Auth/Login/login"; // Thay đổi đường dẫn API tương ứng
        var requestBody = new { Username = username, Password = password };

        var response = await client.PostAsJsonAsync(loginUrl, requestBody);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không chính xác.");
            return View();
        }
    }
}
