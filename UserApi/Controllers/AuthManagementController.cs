using Microsoft.AspNetCore.Mvc;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthManagementController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);
            return result.Result ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginDto)
        {
            var result = await _userService.LoginUserAsync(loginDto);
            return result.Result ? Ok(result) : Unauthorized(result);
        }
    }
}
