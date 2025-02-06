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

        public async Task<Wallet> CreateWalletAsync(string userId)
        {
            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0 // موجودی اولیه صفر
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(string userId)
        {
            return await _context.Wallets
                                 .FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
