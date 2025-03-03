using Microsoft.EntityFrameworkCore;
using UserApi.Core.Interfaces;
using UserApi.Core.Models;

namespace UserApi.Data.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApiDbContext _context;

        public WalletRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task CreateWalletAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> GetWalletByUserIdAsync(string userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
