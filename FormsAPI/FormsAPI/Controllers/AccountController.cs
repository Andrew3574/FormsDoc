using AutoMapper;
using FormsAPI.ModelsDTO;
using FormsAPI.Services;
using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using OnixLabs.Core.Text;
using Repositories;


namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly EncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public AccountController(UsersRepository usersRepository, IMapper mapper, EncryptionService encryptionService)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }
        
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO userDto)
        {
            if (userDto != null)
            {
                try
                {
                    userDto.Password = _encryptionService.HashPassword(userDto.Email + userDto.Password);
                    User user = _mapper.Map<User>(userDto);
                    await _usersRepository.Create(user);
                    var token = JwtAuthenticationService.GenerateJSONWebToken(user);
                    return Ok(token);
                }
                catch (Exception)
                {
                    return BadRequest("Current email is already taken");
                }
            }
            return BadRequest("Invalid data input");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]LoginDTO userDto)
        {
            if(userDto != null)
            {
                try
                {
                    var user = await _usersRepository.GetByEmail(userDto.Email);
                    if (IsLoginSuccessful(user, userDto,out string message))
                    {
                        var token = JwtAuthenticationService.GenerateJSONWebToken(user!);
                        return Ok(token);
                    }
                    return BadRequest(message);
                }
                catch (Exception)
                {
                    return BadRequest("Invalid data input");
                }
            }
            return BadRequest("Invalid data input");
        }

        /// <summary>
        /// Returns user data for user page
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [JwtAuth]
        [HttpGet("GetUserInfo/{email:alpha}")]
        public async Task<ActionResult> GetUserInfo(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _usersRepository.GetByEmail(email);
                return Ok(user);    
            }
            return BadRequest("user not found");
        }

        private bool IsLoginSuccessful(User? user, LoginDTO userDto, out string message)
        {
            message = string.Empty;
            if (user == null)
            {
                message = "User not found";
                return false;
            }
            if (user!.Passwordhash != _encryptionService.HashPassword(userDto.Email + userDto.Password))
            {
                message = "Wrong password";
                return false;
            }
            if (user.State == Models.Enums.UserState.blocked)
            {
                message = "You were blocked";
                return false;
            }
            return true;
        }
    }
}
