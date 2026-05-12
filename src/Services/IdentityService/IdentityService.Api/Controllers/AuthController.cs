using IdentityService.Business.Abstractions;
using IdentityService.Business.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService userService) : ControllerBase
    {
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await userService.RegisterAsync(request);
            return Ok(result);
        }

        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await userService.LoginAsync(request);
            return Ok(result);
        }
    }
}
