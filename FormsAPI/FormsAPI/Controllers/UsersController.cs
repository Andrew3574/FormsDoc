using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
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

        public UsersController(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("GetByBatch")]
        public async Task<ActionResult> GetUsersByBatch([FromBody] int batch)
        {
            return Ok(await _usersRepository.GetByBatch(batch));
        }

        [HttpPost("Block")]
        public async Task<ActionResult> Block([FromBody] IEnumerable<string> emails)
        {
            if(emails != null && emails!.Any())
            {
                var users = await _usersRepository.GetByEmail(emails!);
                users!.ForEach(u =>u.State = Models.Enums.UserState.blocked);
                await _usersRepository.UpdateRange(users!);
                return Ok("successfully blocked");
            }
            return BadRequest("invalid data input");
        }

        [HttpPost("Unblock")]
        public async Task<ActionResult> Unblock([FromBody] IEnumerable<string> emails)
        {
            if (emails != null && emails!.Any())
            {
                var users = await _usersRepository.GetByEmail(emails!);
                users!.ForEach(u => u.State = Models.Enums.UserState.active);
                await _usersRepository.UpdateRange(users!);
                return Ok("successfully unblocked");
            }
            return BadRequest("invalid data input");
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] IEnumerable<string> emails)
        {
            if (emails != null && emails!.Any())
            {
                var users = await _usersRepository.GetByEmail(emails!);
                users!.ForEach(u => u.State = Models.Enums.UserState.active);
                await _usersRepository.DeleteRange(users!);
                return Ok("successfully deleted");
            }
            return BadRequest("invalid data input");
        }
       
        [HttpGet("FilterByEmail")]
        public async Task<IActionResult> FilterByEmail([FromBody] string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var filteredUsers = await _usersRepository.FilterByEmail(email);
                return Ok(filteredUsers);
            }
            return BadRequest("invalid data input");
        }
    }
}
