using AutoMapper;
using FormsAPI.ModelsDTO.FormAnswers;
using FormsAPI.ModelsDTO.FormAnswers.CRUD;
using FormsAPI.ModelsDTO.Forms;
using FormsAPI.ModelsDTO.Forms.CRUD_DTO;
using FormsAPI.Services;
using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ElasticsearchService _elasticsearchService;
        private readonly ImageService _imageService;
        private readonly FormsRepository _formsRepository;
        private readonly UsersRepository _usersRepository;
        private readonly TagsRepository _tagsRepository;
        private readonly LikesRepository _likesRepository;
        private readonly CommentsRepository _commentsRepository;
        private readonly FormAnswersRepository _formAnswersRepository;
        private readonly AccessFormUsersRepository _accessFormUsersRepository;

        public FormsController(IMapper mapper, ElasticsearchService elasticsearchService,FormsRepository formsRepository,
            UsersRepository usersRepository, TagsRepository tagsRepository, LikesRepository likesRepository,
            CommentsRepository commentsRepository,ImageService imageService, FormAnswersRepository formAnswersRepository,
            AccessFormUsersRepository accessFormUsersRepository)
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
            try
            {
                if (formDTO != null)
                {
                    var form = _mapper.Map<Form>(formDTO);
                    await InsertOrCreateTags(form, formDTO.Tags);
                    await _formsRepository.Create(form);
                    /*await _elasticsearchService.IndexForm(form);*/
                    return Ok(_mapper.Map<FormDTO>(form));
                }
                return BadRequest("Invalid data input");
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data input. Reason: " + ex.Message);
            }
        }

        [JwtAuth]
        [HttpPut("UpdateForm")]
        public async Task<ActionResult> UpdateForm([FromBody] UpdateFormDTO formDTO)
        {
            try
            {
                if (formDTO != null)
                {
                    var form = _mapper.Map<Form>(formDTO);
                    await InsertOrCreateTags(form,formDTO.NewTags);
                    await _formsRepository.Update(form);
                    /*await _elasticsearchService.IndexForm(form);*/
                    return Ok("Form successfully updated");
                }
                return BadRequest("Invalid data input");
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data input. Reason: " + ex.Message);
            }
        }

        [JwtAuth]
        [HttpPost("CreateAnswer")]
        public async Task<ActionResult> CreateAnswer([FromBody]CreateFormAnswerDTO answerDTO)
        {
            try
            {
                if (answerDTO != null)
                {
                    if (await CheckFormVersion(answerDTO.FormId,answerDTO.Version))
                    {
                        var answer = _mapper.Map<FormAnswer>(answerDTO);
                        await _formAnswersRepository.Create(answer);
                        return Ok("Answer successfully created");
                    }
                    return BadRequest("Current form recently has been updated. Please try again");
                }
                return BadRequest("Invalid data input");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occured. Reason: " + ex.Message);
            }
        }

        [JwtAuth]
        [HttpGet("GetFormForUpdate")]
        public async Task<ActionResult> GetFormForUpdate([FromQuery]int formId)
        {
            if (formId != 0)
            {
                var form = await _formsRepository.GetById(formId);
                if (form != null)
                {
                    return Ok(_mapper.Map<UpdateFormDTO>(form));
                }
                return BadRequest("form not found");
            }
            return BadRequest("invalid data input");
        }

        [JwtAuth]
        [HttpGet("FormTemplateInfo")]
        public async Task<ActionResult> FormTemplateInfo([FromQuery]int formId, [FromQuery]int userId, [FromQuery]string userRole)
        {
            if (userId != 0 && formId != 0)
            {
                var form = await _formsRepository.GetById(formId);
                if(form != null)
                {
                    if (form.Accessibility == FormAccessibility.@public || form.UserId == userId || await _accessFormUsersRepository.CheckAccess(userId, formId) || userRole == UserRole.admin.ToString())
                    {
                        return Ok(_mapper.Map<FormTemplateDTO>(form));
                    }
                    return BadRequest("Insufficient access");
                }
            }
            return BadRequest("Invalid data input");
        }

        [JwtAuth]
        [HttpGet("AnsweredFormTemplateInfo")]
        public async Task<ActionResult> AnsweredFormTemplateInfo([FromQuery] int answerId, [FromQuery] int userId, [FromQuery] string userRole)
        {
            if (userId != 0 && answerId != 0)
            {
                var formAnswer = await _formAnswersRepository.GetById(answerId);
                if (formAnswer != null)
                {
                    if (formAnswer.Form!.Accessibility == FormAccessibility.@public || formAnswer.Form.UserId == userId || await _accessFormUsersRepository.CheckAccess(userId, formAnswer.FormId) || userRole == UserRole.admin.ToString())
                    {
                        return Ok(_mapper.Map<AnsweredFormTemplateDTO>(formAnswer));
                    }
                    return BadRequest("Insufficient access");
                }
            }
            return BadRequest("Invalid data input");
        }

        [JwtAuth]
        [HttpPut("UpdateAnswer")]
        public async Task<ActionResult> UpdateAnswer([FromBody] AnsweredFormTemplateDTO answerDTO)
        {
            try
            {
                if (answerDTO != null)
                {
                    if (await CheckFormVersion(answerDTO.FormId, answerDTO.Version))
                    {
                        var answer = _mapper.Map<FormAnswer>(answerDTO);
                        await _formAnswersRepository.Update(answer);
                        return Ok("Answer successfully updated");
                    }
                    return BadRequest("Current form recently has been updated. Please try again");
                }
                return BadRequest("Invalid data input");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occured. Reason: " + ex.Message);
            }
        }

        [JwtAuth]
        [HttpGet("GetFormAnswers")]
        public async Task<ActionResult> GetFormAnswers([FromQuery] int formId, [FromQuery] int userId, [FromQuery] string userRole)
        {
            if (userId != 0 && formId != 0)
            {
                var formAnswers = await _formAnswersRepository.GetByFormId(formId);
                if (formAnswers != null && formAnswers.Any())
                {
                    if (formAnswers.First().Form!.UserId == userId || userRole == UserRole.admin.ToString())
                    {
                        return Ok(new FormStatisticsDTO { QuestionList = _mapper.Map<List<FormQuestionDTO>>(formAnswers.First().Form!.FormQuestions), Answers = _mapper.Map<List<FormAnswerDTO>>(formAnswers) });
                    }
                    return BadRequest("Insufficient access");
                }
                return BadRequest("answers not found");
            }
            return BadRequest("Invalid data input");
        }

        [JwtAuth]
        [HttpPost("LikeAction")]
        public async Task<ActionResult> LikeAction([FromBody]LikeDTO likeDto)
        {
            if(likeDto != null)
            {
                await isLikedAction(likeDto);
                var likes =  await _likesRepository.GetLikesByFormId(likeDto.FormId);
                return Ok(likes!.Count());
            }
            return BadRequest("An error occured");
        }

        [JwtAuth]
        [HttpPost("CommentAction")]
        public async Task<ActionResult> CommentAction([FromBody]CreateCommentDTO commentDto)
        {
            if (commentDto != null)
            {
                var comment = _mapper.Map<Comment>(commentDto);
                await _commentsRepository.Create(comment);       
                return Ok(_mapper.Map<CommentDTO>(comment));
            }
            return BadRequest("An error occured");
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
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();                
                return await _imageService.UploadImage(stream, file.FileName);
            }
            return string.Empty;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("UpdateUploadedImage")]
        public async Task<string> UpdateUploadedImage([FromForm]IFormFile? file, [FromForm] string oldImageUrl)
        {
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                return await _imageService.UpdateImage(stream, oldImageUrl, file.FileName);
            }
            return string.Empty;
        }

        private async Task<bool> CheckFormVersion(int formId, int version)
        {
            var form = await _formsRepository.GetById(formId);
            if (form!.Version == version)
            {
                return true;
            }
            return false;
        }

        private async Task isLikedAction(LikeDTO likeDto)
        {
            var like = await _likesRepository.isLiked(likeDto.FormId, likeDto.UserId);
            if (like != null)
            {
                await _likesRepository.Delete(like);
            }
            else
            {
                await _likesRepository.Create(_mapper.Map<Like>(likeDto));
            }
        }

        private async Task InsertOrCreateTags(Form form, List<string> insertTags)
        {
            if (insertTags.Any())
            {
                var tags = await _tagsRepository.GetOrCreateByName(insertTags);
                tags.ForEach(t => form.FormTags.Add(new FormTag { Tag = t }));
            }
        }
    }
}
