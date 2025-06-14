using FormsAPP.Models;
using FormsAPP.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FormsAPP.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClientService httpClientService)
        {
            _httpClient = httpClientService.GetClient()!;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Account/Register", model);
                if (response.IsSuccessStatusCode)
                {
                    string token = await response.Content.ReadAsStringAsync();
                    HttpContext.Response.Cookies.Append("jwt", token);
                    return RedirectToAction("Index","Home");
                }
                var message = await response.Content.ReadAsStringAsync();
                ViewData["ErrorMessage"] = message;
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Account/Login", model);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    SetTokenByRememberMe(model.RememberMe, token);
                    return RedirectToAction("Index", "Home");
                }
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
                return View(model);
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Account");
        }

        private void SetTokenByRememberMe(bool rememberMe, string token)
        {
            if (rememberMe)
            {
                var options = new CookieOptions() { Expires = DateTime.Now.AddDays(14), Secure = true };
                HttpContext.Response.Cookies.Append("jwt", token, options);
            }
            HttpContext.Response.Cookies.Append("jwt", token);
        }
    }
}
