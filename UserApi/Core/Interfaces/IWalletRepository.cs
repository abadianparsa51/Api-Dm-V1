using UserApi.Core.Models;

public interface IWalletRepository
{
    Task CreateWalletAsync(Wallet wallet);
    Task<Wallet> GetWalletByUserIdAsync(string userId);
    // سایر متدها
}
