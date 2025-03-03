using UserApi.Core.Models.DTOs;
using UserApi.Core.Models;

public interface IUserService
{
    Task<AuthResult> RegisterUserAsync(UserRegistrationRequestDto userDto);
    Task<AuthResult> LoginUserAsync(UserLoginRequestDto loginDto);
    Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber);
    string GenerateJwtToken(ApplicationUser user);

    // Ensure these methods are defined in the IUserService interface with correct return types.
    Task<string> GenerateOtpAsync(string phoneNumber, string userId);
    Task<AuthResult> VerifyOtpAsync(string phoneNumber, string otp);
}
