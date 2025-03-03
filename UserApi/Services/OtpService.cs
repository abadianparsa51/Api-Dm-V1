using UserApi.Core.Interfaces;
using UserApi.Core.Models;

namespace UserApi.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;

        public OtpService(IOtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public async Task<string> GenerateOtpAsync(string phoneNumber, string userId)
        {
            var otp = new Otp
            {
                UserId = userId, // ذخیره userId
                PhoneNumber = phoneNumber,
                Code = new Random().Next(100000, 999999).ToString(),
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            await _otpRepository.SaveOtpAsync(otp);
            return otp.Code;
        }
        public async Task<bool> VerifyOtpAsync(string userId, string otp)
        {
            var storedOtp = await _otpRepository.GetOtpAsync(userId, otp);

            if (storedOtp != null && storedOtp.ExpiryTime > DateTime.UtcNow && !storedOtp.IsUsed)
            {
                await _otpRepository.MarkOtpAsUsedAsync(storedOtp);
                return true;
            }

            return false;
        }

    }
}
