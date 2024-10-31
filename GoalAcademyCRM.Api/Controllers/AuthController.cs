using GoalAcademyCRM.Api.Models.Auth;
using GoalAcademyCRM.Api.Services.Foundations.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoalAcademyCRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpGet("sign-in")]
        public async ValueTask<IActionResult> SignInAsync([FromQuery] LoginDetails loginDetails)
        {
            var result = await authService.LoginAsync(loginDetails);
            return Ok(result);
        }

        [Authorize(Roles = "System")]
        [HttpGet("me")]
        public async ValueTask<IActionResult> Me()
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)!.Value);
            var result = await authService.GetCurrentUser(userId);

            return Ok(result);
        }
    }
}
