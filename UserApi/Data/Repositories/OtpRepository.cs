using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;

namespace UserApi.Data.Repositories
{
    public class OtpRepository
    {
        private readonly ApiDbContext _context;

        public OtpRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task SaveOtpAsync(Otp otp)
        {
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<Otp> GetOtpAsync(string phoneNumber, string otpCode)
        {
            return await _context.Otps.FirstOrDefaultAsync(o =>
                o.PhoneNumber == phoneNumber && o.Code == otpCode && !o.IsUsed && o.ExpiryTime > DateTime.UtcNow);
        }

        public async Task MarkOtpAsUsedAsync(Otp otp)
        {
            otp.IsUsed = true;
            await _context.SaveChangesAsync();
        }
    }
}
