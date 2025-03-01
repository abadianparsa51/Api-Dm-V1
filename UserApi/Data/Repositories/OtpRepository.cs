using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;
using UserApi.Data;

namespace UserApi.Data.Repositories
{
    public class OtpRepository : IOtpRepository
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

        public async Task<Otp?> GetOtpAsync(string userId, string otp)
        {
            return await _context.Otps
                .Where(o => o.UserId == userId && o.Code == otp && !o.IsUsed && o.ExpiryTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task MarkOtpAsUsedAsync(Otp otp)
        {
            otp.IsUsed = true;
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}
