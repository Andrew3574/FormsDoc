using AutoMapper;
using FormsAPP.Hubs;
using FormsAPP.Models.FormAnswers;
using FormsAPP.Models.FormAnswers.CRUD;
using FormsAPP.Models.Forms;
using FormsAPP.Models.Forms.CRUD;
using FormsAPP.Models.Users;
using FormsAPP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FormsAPP.Controllers
{
    public class FormsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHubContext<FormsHub> _formHubContext;
        private readonly IMapper _mapper;

        public FormsController(HttpClientService httpClientService, IHubContext<FormsHub> formHubContext, IMapper mapper)
        {
            _httpClient = httpClientService.GetClient()!;
            _formHubContext = formHubContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {             
            return View(await _httpClient.GetFromJsonAsync<IEnumerable<FormModel>>("Forms/GetByBatch/0"));
        }

        public async Task<IActionResult> FullTextSearch(string query)
        {
            var response = await _httpClient.GetAsync($"Forms/FullTextSearch?query={query}");
            if (response.IsSuccessStatusCode) return View("Index", await response.Content.ReadFromJsonAsync<IEnumerable<FormModel>>());
                return RedirectToAction("Index");
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
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await UploadImage(model.ImageFile);
                }
                var response = await _httpClient.PostAsJsonAsync("Forms/CreateForm", model);
                if (response.IsSuccessStatusCode)
                {
                    await _formHubContext.Clients.All.SendAsync("FormCreatedAction", await response.Content.ReadFromJsonAsync<FormModel>());
                    return RedirectToAction("Index");
                }
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return View(model);

        }
        
        public async Task<IActionResult> UpdateFormPage([FromQuery]int formId)
        {
            var response = await _httpClient.GetAsync($"Forms/GetFormForUpdate?formId={formId}");
            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadFromJsonAsync<UpdateFormModel>();
                return View(model);
            }
            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateForm(UpdateFormModel model)
        {
            if (ModelState.IsValid)
            {
                await UpdateImage(model);
                var response = await _httpClient.PutAsJsonAsync("Forms/UpdateForm", model);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("UserProfile","Account");
                }
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return View("UpdateFormPage",model);
        }

        public async Task<IActionResult> CreateFormAnswer([FromQuery]int formId)
        {
            var response = await _httpClient.GetAsync($"Forms/FormTemplateInfo?formId={formId}&userId={HttpContext.Session.GetInt32("UserId")}&userRole={HttpContext.Session.GetString("Role")}");
            if (response.IsSuccessStatusCode)
            {
                return View(await response.Content.ReadFromJsonAsync<FormTemplateInfoModel>());
            }
            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFormAnswer(FormTemplateInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Forms/CreateAnswer", _mapper.Map<CreateFormAnswer>(model));
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAnswerPage([FromQuery] int answerId)
        {
            var response = await _httpClient.GetAsync($"Forms/AnsweredFormTemplateInfo?answerId={answerId}&userId={HttpContext.Session.GetInt32("UserId")}&userRole={HttpContext.Session.GetString("Role")}");
            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadFromJsonAsync<AnsweredFormTemplateInfo>();
                return View(model);
            }
            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAnswer(AnsweredFormTemplateInfo model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync("Forms/UpdateAnswer", model);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("UserProfile", "Account");
                }
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return View("UpdateAnswerPage", model);
        }
        
        public async Task<IActionResult> FormStatistics([FromQuery]int formId)
        {
            if(formId != 0)
            {
                var response = await _httpClient.GetAsync($"Forms/GetFormAnswers?formId={formId}&userId={HttpContext.Session.GetInt32("UserId")}&userRole={HttpContext.Session.GetString("Role")}");
                if (response.IsSuccessStatusCode)
                {
                    var model = await response.Content.ReadFromJsonAsync<FormStatisticsModel>();
                    return View(model);
                }
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("GetUserForms", "Account");
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

        private async Task<string> UploadImage(IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(image.OpenReadStream()), "file", image.FileName);
            var response = await _httpClient.PostAsync("Forms/UploadImage", content);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task UpdateImage(UpdateFormModel model)
        {
            if (model.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = await UpdateUploadedImage(model.ImageUrl, model.ImageFile);
                }
                else
                {
                    model.ImageUrl = await UploadImage(model.ImageFile);
                }
            }
        }

        private async Task<string> UpdateUploadedImage(string oldImageUrl, IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(image.OpenReadStream()), "file", image.FileName); 
            content.Add(new StringContent(oldImageUrl), "oldImageUrl");
            var response = await _httpClient.PutAsync("Forms/UpdateUploadedImage", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
