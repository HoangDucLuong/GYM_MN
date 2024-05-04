using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public LoginModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> OnPostAsync(string username, string password)
    {
        var client = _clientFactory.CreateClient();

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });

        var response = await client.PostAsync("https://localhost:7178/api/Auth/Login/login", content);
        if (response.IsSuccessStatusCode)
        {
            // ??ng nh?p thành công, chuy?n h??ng ??n trang chính
            return RedirectToPage("/Index");
        }
        else
        {
            // ??ng nh?p không thành công, hi?n th? thông báo l?i
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return Page();
        }
    }
}
