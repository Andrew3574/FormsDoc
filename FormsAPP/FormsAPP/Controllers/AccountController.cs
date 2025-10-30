using AutoMapper;
using FormsAPP.Models.Account;
using FormsAPP.Models.Account.Salesforce;
using FormsAPP.Models.FormAnswers;
using FormsAPP.Models.Forms.CRUD;
using FormsAPP.Models.Users;
using FormsAPP.Profiles;
using FormsAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Reflection;
using System.Threading.Tasks;

namespace FormsAPP.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public AccountController(HttpClientService httpClientService, IMapper mapper)
        {
            _httpClient = httpClientService.GetClient()!;
            _mapper = mapper;
        }

        [HttpPost]  
        public async Task<IActionResult> ReportBug(BugReport report)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/ReportBug",report);
            if(response.IsSuccessStatusCode) TempData["SuccessMessage"] = await response.Content.ReadAsStringAsync();            
            else TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Redirect(report.Link);
        }

        public IActionResult CreateSFContact(UserModel model)
        {
            return View(_mapper.Map<SalesforceContact>(model));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSFContact(SalesforceContact contact)
        {
            if(ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Account/CreateSalesforceContact", contact);
                if (response.IsSuccessStatusCode) return RedirectToAction("UserProfile");
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return View(contact);
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
                    return View("Login", _mapper.Map<LoginModel>(model));
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
                    var authModel = await response.Content.ReadFromJsonAsync<AuthModel>();
                    LoginProcess(authModel!, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
                return View(model);
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmation(string email)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/GetCode", email);
            if (response.IsSuccessStatusCode)
            {
                ViewData["SuccessMessage"] = await response.Content.ReadAsStringAsync();
                return View("EmailConfirmation", new RecoveryModel { Email = email, Code="" });
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoveryCodeConfirmation(RecoveryModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/CheckCode", model);
            if (response.IsSuccessStatusCode)
            {
                return View("RecoveryPasswordPage", new LoginModel { Email = model.Email });
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return View("EmailConfirmation",model);
        }

        public IActionResult RecoveryPaswordPage(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/RecoverPassword", model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login","Account",model);
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return View("RecoveryPasswordPage",model);
        }

        public async Task<IActionResult> UserProfile()
        {
            if (HttpContext.Request.Cookies.TryGetValue("jwt", out string? token))
            {
                var response = await _httpClient.GetAsync($"Account/GetUserProfileByToken?token={token}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserModel>();
                    if(!HttpContext.Session.TryGetValue("UserId",out var id))
                    {
                        SetSessionValues(user!);
                    }
                    return View("UserProfile", user);
                }
            }            
            return View("Login");
        }

        public async Task<IActionResult> UserProfileByUserId([FromQuery] int userId)
        {
            if (userId != 0)
            {
                var response = await _httpClient.GetAsync($"Account/GetUserProfileByUserId?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserModel>();
                    if (!HttpContext.Session.TryGetValue("UserId", out var id))
                    {
                        SetSessionValues(user!);
                    }
                    return View("UserProfile", user);
                }
            }
            return View("Login");
        }

        public async Task<IActionResult> GetUserForms()
        {
            var response = await _httpClient.GetAsync($"Account/GetUserForms?userId={HttpContext.Session.GetInt32("UserId")}");
            if (response.IsSuccessStatusCode)
            {
                return View("GetUserForms", await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>());
            }
            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction("UserProfile");            
        }
        public async Task<IActionResult> GetUserFormsbyId([FromQuery] int userId)
        {
            if (userId != 0)
            {
                var response = await _httpClient.GetAsync($"Account/GetUserForms?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    return View("GetUserForms", await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>());
                }
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("UserProfile");
        }

        public async Task<IActionResult> GetAnsweredForms()
        {
            var response = await _httpClient.GetAsync($"Account/GetAnsweredForms?userId={HttpContext.Session.GetInt32("UserId")}");
            if (response.IsSuccessStatusCode)
            {
                return View(await response.Content.ReadFromJsonAsync<IEnumerable<AnsweredFormModel>>());
            }
            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction("UserProfile");
        }

        public async Task<IActionResult> GetAnsweredFormsById([FromQuery] int userId)
        {
            if(userId != 0)
            {
                var response = await _httpClient.GetAsync($"Account/GetAnsweredForms?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    return View("GetAnsweredForms", await response.Content.ReadFromJsonAsync<IEnumerable<AnsweredFormModel>>());
                }
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("UserProfile");
        }

        private void LoginProcess(AuthModel model, bool rememberMe)
        {
            SetSessionValues(model);
            if (rememberMe)
            {
                var options = new CookieOptions() { Expires = DateTime.Now.AddDays(14), Secure = true, HttpOnly = true };
                HttpContext.Response.Cookies.Append("jwt", model.Token, options);
            }
            else
            {
                HttpContext.Response.Cookies.Append("jwt", model.Token);
            }
        }

        private void SetSessionValues(AuthModel model)
        {
            HttpContext.Session.SetString("Role", model.Role);
            HttpContext.Session.SetString("Email", model.Email);
            HttpContext.Session.SetInt32("UserId", model.UserId);
        }
        private void SetSessionValues(UserModel model)
        {
            HttpContext.Session.SetString("Role", model.Role);
            HttpContext.Session.SetString("Email", model.Email);
            HttpContext.Session.SetInt32("UserId", model.Id);
        }
    }
}
