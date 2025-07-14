using Asp.Versioning;
using AutoMapper;
using FormsAPI.ModelsDTO.Users;
using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using OnixLabs.Core.Linq;
using Repositories;

namespace FormsAPI.Controllers
{
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
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
            if (!IsValidIndexes(indexes)) return BadRequest("invalid data input");
            var users = await _usersRepository.GetById(indexes!);
            users!.ForEach(u => u.Role = UserRole.admin);
            await _usersRepository.UpdateRange(users!);
            return Ok("successfully promoted");
            
        }        

        [HttpPost("DemoteToUser")]
        public async Task<ActionResult> DemoteToUser([FromBody] IEnumerable<int> indexes)
        {
            if (!IsValidIndexes(indexes)) return BadRequest("invalid data input");
            var users = await _usersRepository.GetById(indexes!);
            users!.ForEach(u => u.Role = UserRole.user);
            await _usersRepository.UpdateRange(users!);
            return Ok("successfully demoted");
        }

        [HttpPost("Block")]
        public async Task<ActionResult> Block([FromBody] IEnumerable<int> indexes)
        {
            if (!IsValidIndexes(indexes)) return BadRequest("invalid data input");
            var users = await _usersRepository.GetById(indexes!);
            users!.ForEach(u =>u.State = UserState.blocked);
            await _usersRepository.UpdateRange(users!);
            return Ok("successfully blocked");
        }

        [HttpPost("Unblock")]
        public async Task<ActionResult> Unblock([FromBody] IEnumerable<int> indexes)
        {
            if (!IsValidIndexes(indexes)) return BadRequest("invalid data input");
            var users = await _usersRepository.GetById(indexes!);
            users!.ForEach(u => u.State = UserState.active);
            await _usersRepository.UpdateRange(users!);
            return Ok("successfully unblocked"); 
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] IEnumerable<int> indexes)
        {
            if (!IsValidIndexes(indexes)) return BadRequest("invalid data input");
            var users = await _usersRepository.GetById(indexes!);
            users!.ForEach(u => u.State = UserState.active);
            await _usersRepository.DeleteRange(users!);
            return Ok("successfully deleted");
            
        }
       
        [HttpGet("FilterByEmail")]
        public async Task<ActionResult> FilterByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("invalid data input");
            var filteredUsers = await _usersRepository.FilterByEmail(email);
            return Ok(ConvertToUserDTO(filteredUsers));
        }
        
        private IEnumerable<UserDTO> ConvertToUserDTO(IEnumerable<User>? users)
        {
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
        private bool IsValidIndexes(IEnumerable<int> indexes)
        {
            return indexes != null && indexes!.Any() ? true : false;
        }
    }
}
