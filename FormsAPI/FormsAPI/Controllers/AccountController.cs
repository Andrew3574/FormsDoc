using FormsAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuth("admin,user")]
    public class AccountController : ControllerBase
    {
    }
}
