using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Core.Interfaces;
using UserApi.Core.Models.DTOs;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] OtpRequestDto request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("User ID is required");

            var otp = await _otpService.GenerateOtpAsync(request.PhoneNumber);
            return Ok(new { Message = "OTP generated", Otp = otp });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpRequestDto request, [FromQuery] string otp)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(otp))
                return BadRequest("User ID and OTP are required");

            var isValid = await _otpService.VerifyOtpAsync(request.PhoneNumber, otp);
            return isValid ? Ok("OTP is valid") : BadRequest("Invalid OTP");
        }
    }
}
