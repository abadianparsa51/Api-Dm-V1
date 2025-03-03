public interface IOtpService
{
    Task<string> GenerateOtpAsync(string phoneNumber, string userId);
    Task<bool> VerifyOtpAsync(string userId, string otp);
}
