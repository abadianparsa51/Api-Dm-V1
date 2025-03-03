using Microsoft.AspNetCore.Mvc;
using MediatR;
using UserApi.Core.Models.DTOs;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto userDto)
        {
            var result = await _mediator.Send(new RegisterUserCommand(userDto));
            return result.Result ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login/email")]
        public async Task<IActionResult> LoginWithEmail([FromBody] UserLoginRequestDto loginDto)
        {
            var result = await _mediator.Send(new LoginUserCommand(loginDto));
            return result.Result ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("login/phone/generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] OtpRequestDto request)
        {
            var otp = await _mediator.Send(new GenerateOtpCommand(request.PhoneNumber));
            return Ok(new { Message = "OTP generated", Otp = otp });
        }

        [HttpPost("login/phone/verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpRequestDto request, [FromQuery] string otp)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(request.PhoneNumber, otp));
            return result.Result ? Ok(result) : BadRequest("Invalid OTP");
         }
    }
}
