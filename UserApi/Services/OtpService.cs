using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Data.Repositories;

namespace UserApi.Services
{
    public class OtpService : IOtpService
    {
        private readonly OtpRepository _otpRepository;

        public OtpService(OtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public async Task<string> GenerateOtpAsync(string phoneNumber)
        {
            var otp = new Otp
            {
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

            if (storedOtp != null)
            {
                await _otpRepository.MarkOtpAsUsedAsync(storedOtp);
                return true;
            }

            return false;
        }
    }
}
