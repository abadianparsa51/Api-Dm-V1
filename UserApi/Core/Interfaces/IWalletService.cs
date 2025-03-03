using UserApi.Core.Models;

public interface IWalletService
{
    Task<Wallet> GetWalletByUserIdAsync(string userId);
}
