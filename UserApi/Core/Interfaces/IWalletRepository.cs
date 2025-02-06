using UserApi.Core.Models;

namespace UserApi.Core.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> CreateWalletAsync(string userId);
        Task<Wallet> GetWalletByUserIdAsync(string userId);
    }
}
