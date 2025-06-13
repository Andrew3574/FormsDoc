using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Enums;
using Repositories;
using System.Security.Claims;

namespace FormsAPI.Services.Auth
{
    public class JwtAuthAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _role;

        public JwtAuthAttribute(string role = "user")
        {
            _role = role.ToLower();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            await ValidateClaims(token, context);

            await next();
        }

        private async Task ValidateClaims(string token, ActionExecutingContext context)
        {
            ClaimsPrincipal? claims = JwtAuthenticationService.ValidateToken(token);
            CheckAuthentication(claims, context);
            CheckAuthorization(claims, context);
            await ValidateUser(claims, context);
        }

        private void CheckAuthentication(ClaimsPrincipal? claims, ActionExecutingContext context)
        {
            if (claims == null || !claims.Identity!.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            if (string.IsNullOrEmpty(claims?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
        }

        private void CheckAuthorization(ClaimsPrincipal? claims, ActionExecutingContext context)
        {
            var role = claims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role) || !_role.Contains(role))
            {
                context.Result = new UnauthorizedObjectResult("Insufficient access");
                return;
            }
        }

        private async Task ValidateUser(ClaimsPrincipal? claims, ActionExecutingContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetService<UsersRepository>();
            var user = await userRepository!.GetByEmail(claims?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value!);
            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult("User not found.");
                return;
            }
            if (user.State == UserState.blocked)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
        }
    }
}
