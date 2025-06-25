using AutoMapper;
using FormsAPI.Models.FormAnswers;
using FormsAPI.ModelsDTO.FormAnswers;
using FormsAPI.ModelsDTO.Forms;
using FormsAPI.Services;
using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using OnixLabs.Core.Linq;
using Repositories;

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
                    await InsertTags(form, formDTO);
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
        [HttpPost("CreateAnswer")]
        public async Task<ActionResult> CreateAnswer([FromBody]CreateFormAnswerDTO answerDTO)
        {
            try
            {
                if (answerDTO != null)
                {
                    var answer = _mapper.Map<FormAnswer>(answerDTO);
                    await _formAnswersRepository.Create(answer);
                    return Ok();
                }
                return BadRequest("Invalid data input");
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data input. Reason: " + ex.Message);
            }
        }

        [JwtAuth]
        [HttpGet("FormTemplateInfo")]
        public async Task<ActionResult> FormTemplateInfo([FromQuery]int formId, [FromQuery]int userId)
        {
            if (userId != 0 && formId != 0)
            {
                var form = await _formsRepository.GetById(formId);
                if (form != null && form.Accessibility == FormAccessibility.@public || await _accessFormUsersRepository.HasAccess(userId))
                {
                    return Ok(_mapper.Map<FormTemplateInfoDTO>(form));
                }
                return BadRequest("Insufficient access");
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

        private async Task InsertTags(Form form, CreateFormDTO formDTO)
        {
            var tags = await _tagsRepository.GetOrCreateByName(formDTO.Tags);
            tags.ForEach(t => form.FormTags.Add(new FormTag { Tag = t }));
        }

    }
}
