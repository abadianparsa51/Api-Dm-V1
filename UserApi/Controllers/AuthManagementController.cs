using Microsoft.AspNetCore.Mvc;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;
using UserApi.Helpers;
using UserApi.Services;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;
        private readonly IJwtHelper _jwtHelper;  // Inject IJwtHelper

        public AuthManagementController(IUserService userService, IOtpService otpService, IJwtHelper jwtHelper)
        {
            _userService = userService;
            _otpService = otpService;
            _jwtHelper = jwtHelper;  // Initialize
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);
            return result.Result ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login/email")]
        public async Task<IActionResult> LoginWithEmail([FromBody] UserLoginRequestDto loginDto)
        {
            var result = await _userService.LoginUserAsync(loginDto);
            return result.Result ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("login/phone/generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] OtpRequestDto request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("Phone number is required");

            // Generate and send OTP
            var otp = await _otpService.GenerateOtpAsync(request.PhoneNumber);

            // Normally, you would also want to send this OTP to the user's phone number using a service like Twilio
            // But for now, we're just returning it as a response to test
            return Ok(new { Message = "OTP generated", Otp = otp });
        }

        [HttpPost("login/phone/verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpRequestDto request, [FromQuery] string otp)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(otp))
                return BadRequest("Phone number and OTP are required");

            var isValid = await _otpService.VerifyOtpAsync(request.PhoneNumber, otp);

            if (isValid)
            {
                // OTP is valid, so log in the user
                var user = await _userService.GetUserByPhoneNumberAsync(request.PhoneNumber);
                if (user != null)
                {
                    // Generate JWT Token using IJwtHelper
                    var token = _jwtHelper.GenerateJwtToken(user); // Now using IJwtHelper
                    return Ok(new { Message = "Login successful", Token = token });
                }

                return BadRequest("User not found");
            }

            return BadRequest("Invalid OTP");
        }

    }
}
