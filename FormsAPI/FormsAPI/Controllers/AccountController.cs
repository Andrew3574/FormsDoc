using AutoMapper;
using FormsAPI.ModelsDTO.Account;
using FormsAPI.ModelsDTO.FormAnswers;
using FormsAPI.ModelsDTO.Forms.CRUD_DTO;
using FormsAPI.ModelsDTO.Users;
using FormsAPI.Services;
using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Enums;
using OnixLabs.Core.Text;
using Repositories;
using System.Security.Claims;


namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly FormAnswersRepository _formAnswersRepository;
        private readonly FormsRepository _formsRepository;
        private readonly EncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;
        private readonly IMemoryCache _memoryCache;

        public AccountController(UsersRepository usersRepository, IMapper mapper, EncryptionService encryptionService,
            EmailService emailService, IMemoryCache memoryCache, FormAnswersRepository formAnswersRepository,
            FormsRepository formsRepository)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
            _emailService = emailService;
            _memoryCache = memoryCache;
            _formAnswersRepository = formAnswersRepository;
            _formsRepository = formsRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO userDto)
        {
            if(userDto is null) return BadRequest("Invalid data input");
            try
            {
                userDto.Password = _encryptionService.HashPassword(userDto.Email + userDto.Password);
                User user = _mapper.Map<User>(userDto);
                await _usersRepository.Create(user);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Current email is already taken");
            }            
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]LoginDTO userDto)
        {
            if (userDto is null) return BadRequest("Invalid data input");    
            var user = await _usersRepository.GetByEmail(userDto.Email);
            if (!IsLoginSuccessful(user, userDto,out string message)) return BadRequest(message);
            user!.Lastlogin = DateTime.UtcNow;
            await _usersRepository.Update(user);
            return Ok(new AuthDTO() { UserId = user.Id, Email = userDto.Email, Role = user.Role.ToString(), Token = JwtAuthenticationService.GenerateJSONWebToken(user!) });                
        }

        [HttpGet("GetUserProfileByToken")]
        public async Task<ActionResult> GetUserProfileInfo([FromQuery]string token)
        {
            if(string.IsNullOrEmpty(token)) return BadRequest("token not found");
            var email = JwtAuthenticationService.GetEmailByToken(token);
            var user = await _usersRepository.GetByEmail(email);
            if(user is null) return BadRequest("user not found");
            return Ok(_mapper.Map<UserDTO>(user));            
        }

        [HttpGet("GetUserProfileByUserId")]
        public async Task<ActionResult> GetUserProfileInfo([FromQuery]int userId)
        {
            var user = await _usersRepository.GetById(userId);
            if (user is null) return BadRequest("user not found");
            return Ok(_mapper.Map<UserDTO>(user));            
        }

        [HttpPost("GetCode")]
        public async Task<ActionResult> SendRecoveryCode([FromBody]string email)
        {
            var user = await _usersRepository.GetByEmail(email);
            if(user is null) return BadRequest("user not found");
            var code = await _emailService.SendRecoveryCode(email);
            _memoryCache.Set(email, code, DateTimeOffset.Now.AddMinutes(2));
            return Ok("Recovery code has been sent to email");            
        }

        [HttpPost("CheckCode")]
        public ActionResult CheckRecoveryCode([FromBody]RecoveryDTO userDto)
        {
            if(_memoryCache.TryGetValue(userDto.Email, out string? storedCode) && userDto.Code==storedCode)
            {
                _memoryCache.Remove(userDto.Email);
                return Ok();
            }            
            return BadRequest("Invalid code");
        }

        [HttpPost("RecoverPassword")]
        public async Task<ActionResult> RecoverPassword([FromBody]LoginDTO userDto)
        {
            var user = await _usersRepository.GetByEmail(userDto.Email);
            if(user is null) return BadRequest("user not found");
            user.Passwordhash = _encryptionService.HashPassword(userDto.Email+userDto.Password);
            await _usersRepository.Update(user);
            return Ok();                
        }

        [HttpGet("GetUserForms")]
        public async Task<ActionResult> GetUserForms([FromQuery]int userId)
        {
            if(userId == 0) return BadRequest("user not found");
            return Ok(_mapper.Map<IEnumerable<FormDTO>>(await _formsRepository.FilterByUserId(userId)));
        }

        [HttpGet("GetAnsweredForms")]
        public async Task<ActionResult> GetAnsweredForms([FromQuery]int userId)
        {
            if (userId == 0) return BadRequest("user not found");
            return Ok(_mapper.Map<IEnumerable<AnsweredFormDTO>>(await _formAnswersRepository.FilterByUserId(userId)));
        }

        private bool IsLoginSuccessful(User? user, LoginDTO userDto, out string message)
        {
            message = string.Empty;
            if(user == null)
            {
                message = "User not found";
                return false;
            }
            if(user!.Passwordhash != _encryptionService.HashPassword(userDto.Email + userDto.Password))
            {
                message = "Wrong password";
                return false;
            }
            if(user.State == UserState.blocked)
            {
                message = "You were blocked";
                return false;
            }
            return true;
        }
    }
}
