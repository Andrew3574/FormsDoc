using FormsAPP.Hubs;
using FormsAPP.Models.FormAnswers;
using FormsAPP.Models.Forms;
using FormsAPP.Models.Users;
using FormsAPP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FormsAPP.Controllers
{
    public class FormsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHubContext<FormsHub> _formHubContext;

        public FormsController(HttpClientService httpClientService, IHubContext<FormsHub> formHubContext)
        {
            _httpClient = httpClientService.GetClient()!;
            _formHubContext = formHubContext;
        }

        public async Task<IActionResult> Index()
        {             
            return View(await _httpClient.GetFromJsonAsync<IEnumerable<FormModel>>("Forms/GetByBatch/0"));
        }

        public IActionResult CreateForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateForm(CreateFormModel model)
        {
            if(ModelState.IsValid)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await UploadImage(response, model.ImageFile);
                }
                response = await _httpClient.PostAsJsonAsync("Forms/CreateForm", model);
                if (response.IsSuccessStatusCode)
                {
                    await _formHubContext.Clients.All.SendAsync("FormCreatedAction", await response.Content.ReadFromJsonAsync<FormModel>());
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return View(model);

        }       

        public async Task<IActionResult> CreateFormAnswer([FromQuery]int formId, [FromQuery]string accessibility)
        {
            var response = await _httpClient.GetAsync($"Forms/FormTemplateInfo?formId={formId}&userId={HttpContext.Session.GetInt32("UserId")}&accessibility={accessibility}");
            if (response.IsSuccessStatusCode)
            {
                return View(await response.Content.ReadFromJsonAsync<FormTemplateInfoModel>());
            }
            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFormAnswer([FromBody] CreateFormAnswer model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Forms/CreateAnswer", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return View(model);
        }

        public async Task<JsonResult?> LikeAction([FromBody]LikeModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Forms/LikeAction",model);
            if(response.IsSuccessStatusCode)
            {
                var likesCount = await response.Content.ReadAsStringAsync();
                await _formHubContext.Clients.All.SendAsync("LikeAction", model.FormId, likesCount);
                return Json(likesCount);
            }
            return null;
        }

        public async Task<JsonResult?> CommentAction([FromBody]CreateCommentModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Forms/CommentAction", model);
            if (response.IsSuccessStatusCode)
            {
                var comment = await response.Content.ReadFromJsonAsync<CommentModel>();
                await _formHubContext.Clients.All.SendAsync("CommentAction", comment, model.FormId);
                return Json(comment);
            }
            return null;
        }

        [HttpGet]
        public async Task<JsonResult?> FilterTagsByName([FromQuery]string query)
        {
            var response = await _httpClient.GetAsync($"Forms/FilterTagsByName?query={query}");
            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<IEnumerable<FilterTagModel>>();
                return Json(tags);  
            }
            return Json(new List<FilterTagModel>());
        }

        [HttpGet]
        public async Task<JsonResult?> FilterUsersbyEmail([FromQuery] string query)
        {
            var response = await _httpClient.GetAsync($"Forms/FilterUsersByEmail?query={query}");
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<IEnumerable<FilterUserModel>>();
                return Json(users);
            }
            return Json(new List<FilterUserModel>());
        }

        [HttpGet]
        public async Task<JsonResult> LoadForms([FromQuery]int batch)
        {
            var forms = await _httpClient.GetFromJsonAsync<IEnumerable<FormModel>>($"Forms/GetByBatch/{batch}");
            return Json(forms);
        }
        private async Task<string> UploadImage(HttpResponseMessage response, IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(image.OpenReadStream()), "file", image.FileName);
            response = await _httpClient.PostAsync("Forms/UploadImage", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
