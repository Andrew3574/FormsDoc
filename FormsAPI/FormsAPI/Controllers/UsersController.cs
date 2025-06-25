using AutoMapper;
using FormsAPI.ModelsDTO;
using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using OnixLabs.Core.Linq;
using Repositories;

namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuth("admin")]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersController(UsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [HttpGet("GetByBatch/{batch:int}")]
        public async Task<ActionResult> GetUsersByBatch(int batch)
        {
            var users = await _usersRepository.GetByBatch(batch);
            return Ok(ConvertToUserDTO(users));
        }

        [HttpPost("PromoteToAdmin")]
        public async Task<ActionResult> PromoteUser([FromBody] IEnumerable<int> indexes)
        {
            if (indexes != null && indexes!.Any())
            {
                var users = await _usersRepository.GetById(indexes!);
                users!.ForEach(u => u.Role = UserRole.admin);
                await _usersRepository.UpdateRange(users!);
                return Ok("successfully promoted");
            }
            return BadRequest("invalid data input");
        }

        [HttpPost("DemoteToUser")]
        public async Task<ActionResult> DemoteToUser([FromBody] IEnumerable<int> indexes)
        {
            if (indexes != null && indexes!.Any())
            {
                var users = await _usersRepository.GetById(indexes!);
                users!.ForEach(u => u.Role = UserRole.user);
                await _usersRepository.UpdateRange(users!);
                return Ok("successfully demoted");
            }
            return BadRequest("invalid data input");
        }

        [HttpPost("Block")]
        public async Task<ActionResult> Block([FromBody] IEnumerable<int> indexes)
        {
            if(indexes != null && indexes!.Any())
            {
                var users = await _usersRepository.GetById(indexes!);
                users!.ForEach(u =>u.State = UserState.blocked);
                await _usersRepository.UpdateRange(users!);
                return Ok("successfully blocked");
            }
            return BadRequest("invalid data input");
        }

        [HttpPost("Unblock")]
        public async Task<ActionResult> Unblock([FromBody] IEnumerable<int> indexes)
        {
            if (indexes != null && indexes!.Any())
            {
                var users = await _usersRepository.GetById(indexes!);
                users!.ForEach(u => u.State = UserState.active);
                await _usersRepository.UpdateRange(users!);
                return Ok("successfully unblocked");
            }
            return BadRequest("invalid data input");
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] IEnumerable<int> indexes)
        {
            if (indexes != null && indexes!.Any())
            {
                var users = await _usersRepository.GetById(indexes!);
                users!.ForEach(u => u.State = UserState.active);
                await _usersRepository.DeleteRange(users!);
                return Ok("successfully deleted");
            }
            return BadRequest("invalid data input");
        }
       
        [HttpGet("FilterByEmail")]
        public async Task<ActionResult> FilterByEmail([FromQuery] string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var filteredUsers = await _usersRepository.FilterByEmail(email);
                return Ok(ConvertToUserDTO(filteredUsers));
            }
            return BadRequest("invalid data input");
        }
        
        private IEnumerable<UserDTO> ConvertToUserDTO(IEnumerable<User>? users)
        {
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}
