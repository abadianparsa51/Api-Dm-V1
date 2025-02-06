namespace UserApi.Core.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string phoneNumber); // Keep the correct method signature
        Task<bool> VerifyOtpAsync(string phoneNumber, string otp); // Ensure this matches the implementation
    }
}
