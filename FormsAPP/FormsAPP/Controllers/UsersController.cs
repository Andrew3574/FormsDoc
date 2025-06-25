using FormsAPP.Models.Users;
using FormsAPP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormsAPP.Controllers
{   

    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsersController(HttpClientService httpClientService)
        {
            _httpClient = httpClientService.GetClient()!;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"Users/GetByBatch/0");
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
                return View(users);
            }
            response.Headers.TryGetValues("ErrorMessage", out var message);
            TempData["ErrorMessage"] = message!.FirstOrDefault();
            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        public async Task<IActionResult> PromoteUser(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                var response = await _httpClient.PostAsJsonAsync("Users/PromoteToAdmin", indexes);
                await CheckResponseStatus(response);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DemoteUser(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                var response = await _httpClient.PostAsJsonAsync("Users/DemoteToUser", indexes);
                await CheckResponseStatus(response);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Block(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                var response = await _httpClient.PostAsJsonAsync("Users/Block",indexes);
                await CheckResponseStatus(response);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnBlock(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                var response = await _httpClient.PostAsJsonAsync("Users/Unblock", indexes);
                await CheckResponseStatus(response);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                var response = await _httpClient.PostAsJsonAsync("Users/Delete", indexes);
                await CheckResponseStatus(response);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Filter(string filterEmail)
        {
            if (!string.IsNullOrEmpty(filterEmail))
            {
                var filteredUsers = await _httpClient.GetFromJsonAsync<IEnumerable<UserModel>>($"Users/FilterByEmail?email={filterEmail}");
                return View("Index", filteredUsers);
            }
            return RedirectToAction("Index");
        }

        private async Task CheckResponseStatus(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                ViewData["SuccessMesage"] = await response.Content.ReadAsStringAsync();
            }
            else
            {
                ViewData["ErrorMesage"] = await response.Content.ReadAsStringAsync();
            }
        }

        [HttpGet]
        public async Task<JsonResult> LoadUsers([FromQuery]int batch)
        {
            var users = await _httpClient.GetFromJsonAsync<IEnumerable<UserModel>?>($"Users/GetByBatch/{batch}");
            return Json(users);
        }
    }
}
