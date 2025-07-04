using AutoMapper;
using FormsAPI.ModelsDTO.FormAnswers;
using FormsAPI.ModelsDTO.FormAnswers.CRUD;
using FormsAPI.ModelsDTO.Forms;
using FormsAPI.ModelsDTO.Forms.CRUD_DTO;
using FormsAPI.Services;
using FormsAPI.Services.Auth;
using FormsAPI.Services.Elastic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Models;
using Models.Enums;
using OnixLabs.Core.Linq;
using Repositories;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IElasticService _elasticsearchService;
        private readonly ImageService _imageService;
        private readonly FormsRepository _formsRepository;
        private readonly UsersRepository _usersRepository;
        private readonly TagsRepository _tagsRepository;
        private readonly LikesRepository _likesRepository;
        private readonly CommentsRepository _commentsRepository;
        private readonly FormAnswersRepository _formAnswersRepository;
        private readonly AccessFormUsersRepository _accessFormUsersRepository;
        private readonly FormTagsRepository _formTagsRepository;
        private readonly FormQuestionRepository _formQuestionRepository;
        private readonly FormQuestionOptionsRepository _formQuestionOptionsRepository;

        public FormsController(IMapper mapper, IElasticService elasticsearchService, FormsRepository formsRepository,
            UsersRepository usersRepository, TagsRepository tagsRepository, LikesRepository likesRepository,
            CommentsRepository commentsRepository, ImageService imageService, FormAnswersRepository formAnswersRepository,
            AccessFormUsersRepository accessFormUsersRepository, FormTagsRepository formTagsRepository,
            FormQuestionRepository formQuestionRepository, FormQuestionOptionsRepository formQuestionOptionsRepository)
        {
            _mapper = mapper;
            _elasticsearchService = elasticsearchService;
            _formsRepository = formsRepository;
            _usersRepository = usersRepository;
            _tagsRepository = tagsRepository;
            _likesRepository = likesRepository;
            _commentsRepository = commentsRepository;
            _imageService = imageService;
            _formAnswersRepository = formAnswersRepository;
            _accessFormUsersRepository = accessFormUsersRepository;
            _formTagsRepository = formTagsRepository;
            _formQuestionRepository = formQuestionRepository;
            _formQuestionOptionsRepository = formQuestionOptionsRepository;
        }

        [HttpPost("CreateIndex")]
        public async Task<IActionResult> CreateIndex(string indexName)
        {
            await _elasticsearchService.CreateIndexIfNotExists(indexName);
            return Ok($"{indexName} index created");
        }

        [HttpGet("SearchForm/{key}")]
        public async Task<IActionResult> GetFormByKey(string key)
        {
            var form = await _elasticsearchService.Get(key);
            return Ok(form);
        }

        [HttpGet("GetAllForms")]
        public async Task<IActionResult> GetAllFormsByKey()
        {
            var form = await _elasticsearchService.GetAll();
            return Ok(form);
        }

        [HttpGet("FullTextSearch")]
        public async Task<IActionResult> FullTextSearch([FromQuery]string query)
        {
            if (!string.IsNullOrEmpty(query)) return Ok(await _elasticsearchService.SearchFormsByQuery(query));
            return BadRequest("invalid filter input");
        }

        [HttpGet("GetByBatch/{batch:int}")]
        public async Task<ActionResult> GetFormsByBatch(int batch)
        {
            return Ok(_mapper.Map<IEnumerable<FormDTO>>(await _formsRepository.GetByBatch(batch,10)));
        }

        [JwtAuth]
        [HttpPost("CreateForm")]
        public async Task<ActionResult> CreateForm([FromBody]CreateFormDTO formDTO)
        {
            var form = _mapper.Map<Form>(formDTO);
            if (form is null) return BadRequest("form not found");
            await InsertOrCreateTags(form, formDTO.Tags);
            await _formsRepository.Create(form);
            await _elasticsearchService.AddOrUpdate(_mapper.Map<FormDTO>(await _formsRepository.GetById(form.Id)));
            return Ok(_mapper.Map<FormDTO>(form));
        }

        [JwtAuth]
        [HttpPut("UpdateForm")]
        public async Task<ActionResult> UpdateForm([FromBody] UpdateFormDTO formDTO)
        {
            var form = _mapper.Map<Form>(formDTO);
            if (form is null) return BadRequest("form not found");
            await InsertOrCreateTags(form,formDTO.NewTags);
            MarkDeletingEntities(formDTO);
            await _formsRepository.Update(form);
            await _elasticsearchService.AddOrUpdate(_mapper.Map<FormDTO>(await _formsRepository.GetById(form.Id)));
            return Ok("Form successfully updated");          
        }        

        [JwtAuth]
        [HttpPost("CreateAnswer")]
        public async Task<ActionResult> CreateAnswer([FromBody]CreateFormAnswerDTO answerDTO)
        {
            if (!await CheckFormVersion(answerDTO.FormId,answerDTO.Version)) return BadRequest("Current form recently has been updated. Please try again");
            var answer = _mapper.Map<FormAnswer>(answerDTO);
            await _formAnswersRepository.Create(answer);
            return Ok("Answer successfully created");                
        }

        [JwtAuth]
        [HttpGet("GetFormForUpdate")]
        public async Task<ActionResult> GetFormForUpdate([FromQuery]int formId)
        {
            var form = await _formsRepository.GetById(formId);
            if (form is null) return BadRequest("form not found");
            return Ok(_mapper.Map<UpdateFormDTO>(form));
        }

        [JwtAuth]
        [HttpGet("FormTemplateInfo")]
        public async Task<ActionResult> FormTemplateInfo([FromQuery]int formId, [FromQuery]int userId, [FromQuery]string userRole)
        {
            var form = await _formsRepository.GetById(formId);
            if (form is null) return BadRequest("form not found");
            if (form.Accessibility == FormAccessibility.@public || form.UserId == userId || await _accessFormUsersRepository.CheckAccess(userId, formId) || userRole == UserRole.admin.ToString())
                return Ok(_mapper.Map<FormTemplateDTO>(form));            
            return BadRequest("Insufficient access");
            
        }

        [JwtAuth]
        [HttpGet("AnsweredFormTemplateInfo")]
        public async Task<ActionResult> AnsweredFormTemplateInfo([FromQuery] int answerId, [FromQuery] int userId, [FromQuery] string userRole)
        {
            var formAnswer = await _formAnswersRepository.GetById(answerId);
            if (formAnswer is null) return BadRequest("answer not found");
            if (formAnswer.Form!.Accessibility == FormAccessibility.@public || formAnswer.Form.UserId == userId || await _accessFormUsersRepository.CheckAccess(userId, formAnswer.FormId) || userRole == UserRole.admin.ToString())
                return Ok(_mapper.Map<AnsweredFormTemplateDTO>(formAnswer));
            return BadRequest("Insufficient access");
        }

        [JwtAuth]
        [HttpPut("UpdateAnswer")]
        public async Task<ActionResult> UpdateAnswer([FromBody] AnsweredFormTemplateDTO answerDTO)
        {
            if (!await CheckFormVersion(answerDTO!.FormId, answerDTO.Version)) return BadRequest("Current form recently has been updated. Please try again");
            var answer = _mapper.Map<FormAnswer>(answerDTO);
            await _formAnswersRepository.Update(answer);
            return Ok("Answer successfully updated");          
        }

        [JwtAuth]
        [HttpGet("GetFormAnswers")]
        public async Task<ActionResult> GetFormAnswers([FromQuery] int formId, [FromQuery] int userId, [FromQuery] string userRole)
        {
            var formAnswers = await _formAnswersRepository.GetByFormId(formId);
            if (formAnswers is null || !formAnswers.Any()) return BadRequest("answers not found");
            if (formAnswers.First().Form!.UserId == userId || userRole == UserRole.admin.ToString())
                return Ok(new FormStatisticsDTO { QuestionList = _mapper.Map<List<FormQuestionDTO>>(formAnswers.First().Form!.FormQuestions), Answers = _mapper.Map<List<FormAnswerDTO>>(formAnswers) });
            return BadRequest("Insufficient access");                
        }

        [JwtAuth]
        [HttpPost("LikeAction")]
        public async Task<ActionResult> LikeAction([FromBody]LikeDTO likeDto)
        {
            await isLikedAction(likeDto);
            var likes =  await _likesRepository.GetLikesByFormId(likeDto.FormId);
            return Ok(likes!.Count());
        }

        [JwtAuth]
        [HttpPost("CommentAction")]
        public async Task<ActionResult> CommentAction([FromBody]CreateCommentDTO commentDto)
        {
            if (commentDto is null) return BadRequest("invalid data input");
            var comment = _mapper.Map<Comment>(commentDto);
            await _commentsRepository.Create(comment);            
            await _elasticsearchService.AddOrUpdate(_mapper.Map<FormDTO>(await _formsRepository.GetById(commentDto.FormId)));
            return Ok(_mapper.Map<CommentDTO>(comment));
        }

        [HttpGet("FilterTagsByName")]
        public async Task<IEnumerable<FilterTagDTO>?> FilterTagsByName([FromQuery]string query)
        {
            var tags = await _tagsRepository.FilterByName(query);
            return _mapper.Map<IEnumerable<FilterTagDTO>>(tags);
        }

        [HttpGet("FilterUsersByEmail")]
        public async Task<IEnumerable<FilterUserDTO>?> FilterUsersByEmail([FromQuery] string query)
        {
            var users = await _usersRepository.FilterByEmail(query);
            return _mapper.Map<IEnumerable<FilterUserDTO>>(users);
        }

        [HttpPost("UploadImage")]
        public async Task<string> UploadImage(IFormFile? file)
        {
            if (file is null || file.Length == 0) return string.Empty;
            using var stream = file.OpenReadStream();                
            return await _imageService.UploadImage(stream, file.FileName);            
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("UpdateUploadedImage")]
        public async Task<string> UpdateUploadedImage([FromForm]IFormFile? file, [FromForm] string oldImageUrl)
        {
            if (file is null || file.Length == 0) return string.Empty;
            using var stream = file.OpenReadStream();
            return await _imageService.UpdateImage(stream, oldImageUrl, file.FileName);            
        }

        private async Task<bool> CheckFormVersion(int formId, int version)
        {
            var form = await _formsRepository.GetById(formId);
            if (form is null) throw new ArgumentNullException("form not found");
            if (form!.Version == version) return true;
            return false;
        }

        private async Task isLikedAction(LikeDTO likeDto)
        {
            var like = await _likesRepository.isLiked(likeDto.FormId, likeDto.UserId);
            if (like is null) await _likesRepository.Create(_mapper.Map<Like>(likeDto));
            else await _likesRepository.Delete(like);
        }
        private void MarkDeletingEntities(UpdateFormDTO formDTO)
        {
            if (formDTO.DeletedAccessFormUsers.Any()) _accessFormUsersRepository.MarkDelete(_mapper.Map<IEnumerable<AccessformUser>>(formDTO.DeletedAccessFormUsers));
            if (formDTO.DeletedFormTags.Any()) _formTagsRepository.MarkDelete(_mapper.Map<IEnumerable<FormTag>>(formDTO.DeletedFormTags));
            if (formDTO.DeletedQuestions.Any()) _formQuestionRepository.MarkDelete(_mapper.Map<IEnumerable<FormQuestion>>(formDTO.DeletedQuestions));
            if (formDTO.DeletedQuestionOptions.Any()) _formQuestionOptionsRepository.MarkDelete(_mapper.Map<IEnumerable<FormQuestionOption>>(formDTO.DeletedQuestionOptions));
        }

        private async Task InsertOrCreateTags(Form form, List<string>? insertTags)
        {            
            if (insertTags is not null && insertTags.Any())
            {
                var tags = await _tagsRepository.GetOrCreateByName(insertTags);
                tags.ForEach(t => form.FormTags.Add(new FormTag { Tag = t }));
            }
        }

    }
}
